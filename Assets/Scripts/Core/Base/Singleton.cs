using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Singleton<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour
{
    public static T Instance { get; private set; }
    public bool dontdestroyOnLoad;

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("A instance already exists");
            Destroy(this); //Or GameObject as appropriate
            return;
        }
        Instance = this as T;
        if (dontdestroyOnLoad)
            DontDestroyOnLoad(gameObject);

 
    }

}
