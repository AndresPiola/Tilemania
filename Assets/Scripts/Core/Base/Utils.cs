using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Timer
{
    float currentTime;
    float time;

    public delegate void FTimer_V();


    public FTimer_V OnTimerCompleted;

    public Timer(float _duration, FTimer_V _callBackFunction)
    {
        time = _duration;

        OnTimerCompleted += _callBackFunction;

    }


    public void UpdateTimer(float _deltaTime)
    {
        currentTime -= _deltaTime;

        if (currentTime <= 0)
        {
            if (OnTimerCompleted != null)
                OnTimerCompleted();

            currentTime = time;


        }
    }

}