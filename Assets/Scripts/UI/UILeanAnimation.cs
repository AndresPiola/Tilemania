using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public enum EAnimationType { SCALE,MOVE}
public class UILeanAnimation : SerializedMonoBehaviour
{
    public EAnimationType animationType;

    public Vector2 targetPosition;
    public float moveTime;
    public int beginDelay;


    // Start is called before the first frame update
    void Start()
    {
        switch (animationType)
        {
            case EAnimationType.MOVE:
                BeginMoveAnim();
                break;
            case EAnimationType.SCALE:
                BeginScaleAnim();
                break;
        }
    }

    async Task BeginMoveAnim()
    {
        await Task.Delay(beginDelay);

        LeanTween.move(GetComponent<RectTransform>(), targetPosition, moveTime);

    }
    async Task BeginScaleAnim()
    {
        LeanTween.scale(GetComponent<RectTransform>(), targetPosition, moveTime);

    }

}
