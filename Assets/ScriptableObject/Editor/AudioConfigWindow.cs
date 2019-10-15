using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Sirenix.OdinInspector.Editor;


public class AudioConfigWindow : OdinEditorWindow {

    public AudioData objRef;

    [MenuItem("Inner Child/AudioData")]
    private static void OpenWindow()
    {
        GetWindow<AudioConfigWindow>().Show();
    }
    protected override void Initialize()
    {
        objRef = (AudioData)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObject/SO/AudioData.asset", typeof(AudioData));

        //this.WindowPadding = Vector4.zero;
    }

    protected override object GetTarget()
    {
        return objRef;
    }
    
    private void OnEnable()
    {
        base.OnEnable();
        objRef = (AudioData)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObject/SO/AudioData.asset", typeof(AudioData));
    }
    /*
 protected override void DrawEditors()
 {
    // base.DrawEditors();
     OdinEditor edit = (OdinEditor)OdinEditor.CreateEditor(objRef,typeof(OdinEditor));
     edit.DrawDefaultInspector();

 }


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

 }*/
}
