using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : Singleton<CameraFollow> {

    public Transform target;

    Transform pivot;
    Transform player;
    public float smoothing = 5f;
    public float smoothTime = 0.3F;
  
    public bool bCanFollow;

    public Vector3 endLocation;
    public Vector3 endRotation;

    public Vector3 offSet;

    Transform transformRef;
    Vector3 targetPosition;

    [Header("Camera Speed Axis")]
    public Vector3 axisSpeed;

    private Vector3 velocity = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();
        transformRef = transform;
        targetPosition = transform.position;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        pivot = GetComponentInParent<Transform>();
        target = player;

    }
    private void OnEnable()
    {
        GameMode.OnGameState += GameMode_OnGameState;
        GameMode.OnUpdate += GameMode_OnUpdate;
        GateWayComponent.OnGatewayCross += GateWayComponent_OnGatewayCross;
 
    }

    private void GateWayComponent_OnGatewayCross(GateWayComponent _instance)
    {

        axisSpeed.x = 1;
        offSet.y = 0;
       

    }

    void ZoomTo()
    {
      
        LeanTween.value(GetComponent<Camera>().orthographicSize, 0, 1).setOnUpdate((float _val) =>
        {
            GetComponent<Camera>().orthographicSize = _val;

        });


    }

    private void PlayerPawn_OnPlayerHit(Vector3 _pos, Color32 _color, int _playerIndex, float _dmg = 1)
    {
        CameraShake(1);
 

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
                bCanFollow = true;
                break;
            case EGameStates.ROUND_OVER:

                LeanTween.delayedCall(1, () => {
                    bCanFollow = false;
                });
            
                break;
            case EGameStates.GAME_OVER:
                CameraShake(11);
                bCanFollow = false;
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        GameMode.OnGameState -= GameMode_OnGameState;

        GameMode.OnUpdate -= GameMode_OnUpdate;
        GateWayComponent.OnGatewayCross -= GateWayComponent_OnGatewayCross;

    }



    private void GameMode_OnUpdate()
    {
        FollowPlayer();

    }

   
   

    public void CameraShake(float _height=11)
    {
        /**************
                    * Camera Shake
                    **************/

        float shakeAmt = _height * 0.2f; // the degrees to shake the camera
        float shakePeriodTime = 0.42f; // The period of each shake
        float dropOffTime = 1.6f; // How long it takes the shaking to settle down to nothing
        LTDescr shakeTween = LeanTween.rotateAroundLocal(gameObject, Vector3.right, shakeAmt, shakePeriodTime)
        .setEase(LeanTweenType.easeShake) // this is a special ease that is good for shaking
        .setLoopClamp()
        .setRepeat(-1);

        // Slow the camera shake down to zero
        LeanTween.value(gameObject, shakeAmt, 0f, dropOffTime).setOnUpdate(
            (float val) => {
                shakeTween.setTo(Vector3.right * val);
            }
        ).setEase(LeanTweenType.easeOutQuad);

    }
    void FollowPlayer()
    {
        
        if (!bCanFollow ) return;
        if (targetPosition == null) return;



        //         if (targetPosition.y < transform.position.y)
        //             return;
        

        targetPosition.x = transform.position.x* axisSpeed.x;
        targetPosition.y = (target.position.y) * axisSpeed.y;
        targetPosition.z = -10;
        targetPosition += offSet;

       
        transformRef.position = Vector3.SmoothDamp(transformRef.position, targetPosition, ref velocity, smoothTime)   ;
       // pivot.position = player.position;
        //pivot.rotation = player.rotation;
    }


    private void Update()
    {

        FollowPlayer();

    }
}
