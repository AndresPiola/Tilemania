using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : Singleton<MainMenuManager>
{
    public TextMeshProUGUI bestScore;

    void OnDisable()
    {
      }

    void OnEnable()
    {
     }

    private void Start()
    {
        bestScore.SetText(GameInstance.Instance.GetBestScore().ToString());
    }
}
