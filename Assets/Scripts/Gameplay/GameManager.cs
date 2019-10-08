using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public Sprite[] targetIcons;

    public GameObject targetScoreHolder;
    public SpriteRenderer targetIconRenderer;
    public TextMeshPro targetScoreText;
    [Header("TargetValues")]
    public int targetScore;
    public int targetIcon;
    private bool bTargetCompleted;

    public static event FNotify OnTargetScoreReady;

    private void Start()
    {
        GenerateRandomTarget();
    }


    void GenerateRandomTarget()
    {
        targetScore = Random.Range(4, 10);
        targetScoreText.SetText(targetScore.ToString());
        targetIcon = Random.Range(0, targetIcons.Length);
        targetIconRenderer.sprite = targetIcons[targetIcon];


    }

    public bool CheckTargetCompleted(int TestIcon, int TestValue)
    {
        if (TestIcon != targetIcon) return false;
        if ( TestValue< targetScore) return false;
        bTargetCompleted = true;
        OnTargetScoreReady?.Invoke();
        targetScoreHolder.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        targetScoreHolder.GetComponent<Rigidbody2D>().AddTorque(256);
        targetScoreHolder.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f,1f),1)*4,ForceMode2D.Impulse);
         
        return true;
 
    }

    public void CheckRoundResults()
    {
        if(bTargetCompleted)GameMode.Instance.SetRoundOver();
        else
        {
            GameMode.Instance.SetGameOver();
        }
    }
}
