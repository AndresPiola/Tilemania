using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FWorldLevel
{
    public int world;
    public int level;
    public bool unlocked;
    public bool completed;
    public int rating;



    public FWorldLevel(int _world, int _level,bool _unlocked=true,bool _completed=false,int _rating=0)
    {
        world = _world;
        level = _level;
        unlocked = _unlocked;
        completed = _completed;
         rating = _rating;
    }

    public bool CompareLevel(int _world,int _level)
    {
        return (world == _world && level == _level);

    }
}
[System.Serializable]
public class SaveGame
{
    public string name;
    public int score;
    public int level=1;
    public int xp;

    public int[] currency   = new int[5];
    public List<FWorldLevel>  levels = new List<FWorldLevel>();
    public FWorldLevel selectedLevel;
    


    public SaveGame()
    {
        score = 0; 
        currency = new int[5];
        levels = new List<FWorldLevel>();
        selectedLevel = new FWorldLevel(0, 0); 
        levels.Add(selectedLevel); 
        
        name ="Player1"   ;
        level = 1;

        
    }
    public SaveGame(int progressSize=5,int minLevel=1)
    {
        currency = new int[5];

        score  = 0; 
        
        level = 1;

    }
    public void SetScore(int _newScore)
    { 
        score = _newScore; 
    }
     
     
}
