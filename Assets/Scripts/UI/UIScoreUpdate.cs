using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIScoreUpdate : MonoBehaviour {

    TextMeshProUGUI score;


    private void Awake()
    {
        score = GetComponent<TextMeshProUGUI>();

    }

    private void OnEnable()
    {
        GameMode.OnScoreChange += GameMode_OnScoreUpdate;
    }

   

    private void OnDisable()
    {
        GameMode.OnScoreChange -= GameMode_OnScoreUpdate;
    }

    private void GameMode_OnScoreUpdate(int _score )
    {
        score.SetText(_score.ToString());

    }
}
