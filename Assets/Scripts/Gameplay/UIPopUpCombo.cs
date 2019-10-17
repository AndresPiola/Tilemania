using System.Collections;
using System.Collections.Generic;
using BezierSolution;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIPopUpCombo : PopUpText
{ 
    public LeanTweenType ease;
    public float tweenTime;
    [ReadOnly]
    public List<Vector3> path;

    [Button]
    public void SetupPath()
    {
        BezierSpline spline = GetComponent<BezierSpline>();
        path = new List<Vector3> { Vector3.zero };

        for (int i = 0; i < spline.Count; i++)
        {
            path.Add(spline[i].localPosition);

        }

        path.Add(Vector3.zero);

    }

    [Button]
    public void Test()
    {
        ShowPopUp( 2);
        gameObject.SetActive(true); 
    }

    public override void Deactivate()
    {

        text.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ShowPopUp(int ComboMultiplier)
    {
        text.gameObject.SetActive(true);
        text.SetText("combo x"+ComboMultiplier);
        text.transform.localScale = Vector3.zero;

        
        LTSpline spline = new LTSpline(path.ToArray());
        LeanTween.moveLocal(text.gameObject , spline, tweenTime)
            .setEase(ease)
            .setOnComplete(Deactivate);

        LeanTween.scale(text.gameObject, Vector3.one, .2f).setEaseInBounce();


    }
}
