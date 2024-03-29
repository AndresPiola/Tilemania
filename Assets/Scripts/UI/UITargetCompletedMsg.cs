﻿using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using  UnityEngine.UI;
public class UITargetCompletedMsg : SerializedMonoBehaviour
{
    public RectTransform panel;

    void OnDisable()
    {
        GameManager.OnTargetScoreReady -= GameManager_OnTargetScoreReady;

    }

    void OnEnable()
    {
        GameManager.OnTargetScoreReady += GameManager_OnTargetScoreReady;
    }

    private void GameManager_OnTargetScoreReady()
    {
        LTSeq seq = LeanTween.sequence();


        seq.append(LeanTween.scale(panel, Vector3.one, .5f).setEase(LeanTweenType.easeInOutBounce));
        seq.append(1);
        seq.append(LeanTween.scale(panel, Vector3.zero, .5f).setEase(LeanTweenType.easeInCubic));

    }
}
