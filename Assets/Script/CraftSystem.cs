using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List class;
using System;

static class CraftSystem {
	
	static Inventory _Inventory = GameObject.FindGameObjectWithTag("PlayerMaster").GetComponent<Inventory>();
	static PlayerHUD _PlayerHUD = GameObject.FindGameObjectWithTag("PlayerMaster").GetComponent<PlayerHUD>();
	
	public struct ExpenditureNeeded
    {
        public int nbr;
        public string type;
    };	
	
	public static void CraftItem (Item _ItemToCraft) 
	{	
		List<ExpenditureNeeded> _itemRessourceNeeded  = new List<ExpenditureNeeded>();	//Declare a list that contain all the ressource needed
		_itemRessourceNeeded = parseString(_ItemToCraft.Recipe);
		
		if(TestRessource(_itemRessourceNeeded) == true) //Test if all ressources are available
		{
			ItemInventory.AddItem (_ItemToCraft);
			_ItemToCraft.NbrCrafted++;
			SpendRessource(_itemRessourceNeeded);
			_PlayerHUD.AddChatLog("[CRAF] " + _ItemToCraft.Name + " has been crafted");
		}
		
		
	}
	
	public static void SpendRessource(List<ExpenditureNeeded> _ressourceToSpend)
	{
		for(int i = 0; i <  _ressourceToSpend.Count; i++)
		{
			RessourceName RessourceIndex = (RessourceName) Enum.Parse(typeof(RessourceName), _ressourceToSpend[i].type);  

			_Inventory.RessourceList[(int)RessourceIndex].CurValue  -= _ressourceToSpend[i].nbr;

		}
	}
	
	public static bool TestRessource(List<ExpenditureNeeded> _ressourceToTest)
	{
		bool isRessourceAvailable = true;								//Fill the list by parsing the string
		
		for(int i = 0; i <  _ressourceToTest.Count; i++)
		{
			
			RessourceName RessourceIndex = (RessourceName) Enum.Parse(typeof(RessourceName), _ressourceToTest[i].type);  

			if( _Inventory.RessourceList[(int)RessourceIndex].CurValue  < _ressourceToTest[i].nbr)
			{
				isRessourceAvailable = false;
			}
		}
		return isRessourceAvailable;
	}
	
	public static List<ExpenditureNeeded> parseString(string __string)
	{
		List<ExpenditureNeeded> __curList = new List<ExpenditureNeeded>();
		char __delimiter  = '+';
		char __delimiter2 = '*';
		ExpenditureNeeded __ParsedString;
		
		string[] __compoundsString = __string.Split (__delimiter); //Parse the input __string with the __delimiter char
		
			for(int i = 0; i < __compoundsString.Length; i++)
			{
				
				string[] __nbrAndCompound = __compoundsString[i].Split (__delimiter2); //Parse the input __string with the __delimiter char
				//Debug.Log ("Length :" + __compoundsString.Length);
				//Debug.Log (" 0 : " + __nbrAndCompound[0]);
				//Debug.Log (" 1 : " + __nbrAndCompound[1]);
			
				__ParsedString.nbr = int.Parse(__nbrAndCompound[0]);
				__ParsedString.type = __nbrAndCompound[1];
				__curList.Add(__ParsedString);
			}
		return __curList;
	}
	
	public static List<string> parseStringToListString(string __string)
	{
		List<string>		   ListString = new List<string>();
		List<ExpenditureNeeded> ListRessourceToListString  = new List<ExpenditureNeeded>();	//Declare a list that contain all the ressource needed
		ListRessourceToListString = parseString(__string);
		
		for(int i = 0; i < ListRessourceToListString.Count; i++)
		{
			ListString.Add (ListRessourceToListString[i].type + " : " + ListRessourceToListString[i].nbr);	
		}
		
		return ListString;
		
		
	}
	
}
