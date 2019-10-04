using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GateWayComponent : MonoBehaviour {

    public delegate void FNotify(GateWayComponent _instance);
    public static event FNotify OnGatewayCross;
    public bool bCanStart;

    private void OnEnable()
    {
        bCanStart = GameInstance.Instance.bHideMainMenu;

        GameMode.OnGameState += GameMode_OnGameState;
    }

   

    private void OnDisable()
    {
        GameMode.OnGameState -= GameMode_OnGameState;

    }
    private void GameMode_OnGameState(EGameStates _newGameState)
    {
        switch (_newGameState)
        {
            case EGameStates.MAIN_MENU:
                break;
            case EGameStates.CONNECTING:
                break;
            
            case EGameStates.LOADING_NEXTROUND:
                break;
            case EGameStates.GAMEPLAY:
                if(bCanStart)
                {
                    GetComponent<AudioSource>().Play();
                    LeanTween.value(gameObject, 1, 0, 2f).setOnUpdate((float _val) =>
                    {
                        print(_val);

                        GetComponent<AudioSource>().volume = _val;

                         

                    });
                }
                break;
            case EGameStates.ROUND_OVER:
                break;
            case EGameStates.GAME_OVER:
                break;
            default:
                break;
        }
    }
    public void Activate()
    {
        if (OnGatewayCross != null)
            OnGatewayCross(this);
        GetComponent<AudioSource>().volume = 1;

        GetComponent<AudioSource>().Play();

        LeanTween.moveY(gameObject, transform.position.y-10, .5f);

    }
}
