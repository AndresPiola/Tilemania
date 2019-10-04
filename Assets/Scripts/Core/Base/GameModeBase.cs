using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
//using GameAnalyticsSDK;

public enum EGameStates { MAIN_MENU, CONNECTING, RELOADING_ROUND, LOADING_NEXTROUND, LOADING_REMATCH, GAMEPLAY, ROUND_OVER, GAME_OVER }

public class GameModeBase<T> : MonoBehaviour where T : MonoBehaviour
{
     
    public static T Instance { get; private set; }


    public EGameStates gameState { get { return _gameState; } }
    public EGameStates _gameState;

    public Pawn player;

    public int score = 0;
    public int level = 0;
    public float progress = 0;

     public static event FNotify  OnUpdate, OnReturnHome; 
      public static event FNotify_1Params<int > OnScoreChange,OnLevelLoaded,OnLevelReset;
    public static event FNotify_2Params<int,int> OnScoreUpdate;

    public static event FNotify_1Params<EGameStates> OnGameState;

  
    public static event FNotify_1Params< float> OnProgressChanged;
     public static event FNotify_3Params<int,int,int> OnXpCount;
    public static event FNotify_1Params<int> OnTimerChange;
    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("A instance already exists");
            Destroy(this); //Or GameObject as appropriate
            return;
        }
        Instance = this as T;

 
    }
   
    protected virtual void Start()
    {
          
    }
    
    /*
    protected virtual void Update()
    {
        if (OnUpdate != null)
            OnUpdate();
    }
    */
    public virtual void BeginSession()
    {

        if (gameState == EGameStates.GAMEPLAY) return;

        ChangeGameState(EGameStates.GAMEPLAY, true);
      //  GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "game");
    }

    public virtual void SetGameOver()
    {
        
       // GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "game", score);
        ChangeGameState(EGameStates.GAME_OVER, true);
    }

    public virtual void SetRoundOver()
    {
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "game", score);

        ChangeGameState(EGameStates.ROUND_OVER,true);
 

    }
    public virtual bool SaveScore(int _score )
    {
        bool bBestScore = GameInstance.Instance.CheckIsHighScore(_score);

        int _bestScore = GameInstance.Instance.SaveScore(score);

        OnScoreUpdate?.Invoke(score, _bestScore);

        GameInstance.Instance.SavePlayerData();
        return bBestScore;

    }
    public virtual void LevelCompleted(bool _bLoadNextLevel=false)
    {
        ChangeGameState(EGameStates.ROUND_OVER, true);
        GameInstance.Instance.LevelUp();
       
        if(_bLoadNextLevel)        
        StartCoroutine(WaitForNextLevel());

    }
    public virtual void TryAgain()
    {
        
        StartCoroutine(WaitForNextLevel(0));

    }

    public virtual void RetryLevel()
    {
        ChangeGameState(EGameStates.LOADING_REMATCH, true);
        StartCoroutine(WaitForNextLevel(0));

    }


    public virtual IEnumerator WaitForNextLevel(float waitTime=1f)
    {
        yield return Utils.GetWaitForSeconds(waitTime);
        LoadNextlevel();
    }
    public virtual int AddScore(int _value = 1)
    {
        score += _value;

        OnScoreChange?.Invoke(score);

        return score; 
    }


    public virtual int GetScore(out int _bestScore )
    {
        _bestScore = GameInstance.Instance.GetBestScore(score);

        return score;
    }

    public virtual int GetScore()
    {
        

        return score;
    }
    public virtual void ChangeProgress(float _progress)
    {
        progress = _progress;
        OnProgressChanged?.Invoke(_progress);

    }
    public virtual float GetAdvancePercent()
    {
        float progress=0;

        return (float)progress;


    }
    public virtual void UpdateProgress(float _progress)
    {
        OnProgressChanged?.Invoke(_progress);

    }
    public void ReturnHome()
    {


        OnReturnHome?.Invoke();

        SceneManager.LoadSceneAsync(1);
    }

    
    public void LoadNextlevel()
    {
 
        SceneManager.LoadSceneAsync(1);
    }

    public void ChangeGameState(EGameStates _newGameState,bool bCallEvent=false)
    {
        _gameState = _newGameState;

        if (bCallEvent)
            CallGameStateEvent(_gameState);
    }

    public void CallGameStateEvent(EGameStates _gamestate)
    {

        if (OnGameState != null)
            OnGameState(_gamestate);

    }

}
