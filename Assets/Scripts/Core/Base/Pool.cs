using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
    public class Pool<T> : MonoBehaviour where T : MonoBehaviour
    {

    public static T Instance { get; private set; }
 
    public GameObject objectPrefab;

    public int poolSize = 10;

    public HideFlags hideFlag = HideFlags.HideInHierarchy;
    List<GameObject> pool = new List<GameObject>();

    GameObject tmpObj;
    protected virtual void Awake()
    {
            if (Instance != null)
            {
                Debug.LogError("A instance already exists");
                Destroy(this); //Or GameObject as appropriate
                return;
            }
        Instance = this as T;

        Initialize();

    }

     
    public void Initialize()
    {
        tmpObj = null;

        for (int i = 0; i < poolSize; i++)
        {
            tmpObj = Instantiate(objectPrefab, transform.position, Quaternion.identity);
            tmpObj.SetActive(false);
            tmpObj.hideFlags = hideFlag;

            pool.Add(tmpObj);

        }
    }

    public GameObject GetPooledObject(Vector2 _spawnPosition)
    {
        tmpObj = GetPooledObject();
        tmpObj.transform.position = _spawnPosition;
        return tmpObj;

    }

    public GameObject GetPooledObject(Vector2 _spawnPosition,Quaternion _rotation)
    {
        tmpObj = GetPooledObject();
        tmpObj.transform.position = _spawnPosition;
        tmpObj.transform.rotation = _rotation;

        return tmpObj;

    }

    public T GetPooledObjectComponent<T>()
    {
        
        return (T)GetPooledObject().GetComponent<T>();

    }
    public T GetPooledObjectComponent<T>(Vector3 Position)
    {

        return (T)GetPooledObject(Position).GetComponent<T>();

    }
    public GameObject GetPooledObject()
    {
        tmpObj = null;

        for (int i = 0; i < pool.Count; i++)
        {

            if (!pool[i].activeInHierarchy)
            {
                tmpObj = pool[i];

                tmpObj.SetActive(true);
                return tmpObj;

            }

        }
        //if we are here most obj are busy. we must expand pool list

        tmpObj = Instantiate(objectPrefab, transform.position, Quaternion.identity);
        tmpObj.SetActive(true);
        tmpObj.hideFlags = hideFlag;

        pool.Add(tmpObj);

        return tmpObj;

    }

    public GameObject GetPooledObject(Vector3 _spawnPosition)
    {
        tmpObj = GetPooledObject();
        tmpObj.transform.position = _spawnPosition;

        return tmpObj;


    }

    public GameObject GetPooledObject(Vector3 _spawnPosition, Quaternion _rot)
    {
        tmpObj = GetPooledObject();
        tmpObj.transform.position = _spawnPosition;
        tmpObj.transform.rotation = _rot;

        return tmpObj;


    }
}
 
