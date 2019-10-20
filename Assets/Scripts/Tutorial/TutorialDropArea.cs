using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDropArea : DropArea
{
     public bool[,] dropArea= new bool[3,2];


     public override void OnDisable()
     {
         base.OnDisable();
        TutorialManager.OnTutorialIndexChange -= TutorialManager_OnTutorialIndexChange;
     }
     
    public override void OnEnable()
     {
         base.OnEnable();
         TutorialManager.OnTutorialIndexChange += TutorialManager_OnTutorialIndexChange;

    }

    private void TutorialManager_OnTutorialIndexChange(int Param1)
    {
        switch (Param1)
        {
            case 4:
                CreateTile(2,0,true);
                break;
        }
    }
    public override void GenerateDropArea()
     {
        Vector2 tilePos = Vector2.zero;
        tileSize = 1.28f;// GameConfig.Instance.tileSize;

        GameObject floorTileTmp;
        tiles = new Tile[4, 2];
        Vector2Int griPositionTmp = Vector2Int.zero;

        
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 3; x++)
            { 
                if(!dropArea[x,y])continue;
                  CreateTile(x,y); 
            }
        }

        CallDropZoneReady();
    }
}
