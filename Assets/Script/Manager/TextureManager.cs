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
    // class  //TODO: Change Texture to Icon
    public Texture Texture_RockHammer;
	public Texture Texture_RockPickaxe;
	public Texture Texture_RockAxe;
	public Texture Texture_RockSword;
	public Texture Texture_RockSpear;
	public Texture Texture_ProgBackground;
	public Texture Texture_ProgHealtBar;
	
	public Texture Icon_Spell_IceBolt;
	public Texture Icon_Spell_FireBat;
	
	public Texture Texture_HP;
	public Texture Texture_MP;
	
	public Texture GUI_TaskAvailable;
	public Texture GUI_TaskPending;
	public Texture GUI_TaskCompleted;
	
	public Material Material_Dungeon_BrickWall;
	public Material Material_Skybox_Camp;
	public Material Material_Skybox_Dungeon;
}