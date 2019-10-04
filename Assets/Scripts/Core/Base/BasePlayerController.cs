using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 
public enum EDirections {NONE, UP,DOWN,LEFT,RIGHT}
public enum EInputType { TAP,JOYSTICK,SCREEN_SIDES,ACCELEROMETER}

public struct FTouchInfo 
{
    public Vector3 touchWorldPoint;
    public Vector2 touchScreenCoordinate;
    public Vector2 endTouchLocation  ;
    public Vector2 deltaRelease ;
    public Vector2 touchScreenRatio;

}
public class BasePlayerController : Singleton<BasePlayerController> {

    [Header("Default Input")]
    public EInputType inputType;
    public Vector3 position { get { return transform.position; } }

    protected Transform transformRef;
    /// <summary>
    /// touch detections vars
    /// </summary>
    ///   
    FTouchInfo touchInfo;
    protected Camera cameraRef { get { return Camera.main; } }

    protected Vector3 touchWorldPoint;
    protected Vector2 touchScreenCoordinate;
    protected Vector2 touchScreenRatio;
    protected Vector2 startTouchLocation;
    protected Vector2 endTouchLocation;
    public Vector2 deltaRelease;
    protected Vector3 targetShootDirection;
    protected Ray rayTouchTest;
    protected Plane plane;

    

    [Header("Stick and buttons")]
    // public Joystick joystick;
  //  public Button button;

    private Vector2 _axisValue;
    public Vector2 axisValue { get { return _axisValue; } }


  
    public static event FNotify  OnDragEnd;
    
    public static event FNotify_1Params<FTouchInfo> OnTouchBegin,  OnTouchRelease, OnTouchHold;
 
    public static event FNotify_1Params<float> OnPlayerMove;

    public delegate void PlayerControllerNotify_V(EDirections _direction);
    public static event PlayerControllerNotify_V OnSidePress;

    public delegate void PlayerControllerNotify_I(int _Button);
    public static event PlayerControllerNotify_I OnButtonPressed,OnButtonRelease;

     

    // Use this for initialization
    public virtual void Start () {
        //joystick = GameObject.FindObjectOfType<Joystick>();


    }


    protected override void Awake()
    {
        base.Awake();

        transformRef = transform;
        _axisValue = Vector2.zero;

         
    }
    protected virtual void OnEnable()
    {
        

 
    }

    protected virtual void OnDisable()
    {
 
    }
    protected virtual void GameMode_OnGameOver()
    {
        
    }
    
    public virtual void  TouchBegin(Vector2 _touchScreenLocation)
    {
        touchWorldPoint = cameraRef.ScreenToWorldPoint(_touchScreenLocation);
        touchScreenCoordinate = _touchScreenLocation;
        startTouchLocation = endTouchLocation = _touchScreenLocation;
        _axisValue.x = 0;
        _axisValue.y = 0;
        touchInfo.touchWorldPoint = touchWorldPoint;
        touchInfo.touchScreenCoordinate = touchScreenCoordinate;
        touchInfo.endTouchLocation = endTouchLocation;
        touchInfo.deltaRelease = deltaRelease;
        touchInfo.touchScreenRatio = touchScreenRatio;
        OnTouchBegin?.Invoke(touchInfo);

    }
    public virtual void TouchHold(Vector2 _touchScreenLocation)
    {
        touchWorldPoint = cameraRef.ScreenToWorldPoint(_touchScreenLocation);
        touchScreenCoordinate = _touchScreenLocation;
        endTouchLocation = _touchScreenLocation;
        deltaRelease = endTouchLocation - startTouchLocation;

        touchScreenRatio.x = _touchScreenLocation.x / Screen.width;
        touchScreenRatio.y = _touchScreenLocation.y / Screen.height;

     
        touchInfo.touchWorldPoint = touchWorldPoint;
        touchInfo.touchScreenCoordinate = touchScreenCoordinate;
        touchInfo.endTouchLocation = endTouchLocation;
        touchInfo.deltaRelease = deltaRelease;
        touchInfo.touchScreenRatio = touchScreenRatio;


        OnTouchHold?.Invoke(touchInfo);
    }
    public virtual void  TouchRelease(Vector2 _touchScreenLocation)
    {
        touchWorldPoint = cameraRef.ScreenToWorldPoint(_touchScreenLocation);

        touchScreenCoordinate = _touchScreenLocation;
        endTouchLocation = _touchScreenLocation;
        deltaRelease = endTouchLocation - startTouchLocation;

        _axisValue.x = 0;
        _axisValue.y = 0;

      
        touchInfo.touchWorldPoint = touchWorldPoint;
        touchInfo.touchScreenCoordinate = touchScreenCoordinate;
        touchInfo.endTouchLocation = endTouchLocation;
        touchInfo.deltaRelease = deltaRelease;


        OnTouchRelease?.Invoke(touchInfo);

        if (touchScreenRatio.x< .5f)
            SidePress(EDirections.LEFT);
        if (touchScreenRatio.x > .5f)
            SidePress(EDirections.RIGHT);


    }

    public virtual void ButtonDown(int _buttonIndex)
    {

        if (OnButtonPressed != null)
            OnButtonPressed(_buttonIndex);

    }

    public virtual void ButtonUp(int _buttonIndex)
    {

        if (OnButtonRelease != null)
            OnButtonRelease(_buttonIndex);

    }
    public virtual Vector3 ScreenToWorldPosition(Vector2 _screenPos)
    {
       Vector3 point = cameraRef.ScreenToWorldPoint(new Vector3(_screenPos.x, _screenPos.y, cameraRef.nearClipPlane));

        return point;

    }
    public virtual void SidePress(EDirections _direction)
    {
        if(OnSidePress!=null)
         OnSidePress(_direction);

     

    }

   
    protected virtual void ProcessTouch()
    {


#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            TouchBegin(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            TouchHold(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            TouchRelease(Input.mousePosition);
        }
#endif
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                TouchBegin(Input.GetTouch(0).position);
            }

            if (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                TouchHold(Input.GetTouch(0).position);

            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                TouchRelease(Input.GetTouch(0).position);

            }
        }
    }
   
    protected virtual void Update()
    {

        if (cameraRef == null) return;

        ProcessTouch();

        switch (inputType)
        {
            case EInputType.TAP:
                break;
            case EInputType.JOYSTICK:


                
                break;
            case EInputType.SCREEN_SIDES:

#if UNITY_EDITOR
                if (Input.GetKey(KeyCode.A) && OnSidePress!=null)
                    OnSidePress(EDirections.LEFT);

                if (Input.GetKey(KeyCode.D ) && OnSidePress != null)
                    OnSidePress(EDirections.RIGHT);

                if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
                {
                    OnSidePress?.Invoke(EDirections.NONE);
                }


#endif
                    break;
            case EInputType.ACCELEROMETER:
                break;
            default:
                break;
        }

    }
  
}
