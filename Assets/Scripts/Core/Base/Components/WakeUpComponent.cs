using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public enum EActivationType { ON_SCREEN,DISTANCE}
public class WakeUpComponent : MonoBehaviour {

    public EActivationType type;

    public delegate void FWakeUp(Transform _instigator=null);
    public FWakeUp OnWakeUp;

    public Transform target;
    public float activationDistance = 2f;
    public bool ShowArea;
    bool bActivated;

    Camera cameraRef;

   

    void OnDrawGizmosSelected()
    {
        if (!ShowArea) return;

        Gizmos.DrawWireSphere(transform.position, activationDistance);


    }
    private void Start()
    {
        cameraRef = Camera.main;
        if (target == null)
            target = GameObject.FindWithTag("Player").transform;

    }
    private void OnEnable()
    {
        switch(type)
        {
            case EActivationType.DISTANCE:
                GameMode.OnUpdate += GameMode_OnUpdate;

                break;

            case EActivationType.ON_SCREEN:
                GameMode.OnUpdate += GameMode_OnUpdate;

                break;

        }
      
        bActivated = false;

    }


    private void OnDisable()
    {
        switch (type)
        {
            case EActivationType.DISTANCE:
                GameMode.OnUpdate -= GameMode_OnUpdate;

                break;

            case EActivationType.ON_SCREEN:
                GameMode.OnUpdate -= GameMode_OnUpdate;

                break;
        }

    }

    void Activate()
    {
        if (bActivated) return;
        bActivated = true;
        GameMode.OnUpdate -= GameMode_OnUpdate;
        OnWakeUp(target);
       // gameObject.SetActive(false);

    }
    void CheckPlayerDistance()
    {
        if (target == null) return;

        if ((target.position - transform.position).magnitude < activationDistance)
        {

            Activate();
        }
    }

    void CheckScreen()
    {

        if (type != EActivationType.ON_SCREEN) return;
        if (cameraRef == null) return;

       Vector3 screenPoint= cameraRef.WorldToScreenPoint(transform.position);
        screenPoint.x = screenPoint.x / Screen.width;
        screenPoint.y = screenPoint.y / Screen.height;

        if(screenPoint.x>0 && screenPoint.x<1 && screenPoint.y>0 && screenPoint.y<1)
        {
            Activate();


        }

      

    }


    private void GameMode_OnUpdate()
    {
        if (bActivated) return;
        switch (type)
        {
            case EActivationType.DISTANCE:
                CheckPlayerDistance();


                break;

            case EActivationType.ON_SCREEN:

                CheckScreen();

                break;
        }
       
    }


}
