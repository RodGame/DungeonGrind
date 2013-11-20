using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List class;
using System;

static class CraftSystem {
	
	static PlayerHUD _PlayerHUD = GameObject.FindGameObjectWithTag("PlayerMaster").GetComponent<PlayerHUD>();
	
	public static void CraftItem (Weapon _WeaponToCraft) 
	{	
		List<Utility.ParsedString> _itemRessourceNeeded  = new List<Utility.ParsedString>();	//Declare a list that contain all the ressource needed
		_itemRessourceNeeded = Utility.parseString(_WeaponToCraft.Recipe);
		
		if(TestRessource(_itemRessourceNeeded) == true) //Test if all ressources are available
		{
			ItemInventory.AddItem (_WeaponToCraft);
			_WeaponToCraft.NbrCrafted++;
			SpendRessource(_itemRessourceNeeded);
			_PlayerHUD.AddChatLog("[CRAF] " + _WeaponToCraft.Name + " has been crafted");
		}
		
		
	}
	
	public static void SpendRessource(List<Utility.ParsedString> _ressourceToSpend)
	{
		for(int i = 0; i <  _ressourceToSpend.Count; i++)
		{
			RessourceName RessourceIndex = (RessourceName) Enum.Parse(typeof(RessourceName), _ressourceToSpend[i].type);  

			Inventory.RessourceList[(int)RessourceIndex].CurValue  -= _ressourceToSpend[i].nbr;

		}
	}
	
	public static bool TestRessource(List<Utility.ParsedString> _ressourceToTest)
	{
		bool isRessourceAvailable = true;								//Fill the list by parsing the string
		
		for(int i = 0; i <  _ressourceToTest.Count; i++)
		{
			
			RessourceName RessourceIndex = (RessourceName) Enum.Parse(typeof(RessourceName), _ressourceToTest[i].type);  

			if( Inventory.RessourceList[(int)RessourceIndex].CurValue  < _ressourceToTest[i].nbr)
			{
				isRessourceAvailable = false;
			}
		}
		return isRessourceAvailable;
	}
	
}
