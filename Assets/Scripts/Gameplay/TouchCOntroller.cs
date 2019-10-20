using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCOntroller : MonoBehaviour
{
    public bool bTouchDisabled;

    void OnDisable()
    {
        PlayerController.OnTouchBegin -= PlayerController_OnTouchBegin; ;

        DropArea.OnResolvingMatch -= DropArea_OnResolvingMatch;
    }

    void OnEnable()
    {
        PlayerController.OnTouchBegin += PlayerController_OnTouchBegin;

        DropArea.OnResolvingMatch += DropArea_OnResolvingMatch;
    }

    private void DropArea_OnResolvingMatch(bool Param1)
    {
        bTouchDisabled = Param1;

    }

    private void PlayerController_OnTouchBegin(FTouchInfo TouchInfo)
    {
        if(bTouchDisabled)return;
        ;

        Collider2D  colHit= Physics2D.OverlapBox(TouchInfo.touchWorldPoint, Vector2.one, 0);

 
        if(colHit==null)return;
          colHit.GetComponent<Tile>()?.SetSelected(true);
    }
}
