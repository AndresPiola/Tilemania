using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool bSelected;
    // Start is called before the first frame update

    public void SetSelected(bool IsSelected)
    {
        bSelected = IsSelected;

    }

    void OnDisable()
    {
        PlayerController.OnTouchHold -= PlayerController_OnTouchHold;
    }

    void OnEnable()
    {
        PlayerController.OnTouchHold += PlayerController_OnTouchHold;
    }

    private void PlayerController_OnTouchHold(FTouchInfo TouchInfo)
    {
        Vector3 targetpos = TouchInfo.touchWorldPoint;
        targetpos.z = 0;
        transform.position = targetpos;

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
