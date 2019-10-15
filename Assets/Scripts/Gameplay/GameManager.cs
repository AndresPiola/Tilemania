using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public Sprite[] targetIcons;

    public GameObject targetScoreHolder;
    public UITarget target;
    private Rigidbody2D targetHolderRb;
    public SpriteRenderer targetIconRenderer;
    public TextMeshPro targetScoreText;
    [Header("TargetValues")]
    public int targetScore;
    public int targetIcon;
    private bool bTargetCompleted;

    public int comboCount;
    public static event FNotify OnTargetScoreReady;
    public static event FNotify_1Params<int> OnCombo;
    

    private void OnDisable()
    {
        GameMode.OnGameState -= GameMode_OnGameState;
    }

    private void OnEnable()
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
                ResetGM();
                break;
            case EGameStates.ROUND_OVER:
                break;
            case EGameStates.GAME_OVER:
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        targetHolderRb = targetScoreHolder.GetComponent<Rigidbody2D>();
        GenerateRandomTarget();
    }

    void ResetGM()
    {
        bTargetCompleted=false;
    }
    void ResetTargetScoreHolder()
    {
        target.ResetBackground();

        targetHolderRb.bodyType = RigidbodyType2D.Static;
        targetHolderRb.velocity = Vector2.zero;
        targetHolderRb.angularVelocity = 0;
        targetScoreHolder.transform.position = Vector3.zero;
        targetScoreHolder.transform.rotation = Quaternion.identity;

        targetScoreHolder.transform.localScale = Vector3.zero;
        LeanTween.scale(targetScoreHolder, Vector3.one, .2f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.move(targetScoreHolder, Vector2.up *4, 1.4f)
            .setEase(LeanTweenType.easeOutElastic).setOnComplete(() =>
            {
                target.HideBackground();
            });
        
       
    }
    void GenerateRandomTarget()
    {
        ResetTargetScoreHolder();
        
       targetScore = Random.Range(4, 5);
        targetScoreText.SetText(targetScore.ToString());
        targetIcon = Random.Range(0, targetIcons.Length);
        targetIconRenderer.sprite = targetIcons[targetIcon];


    }

    public void AddComboCount(int SumValue = 1)
    {
        comboCount += 1;
        if(comboCount>1)
            OnCombo?.Invoke(comboCount);

        Debug.Log("combocount"+comboCount);

    }

    public void ResetComboCount()
    {
        comboCount = 0;
    }
    public bool CheckTargetCompleted(int TestIcon, int TestValue)
    {
        if (TestIcon != targetIcon) return false;
        if ( TestValue< targetScore) return false;
        bTargetCompleted = true;
        AudioManager.Instance.PlaySound(ESfx.TARGET_COMPLETED);
        OnTargetScoreReady?.Invoke();
        targetHolderRb.bodyType = RigidbodyType2D.Dynamic;
        targetHolderRb.AddTorque(256);
        targetHolderRb.AddForce(new Vector2(Random.Range(-1f,1f),1)*4,ForceMode2D.Impulse);
        LeanTween.delayedCall(5,() => { targetHolderRb.bodyType = RigidbodyType2D.Static; });
        return true;
 
    }

    public void CheckRoundResults()
    {
        if (bTargetCompleted)
        {
            int score = 0;
            List<Tile> finalTiles = DropArea.Instance.activeTiles;
            for (int i = 0; i < finalTiles.Count; i++)
            {
                score += finalTiles[i].blockScore;
            }

            GameMode.Instance.AddScore(score);
            GameMode.Instance.SetRoundOver();
            AudioManager.Instance.PlaySound(ESfx.ROUND_OVER);
            StartCoroutine(WaitForNextRound());
        }
        else
        {
            GameMode.Instance.SetGameOver();
        }
    }

    IEnumerator WaitForNextRound()
    {
        yield return Utils.GetWaitForSeconds(3);
        GenerateRandomTarget();

        GameMode.Instance.ChangeGameState(EGameStates.GAMEPLAY);
    }

     
}
