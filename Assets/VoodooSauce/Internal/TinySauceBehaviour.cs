using System;
using GameAnalyticsSDK;
using UnityEngine;

namespace Voodoo.Sauce.Internal
{
    internal class TinySauceBehaviour : MonoBehaviour

    {
        private const string TenjinApiKey = "E3OTCZ3UQ1OUEWUD3W9SO8JT6REPCZ5U";
        private const string Tag = "TinySauceBehaviour";
        private static TinySauceBehaviour _instance;
        [SerializeField] private GameAnalytics _gameAnalyticsPrefab;
        private void Awake()
        {
            
            if (transform != transform.root)
                throw new Exception("TinySauce prefab HAS to be at the ROOT level!");
            
            VoodooLog.Initialize(VoodooLogLevel.WARNING);
            
            if (_instance != null)
            {
                VoodooLog.LogW(Tag,"TinySauce is already initialized! Please avoid creating multiple instance of TinySauce. This object will be destroyed: " + gameObject.name );
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(this);

            // init TinySauce sdk
            InitGameAnalytics();
            InitTenjin();
        }

        private static void InitTenjin()
        {
            Tenjin.getInstance(TenjinApiKey).Connect();
        }
        
        private void InitGameAnalytics()
        {
            var gameAnalyticsInstance = FindObjectOfType<GameAnalytics>();
            if (gameAnalyticsInstance == null)
            {
                gameAnalyticsInstance = Instantiate(_gameAnalyticsPrefab);
                gameAnalyticsInstance.gameObject.SetActive(true);
            }
            GameAnalytics.Initialize();
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            // Brought forward after soft closing 
            if (!pauseStatus)
            {
                InitTenjin();
            }
        }
    }
}
