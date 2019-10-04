using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


public class SpawnDataWindow : EditorWindow {

    public SpawnData objRef;

    [MenuItem("Inner Child/SpawnData")]
	 static void Init()
    {
        SpawnDataWindow window = (SpawnDataWindow)EditorWindow.GetWindow<SpawnDataWindow>();
        window.Show();
 
    }

    private void OnEnable()
    {
        objRef = (SpawnData)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObject/SpawnData.asset", typeof(SpawnData));
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
