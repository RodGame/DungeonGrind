using UnityEngine;
using System.Collections;
using System; // For List class;

static class Inventory {
	
	static private TextureManager _TextureManager =  GameObject.FindGameObjectWithTag("GameMaster").GetComponent<TextureManager>();
	static private PrefabManager _PrefabManager   =  GameObject.FindGameObjectWithTag("GameMaster").GetComponent<PrefabManager>();
	static private PlayerHUD      _PlayerHUD      =  GameObject.FindGameObjectWithTag("PlayerMaster").GetComponent<PlayerHUD>();
	
	static public Weapon[] WeaponList;
	static public Building[] BuildingList;
	static public Ressource[] RessourceList;
	
	
	static public void IniInventory()
	{
		WeaponList        = new Weapon[Enum.GetValues(typeof(WeaponName)).Length];	
		BuildingList    = new Building[Enum.GetValues(typeof(BuildingName)).Length];	
		RessourceList   = new Ressource[Enum.GetValues(typeof(RessourceName)).Length];	
		IniWeaponList();
		IniBuildingList();
		IniRessourceList();	
	}
	
	static public void IniBuildingList()
	{
		for(int i = 0; i < BuildingList.Length; i++)
		{
			BuildingList[i] = new Building();
			BuildingList[i].Id = i;
			BuildingList[i].Name = ((BuildingName)i).ToString();
			if(BuildingList[i].Name == "CraftingTable")
			{
				BuildingList[i].Name            = "Crafting Table";
				BuildingList[i].IsBuildable     = true;
				BuildingList[i].IsUnlocked 		= true;
				BuildingList[i].Recipe          = "25*Wood";
				BuildingList[i].BuildingPrefab  = _PrefabManager.Building_CraftingTable;
			}
			else if(BuildingList[i].Name == "WoodenWall")
			{
				BuildingList[i].Name            = "Wooden Wall";
				BuildingList[i].IsBuildable     = true;
				BuildingList[i].IsUnlocked 		= true;
				BuildingList[i].Recipe          = "12*Wood";
				BuildingList[i].BuildingPrefab  = _PrefabManager.Building_WoodenWall;
			}
			else if(BuildingList[i].Name == "WoodenFence")
			{
				BuildingList[i].Name            = "Wooden Fence";
				BuildingList[i].IsBuildable     = true;
				BuildingList[i].IsUnlocked 		= true;
				BuildingList[i].Recipe          = "5*Wood";
				BuildingList[i].BuildingPrefab  = _PrefabManager.Building_WoodenFence;
			}
			else if(BuildingList[i].Name == "WoodenFenceCurve")
			{
				BuildingList[i].Name            = "Wooden Fence Curve";
				BuildingList[i].IsBuildable     = true;
				BuildingList[i].IsUnlocked 		= true;
				BuildingList[i].Recipe          = "5*Wood";
				BuildingList[i].BuildingPrefab  = _PrefabManager.Building_WoodenFenceCurve;
			}
			else if(BuildingList[i].Name == "Tent")
			{
				BuildingList[i].Name            = "Tent";
				BuildingList[i].IsBuildable     = true;
				BuildingList[i].IsUnlocked 		= true;
				BuildingList[i].Recipe          = "10*Wood";
				BuildingList[i].BuildingPrefab  = _PrefabManager.Building_Tent;
			}
		}
	}
	
