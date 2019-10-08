
 using System.Collections;
using System.Collections.Generic;
 using Sirenix.OdinInspector;
 using UnityEngine;
 

public enum SFX { BALLOON_POP} 

[CreateAssetMenu(fileName = "AudioData", menuName = "InnerChild/AudioData SO", order = 1)]
public class AudioData : SingletonScriptableObject<AudioData>
{

    [Header("Gameplay")]
    [PreviewField]
    public AudioClip comboBonus;
    public AudioClip putTileInArea;
    public AudioClip tileReturn;

    [Header("Gameplay Events")]

    public AudioClip goalCompleted;
    public AudioClip gameOver;
    [Header("items")]
    public AudioClip[] itemSounds ;

    
    [Header("Music")]
    public AudioClip winMusic;
    public AudioClip firstPositionWinMusic;
    public AudioClip loseMusic;


    public AudioClip GetRandClip(AudioClip[] _clipArray)
    {
        return _clipArray[ Random.Range(0, _clipArray.Length)];

    }
}