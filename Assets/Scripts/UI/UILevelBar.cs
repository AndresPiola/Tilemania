using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UILevelBar : MonoBehaviour {

    public TextMeshProUGUI level;
    public TextMeshProUGUI nextlevel;
  public Image fillbar;

    int currentLevel=-1;

    private void OnDisable()
    {
        GameMode.OnLevelLoaded -= GameMode_OnLevelLoaded;

        GameMode.OnGameState -= GameMode_OnGameState;
       
        GameMode.OnLevelReset -= GameMode_OnLevelReset;
        GameMode.OnProgressChanged -= GameMode_OnProgressChanged;

    }

    private void OnEnable()
    {
        GameMode.OnProgressChanged += GameMode_OnProgressChanged;
         GameMode.OnGameState += GameMode_OnGameState;
        GameMode.OnLevelLoaded += GameMode_OnLevelLoaded;
        GameMode.OnLevelReset += GameMode_OnLevelReset;

        
    }

    private void GameMode_OnProgressChanged(float _progress)
    {
 
        fillbar.fillAmount = _progress;

    }

    private void GameMode_OnLevelReset(int _val)
    { 
    }

    private void GameMode_OnLevelLoaded(int _val)
    {
         
        UpdateLevel(GameInstance.Instance.playerLevel);
    }

    private void GameMode_OnGameState(EGameStates _newGameState)
    { 
        int lvl = GameInstance.Instance.playerLevel;

        switch (_newGameState)
        {
            case EGameStates.MAIN_MENU:
                
                break;
            case EGameStates.CONNECTING:
                break;
            case EGameStates.LOADING_REMATCH:
                UpdateLevel(lvl);
                break;
            case EGameStates.LOADING_NEXTROUND:
                
                break;
            case EGameStates.GAMEPLAY:
               
                

                break;
            case EGameStates.ROUND_OVER:
                 break;
            case EGameStates.GAME_OVER:
                break;
            
        }
    }

    

  

    private void Start()
    {
        UpdateLevel(GameInstance.Instance.playerLevel);
    }

     
    private void GameMode_OnLevelProgressChanged(int _level, float _progress)
    {
         
 

    }
    private void GameMode_OnXpChanged(int _level, float _percent)
    {
        

    }

    void UpdateLevel(int lvl)
    {

        if (level == null)
            print(gameObject);

        level.SetText(lvl.ToString());

        //nextlevel.SetText((lvl + 1).ToString());
     }   
}
