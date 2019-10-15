using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
 

public class UIGameOver : SerializedMonoBehaviour
{

    public RectTransform gameOverPanel;
    public Button retryButton;
 
    public float waitTime = 1;
    public RectTransform gameOverRibbon;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI bestScoreTxt;
    public RectTransform grayBackground;

    [Header("ShowParams")]
    public Vector3 showPosition;
    public float showTweenTime;
    public LeanTweenType showEaseType;

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
        retryButton?.onClick.RemoveAllListeners();

    }

    private void OnEnable()
    {
        GameMode.OnGameState += GameMode_OnGameState;
        retryButton?.onClick.AddListener(() =>
        {
            GameMode.Instance.RetryLevel();

        });
 
    }
    [Button]
    public void SetGameOver()
    {
        GameMode.Instance.SetGameOver();
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
        //level?.SetText("Level "+GameInstance.Instance.playerLevel.ToString());

        gameOverPanel.gameObject.SetActive(true);
        LeanTween.alpha(grayBackground, .5f, .2f);

        scoreTxt.SetText(GameMode.Instance.score+" pts");  
        bestScoreTxt.SetText(GameInstance.Instance.GetBestScore() + " pts");
        LeanTween.move(gameOverPanel, showPosition,showTweenTime);


        yield return null;
    }

}
