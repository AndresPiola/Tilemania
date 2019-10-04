using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UILevelBegin : MonoBehaviour {

    public TextMeshProUGUI level;

    public RectTransform StartPanel;


    private void Start()
    {
        StartCoroutine(StartSignal());
        if(GameInstance.Instance)
        {
            int levelN = GameInstance.Instance.playerLevel + 1;
            level.SetText(levelN.ToString());
        }
        
    }

    IEnumerator StartSignal()
    {
        int blinkCount = 18;
        bool bShow=true;
        WaitForSeconds wait = new WaitForSeconds(.1f);
        while(blinkCount>0)
        {
            StartPanel.gameObject.SetActive(bShow);
            blinkCount--;
            bShow = !bShow;

            yield return wait;
        }

        gameObject.SetActive(false);

    }
}
