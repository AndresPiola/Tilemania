using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISign : MonoBehaviour {
    public TextMeshPro levelSign;


    private void OnEnable()
    {
        levelSign.SetText(GameInstance.Instance.playerLevel.ToString());

    }
}
