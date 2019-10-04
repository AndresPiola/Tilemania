using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCOntroller : MonoBehaviour
{
    void OnDisable()
    {
        PlayerController.OnTouchBegin -= PlayerController_OnTouchBegin; ;

    }

    void OnEnable()
    {
        PlayerController.OnTouchBegin += PlayerController_OnTouchBegin; ;
    }

    private void PlayerController_OnTouchBegin(FTouchInfo TouchInfo)
    {
        Collider2D   colHit= Physics2D.OverlapBox(TouchInfo.touchWorldPoint, Vector2.one, 0);

          colHit.GetComponent<Tile>()?.SetSelected(true);
    }
}
