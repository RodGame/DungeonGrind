using UnityEngine;
using System.Collections;

public class PrefabManager : MonoBehaviour {
    // singleton
    private static PrefabManager m_Instance = null;
    public static PrefabManager Get()
    {
        if (m_Instance == null)
            m_Instance = (PrefabManager)FindObjectOfType(typeof(PrefabManager));
        return m_Instance;
    }
    // class 
	
	///////////////
	// BUILDING //
	/////////////
	
	// Utility
    public GameObject Building_CraftingTable;
	
	// Storage
	public GameObject Building_WoodStorage;
	public GameObject Building_WoodenBarrel;
	
	// Structural
	public GameObject Building_WoodenWall;
	public GameObject Building_WoodenFence01;
	public GameObject Building_WoodenFence01Curve;
	public GameObject Building_WoodenFence02;
	public GameObject Building_WoodenFence03;
	public GameObject Building_StoneFence;
	public GameObject Building_HighPillar;
	public GameObject Building_Gate;
	
	// Decoration
	public GameObject Building_Tent;
	public GameObject Building_FirePillar;
	public GameObject Building_LowStatue;
	public GameObject Building_HighStatue;
	public GameObject Building_GargoyleStatue;
	
	///////////////
	// Monster  //
	/////////////
	
	public GameObject Monster_PseudoSpider;
	public GameObject Monster_Spider;
	public GameObject Monster_SpiderQueen;
	public GameObject Monster_SkeletonToon;
	public GameObject Monster_SkeletonFighter;
	public GameObject Monster_SkeletonKing;
	
	///////////////
	//   ITEM   //
	/////////////	
	
	public GameObject Weapon_RockSword;
	public GameObject Weapon_RockAxe;
	public GameObject Weapon_RockSpear;
	public GameObject Weapon_SimpleHammer1;
	public GameObject Weapon_SimpleHammer2;
	
	///////////////
	//  SPELL   //
	/////////////
	
	public GameObject Spell_IceBolt;
	public GameObject Spell_FireBat;
	
	//////////////////
	// ENVIRONMENT //
	////////////////
	
	public GameObject Environment_SpiderEggs;
	public GameObject Environment_CartBroken;
	public GameObject Environment_Cart;
	
	///////////////
	// NGUI     //
	/////////////
	
	public GameObject NGUI_Task_TableTemplate;
	public GameObject NGUI_Skill_SkillTemplate;
	public GameObject NGUI_Build_BuildingTemplate;
	public GameObject NGUI_Craft_ItemTemplate;
}