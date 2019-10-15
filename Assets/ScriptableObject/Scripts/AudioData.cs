
 using System.Collections;
using System.Collections.Generic;
 using Sirenix.OdinInspector;
 using UnityEngine;


 public enum ESfx
 {
     TILE_PICK,
     TILE_DROP, 
     TILE_RETURN,
     COMBO_MERGE,
     COMBO_SWAP,
     TARGET_COMPLETED,


     ROUND_OVER
 };


[CreateAssetMenu(fileName = "AudioData", menuName = "InnerChild/AudioData SO", order = 1)]
public class AudioData : SingletonScriptableObject<AudioData>
{
    
    public Dictionary<ESfx, AudioClip> gameClips = new Dictionary<ESfx, AudioClip>();


    [Header("Gameplay")]
     
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

    public AudioClip GetAudioClip(ESfx Key)
    {
        return gameClips[Key];

    }

     
    public AudioClip GetRandClip(AudioClip[] _clipArray)
    {
        return _clipArray[ Random.Range(0, _clipArray.Length)];

    }
}