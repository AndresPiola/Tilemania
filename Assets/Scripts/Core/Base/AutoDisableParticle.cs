using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisableParticle : MonoBehaviour {

    WaitForSeconds durationManager;

    private void Awake()
    {
        durationManager = new WaitForSeconds(2.0f);


    }
    // Use this for initialization
    private void OnEnable()
    {
        StartCoroutine(SetAutoKill());

    }

    private void OnDisable()
    {
        StopAllCoroutines();

    }

    IEnumerator SetAutoKill()
    {
        yield return durationManager;
        if(isActiveAndEnabled)
        gameObject.SetActive(false);

    }
}
