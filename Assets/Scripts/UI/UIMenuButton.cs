using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public  enum EButtonActions { MainMenu,MainGame}
public class UIMenuButton : SerializedMonoBehaviour
{
    public EButtonActions buttonAction;

    public static  event  FNotify_1Params<EButtonActions> OnButtonPress;

    private void Awake()
    {
        
        GetComponent<Button>().onClick.AddListener(() =>
        {
            OnButtonPress?.Invoke(buttonAction);
        });
    }
}
