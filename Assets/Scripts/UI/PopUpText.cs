using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpText : MonoBehaviour {

    public TextMeshPro text;
    LTSeq seq;


    private void Awake()
    {
        
    }



    public void Deactivate()
    {
        text.transform.localPosition = Vector3.zero;
        text.rectTransform.localScale = Vector3.one;

        gameObject.SetActive(false);

    }

    public void ShowPopUp(string _text,float _time=.2f)
    {
        text.SetText(_text);

        seq = LeanTween.sequence();
        LeanTween.scale(gameObject, Vector3.one, .1f).setEaseInBounce();

        seq.append(LeanTween.moveLocalY(text.gameObject, 1, _time)
            .setEase(LeanTweenType.easeInQuad));
        seq.append(.5f);
        seq.append(LeanTween.scale(text.rectTransform,Vector3.zero, _time/2)
         .setOnComplete(this.Deactivate )); 
    }

     
}
