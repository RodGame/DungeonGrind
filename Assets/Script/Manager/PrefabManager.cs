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
    public GameObject Building_CraftingTable;
	public GameObject Building_WoodenWall;
	public GameObject Building_WoodenFence;
	public GameObject Building_WoodenFenceCurve;
	public GameObject Building_Tent;
	
	public GameObject Monster_PseudoSpider;
	public GameObject Monster_Spider;
	public GameObject Monster_SpiderQueen;
	public GameObject Monster_SkeletonToon;
	public GameObject Monster_SkeletonFighter;
	public GameObject Monster_SkeletonKing;
	
	public GameObject Weapon_RockSword;
	public GameObject Weapon_RockAxe;
	public GameObject Weapon_RockSpear;
	public GameObject Weapon_SimpleHammer1;
	public GameObject Weapon_SimpleHammer2;

	public GameObject Spell_IceBolt;
	public GameObject Spell_FireBat;
	
	public GameObject Environment_SpiderEggs;
	public GameObject Environment_CartBroken;
	public GameObject Environment_Cart;
}