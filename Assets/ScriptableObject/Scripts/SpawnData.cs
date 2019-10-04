
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [CreateAssetMenu(fileName = "SpawnData", menuName = "InnerChild/SpawnData", order = 1)]
public class SpawnData : SingletonScriptableObject<SpawnData>
{

    public List<EnemyData> enemies= new List<EnemyData>();

 
   public EnemyData GetObject(int _id)
    {
 
        for (int i=0;i<enemies.Count;i++)
        {
            if (enemies[i].id == _id)
                return enemies[i];


        }

        return null;
    }

}
[System.Serializable]
public class EnemyData
{
    public int id;
    public string name;
    public GameObject prefab;

    public int hp;
    public AudioClip criticalSfx;
     
    public EnemyData()
    {

    }

    public EnemyData(EnemyData _data)
    {
        id = _data.id;
      
    }
    

}

 

 