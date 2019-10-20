using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

//public class GameManager<T>  :  SingletonScriptableObject<T> where T:SingletonScriptableObject<GameManager<T>>

public class TutorialManager : GameManager 
{
     
    [Header("Tutorial")] public GameObject arrowObject;
   
    private int tutorialStage;

    public static event FNotify_1Params<int> OnTutorialIndexChange;
    public static event FNotify OnTutorialFinished;

    public override void Start()
    {
        targetHolderRb = targetScoreHolder.GetComponent<Rigidbody2D>();
        GenerateTutorialTarget();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        DropArea.OnTileDropped -= DropArea_OnTileDropped;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        DropArea.OnTileDropped += DropArea_OnTileDropped;

    }

    private void DropArea_OnTileDropped(Tile Param1)
    {
        tutorialStage++;

        switch (tutorialStage)
        {
            case 1:
                arrowObject.SetActive(true);
                break;
            case 2:
                arrowObject.SetActive(false);
                break;

            case 7:
                OnTutorialFinished?.Invoke();
                 DelayedNewLevel();
                break;

        }

        OnTutorialIndexChange?.Invoke(tutorialStage);
    }

    async Task DelayedNewLevel()
    {
        await Task.Delay(2000);

        SceneManager.LoadSceneAsync("Main");
    }
    void GenerateTutorialTarget()
    {
        ResetTargetScoreHolder();

        targetScore = 5; 
        targetScoreText.SetText(targetScore.ToString());
        targetIcon = 1;
        targetIconRenderer.sprite = targetIcons[targetIcon];
    }

}
