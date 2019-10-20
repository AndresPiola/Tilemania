using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : Singleton<MainMenuManager>
{

    void OnDisable()
    {
        UIMenuButton.OnButtonPress -= UIMenuButton_OnButtonPress; 
    }

    void OnEnable()
    {
        UIMenuButton.OnButtonPress += UIMenuButton_OnButtonPress;
    }

    private void UIMenuButton_OnButtonPress(EButtonActions Param1)
    {
        switch (Param1)
        {
            case EButtonActions.MainGame:
                SceneManager.LoadScene("Main");

                break;
            case EButtonActions.Tutorial:
                SceneManager.LoadScene("MainTutorial");

                break;
        }
    }

    public void LoadMainGame()
    {
        SceneManager.LoadScene("Main");

    }
}
