using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "GameInstance", menuName = "InnerChild/GameInstance SO", order = 1)]
 
public class GameInstance : SingletonScriptableObject<GameInstance> {

 
    public SaveGame saveGameReference;
    public SaveGame savegame { get { return saveGameReference; } }


    //level managment
    public bool bSkipMainMenu;
    public int currentevel=1;
    public int maxLevel = 15;
    public bool bMaxLevelReached { get { return saveGameReference.level >= maxLevel ; } }
    /// <summary>
    /// Player Data
    /// </summary>
    public string playerName { get { return savegame.name; } }
    public int playerLevel { get { return savegame.level; } }
    public int playerXp { get { return savegame.xp; } }
    public int money { get { return savegame.currency[0]; } set { savegame.currency[0] = value; } }
    public int diamond { get { return savegame.currency[1]; } set { savegame.currency[1] = value; } }

    public int playerScore;
    public int hiScore;
    public bool bHideMainMenu;
    public int[] roundCurrency = new int[5];
     
    public delegate void FNotifyMoney(int _newValue, int _type);
    public static event FNotifyMoney OnMoneyChanged;

    public delegate void FNotifyInt(int _newValue);
    public static event FNotifyInt OnUpgradeComplete;

    /* public SO_Ads ads;
     public SO_LeaderBoard leaderBoard;
      
     */

    #region GAME SPECIFIC

    public FWorldLevel GetSelectedLevel()
    { 
        return saveGameReference.selectedLevel;

    }
  
    public FWorldLevel SetSelectedLevel(int _world,int _level)
    {

        for (int i = 0; i < saveGameReference.levels.Count; i++)
        {
            if(saveGameReference.levels[i].CompareLevel(_world,_level))
            {
                saveGameReference.selectedLevel= saveGameReference.levels[i];

            }

        }
        return saveGameReference.selectedLevel;

    }
    public void SetLevelCompleted(FWorldLevel _completedLevel)
    {

        for (int i = 0; i < saveGameReference.levels.Count; i++)
        {
            if (saveGameReference.levels[i].world == _completedLevel.world && saveGameReference.levels[i].level == _completedLevel.level)
            { 
                saveGameReference.levels[i].completed = true;
                break;
            }

        }

        TryUnlockNextLevel(_completedLevel.world, _completedLevel.level);


    }


    public void  TryUnlockNextLevel(int _world,int _level )
    {

        int nextLevel = _level + 1;

        if(!CheckThereIsMoreLevels(  _world, nextLevel))
        {
            TryUnlockNextWorld(_world);
            return;

        }
        
        for (int i = 0; i < saveGameReference.levels.Count; i++)
        {
            ///if level exists forget about unlock
            if (saveGameReference.levels[i].CompareLevel(_world, nextLevel))
            {
 
                SetSelectedLevel(_world, nextLevel);
                return;

            }

        }

        ///if we are here the world still have levels and they are new 
        FWorldLevel newLevel = new FWorldLevel(_world, nextLevel);
   
        saveGameReference.levels.Insert(_world, newLevel);
        saveGameReference.selectedLevel = newLevel;

    }
    /// <summary>
    /// If returns false the world iss completed
    /// </summary>
    /// <returns></returns>
    bool CheckThereIsMoreLevels(int _world, int _level)
    { 

        return false;

    }

    void TryUnlockNextWorld(int currentWorld)
    {

    }

  
    #endregion

    public void Initialize()
    {


        LoadPlayerData();

        //  SO_Progression.Instance.Initialize(savegame);
        bHideMainMenu = false;
        playerScore = 0;
        hiScore = savegame.score;
        currentevel = 1;
         
    }

    void OnLevelWasLoaded(int level)
    {
        roundCurrency = new int[5];

    }
    
        


    public void LoadPlayerData()
    {
        saveGameReference = DataAccess.Load();
 
    }

    public void SavePlayerData()
    {
        DataAccess.Save(saveGameReference);

    }

    public void ClearSavegame()
    {
        DataAccess.Format();
    }
     
