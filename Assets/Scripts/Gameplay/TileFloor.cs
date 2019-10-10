using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFloor : MonoBehaviour
{
    public Vector2Int gridPosition;

    void OnEnable()
    {
        GameMode.OnGameState += GameMode_OnGameState;
    }


    void OnDisable()
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
                
                break;
            case EGameStates.ROUND_OVER:
                break;
            case EGameStates.GAME_OVER:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_val1), _val1, null);
        }
    }
    public void Initialize(Vector2Int NewGridPosition)
    {
        gridPosition=NewGridPosition;
    }
}
