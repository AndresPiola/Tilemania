using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public enum ECombinationType
{
    NONE,MERGE,SWAP
};

public class DropArea : Singleton<DropArea>
{
     
     [TableMatrix]
    public Tile[,] tiles;
    [HideInInspector]
    public List<Tile> activeTiles = new List<Tile>();
    [HideInInspector]
    public List<GameObject> activeFloorTiles=new List<GameObject>();

    [HideInInspector]
    public float tileSize;

    private bool bTargetCompleted;

    Vector2Int[] checkAroundCoords=new Vector2Int[]{new Vector2Int(0,1), new Vector2Int(1,0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };
    public static event FNotify_1Params<Tile> OnTileDropped ;
    public static event FNotify OnDropZoneReady, OnAddNewTile, OnComboBonus;

    public static event FNotify_1Params<bool> OnResolvingMatch;
    
    void OnDisable()
    {
        GameMode.OnGameState -= GameMode_OnGameState;

    }

    void OnEnable()
    {
        GameMode.OnGameState += GameMode_OnGameState;
    }

    private void GameMode_OnGameState(EGameStates _val1)
    {
        switch (_val1)
        {
            case EGameStates.MAIN_MENU:
                break;
            case EGameStates.CONNECTING:
                break;
            case EGameStates.RELOADING_ROUND:
                break;
            case EGameStates.LOADING_NEXTROUND:
                break;
            case EGameStates.LOADING_REMATCH:
                break;
            case EGameStates.GAMEPLAY:
                ResetBoard();
                break;
            case EGameStates.ROUND_OVER:
                break;
            case EGameStates.GAME_OVER:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_val1), _val1, null);
        }
    }

    void ResetBoard()
    {
        bTargetCompleted=false;

        for (int i = 0; i < activeFloorTiles.Count; i++)
        {
            activeFloorTiles[i].SetActive(false);
        }
        activeFloorTiles.Clear();
        activeTiles.Clear();
        GenerateDropArea();
         
    }
    void GenerateDropArea()
    {
       
        Vector2 tilePos = Vector2.zero;
        tileSize = 1.28f;// GameConfig.Instance.tileSize;
    
        GameObject floorTileTmp;
        tiles= new Tile[4,2];
        Vector2Int griPositionTmp=Vector2Int.zero;

        int randomSpace = Random.Range(0, 3);

        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if(y==1 && randomSpace==x)continue;

                griPositionTmp.x = x;
                griPositionTmp.y = y;
                 
                tilePos = GridPositionToWorldPosition(griPositionTmp); 

                floorTileTmp= DropTilePool.Instance.GetPooledObject(tilePos); 
              
                floorTileTmp.transform.SetParent(transform);
                floorTileTmp.GetComponent<TileFloor>().Initialize(new Vector2Int(x,y));
                floorTileTmp.SetActive(true);
                 activeFloorTiles.Add(floorTileTmp);

            }
        }

        OnDropZoneReady?.Invoke();
    }

    public Vector2 GridPositionToWorldPosition(Vector2Int GridPosition)
    {
        Vector2 tilePos;
        float offsetX =  1.5f;
        tilePos.x = GridPosition.x * tileSize   - offsetX;
        tilePos.y = GridPosition.y * tileSize   ;
        return tilePos;

    }
    public Vector2 GridPositionToWorldPosition(int X,int Y)
    {
        Vector2 tilePos;
        float offsetX = 1.5f;
        tilePos.x = X* tileSize - offsetX;
        tilePos.y = Y * tileSize;
        return tilePos;

    }
    public bool TryToDropTile(Tile DragTile, Vector2Int GridPosition)
    {
        if (tiles[GridPosition.x, GridPosition.y] != null) return false;
        tiles[GridPosition.x, GridPosition.y] = DragTile;
        activeTiles.Add(DragTile);  

        Vector2 targetPos = GridPositionToWorldPosition(GridPosition);
        DragTile.SetInPosition();
        DragTile.MoveTowards(targetPos, () =>
        {
            AudioManager.Instance.PlaySound(ESfx.TILE_DROP);
        });
        DragTile.SetSelected(false);
        OnTileDropped?.Invoke(DragTile);
        OnResolvingMatch?.Invoke(true);
        QueueSearch(DragTile, GridPosition);

        ExecutePendingSearch();
        return true;

    }
  public List<Action > searchQueue=new List<Action>();
  //  public List<Func<Task>> searchQueue = new List<Func<Task>>();

    public void QueueSearch(Tile DragTile, Vector2Int GridPosition)
    {
        if(searchQueue==null)
            searchQueue = new List<Action>();
         searchQueue.Add(() => { SearchForMatch(DragTile, GridPosition);});
          
    }
     

    public async Task SearchForMatch (Tile DragTile, Vector2Int GridPosition)
    { 
      //  Debug.Log("SearchForMatch"+ DragTile.name+"  "+GridPosition.ToString());
        await Task.Delay(100);

        if (FindMergeTiles(DragTile, GridPosition))
        {
            await Task.Delay(200);
            PopUpMergeLinkPool.Instance.GetPooledObjectComponent<UIPopUpMergeLink>(DragTile.transform.position).
                ShowPopUp(ECombinationType.MERGE);
            AudioManager.Instance.PlaySound(ESfx.COMBO_MERGE);

            GameManager.Instance.AddComboCount(DragTile.transform.position);
            QueueSearch(DragTile, GridPosition);
        }
        else //try to find swaps
        {
            await Task.Delay(200);
            if (FindSwapTiles(DragTile, GridPosition, out Tile OtherTile, out var swapedPosition))
            {
                await Task.Delay(200);
                PopUpMergeLinkPool.Instance.GetPooledObjectComponent<UIPopUpMergeLink>(DragTile.transform.position).
                    ShowPopUp(ECombinationType.SWAP);
                AudioManager.Instance.PlaySound(ESfx.COMBO_SWAP);
                GameManager.Instance.AddComboCount(DragTile.transform.position);
                OnAddNewTile?.Invoke();
                QueueSearch(DragTile, swapedPosition);
                QueueSearch(OtherTile, GridPosition);


            }
           
        }
        await Task.Delay(201);
        
        CheckCompletedTargetScore();
        ExecutePendingSearch();


    }
    /*
    IEnumerator SearchForMatch(Tile DragTile, Vector2Int GridPosition)
    {
        yield return Utils.GetWaitForSeconds(.1f);
        if (!FindMergeTiles(DragTile, GridPosition))
        {
            yield return Utils.GetWaitForSeconds(.2f);
            if (FindSwapTiles(DragTile, GridPosition,out Tile OtherTile,out var swapedPosition))
            {
                yield return Utils.GetWaitForSeconds(.2f);
                PopUpMergeLinkPool.Instance.GetPooledObjectComponent<UIPopUpMergeLink>(DragTile.transform.position).
                 ShowPopUp(ECombinationType.SWAP);
                AudioManager.Instance.PlaySound(ESfx.COMBO_SWAP);
                GameManager.Instance.AddComboCount();
                OnAddNewTile?.Invoke();
                AddToQueue(SearchForMatch(DragTile, swapedPosition)) ;
                AddToQueue(SearchForMatch(OtherTile, GridPosition)) ;
                 
                
            }
            else
            { 
                yield return Utils.GetWaitForSeconds(.2f);

                MatchingFinished();
            }
        }
        else///merge combo
        {
            yield return Utils.GetWaitForSeconds(.2f);
            PopUpMergeLinkPool.Instance.GetPooledObjectComponent<UIPopUpMergeLink>(DragTile.transform.position).
                ShowPopUp(ECombinationType.MERGE);
            AudioManager.Instance.PlaySound(ESfx.COMBO_MERGE);

            GameManager.Instance.AddComboCount();
            AddToQueue(SearchForMatch(DragTile, GridPosition)); 
        }
        CheckCompletedTargetScore();

    }
    */
    void ExecutePendingSearch()
    { 
        for (int i = 0; i < searchQueue.Count; i++)
        {
            if (searchQueue[i] != null)
            {
                Debug.Log("execute pending"+ searchQueue[i]);
                searchQueue[i]( );
                searchQueue[i] = null;
                return;
                
            }
             
        }

        _= ResolvePendingMovements();

    }
    void MatchingFinished()
    {
        if (activeTiles.Count >= 5)
        {
            GameManager.Instance.CheckRoundResults();
        }
        
    }

    async  Task ResolvePendingMovements()
    {
        Debug.Log("Called resolved");
        int comboCount = GameManager.Instance.comboCount;
        if (comboCount > 1)
        {
            await Task.Delay(100);

            for (int i = 0; i < activeTiles.Count; i++)
            {
                activeTiles[i].AddValue(comboCount);
                AudioManager.Instance.PlaySound(ESfx.COMBO_COUNT);
                await Task.Delay(200);

            }
        }
        searchQueue.Clear();

      //  await Task.Delay(1000);
        GameManager.Instance.ResetComboCount();
            OnResolvingMatch?.Invoke(false);
        MatchingFinished();

    }
    
    bool FindMergeTiles(Tile DragTile, Vector2Int GridPosition)
    {
        Vector2Int testCoords = GridPosition;
        bool bMergeFound=false;
        for (int i = 0; i < checkAroundCoords.Length; i++)
        {
            testCoords = GridPosition + checkAroundCoords[i];
            if (testCoords.x < 0 || testCoords.y < 0 || testCoords.x >= tiles.GetLength(0) || testCoords.y >= tiles.GetLength(1)) continue;
            if (tiles[testCoords.x, testCoords.y]==null)continue;

            if (DragTile.blockType != tiles[testCoords.x, testCoords.y].blockType) continue;
            DragTile.AbsorbOtherTile(tiles[testCoords.x, testCoords.y]);
            tiles[testCoords.x, testCoords.y].MergeTowards(GridPositionToWorldPosition(GridPosition.x, GridPosition.y),
                DragTile.AddValue);
          RemoveTile(testCoords);
            bMergeFound = true;
            OnAddNewTile?.Invoke();

            CheckCompletedTargetScore();
        }

        return bMergeFound;
    }

     
    void RemoveTile(Vector2Int GridPosition)
    {
        activeTiles.Remove(tiles[GridPosition.x, GridPosition.y]);
        tiles[GridPosition.x, GridPosition.y] = null; 
    }
    bool FindSwapTiles(Tile DragTile, Vector2Int GridPosition,out Tile OtherTile, out Vector2Int NewGridPosition)
    {
        Vector2Int testCoords = GridPosition;
        bool bDoOnce = false;
        NewGridPosition = GridPosition;
        EDirections otherDirection;
        OtherTile = null;
        for (int i = 0; i < checkAroundCoords.Length; i++)
        {
            testCoords = GridPosition + checkAroundCoords[i];
            if (testCoords.x < 0 || testCoords.y < 0 || testCoords.x >= tiles.GetLength(0) || testCoords.y >= tiles.GetLength(1)) continue;
            if (tiles[testCoords.x, testCoords.y] == null) continue;
            EDirections joinDirection = GetDirectionTo(GridPosition, testCoords);
            EDirections joinDirectionOther = GetDirectionTo(testCoords,GridPosition );
            bool AtoBCheck = DragTile.GetIndicator(joinDirection).value ==
                             tiles[testCoords.x, testCoords.y].blockSubColor;
            bool BToACheck =
                DragTile.blockSubColor == tiles[testCoords.x, testCoords.y].GetIndicator(joinDirectionOther).value;

            if (AtoBCheck || BToACheck)
            {
                
                if (!bDoOnce)
                {
                    OtherTile =  SwapTiles(GridPosition,testCoords);
                     NewGridPosition = testCoords;
                     DragTile.AddValue(1);
                     OtherTile.AddValue(1);

                }
                
                if(AtoBCheck) DragTile.RemoveSwapIndicator(joinDirection);
                if (BToACheck) tiles[testCoords.x, testCoords.y].RemoveSwapIndicator(joinDirectionOther);

                
                //  Debug.Log(GridPosition.ToString()+"  "+ DragTile.blockType + "  " + tiles[testCoords.x, testCoords.y].blockSubColor);
                bDoOnce = true;
                ReorderTiles(); 
            }
          
        } 
        return bDoOnce;

    }

    bool CheckCompletedTargetScore()
    {
        if (bTargetCompleted) return bTargetCompleted;

        for (int i = 0; i < activeTiles.Count; i++)
        {

            if (GameManager.Instance.CheckTargetCompleted(activeTiles[i].blockType, activeTiles[i].blockScore))
            {
                bTargetCompleted = true;
                return true;
            }
        }

        return false;
    }

    bool CheckSwap(Tile DraggedTile,Vector2Int TestPosition)
    {
 
         if (tiles[TestPosition.x, TestPosition.y] == null) return false;
         Vector2 posA=DraggedTile.transform.position;
        Vector2 posB= tiles[TestPosition.x, TestPosition.y].transform.position;

        EDirections joinDirection = GetDirectionToFromWorldPos(posA, posB);
        EDirections joinDirectionOther = GetDirectionToFromWorldPos(posB, posA);
 
        bool AtoBCheck = DraggedTile.GetIndicator(joinDirection).value ==
                         tiles[TestPosition.x, TestPosition.y].blockSubColor;
        bool BToACheck =
            DraggedTile.blockSubColor == tiles[TestPosition.x, TestPosition.y].GetIndicator(joinDirectionOther).value;
   
        return (AtoBCheck || BToACheck);
           
    }
    EDirections GetDirectionTo(Vector2Int GridPosition, Vector2Int OtherPosition)
    {
        if (GridPosition.x < OtherPosition.x) return EDirections.RIGHT;
        if (GridPosition.x > OtherPosition.x) return EDirections.LEFT;
        if (GridPosition.y < OtherPosition.y) return EDirections.UP;
        if (GridPosition.y > OtherPosition.y) return EDirections.DOWN;

        return EDirections.NONE;

    }
    EDirections GetDirectionToFromWorldPos(Vector2 Position1, Vector2 OtherPosition)
    {
        if (Math.Abs(Position1.x - OtherPosition.x) > Math.Abs(Position1.y - OtherPosition.y))
        {
        if (Position1.x < OtherPosition.x) return EDirections.RIGHT;
        if (Position1.x > OtherPosition.x) return EDirections.LEFT;
        }
        else
        {
             if (Position1.y < OtherPosition.y) return EDirections.UP;
        if (Position1.y > OtherPosition.y) return EDirections.DOWN; 

        }
       
      

        return EDirections.NONE;

    }

    void ReorderTiles()
    {
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (tiles[x, y]==null)continue;
                 tiles[x, y].MoveTowards(GridPositionToWorldPosition(x,y),true);
            }
           
        }
    }
    Tile SwapTiles(Vector2Int TileA, Vector2Int TileB)
    {
        Vector2Int tmpCoord = TileB;
        Tile tmpTile = tiles[TileA.x, TileA.y];
        Tile Other = tiles[TileB.x, TileB.y];
        tiles[TileA.x, TileA.y] = tiles[TileB.x, TileB.y];
        tiles[tmpCoord.x, tmpCoord.y] = tmpTile;
        return Other;
    }
    public void PreviewCombination(Tile DragTile,Vector2Int FloorCoordinate)
    {

        Vector2Int testCoords = FloorCoordinate;
        ECombinationType combiType;
        for (int i = 0; i < checkAroundCoords.Length; i++)
        {
            testCoords = FloorCoordinate + checkAroundCoords[i];
            combiType=ECombinationType.NONE;
            if (testCoords.x < 0 || testCoords.y < 0 || testCoords.x >= tiles.GetLength(0) || testCoords.y >= tiles.GetLength(1)) continue;
            if (tiles[testCoords.x, testCoords.y] == null) continue;

            if (DragTile.blockType == tiles[testCoords.x, testCoords.y].blockType) combiType = ECombinationType.MERGE;
            else
            if(CheckSwap(DragTile, testCoords)) 
                combiType = ECombinationType.SWAP;
          
            if(combiType==ECombinationType.NONE)continue;

            EDirections direction = FindDirectionFromTile(FloorCoordinate, testCoords.x, testCoords.y);
            DragTile.ShowPreviewCombination(DragTile, direction, combiType);


        }


    }

    public EDirections FindDirectionFromTile(Vector2Int OriginGridPosition, Vector2Int OtherGridPosition)
    {
        if (OtherGridPosition.x > OriginGridPosition.x) return EDirections.RIGHT;
        if (OtherGridPosition.x < OriginGridPosition.x) return EDirections.LEFT;
        if (OtherGridPosition.y > OriginGridPosition.y) return EDirections.UP;
        if (OtherGridPosition.y < OriginGridPosition.y) return EDirections.DOWN;

        return EDirections.NONE;
    }

    public EDirections FindDirectionFromTile(Vector2Int OriginGridPosition, int X, int Y)
    {
        if (X > OriginGridPosition.x) return EDirections.RIGHT;
        if (X < OriginGridPosition.x) return EDirections.LEFT;
        if (Y > OriginGridPosition.y) return EDirections.UP;
        if (Y < OriginGridPosition.y) return EDirections.DOWN;

        return EDirections.NONE;
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateDropArea();

    }

    
}
