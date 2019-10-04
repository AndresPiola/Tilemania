using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class UIButtonRetry : MonoBehaviour
{
    Button retryButton;


    private void Awake()
    {
        retryButton = GetComponent<Button>();

    }
    private void OnDisable()
    {
        retryButton?.onClick.RemoveAllListeners();
    }

    private void OnEnable()
    {
        retryButton?.onClick.AddListener(ClickButton);

    }
    
    public virtual void ClickButton()
    {
        GameMode.Instance.RetryLevel();

    }

}
