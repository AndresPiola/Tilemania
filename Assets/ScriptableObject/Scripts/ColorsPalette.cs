using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[System.Serializable]
public struct FSkyColorGradient
{
    public Color32 colorA;
    public Color32 colorB;

}
 
[CreateAssetMenu(fileName = "ColorsPalette", menuName = "InnerChild/Config/ColorsPalette", order = 1)]
public class ColorsPalette : SingletonScriptableObject<ColorsPalette>
{
    [TableList(ShowPaging = true)]
    public FSkyColorGradient[] skyGradientColors;

    [TableList(ShowPaging = true)]
    public Color[] blockColors;



    public Color GetRandomBlockColor()
    {
        return blockColors[Random.Range(0, blockColors.Length)];

    }
}
