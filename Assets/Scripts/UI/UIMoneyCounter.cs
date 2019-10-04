using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
 
[RequireComponent(typeof( TextMeshProUGUI))]
public class UIMoneyCounter : MonoBehaviour {

    public int moneyType = 0;
    TextMeshProUGUI moneyText;
    public RectTransform icon;
    private void Awake()
    {
        moneyText = GetComponent<TextMeshProUGUI>();

    }
    private void OnEnable()
    {
        moneyText.SetText(GameInstance.Instance.GetMoney(moneyType).ToString());

        GameInstance.OnMoneyChanged += GameInstance_OnMoneyChanged;
    }
    private void OnDisable()
    {
        
        GameInstance.OnMoneyChanged -= GameInstance_OnMoneyChanged;
    }

    
    private void GameInstance_OnMoneyChanged(int _newValue, int _type)
    {
        if (moneyType != _type) return;
        moneyText.SetText(_newValue.ToString());


        if(icon)
        {
            LeanTween.scale(icon, Vector2.one * 1.5f, .2f)
                .setEaseOutBounce()
                .setLoopPingPong(1);

        }
    }
}
