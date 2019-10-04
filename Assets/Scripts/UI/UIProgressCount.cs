using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressCount : MonoBehaviour
{
    public UIProgressCountFill prefab;
    public float sizeScale = 1f;
    UIProgressCountFill[] counters;


    private void OnEnable()
    {

 
        GameMode.OnGameState += GameMode_OnGameState;
      

    }

    private void GameMode_OnGameState(EGameStates _newGameState)
    {
        switch (_newGameState)
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
                break;
            case EGameStates.ROUND_OVER:
                break;
            case EGameStates.GAME_OVER:
                break;
            default:
                break;
        }
    }

    void CleanBombMeter()
    {

    }
    private void GameManager_OnUpdateBombCount(int _value)
    {
         
        counters[_value-1]?.SetFill();

    }

    private void OnDisable()
    {

 
         GameMode.OnGameState  -= GameMode_OnGameState;


    }

    private void GameManager_OnFindBomb(int _value)
    {
         

        InitializeSize(_value);

    }

    void InitializeSize(int _nChilds )
    {
         
        for (int i=0;i< counters?.Length; i++)
        {
           Destroy(counters[i].gameObject);

        }
        float width = GetComponent<RectTransform>().sizeDelta.x;
        float cellWidth = (width / _nChilds)*sizeScale;
        GetComponent<GridLayoutGroup>().spacing = new Vector2(10 / sizeScale, 10 / sizeScale);

      GetComponent<GridLayoutGroup>().cellSize = new Vector2(cellWidth, cellWidth);
        GameObject obj = null;
        
        counters = new UIProgressCountFill[_nChilds];

        for (int i = 0; i < _nChilds; i++)
        {

            obj = Instantiate(prefab.gameObject);
            obj.transform.SetParent(this.transform);
            obj.SetActive(true);
            counters[i] = obj.GetComponent<UIProgressCountFill>();
            counters[i].Initialize(i);

        }
    }

    
}
