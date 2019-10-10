using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public enum ECombinationType
{
    MERGE
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
            AudioManager.Instance.PlayPutTile();
        });
        DragTile.SetSelected(false);
        OnTileDropped?.Invoke(DragTile);
        OnResolvingMatch?.Invoke(true);
        StartCoroutine(SearchForMatch(DragTile, GridPosition)) ;
        return true;

    }

    IEnumerator SearchForMatch(Tile DragTile, Vector2Int GridPosition)
    {
        yield return Utils.GetWaitForSeconds(.1f);
        if (!FindMergeTiles(DragTile, GridPosition))
        {
            yield return Utils.GetWaitForSeconds(.2f);
            if (FindSwapTiles(DragTile, GridPosition,out var swapedPosition))
            {
                yield return Utils.GetWaitForSeconds(.2f);
                OnAddNewTile?.Invoke();
                StartCoroutine(SearchForMatch(DragTile, swapedPosition));

            }
            else
            { 
                yield return Utils.GetWaitForSeconds(.2f);
            
                CheckGameOver();
            }
        }
        else
        {
            yield return Utils.GetWaitForSeconds(.2f);
           
            OnAddNewTile?.Invoke();
            CheckGameOver();
        }
        CheckCompletedTargetScore();

    }

    void CheckGameOver()
    {
        if (activeTiles.Count >= 5)
        {
            GameManager.Instance.CheckRoundResults();
        }
        else
        {
            OnResolvingMatch?.Invoke(false);
        }
        
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

           AddComboBonus();
        }

        return bMergeFound;
    }

    void AddComboBonus()
    {
        OnComboBonus.Invoke();
        CheckCompletedTargetScore();


    }
    void RemoveTile(Vector2Int GridPosition)
    {
        activeTiles.Remove(tiles[GridPosition.x, GridPosition.y]);
        tiles[GridPosition.x, GridPosition.y] = null; 
    }
    bool FindSwapTiles(Tile DragTile, Vector2Int GridPosition,out Vector2Int NewGridPosition)
    {
        Vector2Int testCoords = GridPosition;
        bool bSwapFound = false;
        NewGridPosition = GridPosition;
        EDirections otherDirection;

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
                
                if (!bSwapFound)
                {
                     SwapTiles(GridPosition,testCoords);
                     NewGridPosition = testCoords;

                }
                
                if(AtoBCheck) DragTile.RemoveSwapIndicator(joinDirection);
                if (BToACheck) tiles[testCoords.x, testCoords.y].RemoveSwapIndicator(joinDirectionOther);

                //  Debug.Log(GridPosition.ToString()+"  "+ DragTile.blockType + "  " + tiles[testCoords.x, testCoords.y].blockSubColor);
                bSwapFound = true;
                ReorderTiles();
                AddComboBonus();
            }
          
        } 
        return bSwapFound;

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
    EDirections GetDirectionTo(Vector2Int GridPosition, Vector2Int OtherPosition)
    {
        if (GridPosition.x < OtherPosition.x) return EDirections.RIGHT;
        if (GridPosition.x > OtherPosition.x) return EDirections.LEFT;
        if (GridPosition.y < OtherPosition.y) return EDirections.UP;
        if (GridPosition.y > OtherPosition.y) return EDirections.DOWN;

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
        tiles[TileA.x, TileA.y] = tiles[TileB.x, TileB.y];
        tiles[tmpCoord.x, tmpCoord.y] = tmpTile;
        return tmpTile;
    }
    public void CheckCombination(Tile DragTile,Vector2Int FloorCoordinate)
    {

        Vector2Int testCoords = FloorCoordinate;

        for (int i = 0; i < checkAroundCoords.Length; i++)
        {
            testCoords = FloorCoordinate + checkAroundCoords[i];
            if (testCoords.x < 0 || testCoords.y < 0 || testCoords.x >= tiles.GetLength(0) || testCoords.y >= tiles.GetLength(1)) continue;
            if (tiles[testCoords.x, testCoords.y] == null) continue;

            if (DragTile.blockType != tiles[testCoords.x, testCoords.y].blockType) continue;
            
            EDirections direction = FindDirectionFromTile(FloorCoordinate, testCoords.x, testCoords.y);
            DragTile.ShowPreviewCombination(DragTile, direction, ECombinationType.MERGE);


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
