using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameplayUI : MonoBehaviour {

    public GameObject panel;
     

    private void OnEnable()
    {
        GameMode.OnGameState += GameMode_OnGameState;
 

    }


    private void OnDisable()
    {
        GameMode.OnGameState -= GameMode_OnGameState;

    }


    private void GameMode_OnGameState(EGameStates _newGameState)
    {
        switch (_newGameState)
        {
            case EGameStates.MAIN_MENU:
                break;
            case EGameStates.CONNECTING:
                break;
            case EGameStates.RELOADING_ROUND:
                break;
            case EGameStates.LOADING_NEXTROUND:
                break;
            case EGameStates.GAMEPLAY:
                panel.SetActive(true);



                break;
            case EGameStates.ROUND_OVER:
                panel.SetActive(false);

                // LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 0, .15f);
                 break;
            case EGameStates.GAME_OVER:
                panel.SetActive(false);

                break;
            default:
                break;
        }
    }
}
