using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// using GameAnalyticsSDK;
using Facebook.Unity;

public class Preloader : MonoBehaviour {

    public bool debugMode = false;


    public delegate void FNotifyGame();
    public static event FNotifyGame OnPreloadBegin;


   
    // Use this for initialization
    void Start () {
   // GameAnalytics.Initialize();
        Application.targetFrameRate = 300;
        FacebookInit();
        if (!debugMode)
        SceneManager.LoadSceneAsync(1);
 
	}
	
	void FacebookInit()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Handle FB.Init
            FB.Init(() =>
            {
                FB.ActivateApp();
            });
        }
    }
}
