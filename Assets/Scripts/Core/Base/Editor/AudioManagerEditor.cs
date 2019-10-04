using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(AudioManager), true)]

public class AudioManagerEditor : Editor
{
    private Editor cachedEditor;

    private void OnEnable()
    {
        cachedEditor = null;

    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AudioManager myscript = (AudioManager)target;
        


        if (cachedEditor == null)
        {
            cachedEditor = Editor.CreateEditor(myscript.audioData);

        }



        if (cachedEditor != null)
        {

            cachedEditor.DrawDefaultInspector();
        }


    }
}
