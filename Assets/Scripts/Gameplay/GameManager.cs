﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public Sprite[] targetIcons;

    public GameObject targetScoreHolder;
    private Rigidbody2D targetHolderRb;
    public SpriteRenderer targetIconRenderer;
    public TextMeshPro targetScoreText;
    [Header("TargetValues")]
    public int targetScore;
    public int targetIcon;
    private bool bTargetCompleted;

    public static event FNotify OnTargetScoreReady;

    private void Start()
    {
        targetHolderRb = targetScoreHolder.GetComponent<Rigidbody2D>();
        GenerateRandomTarget();
    }

    void ResetTargetScoreHolder()
    {
        targetHolderRb.bodyType = RigidbodyType2D.Static;
        targetHolderRb.velocity = Vector2.zero;
        targetHolderRb.angularVelocity = 0;
        targetScoreHolder.transform.position = Vector3.zero;
        targetScoreHolder.transform.rotation = Quaternion.identity;

        targetScoreHolder.transform.localScale = Vector3.zero;
        LeanTween.scale(targetScoreHolder, Vector3.one, .2f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.move(targetScoreHolder, Vector2.up *4, 1.4f).setEase(LeanTweenType.easeOutElastic);
        
       
    }
    void GenerateRandomTarget()
    {
        ResetTargetScoreHolder();
        
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