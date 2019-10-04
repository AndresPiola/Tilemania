using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIPopUPScore : MonoBehaviour {

    public TextMeshPro[] scoreNumber;

    private void Awake()
    {
        for (int i = 0; i < scoreNumber.Length; i++)
        {
           // scoreNumber[i].GetComponent<MeshRenderer>().sortingLayerID = 2;
            scoreNumber[i].GetComponent<MeshRenderer>().sortingOrder = 22;


        }
    }
    public void Disable()
    {
        SetScore(100);

         gameObject.SetActive(false);

    }

    public void SetScore(int _score)
    {
      int[] digits=   Utils.IntToIntArray(_score);

        for (int i = 0; i < digits.Length; i++)
        {
            scoreNumber[i].SetText(digits[i].ToString());

        }
    }
}
