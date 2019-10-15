using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UITarget : SerializedMonoBehaviour
{

    public GameObject background;

    public void ResetBackground()
    {
        background.transform.localScale = Vector3.one;

    }
    public void HideBackground()
    {
        LeanTween.scale(background, Vector3.zero, .2f);

    }
}
