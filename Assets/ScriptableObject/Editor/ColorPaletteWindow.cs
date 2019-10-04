using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ColorPaletteWindow : EditorWindow
{
    public ColorsPalette objRef;

    [MenuItem("Inner Child/Colors palette")]
    static void Init()
    {
        ColorPaletteWindow window = (ColorPaletteWindow)EditorWindow.GetWindow<ColorPaletteWindow>();

        window.Show(); 
    }

    private void OnEnable()
    {
        objRef = (ColorsPalette)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObject/SO/ColorsPalette.asset", typeof(ColorsPalette));
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
