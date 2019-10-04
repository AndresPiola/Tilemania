using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIArrowButtons : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(GameInstance.Instance.playerLevel>3)
        {
            GetComponent<Image>().CrossFadeAlpha(0, 1, true);

        }
	}
	
	 
}
