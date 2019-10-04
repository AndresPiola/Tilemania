using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(GameFeel))]
public class GameFeelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();

        GameFeel gf = (GameFeel)target;

        if(EditorGUI.EndChangeCheck())
        {
            gf.SetCameraShake();
             

        }


    }
}
