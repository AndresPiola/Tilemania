using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
 

public class UIGameOver : MonoBehaviour {

    public RectTransform gameOverPanel;
    public Button retryButton;
    public TextMeshProUGUI level;
    public float waitTime = 1;


    
     void FindDepedency()
    {
        if(gameOverPanel==null)
        gameOverPanel = transform.GetChild(0).GetComponent<RectTransform>();

    }
    private void Awake()
    {
        gameOverPanel.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        GameMode.OnGameState -= GameMode_OnGameState;
        retryButton.onClick.RemoveAllListeners();

    }

    private void OnEnable()
    {
        GameMode.OnGameState += GameMode_OnGameState;
        retryButton.onClick.AddListener(() =>
        {
            GameMode.Instance.RetryLevel();

        });
 
    }
  

     
    
    private void GameMode_OnGameState(EGameStates _newGameState)
    {
 

        switch (_newGameState)
        {

            case EGameStates.GAMEPLAY:

                StartCoroutine(HidePanel());
                break;
            case EGameStates.ROUND_OVER:

                StartCoroutine(HidePanel());
                break;
            case EGameStates.GAME_OVER:
                StartCoroutine( ShowGameOver());

                break;


            case EGameStates.RELOADING_ROUND:

                StartCoroutine(HidePanel());

                break;
        }

    }

    IEnumerator HidePanel()
    {
        yield return null;
        gameOverPanel.gameObject.SetActive(false);

    }
    IEnumerator ShowGameOver()
    {
        level.SetText("Level "+GameInstance.Instance.playerLevel.ToString());

        gameOverPanel.gameObject.SetActive(true);

        

        yield return null;
    }

}
