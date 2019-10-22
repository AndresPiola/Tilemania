using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : GameModeBase<GameMode>
{
    private bool bIsLoading;


    public static event FNotify_1Params<bool> OnLevelLoading;
    
    void OnDisable()
    {
        UIMenuButton.OnButtonPress -= UIMenuButton_OnButtonPress;
    }

    private void OnEnable()
    {
        UIMenuButton.OnButtonPress += UIMenuButton_OnButtonPress;
    }


    private void UIMenuButton_OnButtonPress(EButtonActions Param1)
    {
      //  if(bIsLoading)return;
     
        string levelName="";

        switch (Param1)
        {
            case EButtonActions.MainGame:
                levelName = "Main";
                ; break;
            case EButtonActions.Tutorial:
                levelName = "MainTutorial";
                ; break;
            case EButtonActions.MainMenu:
                levelName = "MainMenu";
                ; break;

        }

        bIsLoading = true;
        OnLevelLoading?.Invoke(true);
        SceneManager.LoadSceneAsync(levelName);


    }

}
