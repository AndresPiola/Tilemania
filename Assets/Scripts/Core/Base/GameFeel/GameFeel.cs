
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class GameFeel:MonoBehaviour
{
       GameObject owner;

   
    public GameObject[] particles;

    public bool bAddTrail;

    public AudioClip[] sfx;
    public bool UseCameraShake;

      public CinemachineImpulseSource cameraShake;

    private TrailRenderer trail;


    public delegate void FCameraShake();
    public static event FCameraShake OnCameraShake;

    

    public GameFeel()
    {

    }


    public void SpawnParticle(int index =0,Vector3 _spawnLocation= default(Vector3))
    {
        if (particles.Length == 0) return;

        Instantiate(particles[index], _spawnLocation, Quaternion.identity);
         
    }


    //this is for custom editor only
    public void AddorRemoveCameraShake(bool _add)
    {

        if(_add)
        {
            if (GetComponent<CinemachineImpulseSource>() == null)
                gameObject.AddComponent<CinemachineImpulseSource>();

        }else
        {
            if (GetComponent<CinemachineImpulseSource>() != null)
                DestroyImmediate(GetComponent<CinemachineImpulseSource>());

        }

    }
    public void CameraShake()
    {
    
        if (cameraShake == null) return;
         
        cameraShake.GenerateImpulse();



    }


    public void SetCameraShake()
    {
        if(UseCameraShake &&  cameraShake==null)
        {
            cameraShake = gameObject.AddComponent<CinemachineImpulseSource>();

        }
        if (!UseCameraShake && cameraShake != null)
        {

            Object.DestroyImmediate(cameraShake);
             
        }
    }
 
    void SetTrail()
    {
        trail = owner.GetComponent<TrailRenderer>();

        if (trail==null)
        trail=owner.AddComponent<TrailRenderer>(); 
    }

    public void PlaySfx(int _index)
    {
        if (sfx.Length == 0) return;

        AudioManager.Instance.PlaySound(sfx?[_index]);

    }
}
     
 