using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileData
{
    public int BlockTypeIndex;
    public int BlockScoreValue;
    public int BlockSubColorIndex;
    public Dictionary<EDirections, int> sideColor=new Dictionary<EDirections, int>(){{EDirections.NONE,0}};
      
}
public class TileGeneratorTutorial : TileGenerator
{
    public TileData[] tutorialTiles;
    private int nextTiles = 2;
    public override void DropArea_OnDropZoneReady()
    {
        GenerateTutorialTiles();
         
    }

    public override void DropArea_OnAddNewTile()
    { 
        if (freeTiles.Count > 5) return;
        nextTiles++;
  
        Tile tileRef = TilesPool.Instance.GetPooledObjectComponent<Tile>();
        tileRef.transform.position = transform.position + Vector3.right * 5;
       TileData tileTmp = tutorialTiles[nextTiles-1];
        tileRef.Initialize(tileTmp.BlockTypeIndex, tileTmp.BlockScoreValue, tileTmp.BlockSubColorIndex, tileTmp);
         
        freeTiles.Add(tileRef);

        ArrangeTiles();
    }
    void GenerateTutorialTiles()
    {
        Tile tileRef;
        TileData tileTmp;

        for (int i = 0; i < nextTiles; i++)
        {
            tileRef = TilesPool.Instance.GetPooledObjectComponent<Tile>() ; 
            tileTmp = tutorialTiles[i]; 
            tileRef.Initialize(tileTmp.BlockTypeIndex,tileTmp.BlockScoreValue,tileTmp.BlockSubColorIndex,tileTmp.sideColor.Keys.ElementAt(0), tileTmp.sideColor.Values.ElementAt(0));
            freeTiles.Add(tileRef);
        }
        ArrangeTiles();
    }
}
