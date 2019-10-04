using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivot : MonoBehaviour {
    Transform player;
    public float followSpeed = 15;
    private Vector3 velocity = Vector3.zero;
    Vector3 targetPosition;
    Vector3 targetRotation;

    bool bMustFollow;
    // Use this for initialization
    void Start () {
   
        bMustFollow = true;
    }


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
            case EGameStates.LOADING_REMATCH:
                break;
            case EGameStates.LOADING_NEXTROUND:
                break;
            case EGameStates.GAMEPLAY:
                break;
            case EGameStates.ROUND_OVER:

                OnRoundOver();
                break;
            case EGameStates.GAME_OVER:
                break;
            default:
                break;
        }
    }

    void OnRoundOver()
    {
        bMustFollow = false;

        LeanTween.moveY(gameObject, transform.position.y - 100, 5);

    }
    private void OnDisable()
    {
        GameMode.OnGameState -= GameMode_OnGameState;

    }
    void FollowPlayer()
    {
        if (!bMustFollow) return;
        if(player==null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            return;

        }
        targetPosition = player.position;

        targetPosition.z = transform.position.z;
        
        transform.position =  Vector3.SmoothDamp(transform.position, targetPosition,ref velocity, Time.fixedDeltaTime*followSpeed);
        Vector3 dir = player.position - transform.position;


        
    }
    // Update is called once per frame
    void LateUpdate () {
        FollowPlayer();
    }
}