	static public void IniWeaponList()
	{
		for(int i = 0; i < WeaponList.Length; i++)
		{
			WeaponList[i] = new Weapon();
			WeaponList[i].IdWeapon = i;
			WeaponList[i].Name = ((WeaponName)i).ToString();
			if(WeaponList[i].Name == "Hammer")
			{
				WeaponList[i].Name        = "Hammer";
				WeaponList[i].IsCraftable = true;
				WeaponList[i].IsUnlocked  = true;
				WeaponList[i].Recipe      = "5*Wood+2*Rock";
				WeaponList[i].ItemIcon    = _TextureManager.Texture_RockHammer;
				WeaponList[i].ItemPrefab  = _PrefabManager.Weapon_SimpleHammer2;
				WeaponList[i].BaseDamage     = 1;
				WeaponList[i].DamagePerLevel = 0.5f;
				WeaponList[i].BaseSpeed      = 125f;
				WeaponList[i].SpeedPerLevel  = 5f;	
				WeaponList[i].BaseRange      = 5.0f;
				WeaponList[i].RangePerLevel  = 0.5f;	
			}
			else if(WeaponList[i].Name == "RockSword")
			{
				WeaponList[i].Name        = "Rock Sword";
				WeaponList[i].IsCraftable = true;
				WeaponList[i].IsUnlocked  = true;
				WeaponList[i].Recipe      = "5*Wood+4*Rock";
				WeaponList[i].ItemIcon    = _TextureManager.Texture_RockSword;
				WeaponList[i].ItemPrefab  = _PrefabManager.Weapon_RockSword;
				WeaponList[i].BaseDamage     = 4;
				WeaponList[i].DamagePerLevel = 1.25f;
				WeaponList[i].BaseSpeed      = 130f;
				WeaponList[i].SpeedPerLevel  = 4f;	
				WeaponList[i].BaseRange      = 6.0f;
				WeaponList[i].RangePerLevel  = 0.5f;
			}
			else if(WeaponList[i].Name == "RockAxe")
			{
				WeaponList[i].Name        = "Rock Axe";
				WeaponList[i].IsCraftable = true;
				WeaponList[i].IsUnlocked  = true;
				WeaponList[i].Recipe      = "5*Wood+4*Rock";
				WeaponList[i].ItemIcon    = _TextureManager.Texture_RockAxe;
				WeaponList[i].ItemPrefab  = _PrefabManager.Weapon_RockAxe;
				WeaponList[i].BaseDamage     = 7;
				WeaponList[i].DamagePerLevel = 1.0f;
				WeaponList[i].BaseSpeed      = 100f;
				WeaponList[i].SpeedPerLevel  = 5f;	
				WeaponList[i].BaseRange      = 6.0f;
				WeaponList[i].RangePerLevel  = 0.4f;
				
			}
			else if(WeaponList[i].Name == "RockPickaxe")
			{
				WeaponList[i].Name        = "Rock Pickaxe";
				WeaponList[i].IsCraftable = true;
				WeaponList[i].IsUnlocked  = true;
				WeaponList[i].Recipe      = "4*Wood+5*Rock";
				WeaponList[i].ItemIcon    = _TextureManager.Texture_RockPickaxe;
				//WeaponList[i].ItemPrefab   = _PrefabManager.Prefab_Weapon_RockPickaxe;
				WeaponList[i].BaseDamage     = 4;
				WeaponList[i].DamagePerLevel = 1.0f;
				WeaponList[i].BaseSpeed      = 135f;
				WeaponList[i].SpeedPerLevel  = 8f;	
				WeaponList[i].BaseRange      = 5.5f;
				WeaponList[i].RangePerLevel  = 0.4f;
			}
			else if(WeaponList[i].Name == "RockSpear")
			{
				WeaponList[i].Name        = "Rock Spear";
				WeaponList[i].IsCraftable = true;
				WeaponList[i].IsUnlocked 	= true;
				WeaponList[i].Recipe      = "4*Wood+5*Rock";
				WeaponList[i].ItemIcon    = _TextureManager.Texture_RockSpear;
				WeaponList[i].ItemPrefab  = _PrefabManager.Weapon_RockSpear;
				WeaponList[i].BaseDamage     = 4;
				WeaponList[i].DamagePerLevel = 0.5f;
				WeaponList[i].BaseSpeed      = 135f;
				WeaponList[i].SpeedPerLevel  = 5f;	
				WeaponList[i].BaseRange      = 7.0f;
				WeaponList[i].RangePerLevel  = 0.5f;
			}
		}
	}
	
	static public void IniRessourceList()
	{
		for(int i = 0; i < RessourceList.Length; i++)
		{
			RessourceList[i] = new Ressource();
			RessourceList[i].Name = ((RessourceName)i).ToString();
			if(RessourceList[i].Name == "Coin")
			{
				RessourceList[i].CurValue = 5;
				RessourceList[i].MaxValue = 1000;
			}
			else if(RessourceList[i].Name == "Rock")
			{
				RessourceList[i].CurValue = 50;
				RessourceList[i].MaxValue = 1000;
			}
			else if(RessourceList[i].Name == "Wood")
			{
				RessourceList[i].CurValue = 100;
				RessourceList[i].MaxValue = 1000;
			}
			/*else if(RessourceList[i].Name == "Gold")
			{
				RessourceList[i].CurValue = 0;
				RessourceList[i].MaxValue = 1000;
			}*/
		}
	}
	
	static public void ProcAction(string _actionProced)
	{
		switch(_actionProced)
		{
			case "Woodcutting": 
				RessourceList[(int)RessourceName.Wood].CurValue++;
				_PlayerHUD.AddChatLog("[RESS] " + RessourceName.Wood + " + 1");
				break;
			case "Mining":
				RessourceList[(int)RessourceName.Rock].CurValue++;
				_PlayerHUD.AddChatLog("[RESS] " + RessourceName.Rock  + " + 1");
				break;
			case "Default": 
				//Debug.LogWarning ("No Action in GameManager");
				break;
		}
	}
}
