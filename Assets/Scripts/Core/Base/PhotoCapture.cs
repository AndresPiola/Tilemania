using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoCapture : MonoBehaviour
{
#if UNITY_EDITOR
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public string title = "Screenshot";
	void Update ()
    {
        if (Input.GetButtonDown("Jump"))
        {
            ScreenCapture.CaptureScreenshot(title + "." + System.DateTime.Now.Day.ToString() + "." + System.DateTime.Now.Hour.ToString() + "." + System.DateTime.Now.Minute.ToString() + "." + System.DateTime.Now.Second.ToString() + ".png");
        }
    }
#endif

}