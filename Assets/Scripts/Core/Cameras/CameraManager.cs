using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
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

    private void OnEnable()
    {
         GameFeel.OnCameraShake += GameFeel_OnCameraShake;
    }

    private void Awake()
    {
        transform.position = finalPosition;
    }
    private void Start()
    {
        main = Camera.main;

        transform.position = finalPosition;

    }
    private void GameFeel_OnCameraShake()
    {
       // StartCoroutine(Shake());

    }
    IEnumerator Shake()
    {
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
            print(elapsed);

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
        if(MustFollow)
        FindPlayer(_player);
    }

    private void OnDisable()
    {
         GameFeel.OnCameraShake -= GameFeel_OnCameraShake;

    }
    // Use this for initialization

    void FindPlayer(Pawn _player)
    {

             GetComponent<CinemachineVirtualCamera>().Follow = _player.transform;
       GetComponent<CinemachineVirtualCamera>().LookAt = _player.transform;

       
    }
}
