using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class TextMeshSortingLayer : MonoBehaviour {

    public int sortingOrder;
    public string sortingLayerName;

    private void Awake()
    {
        GetComponent<MeshRenderer>().sortingLayerName= sortingLayerName;
        GetComponent<MeshRenderer>().sortingOrder = sortingOrder;


    }

    
}
