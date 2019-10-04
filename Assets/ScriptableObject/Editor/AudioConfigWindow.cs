using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


public class AudioConfigWindow : EditorWindow {

    public AudioData objRef;

    [MenuItem("Inner Child/AudioData")]
	 static void Init()
    {
        AudioConfigWindow window = (AudioConfigWindow)EditorWindow.GetWindow<AudioConfigWindow>();
        window.Show();
 
    }

    private void OnEnable()
    {
        objRef = (AudioData)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObject/SO/AudioData.asset", typeof(AudioData));
    }

    private void OnGUI()
    {
        if(objRef==null)
        {
            Debug.Log("objref ==null");
            return;

        }

        Editor editor = Editor.CreateEditor(objRef);

        editor.DrawDefaultInspector();
 
    }
}