    public void AddXp(int _xp)
    {

        savegame.xp += _xp;

    }

    public void AddRoundMoney(int _ammount, int _currencyType=0)
    {
        roundCurrency[_currencyType] += _ammount;
        OnMoneyChanged?.Invoke(savegame.currency[_currencyType]+ roundCurrency[_currencyType], _currencyType);

    }

    public void MoveRoundCurrencyToSave()
    {
        for (int i = 0; i < roundCurrency.Length; i++)
        {
            AddMoney(roundCurrency[i], i);
            
        }

        roundCurrency = new int[5];

    }
    public void AddMoney(int _ammount,int _currencyType=0)
    {
 
        savegame.currency[_currencyType] += _ammount;


        OnMoneyChanged?.Invoke(savegame.currency[_currencyType], _currencyType);


    }

    public int GetMoney(int _type)
    {
        return savegame.currency[_type];
    }

    public void ResetTempMoney()
    {
        for (int i = 0; i < roundCurrency.Length; i++)
        {
            roundCurrency[i] = 0;
        }
    }
    public void LevelUp()
    {
        if(saveGameReference.level<maxLevel)
        saveGameReference.level++;
        

    }

    public int SaveScore(int _totalScore, bool bReplaceTopScore = false)
    {
        if (bReplaceTopScore)
            saveGameReference.score = _totalScore;

        int prevScore = saveGameReference.score;
        if (_totalScore > prevScore)
        {
            saveGameReference.score = _totalScore;


        }
        hiScore = savegame.score;
        /*
         leaderBoard.AddScore(_totalScore);
         */
        SavePlayerData();

        return prevScore;

    }

    public bool CheckIsHighScore(int _scoreToCheck)
    {
        return _scoreToCheck >= saveGameReference.score;

    }

    public int GetBestScore(int _currentScore=0)
    {
        return Mathf.Max(_currentScore, saveGameReference.score);

    }



    
    public void SavePlayerName(string _newName)
    {
        
        savegame.name = _newName;
        SavePlayerData();

    }

     

    

     
}

public class DataAccess
{

    public const int DEFAULT_PROGRESS_LENGTH = 3;
    public const int DEFAULT_PROGRESS_MIN_LEVEL = 1;


    public static void Save(SaveGame _saveGame)
    {

        string dataPath = string.Format("{0}/" + Application.productName + "_savegame.dat", Application.persistentDataPath);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream;

        try
        {

            if (File.Exists(dataPath))
            {
                File.WriteAllText(dataPath, string.Empty);
                fileStream = File.Open(dataPath, FileMode.Open);
                Debug.Log("File.Exists(dataPath): "+ dataPath.ToString());
            }
            else
            {
                fileStream = File.Create(dataPath);
                Debug.Log("File.Exists(dataPath)False : " + dataPath.ToString());

            }

            binaryFormatter.Serialize(fileStream, _saveGame);
            fileStream.Close();

            
        }
        catch (Exception e)
        {
            PlatformSafeMessage("Failed to Save: " + e.Message);
        }
    }

     
    public static SaveGame Format()
    {
        SaveGame saveGame = null;
        string dataPath = string.Format("{0}/" + Application.productName + "_savegame.dat", Application.persistentDataPath);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        File.Delete(dataPath);


        return saveGame;
    }
    public static SaveGame Load()
    {
        SaveGame saveGame = null;
        string dataPath = string.Format("{0}/" + Application.productName + "_savegame.dat", Application.persistentDataPath);


        try
        {



            if (File.Exists(dataPath))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = File.Open(dataPath, FileMode.Open);

                saveGame = (SaveGame)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();

#if UNITY_EDITOR
                Debug.Log("SaveGame exists");

#endif

            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("SaveGame Not found. Creating one in ");

#endif
  
                saveGame = new SaveGame();
                Save(saveGame);
            }
        }
        catch (Exception e)
        {
            PlatformSafeMessage("Failed to Load: " + e.Message);
        }

        return saveGame;
    }

    private static void PlatformSafeMessage(string message)
    {
         
    }


}
