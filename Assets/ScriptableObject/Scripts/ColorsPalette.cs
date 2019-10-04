using System.Collections;
using System.Collections.Generic;
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
    public FSkyColorGradient[] skyGradientColors;
}
