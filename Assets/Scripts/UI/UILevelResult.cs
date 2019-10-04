using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UILevelResult : MonoBehaviour {

    public TextMeshProUGUI levelText;
    public Image xpBar;
    private void OnEnable()
    {
        GameMode.OnXpCount += GameMode_OnXpCount;
    }

   

    private void OnDisable()
    {
        GameMode.OnXpCount -= GameMode_OnXpCount;

    }

    private void Start()
    {
 

    }
    private void GameMode_OnXpCount(int xp, int nextXp, int _level)
    {
        float percent =(float) xp / nextXp;
        string tmpLevel=xp+" / "+nextXp;

        levelText.SetText(tmpLevel);
         
        LTSeq seq = LeanTween.sequence();

        seq.append(LeanTween.value(0, percent, 1).setOnUpdate((float _val) =>
       {
           if(xpBar)
           xpBar.fillAmount = _val;

       }));

    }
}
