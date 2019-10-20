using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  TMPro;

public class TutorialText : MonoBehaviour
{
    public TextMeshProUGUI msgText;
    public string[] tutorialTexts;

    // Start is called before the first frame update
    void Start()
    {
        msgText.canvasRenderer.SetAlpha(0);
        ShowTextByIndex();
    }

    void OnDisable()
    {
        TutorialManager.OnTutorialIndexChange -= TutorialManager_OnTutorialIndexChange;

    }

    void OnEnable()
    {
        TutorialManager.OnTutorialIndexChange += TutorialManager_OnTutorialIndexChange;
    }

    private void TutorialManager_OnTutorialIndexChange(int Param1)
    {
        msgText.canvasRenderer.SetAlpha(0);
        ShowTextByIndex(Param1);
    }

    public void ShowTextByIndex(int Index=0)
    {
  
        msgText.CrossFadeAlpha(1,1f,true); 
        
        msgText.SetText( tutorialTexts[Index]);
    }
   
}
