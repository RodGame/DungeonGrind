using UnityEngine;
using System.Collections;

public class TextureManager : MonoBehaviour {
    // singleton
    private static TextureManager m_Instance = null;
    public static TextureManager Get()
    {
        if (m_Instance == null)
            m_Instance = (TextureManager)FindObjectOfType(typeof(TextureManager));
        return m_Instance;
    }
    // class 
    public Texture Texture_RockPickaxe;
	public Texture Texture_RockAxe;
	public Texture Texture_RockSword;
	public Texture Texture_RockHammer;
	
	public Material Material_Dungeon_BrickWall;
}