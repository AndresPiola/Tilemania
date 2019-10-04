using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


public class LevelManagerWindow : EditorWindow
{

    public LevelManager objRef;

    [MenuItem("Inner Child/LevelManager")]
    static void Init()
    {
        LevelManagerWindow window = (LevelManagerWindow)EditorWindow.GetWindow<LevelManagerWindow>();
        window.Show();

    }

    private void OnEnable()
    {
        objRef = (LevelManager)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObject/SO/LevelManager.asset", typeof(LevelManager));
    }

    private void OnGUI()
    {
        if (objRef == null)
        {
            Debug.Log("objref ==null");
            return;

        }

        Editor editor = Editor.CreateEditor(objRef);

        editor.DrawDefaultInspector();

    }
}
