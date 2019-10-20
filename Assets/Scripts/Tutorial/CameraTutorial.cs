using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTutorial : MonoBehaviour
{
    public   void OnDisable()
    {
        
        TutorialManager.OnTutorialFinished -= TutorialManager_OnTutorialFinished;
    }

    public   void OnEnable()
    {

        TutorialManager.OnTutorialFinished += TutorialManager_OnTutorialFinished; ;

    }

    private void TutorialManager_OnTutorialFinished()
    {
       MoveCameraAway();
    }

     
    void MoveCameraAway()
    {
        LeanTween.moveY(gameObject, -18, 1.5f).setEase(LeanTweenType.easeInOutBack);

    }
}
