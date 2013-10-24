using UnityEngine;
using System.Collections;
using System; // For List class;

static class Inventory {
	
	static private TextureManager _TextureManager =  GameObject.FindGameObjectWithTag("GameMaster").GetComponent<TextureManager>();
	static private PrefabManager _PrefabManager   =  GameObject.FindGameObjectWithTag("GameMaster").GetComponent<PrefabManager>();
	static private PlayerHUD      _PlayerHUD      =  GameObject.FindGameObjectWithTag("PlayerMaster").GetComponent<PlayerHUD>();
	
	static public Item[] ItemList;
	static public Building[] BuildingList;
	static public Ressource[] RessourceList;
	
	
	static public void IniInventory()
	{
		ItemList        = new Item[Enum.GetValues(typeof(ItemName)).Length];	
		BuildingList    = new Building[Enum.GetValues(typeof(BuildingName)).Length];	
		RessourceList   = new Ressource[Enum.GetValues(typeof(RessourceName)).Length];	
		IniItemList();
		IniBuildingList();
		IniRessourceList();	
	}
	
	static public void IniBuildingList()
	{
		for(int i = 0; i < BuildingList.Length; i++)
		{
			BuildingList[i] = new Building();
			BuildingList[i].Name = ((BuildingName)i).ToString();
			if(BuildingList[i].Name == "CraftingTable")
			{
				BuildingList[i].Name            = "Crafting Table";
				BuildingList[i].IsBuildable     = true;
				BuildingList[i].IsUnlocked 		= true;
				BuildingList[i].Recipe          = "15*Wood";
				BuildingList[i].BuildingPrefab  = _PrefabManager.Building_CraftingTable;
			}
			else if(BuildingList[i].Name == "WoodenWall")
			{
				BuildingList[i].Name            = "Wooden Wall";
				BuildingList[i].IsBuildable     = true;
				BuildingList[i].IsUnlocked 		= true;
				BuildingList[i].Recipe          = "10*Wood";
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
				BuildingList[i].Recipe          = "15*Wood";
				BuildingList[i].BuildingPrefab  = _PrefabManager.Building_Tent;
			}
		}
	}
	
	static public void IniItemList()
	{
		for(int i = 0; i < ItemList.Length; i++)
		{
			ItemList[i] = new Item();
			ItemList[i].Name = ((ItemName)i).ToString();
			if(ItemList[i].Name == "Hammer")
			{
				ItemList[i].Name        = "Hammer";
				ItemList[i].IsCraftable = true;
				ItemList[i].IsUnlocked  = true;
				ItemList[i].Recipe      = "5*Wood+2*Rock";
				ItemList[i].ItemIcon    = _TextureManager.Texture_RockHammer;
				
			}
			else if(ItemList[i].Name == "RockSword")
			{
				ItemList[i].Name        = "Rock Sword";
				ItemList[i].IsCraftable = true;
				ItemList[i].IsUnlocked 	= true;
				ItemList[i].Recipe      = "5*Wood+4*Rock";
				ItemList[i].ItemIcon    = _TextureManager.Texture_RockSword;
				ItemList[i].ItemPrefab  = _PrefabManager.Item_RockSword;
			}
			else if(ItemList[i].Name == "RockAxe")
			{
				ItemList[i].Name        = "Rock Axe";
				ItemList[i].IsCraftable = true;
				ItemList[i].IsUnlocked  = true;
				ItemList[i].Recipe      = "5*Wood+4*Rock";
				ItemList[i].ItemIcon    = _TextureManager.Texture_RockAxe;
				ItemList[i].ItemPrefab  = _PrefabManager.Item_RockAxe;
				
			}
			else if(ItemList[i].Name == "RockPickaxe")
			{
				ItemList[i].Name        = "Rock Pickaxe";
				ItemList[i].IsCraftable = true;
				ItemList[i].IsUnlocked  = true;
				ItemList[i].Recipe      = "4*Wood+5*Rock";
				ItemList[i].ItemIcon    = _TextureManager.Texture_RockPickaxe;
				//ItemList[i].ItemPrefab   = _PrefabManager.Prefab_Item_RockPickaxe;
			}
			else if(ItemList[i].Name == "RockSpear")
			{
				ItemList[i].Name        = "Rock Spear";
				ItemList[i].IsCraftable = true;
				ItemList[i].IsUnlocked 	= true;
				ItemList[i].Recipe      = "4*Wood+5*Rock";
				ItemList[i].ItemIcon    = _TextureManager.Texture_RockPickaxe;
				ItemList[i].ItemPrefab  = _PrefabManager.Item_RockSpear;
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
				RessourceList[i].CurValue = 10;
				RessourceList[i].MaxValue = 1000;
			}
			else if(RessourceList[i].Name == "Rock")
			{
				RessourceList[i].CurValue = 25;
				RessourceList[i].MaxValue = 1000;
			}
			else if(RessourceList[i].Name == "Wood")
			{
				RessourceList[i].CurValue = 30;
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
