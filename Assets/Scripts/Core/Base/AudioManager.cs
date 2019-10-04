 
 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(AudioSource))]
public class AudioManager : Singleton<AudioManager> {

    AudioSource audioSrc;

     public AudioData audioData;


    private void OnEnable()
    {
        
    }


    private void OnDisable()
    {
        
    }
    private void OnGUI()
    {
        
    }
    protected override void Awake()
    {
        base.Awake();

        audioSrc = GetComponent<AudioSource>();

    }
    private void Start()
    {
        audioData = AudioData.Instance;

    }

    public void PlaySound(SFX _sfx)
    {
 
    }
    public void PlaySound(AudioClip _soundClip)
    {
        
         
        audioSrc.PlayOneShot(_soundClip);

    }
  
   
    void Play(AudioClip _clip,bool _bRandomPitch=false)
    {
        audioSrc.pitch = _bRandomPitch?Random.Range(.9f,1.1f):1f;
        audioSrc.PlayOneShot(_clip);
    }

    public void PlayWinMusic()
    {

        Play(audioData.winMusic);

    }
    public void PlayLoseSfx()
    {

        Play(audioData.gameOver);

    }

    public void PlayGoalSound()
    {

        Play(audioData.goalCompleted);

    }

    
    public void PlayPlayerMove()
    {
        Play(audioData.playerDash);

    }
    public void PlayColumnsReposition()
    {
        Play(audioData.columnReposition);
    }
    public void PlayColumnHit()
    {
        Play(audioData.columnHit);
    }
}
