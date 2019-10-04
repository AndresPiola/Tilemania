using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStartPanel : MonoBehaviour {


    public RectTransform panel;
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI levelText;
    //public Button startButton;
    bool bButtonPressed;
    private void OnDisable()
    {
        GameMode.OnGameState -= GameMode_OnGameState;
        PlayerController.OnSidePress -= PlayerController_OnSidePress;

    }

    private void OnEnable()
    {
        GameMode.OnGameState += GameMode_OnGameState;

        PlayerController.OnSidePress += PlayerController_OnSidePress;
          
    }

    private void PlayerController_OnSidePress(EDirections _direction)
    {
        StartButtonClick();
    }

    private void Start()
    {

        int bestScore = GameInstance.Instance.GetBestScore();
        bestScoreText.SetText(bestScore.ToString());
        levelText.SetText("Level "+GameInstance.Instance.playerLevel);

    }
    public void StartButtonClick()
    {
        if (bButtonPressed) return;
        bButtonPressed = true;
        GameMode.Instance.BeginSession(); 
    }

    void OnBeginGame()
    {
        panel.gameObject.SetActive(false);

    }


    private void GameMode_OnGameState(EGameStates _newGameState)
    {
        switch (_newGameState)
        {
            case EGameStates.MAIN_MENU:
                break;
            case EGameStates.CONNECTING:
                break;
            case EGameStates.LOADING_REMATCH:
                break;
            case EGameStates.LOADING_NEXTROUND:
                break;
            case EGameStates.GAMEPLAY:
                OnBeginGame();
                

                break;
            case EGameStates.ROUND_OVER:
                break;
            case EGameStates.GAME_OVER:
                break;
            default:
                break;
        }
    }
}
