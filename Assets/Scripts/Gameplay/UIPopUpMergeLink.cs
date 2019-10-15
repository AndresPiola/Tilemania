using System;
using System.Collections;
using System.Collections.Generic;
using BezierSolution;
using Sirenix.OdinInspector;
using UnityEngine;


public class UIPopUpMergeLink : PopUpText
{
    public GameObject[] messageObject;
    public LeanTweenType ease;
    public float tweenTime;
    [ReadOnly]
    public List<Vector3> path;

    [Button]
    public void SetupPath()
    {
        BezierSpline spline = GetComponent<BezierSpline>();
        path = new List<Vector3>{Vector3.zero};

        for (int i = 0; i < spline.Count; i++)
        {
             path.Add(spline[i].localPosition);  

        }

        path.Add(Vector3.zero);

    }

    [Button]
    public void Test()
    {
        gameObject.SetActive(true);
        ShowPopUp(ECombinationType.MERGE);
    }
    
    public override void Deactivate()
    {
        for (int i = 0; i < messageObject.Length; i++)
        {
            messageObject[i].SetActive(false);
            messageObject[i].transform.localPosition = Vector3.zero;
            messageObject[i].transform.localScale = Vector3.one;

        }

        gameObject.SetActive(false);
    }

    public void ShowPopUp(ECombinationType CombinationType)
    {
        if (CombinationType == ECombinationType.NONE)
        {
            Deactivate();
            return; 
        }

        int indexToShow = CombinationType == ECombinationType.MERGE ? 0 : 1;


        messageObject[indexToShow].gameObject.SetActive(true);
        LTSpline spline=new LTSpline(path.ToArray());
        LeanTween.moveLocal(messageObject[indexToShow].gameObject, spline, tweenTime)
            .setEase(ease)
            .setOnComplete(Deactivate);



    }
}
