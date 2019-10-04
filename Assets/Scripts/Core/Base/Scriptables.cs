using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scriptables : MonoBehaviour {



    [Header("Scriptable Objects")]
    public GameInstance gameInstanceSO;
    public AudioData audioDataSO;
    public SpawnData spawnData;
    public GameConfig gameConfig; 

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
         

    }
    private void OnEnable()
    {
        if(gameInstanceSO)
        gameInstanceSO.Initialize();


    }
}
