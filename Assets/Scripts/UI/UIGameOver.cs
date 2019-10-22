using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
 

public class UIGameOver : SerializedMonoBehaviour
{

    public RectTransform gameOverPanel;
    public Button retryButton;
 
    public int waitTime = 1000;
    public RectTransform gameOverRibbon;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI bestScoreTxt;
    public RectTransform grayBackground; 

    [Header("ShowParams")]
    public Vector3 showPosition;
    public float showTweenTime;
    public LeanTweenType showEaseType;
    public RectTransform[] scorePanel;
    public RectTransform titlePanel;

    public RectTransform buttonsPanel;

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
                ShowGameOver() ;

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

    async Task ShowGameOver()
    {

        Debug.Log("ShowGameOver");
        //level?.SetText("Level "+GameInstance.Instance.playerLevel.ToString());
        await Task.Delay(waitTime);


        gameOverPanel.gameObject.SetActive(true);
        LeanTween.alpha(grayBackground, .5f, .2f);

        scoreTxt.SetText(GameMode.Instance.score + " pts");
        bestScoreTxt.SetText(GameInstance.Instance.GetBestScore() + " pts");
        LeanTween.move(gameOverPanel, showPosition, showTweenTime);
        LeanTween.moveY(titlePanel, 0, .5f);
         
        LeanTween.moveX(scorePanel[0], 0, .5f);
        await Task.Delay(200);
        LeanTween.moveX(scorePanel[1], 0, .5f);
        await Task.Delay(5);

        LeanTween.moveY(buttonsPanel, 0, .5f);


    }


}
