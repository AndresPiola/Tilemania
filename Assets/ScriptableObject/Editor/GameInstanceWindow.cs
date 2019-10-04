using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


public class GameInstanceWindow : EditorWindow {

    public GameInstance objRef;

    [MenuItem("Inner Child/GameInstance")]
	 static void Init()
    {
        GameInstanceWindow window = (GameInstanceWindow)EditorWindow.GetWindow<GameInstanceWindow>();
        window.Show();
 
    }

    private void OnEnable()
    {
        objRef = (GameInstance)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObject/SO/GameInstance.asset", typeof(GameInstance));
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
