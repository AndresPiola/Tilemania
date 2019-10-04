using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UINameInput : MonoBehaviour {

    TMP_InputField nameInput;
     
    private void Awake()
    {
        nameInput = GetComponent<TMP_InputField>();
    }
    private void OnEnable()
    {
        GameMode.OnGameState += GameMode_OnGameState; ;
    }
    private void OnDisable()
    {
        GameMode.OnGameState -= GameMode_OnGameState;
    }
    private void GameMode_OnGameState(EGameStates _newGameState)
    {
        switch(_newGameState)
        {
            case EGameStates.GAMEPLAY:
                nameInput.enabled = false;
                OnsavePlayerName();

                LeanTween.scale(gameObject, Vector3.zero, .25f).setOnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
                 
                break;

            case EGameStates.LOADING_NEXTROUND:
                gameObject.SetActive(false);
                break;

            case EGameStates.LOADING_REMATCH:
                gameObject.SetActive(false);
                break;

        }
    }

    
    private void Start()
    {
       
        nameInput.text = GameInstance.Instance.playerName;
        if (nameInput.text == "") nameInput.text = "Player 1";

    }

    
    public void OnsavePlayerName()
    {
        
         GameInstance.Instance.SavePlayerName(nameInput.text);
        gameObject.SetActive(false);
    }
}
