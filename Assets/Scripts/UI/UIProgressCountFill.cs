using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressCountFill : MonoBehaviour
{

    public int index;
    public Color32 fillColor;



    public void Initialize(int _index)
    {
        index = _index;

    }

    public void SetFill()
    {
        GetComponent<RectTransform>().localScale = Vector3.one*0.4f;
        LeanTween.scale(GetComponent<RectTransform>(), Vector3.one, .3f).setEaseOutBounce();
        GetComponent<Image>().color = fillColor;

    }
}
