using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFormatSavegame : MonoBehaviour {

    private void OnEnable()
    {
#if UNITY_EDITOR
         
#else
        Destroy(gameObject);

#endif
    }
    public void FormatSavegame()
    {
        GameInstance.Instance.ClearSavegame();

    }
}
