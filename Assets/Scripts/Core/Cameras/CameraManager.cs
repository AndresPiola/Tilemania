using System;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraManager : MonoBehaviour {

    Camera main;
    public bool MustFollow = true;
    public AnimationCurve ShakeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    public float Duration = 2;
    public float Speed = 22;
    public float Magnitude = 1;
    public float DistanceForce = 100;
    public float RotationDamper = 2;
    public bool IsEnabled = true;
    public Vector3 finalPosition = new Vector3(0,30,-23);
    [Header("move out path")] public Vector3[] outroWaypoints;

    private void OnDisable()
    {
        GameFeel.OnCameraShake -= GameFeel_OnCameraShake;
        GameMode.OnGameState -= GameMode_OnGameState;

    }

    private void OnEnable()
    {
         GameFeel.OnCameraShake += GameFeel_OnCameraShake;
        GameMode.OnGameState += GameMode_OnGameState;
    }

    private void GameMode_OnGameState(EGameStates _val1)
    {
        LeanTween.cancel(gameObject);
        switch (_val1)
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
                LeanTween.moveY(gameObject, 0, .5f).setEase(LeanTweenType.linear);

                break;
            case EGameStates.ROUND_OVER:
                LeanTween.moveY(gameObject, -8, 1.5f).setEase(LeanTweenType.easeInOutBack);

                break;
            case EGameStates.GAME_OVER:
              LeanTween.moveY(gameObject, 1, .5f).setEase(LeanTweenType.linear);
             //  GameFeel_OnCameraShake();
                break;
            }
    }

    private void Awake()
    { 
    }
    private void Start()
    {
      
    }
    private void GameFeel_OnCameraShake()
    {
      // StartCoroutine(Shake());

    }
    IEnumerator Shake()
    {
       yield return null;

        var elapsed = 0.0f;
        var camT = main.transform;
        var originalCamRotation = camT.rotation.eulerAngles;
        var direction = (transform.position - camT.position).normalized;
        var time = 0f;
        var randomStart = Random.Range(-1000.0f, 1000.0f);
        var distanceDamper = 1 - Mathf.Clamp01((camT.position - transform.position).magnitude / DistanceForce);
        Vector3 oldRotation = Vector3.zero;
        while (elapsed < Duration)
        {
           
            elapsed += Time.deltaTime;
            var percentComplete = elapsed / Duration;
            var damper = ShakeCurve.Evaluate(percentComplete) * distanceDamper;
            time += Time.deltaTime * damper;
            //camT.position -= direction * Time.deltaTime * Mathf.Sin(time * Speed) * damper * Magnitude / 2;

            var alpha = randomStart + Speed * percentComplete / 10;
            var x = Mathf.PerlinNoise(alpha, 0.0f) * 2.0f - 1.0f;
            var y = Mathf.PerlinNoise(1000 + alpha, alpha + 1000) * 2.0f - 1.0f;
            var z = Mathf.PerlinNoise(0.0f, alpha) * 2.0f - 1.0f;

            if (Quaternion.Euler(originalCamRotation + oldRotation) != camT.rotation)
                originalCamRotation = camT.rotation.eulerAngles;
            oldRotation = Mathf.Sin(time * Speed) * damper * Magnitude * new Vector3(0.5f + y, 0.3f + x, 0.3f + z) * RotationDamper;
            camT.rotation = Quaternion.Euler(originalCamRotation + oldRotation);

            yield return null;
        }
    }
    private void PlayerPawn_OnPlayerReady(Pawn _player)
    {
       
    }

   
    
}
