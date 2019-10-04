using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelManager", menuName = "InnerChild/LevelManager ", order = 1)]

public class LevelManager : SingletonScriptableObject<LevelManager>
{
    public int baseXp;
    [Range(0,10)]
    public float progressionRate;



    public int GetXpCostForLevel(int _level)
    {
        int cost = Mathf.RoundToInt(  baseXp*progressionRate * _level);
        return  cost ;

    }

    public bool CheckLevelUP(int currentLevel,int xp)
    {
        bool bCanLevelup = false; ;
        int nextXp=GetXpCostForLevel(currentLevel + 1);

        if (xp >= nextXp)
            bCanLevelup = true;

        return bCanLevelup;

    }
}
