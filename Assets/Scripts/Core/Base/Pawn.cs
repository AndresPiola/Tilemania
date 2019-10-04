using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour {

     
    protected int playerIndex;

    [Header("Custom Physics")]
    public LayerMask floorLayer;
    public LayerMask targetLayer;

    //////////////////////////////////////////////////////////////////////////
    //multiplayer
    protected string playerName;
    protected int rankPos;
    public int rankPosition { get { return rankPos; } set { rankPos = value; } }

    protected int score;

    public float moveSpeed = 10;
    public bool bCanMove;
    protected bool bIsHuman;
    public bool bCanPlay; 

    protected float percentProgress;
    public float percent { get { return percentProgress; } }

    protected virtual void Awake()
    {
       
    }

    public virtual void OnEnable()
    {
        GameMode.OnGameState += GameMode_OnGameState;
    }

    public virtual void GameMode_OnGameState(EGameStates _newGameState)
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
                bCanPlay = true;
                bCanMove = true;
                break;
            case EGameStates.ROUND_OVER:
                bCanPlay = false;
                break;
            case EGameStates.GAME_OVER:
                bCanPlay = false;
                break;
             
        }
    }

    public virtual void OnDisable()
    {
        GameMode.OnGameState -= GameMode_OnGameState;

    }
    public virtual void Start()
    {
        
       
        InitializeData();
    }

    public virtual void InitializeData()
    {
        
       
    }
     
    public string GetOnlineName()
    {
        return playerName;

    }
    protected virtual  float GetDistanceTo(Vector3 _target)
    {
        return (transform.position - _target).magnitude;

    }
    
    public virtual bool ApplyDamage(int _damage)
    {
        return false;

    }
    public virtual bool ApplyDamage(float _damage)
    {
        return false;

    }

    
    public virtual void ForceMoveTo(Vector3 _targetPosition,float _time, LeanTweenType _ease= LeanTweenType.easeInCubic)
    {
        bCanMove = false;
        LeanTween.move(gameObject, _targetPosition, _time).
            setEase(_ease).
            setOnComplete(() =>
         {
             bCanMove = true;

         });

    }
   
     
}
