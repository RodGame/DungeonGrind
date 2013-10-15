using UnityEngine;
using System.Collections;
using System; // For List class;

public class Inventory : MonoBehaviour {
	
	private TextureManager _TextureManager;
	private PlayerHUD      _PlayerHUD;
	
	public Item[] ItemList;
	public Building[] BuildingList;
	public Ressource[] RessourceList;
	
	
	// Use this for initialization
	void Start () {
		_TextureManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<TextureManager>();
		_PlayerHUD      = GameObject.FindGameObjectWithTag("PlayerMaster").GetComponent<PlayerHUD>();
		ItemList        = new Item[Enum.GetValues(typeof(ItemName)).Length];	
		BuildingList    = new Building[Enum.GetValues(typeof(BuildingName)).Length];	
		RessourceList   = new Ressource[Enum.GetValues(typeof(RessourceName)).Length];	
		IniItemList();
		IniBuildingList();
		IniRessourceList();
	}
	
	public void IniBuildingList()
	{
		for(int i = 0; i < BuildingList.Length; i++)
		{
			BuildingList[i] = new Building();
			BuildingList[i].Name = ((BuildingName)i).ToString();
			if(BuildingList[i].Name == "CraftingTable")
			{
				BuildingList[i].Name            = "Crafting Table";
				BuildingList[i].IsBuildable       = false;
				BuildingList[i].IsUnlocked = true;
				BuildingList[i].Recipe          = "15*Wood";
			}
		}
	}
	
	public void IniItemList()
	{
		for(int i = 0; i < ItemList.Length; i++)
		{
			ItemList[i] = new Item();
			ItemList[i].Name = ((ItemName)i).ToString();
			if(ItemList[i].Name == "Hammer")
			{
				ItemList[i].Name            = "Hammer";
				ItemList[i].IsCraftable       = true;
				ItemList[i].IsUnlocked = true;
				ItemList[i].Recipe          = "5*Wood+2*Rock";
				ItemList[i].ItemTexture     = _TextureManager.Texture_RockHammer;
			}
			else if(ItemList[i].Name == "RockSword")
			{
				ItemList[i].Name            = "Rock Sword";
				ItemList[i].IsCraftable     = true;
				ItemList[i].IsUnlocked 		= true;
				ItemList[i].Recipe          = "5*Wood+4*Rock";
				ItemList[i].ItemTexture     = _TextureManager.Texture_RockSword;
				
			}
			else if(ItemList[i].Name == "RockAxe")
			{
				ItemList[i].Name            = "Rock Axe";
				ItemList[i].IsCraftable       = true;
				ItemList[i].IsUnlocked = true;
				ItemList[i].Recipe          = "5*Wood+4*Rock";
				ItemList[i].ItemTexture     = _TextureManager.Texture_RockAxe;
				
			}
			else if(ItemList[i].Name == "RockPickaxe")
			{
				ItemList[i].Name            = "Rock Pickaxe";
				ItemList[i].IsCraftable       = true;
				ItemList[i].IsUnlocked = true;
				ItemList[i].Recipe          = "4*Wood+5*Rock";
				ItemList[i].ItemTexture     = _TextureManager.Texture_RockPickaxe;
			}
			/*else if(ItemList[i].Name == "GoldSword")
			{
				ItemList[i].Name            = "Gold Sword";
				ItemList[i].IsCraftable       = true;
				ItemList[i].IsUnlocked = false;
				ItemList[i].Recipe          = "5*Wood+2*Gold";
			}
			else if(ItemList[i].Name == "GoldAxe")
			{
				ItemList[i].Name            = "Gold Axe";
				ItemList[i].IsCraftable       = true;
				ItemList[i].IsUnlocked = false;
				ItemList[i].Recipe          = "5*Wood+2*Gold";
			}
			else if(ItemList[i].Name == "GoldPickaxe")
			{
				ItemList[i].Name            = "Gold Pickaxe";
				ItemList[i].IsCraftable       = true;
				ItemList[i].IsUnlocked = false;
				ItemList[i].Recipe          = "5*Wood+2*Gold";
			}*/
		}
	}
	
	public void IniRessourceList()
	{
		for(int i = 0; i < RessourceList.Length; i++)
		{
			RessourceList[i] = new Ressource();
			RessourceList[i].Name = ((RessourceName)i).ToString();
			if(RessourceList[i].Name == "Coin")
			{
				RessourceList[i].CurValue =05;
				RessourceList[i].MaxValue = 1000;
			}
			else if(RessourceList[i].Name == "Rock")
			{
				RessourceList[i].CurValue = 25;
				RessourceList[i].MaxValue = 1000;
			}
			else if(RessourceList[i].Name == "Wood")
			{
				RessourceList[i].CurValue = 45;
				RessourceList[i].MaxValue = 1000;
			}
			/*else if(RessourceList[i].Name == "Gold")
			{
				RessourceList[i].CurValue = 0;
				RessourceList[i].MaxValue = 1000;
			}*/
		}
	}
	
	public void ProcAction(string _actionProced)
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
				Debug.LogWarning ("No Action in GameManager");
				break;
		}
	}
}
