using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


public class GameConfigWindow : EditorWindow {

    public GameConfig objRef;

    [MenuItem("Inner Child/GameConfig")]
	 static void Init()
    {
        GameConfigWindow window = (GameConfigWindow)EditorWindow.GetWindow<GameConfigWindow>();
        window.Show();
 
    }

    private void OnEnable()
    {
        objRef = (GameConfig)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObject/SO/GameConfig.asset", typeof(GameConfig));
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
