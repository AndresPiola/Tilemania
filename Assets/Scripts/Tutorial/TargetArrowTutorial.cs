using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetArrowTutorial : MonoBehaviour
{
    void OnEnable()
    {
       // StartAnimationArrow();
    }

    public void StartAnimationArrow()
    {
        LeanTween.moveLocalY(gameObject, .5f, 1.0f).setLoopPingPong();

    }
}
