
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [CreateAssetMenu(fileName = "GameConfig", menuName = "InnerChild/GameConfig", order = 1)]
public class GameConfig : SingletonScriptableObject<GameConfig>
{
    public float blockSpeed = 12f;
    public int worldSizeX;
    public int worldSizeY;

    public float tileSize = 32;

    [Header("Tile Icons")] public Sprite[] tileIcons;
    [Header("WorldColors")]
    public int[] maxLevelsPerWorld;


    [Header("WorldColors")]
    public Color[] skyboxColors;
    public Texture[] backTextures;
    
    
}