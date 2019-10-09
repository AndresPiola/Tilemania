using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using TMPro;

[System.Serializable]
public struct FMergeIndicators
{
    public EDirections fromDirection;
    public GameObject indicator;

}
[System.Serializable]
public class FSwapIndicators
{ 
    public SpriteRenderer indicator;
    public int value;
    private GameObject parent;

    FSwapIndicators()
    {
        parent = indicator.transform.parent.gameObject;

    }
    public void SetIndicatorEnabled(bool bNewState = true)
    {
        if(parent==null) parent = indicator.transform.parent.gameObject;

        value = bNewState?value: - 1;
        parent.SetActive(bNewState);
        
    }

    public void SetValue(int NewColor,Color NewRandomColor)
    {
        if (parent == null) parent = indicator.transform.parent.gameObject;

        value = NewColor;
        parent.SetActive(true);
        indicator.color = NewRandomColor;

    }
}
public class Tile : SerializedMonoBehaviour
{
    private SpriteRenderer render;

    public bool bSelected;
    public LayerMask floorMask;
    private bool bInDropArea;
    private bool bDestroyed;
    public Sprite[] blockIcon;
     public int blockType;
     public int blockSubColor;
     public int blockScore;
    [Header("Merge Indicator")] public FMergeIndicators[] mergeIndicators=new FMergeIndicators[4];

    [Header("Value Indicator")]
    public TextMeshPro valueText;

    [Header("Swap Indicator")]// public FSwapIndicators[] swapIndicators=new FSwapIndicators[4];
    public Dictionary<EDirections, FSwapIndicators> swapIndicators = new Dictionary<EDirections, FSwapIndicators>();

    public SpriteRenderer indicatorColor;


