using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIRoundOver : MonoBehaviour
{

    public RectTransform panel;

    public float showOverMessageTime = .2f;
    public TextMeshProUGUI overMessage;
    public TextMeshProUGUI score;
    public TextMeshProUGUI bestScore;
    public Button nextLevelButton;


    //public TextMeshProUGUI best;
    public float waitTime = 1;
    private void Awake()
    {
        panel.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        GameMode.OnGameState += GameMode_OnGameState;
 
        nextLevelButton?.onClick.AddListener(
            () =>
            {
                GameMode.Instance.LoadNextlevel();
            }
            
            );
    }

   

    private void OnDisable()
    {
        GameMode.OnGameState -= GameMode_OnGameState;
         nextLevelButton?.onClick.RemoveAllListeners();

    }
    private void GameMode_OnGameState(EGameStates _newGameState)
    {


        switch (_newGameState)
        {

            case EGameStates.GAMEPLAY:
                StartCoroutine(HidePanel());
                break; 
            case EGameStates.LOADING_NEXTROUND:

                StartCoroutine(HidePanel());
                break;

            case EGameStates.ROUND_OVER:

                     StartCoroutine(ShowPanel());

                break;
            case EGameStates.GAME_OVER:

                StartCoroutine(HidePanel());
                break;
        }

    }
    
    IEnumerator HidePanel()
    { 
        yield return null;
        LeanTween.moveX(panel, -2000, .2f).setOnComplete(() =>
        {
            panel.gameObject.SetActive(false);
            Vector2 resetPos = panel.anchoredPosition;
            resetPos.x = 1500;
            panel.anchoredPosition= resetPos;
        });
       

    }
    IEnumerator ShowPanel()
    {
        //  panel.anchoredPosition += Vector2.up * 1000;
       // overMessage?.GetComponent<RectTransform>().localScale = Vector3.zero;

         
        yield return Utils.GetWaitForSeconds(waitTime);
        panel.gameObject.SetActive(true);
       int hiScore  ;
        int score = GameMode.Instance.GetScore(out hiScore);

        float percent = GameMode.Instance.GetAdvancePercent();
        bestScore?.SetText(hiScore.ToString());

         panel.gameObject.SetActive(true);
         LeanTween.moveX(panel, 0, .2f);
        yield return Utils.GetWaitForSeconds(.1f);
       // LeanTween.scale(overMessage.GetComponent<RectTransform>(), Vector3.one, showOverMessageTime).setEaseOutBounce();



    }

}
