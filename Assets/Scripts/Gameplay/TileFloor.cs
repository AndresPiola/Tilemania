using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFloor : MonoBehaviour
{
    public Vector2Int gridPosition;


    public void Initialize(Vector2Int NewGridPosition)
    {
        gridPosition=NewGridPosition;
    }
}