    private Vector2 checkBottomSize;
    TileFloor tileFloorTmp = null;
    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        checkBottomSize=new Vector2(0.1f, 0.1f);
    }

    public void SetSelected(bool IsSelected)
    {
        if(bInDropArea)return;
         
        bSelected = IsSelected;

    }

    private void OnDrawGizmos()
    {
       // Handles.Label(transform.position,bSelected.ToString());
    }

    void OnDisable()
    {
        PlayerController.OnTouchHold -= PlayerController_OnTouchHold;
        PlayerController.OnTouchRelease -= PlayerController_OnTouchRelease;
        DropArea.OnComboBonus -= DropArea_OnComboBonus;
        GameMode.OnGameState -= GameMode_OnGameState;
    }

    void OnEnable()
    {
        PlayerController.OnTouchHold += PlayerController_OnTouchHold;
        PlayerController.OnTouchRelease += PlayerController_OnTouchRelease;
        DropArea.OnComboBonus += DropArea_OnComboBonus;
        GameMode.OnGameState += GameMode_OnGameState;
    }

    private void GameMode_OnGameState(EGameStates _val1)
    {
        switch (_val1)
        {
            case EGameStates.MAIN_MENU:
                break;
            case EGameStates.CONNECTING:
                break;
            case EGameStates.RELOADING_ROUND:
                break;
            case EGameStates.LOADING_NEXTROUND:
                break;
            case EGameStates.LOADING_REMATCH:
                break;
            case EGameStates.GAMEPLAY:
                DisableTile();

                
                break;
            case EGameStates.ROUND_OVER:
                break;
            case EGameStates.GAME_OVER:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_val1), _val1, null);
        }
    }

     
    private void PlayerController_OnTouchRelease(FTouchInfo _val1)
    {
        if(!bSelected)return;
        CancelPreview();
        Collider2D hit = Physics2D.OverlapBox(transform.position, checkBottomSize, 0, floorMask.value);

         
       SetSelected(false);
       if (hit != null)
       {
            tileFloorTmp = hit.GetComponent<TileFloor>();

            if (tileFloorTmp != null)
            {
                if (DropArea.Instance.TryToDropTile(this, tileFloorTmp.gridPosition))
                {
                        return;
                };
          
            } 
        }

       AudioManager.Instance.PlayTileReturn();
        TileGenerator.Instance.ReturnTile(this);
    }
 
    private void PlayerController_OnTouchHold(FTouchInfo TouchInfo)
    {
        if(!bSelected)return;
             
        Vector3 targetpos = TouchInfo.touchWorldPoint;
        targetpos.z = 0;
        transform.position = targetpos;

       Collider2D hit=  Physics2D.OverlapBox(transform.position, checkBottomSize, 0,floorMask.value);

       if (hit == null)
       {
           CancelPreview();
           return;
       }
       tileFloorTmp = hit.GetComponent<TileFloor>();
       DropArea.Instance.CheckCombination(this, tileFloorTmp.gridPosition);
     
    }

    public void MergeTowards(Vector2 TargetPosition,FNotify_1Params<int> OnMergeFinished)
    {
        LeanTween.cancel(gameObject);
        bDestroyed = true;
        LeanTween.move(gameObject, TargetPosition, 0.2f).setEase(LeanTweenType.easeInExpo).setOnComplete(() =>
        {
            OnMergeFinished?.Invoke(blockScore);
            DisableTile();
        });

    }

    public  void MoveTowards(Vector2 TargetPosition,bool bForceMove=false, float MoveTime= 0.2f )
    {
        if(bForceMove) LeanTween.cancel(gameObject);
        LeanTween.move( gameObject, TargetPosition, MoveTime).setEase(LeanTweenType.easeInExpo)  ;
     }
    public void MoveTowards(Vector2 TargetPosition, Action OncompleteFNotify, bool bForceMove = false, float MoveTime = 0.2f)
    {
        if (bForceMove) LeanTween.cancel(gameObject);
        LeanTween.move(gameObject, TargetPosition, MoveTime).setEase(LeanTweenType.easeInExpo).setOnComplete(OncompleteFNotify);

    }
    public void Initialize(   )
    {
        ResetTile();
      
        GenerateRandomValues(); 
    }
   
    public void SetInPosition()
    {
        bInDropArea = true;
        bSelected = false; 
    }

    public void GenerateRandomValues()
    {
        int numberColors = ColorsPalette.Instance.blockColors.Length;

        blockType =Random.Range(0, 5); 

        blockScore = Random.Range(1, 7); 
        valueText?.SetText(blockScore.ToString());

       // render.color = ColorsPalette.Instance.blockColors[blockType];
        ///generate subcolor
        blockSubColor = Random.Range(0, numberColors);
         indicatorColor.color = ColorsPalette.Instance.blockColors[blockSubColor];
           indicatorColor.sprite = blockIcon[blockType];

        ///generate swap 
        if (Random.value < .8f)
        {
            EDirections tmpDir = Utils.RandomEnumValue<EDirections>();

            if (tmpDir != EDirections.NONE)
            {
                SetSwapIndicator(tmpDir, Random.Range(0, numberColors));
            }
                
        }
         
    }

    public FSwapIndicators GetIndicator(EDirections Direction)
    {
        if (swapIndicators.TryGetValue(Direction, out FSwapIndicators swapIndicatorTmp))
        {
             
            return swapIndicatorTmp;
        }

        return swapIndicatorTmp;
    }
    public void RemoveSwapIndicator(EDirections Direction)
    {
        swapIndicators[Direction].SetIndicatorEnabled(false);
         
    }
    public void SetSwapIndicator(EDirections Direction, int ColorIndex)
    {
        swapIndicators[Direction].SetValue(ColorIndex, ColorsPalette.Instance.blockColors[ColorIndex]);
        swapIndicators[Direction].value = ColorIndex;

    }

    public void AbsorbOtherTile(Tile OtherTile)
    {
        foreach (KeyValuePair<EDirections, FSwapIndicators> indicator in OtherTile.swapIndicators)
        {
            
            if(indicator.Value.value<0)continue;

            if(swapIndicators[indicator.Key].value<0)
                SetSwapIndicator(indicator.Key,indicator.Value.value);

        }
       
    }
    Vector3 scaleEffect=Vector3.one*1.2f;
    public void AddValue(int PlusValue)
    {
        blockScore += PlusValue;
        valueText?.SetText(blockScore.ToString());
        LeanTween.scale(valueText.gameObject, scaleEffect, 0.2f).setEasePunch();


    }

    void DisableAllIndicators()
    {
        foreach (KeyValuePair<EDirections,FSwapIndicators> indicator in swapIndicators)
        {
            indicator.Value.SetIndicatorEnabled(false);

        }
    }
    public void CancelPreview()
    {
        for (int i = 0; i < mergeIndicators.Length; i++)
        {
            if (mergeIndicators[i].indicator.activeInHierarchy)
                mergeIndicators[i].indicator.SetActive(false);

        }
    }

    public void ShowPreviewCombination(Tile OtherTile,EDirections FromDirection, ECombinationType CombinationType)
    {
        switch (CombinationType)
        {
            case ECombinationType.MERGE:
                ShowMatch(OtherTile, FromDirection);
                break;
        }
    }


    void ShowMatch(Tile otherTile, EDirections FromDirection)
    {
        for (int i = 0; i < mergeIndicators.Length; i++)
        {
            if(mergeIndicators[i].fromDirection==FromDirection)
            mergeIndicators[i].indicator.SetActive(true);
        }
    }

    void ResetTile()
    {
        SetSelected(false);
        bInDropArea = false;
        bDestroyed = false;
        DisableAllIndicators();
    }
    void DisableTile()
    {
        DisableAllIndicators();
        gameObject.SetActive(false);
    }

    private void DropArea_OnComboBonus()
    {
        if (!bInDropArea || bDestroyed) return;
        AddValue(1);
        PopUpTextPool.Instance.GetPooledObjectComponent<PopUpText>(transform.position).ShowPopUp("+1");
         AudioManager.Instance.PlayComboSound();
    }



}
