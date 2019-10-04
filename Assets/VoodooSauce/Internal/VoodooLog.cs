using System;
using UnityEngine;

namespace Voodoo.Sauce.Internal
{
    public static class VoodooLog
    {
        private static VoodooLogLevel _logLevel;
        private const string TAG = "VoodooSauce";

        public const string LOGS_DISPLAY_SETTING = "LogsDisplaySetting";
        public const string ENABLE_LOGS = "EnableLogs";
        public const string DISABLE_LOGS = "DisableLogs";

        public static void Initialize(VoodooLogLevel level)
        {
            _logLevel = level;
            
            if(Debug.isDebugBuild) 
            {
                Log(TAG, "Debug.isDebugBuild = true, always enable logs"); 
                EnableLogs(); 
            }
            else
            {
                if (PlayerPrefs.HasKey(LOGS_DISPLAY_SETTING) &&
                    PlayerPrefs.GetString(LOGS_DISPLAY_SETTING) == ENABLE_LOGS)
                {
                    Log(TAG, "PlayerPrefs = Logs Enabled, do GDPR secret to disable"); 
                    EnableLogs();
                }
                else
                {
                    Log(TAG, "PlayerPrefs = Logs Disabled, do GDPR secret to enable");
                    DisableLogs();
                }
            }
        }

        public static void Log(string tag, string message)
        {
            if (!Application.isEditor || _logLevel >= VoodooLogLevel.DEBUG)
                Debug.Log(Format(tag, message));
        }

        public static void LogE(string tag, string message)
        {
            if (!Application.isEditor || _logLevel >= VoodooLogLevel.ERROR)
                Debug.LogError(Format(tag, message));
        }

        public static void LogW(string tag, string message)
        {
            if (!Application.isEditor || _logLevel >= VoodooLogLevel.WARNING)
                Debug.LogWarning(Format(tag, message));
        }
        
        private static string Format(string tag, string message)
        {
            return string.Format("{0} - {1}/{2}: {3}", DateTime.Now, TAG, tag, message);
        }

        // Separate Log enable/disable separate from VoodooLogLevel 
        public static void DisableLogs()
        {
            Log(TAG, "Disabling Logs"); 
            Debug.unityLogger.logEnabled = false;
            PlayerPrefs.SetString(LOGS_DISPLAY_SETTING, DISABLE_LOGS); 
        }

        public static void EnableLogs()
        {
            Debug.unityLogger.logEnabled = true;
            Log(TAG, "Enabling Logs");
            PlayerPrefs.SetString(LOGS_DISPLAY_SETTING, ENABLE_LOGS); 
        }

        public static bool IsLogsEnabled()
        {
            return Debug.unityLogger.logEnabled; 
        }
    }

    public enum VoodooLogLevel
    {
        ERROR=0,
        WARNING=1,
        DEBUG=2
    }
}