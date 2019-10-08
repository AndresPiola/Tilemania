using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : Singleton<TileGenerator>
{
    // Start is called before the first frame update
    private List<Tile> freeTiles=new List<Tile>();


    void OnDisable()
    {
        DropArea.OnDropZoneReady -= DropArea_OnDropZoneReady;
        DropArea.OnTileDropped -= DropArea_OnTileDropped;
        DropArea.OnAddNewTile -= DropArea_OnAddNewTile;

    }

    void OnEnable()
    {
        DropArea.OnDropZoneReady += DropArea_OnDropZoneReady;
        DropArea.OnTileDropped += DropArea_OnTileDropped;
        DropArea.OnAddNewTile += DropArea_OnAddNewTile;
    }

   
    private void DropArea_OnTileDropped(Tile TileRef)
    {
        freeTiles.Remove(TileRef);
        ArrangeTiles();
    }

    private void DropArea_OnDropZoneReady()
    {
         GenerateStartingtiles();
    }

    void GenerateStartingtiles()
    {
        GameObject tileRef;
    
        for (int i = 0; i < 5; i++)
        {
            tileRef= TilesPool.Instance.GetPooledObject( );
            tileRef.GetComponent<Tile>().Initialize();
            freeTiles.Add(tileRef.GetComponent<Tile>()); 
        }
        ArrangeTiles();
    }
    private void DropArea_OnAddNewTile()
    {
        if(freeTiles.Count>5)return;

        GameObject tileRef = TilesPool.Instance.GetPooledObject();
        tileRef.transform.position = transform.position + Vector3.right * 5;
            tileRef.GetComponent<Tile>().Initialize();
            freeTiles.Add(tileRef.GetComponent<Tile>());
        
        ArrangeTiles();
    }

    public void ReturnTile(Tile TileRef)
    {
        int tileIndex = freeTiles.IndexOf(TileRef);

        if (freeTiles.Count > 1)
        {
            Utils.Swap(freeTiles, freeTiles.Count - 1, tileIndex);
        }
         

        ArrangeTiles();
    }
    void ArrangeTiles()
    {
        Vector2 targetPosition = transform.position;

        for (int i = 0; i < freeTiles.Count; i++)
        {
            
          targetPosition.x = i - 2.5f + (i * 0.2f);

          freeTiles[i].MoveTowards(targetPosition);

        }
     
    }
}
