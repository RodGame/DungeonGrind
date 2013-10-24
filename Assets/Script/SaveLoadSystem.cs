using UnityEngine;
using System.Collections;
using System;					  // For Enum


static class SaveLoadSystem {
	
	static GameManager _GameManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
	
	static public void Save()
	{
		_GameManager.AddChatLogHUD ("[SAVE] Game was saved.");
		
		//Set Save Bool as true
		PlayerPrefs.SetInt ("IsSaveExist",1);
		
		//Save GameManager
		PlayerPrefs.SetInt ("MaxDungeonLevel",_GameManager.MaxDungeonLevel);
		
		//Save Character
		PlayerPrefs.SetInt ("InfluencePoints", Character.InfluencePoints);
		
		// Save Skills
		for(int i = 0; i < Character.SkillList.Length; i++)
		{
			PlayerPrefs.SetInt ("SkillLvl_" + Character.SkillList[i].Name, Character.SkillList[i].Level);
			PlayerPrefs.SetFloat ("SkillExp_" + Character.SkillList[i].Name, Character.SkillList[i].CurExp);
		}
		
		// Save Ressource
		for(int i = 0; i < Inventory.RessourceList.Length; i++)
		{
			PlayerPrefs.SetInt ("RessourceCur_" + Inventory.RessourceList[i].Name, Inventory.RessourceList[i].CurValue);
			PlayerPrefs.SetInt ("RessourceMax_" + Inventory.RessourceList[i].Name, Inventory.RessourceList[i].MaxValue);
		}
		
		// Save Spells
		
		SpellName SpellIndex = (SpellName) Enum.Parse(typeof(SpellName), MagicBook.ActiveSpell.Name);
		
		PlayerPrefs.SetInt       ("ActiveSpell", (int)SpellIndex);  
		for(int i = 0; i < Character.SpellList.Length; i++)
		{
			PlayerPrefs.SetInt   ("SpellDamageLvl_" + Character.SpellList[i].Name, Character.SpellList[i].DamageLevel);
			PlayerPrefs.SetFloat ("SpellDamageExp_" + Character.SpellList[i].Name, Character.SpellList[i].DamageCurExp);
			
			PlayerPrefs.SetInt   ("SpellCdLvl_" + Character.SpellList[i].Name, Character.SpellList[i].CdLevel);
			PlayerPrefs.SetFloat ("SpellCdExp_" + Character.SpellList[i].Name, Character.SpellList[i].CdCurExp);
			
			PlayerPrefs.SetInt   ("SpellManaLvl_" + Character.SpellList[i].Name, Character.SpellList[i].ManaLevel);
			PlayerPrefs.SetFloat ("SpellManaExp_" + Character.SpellList[i].Name, Character.SpellList[i].ManaCurExp);
			
			PlayerPrefs.SetInt   ("SpellRangeLvl_" + Character.SpellList[i].Name, Character.SpellList[i].RangeLevel);
			PlayerPrefs.SetFloat ("SpellRangeExp_" + Character.SpellList[i].Name, Character.SpellList[i].RangeCurExp);
		}
		
	}
	
	static public void Load()
	{
		
		
		if(PlayerPrefs.GetInt ("IsSaveExist") == 1) //If save exist
		{
			
			//Load GameManager
			_GameManager.MaxDungeonLevel = PlayerPrefs.GetInt ("MaxDungeonLevel");
			
			//Load Character
			Character.InfluencePoints = PlayerPrefs.GetInt ("InfluencePoints");
			
			//Load Skills
			for(int i = 0; i < Character.SkillList.Length; i++)
			{
				Character.SkillList[i].Level  = PlayerPrefs.GetInt ("SkillLvl_" + Character.SkillList[i].Name);
				Character.SkillList[i].CurExp = PlayerPrefs.GetFloat ("SkillExp_" + Character.SkillList[i].Name);
			}
			Character.UpdateStats();
			
			// Save Ressource
			for(int i = 0; i < Inventory.RessourceList.Length; i++)
			{
				Inventory.RessourceList[i].CurValue = PlayerPrefs.GetInt ("RessourceCur_" + Inventory.RessourceList[i].Name);
				Inventory.RessourceList[i].MaxValue = PlayerPrefs.GetInt ("RessourceMax_" + Inventory.RessourceList[i].Name);
			}
			
			// Load Spells
			for(int i = 0; i < Character.SpellList.Length; i++)
			{
				Character.SpellList[i].DamageLevel  = PlayerPrefs.GetInt   ("SpellDamageLvl_" + Character.SpellList[i].Name);
				Character.SpellList[i].DamageCurExp = PlayerPrefs.GetFloat ("SpellDamageExp_" + Character.SpellList[i].Name);
																				 
				Character.SpellList[i].CdLevel      = PlayerPrefs.GetInt   ("SpellCdLvl_" + Character.SpellList[i].Name);
				Character.SpellList[i].CdCurExp     = PlayerPrefs.GetFloat ("SpellCdExp_" + Character.SpellList[i].Name);
																					 
				Character.SpellList[i].ManaLevel    = PlayerPrefs.GetInt   ("SpellManaLvl_" + Character.SpellList[i].Name);
				Character.SpellList[i].ManaCurExp   = PlayerPrefs.GetFloat ("SpellManaExp_" + Character.SpellList[i].Name);
																			 
				Character.SpellList[i].RangeLevel   = PlayerPrefs.GetInt   ("SpellRangeLvl_" + Character.SpellList[i].Name);
				Character.SpellList[i].RangeCurExp  = PlayerPrefs.GetFloat ("SpellRangeExp_" + Character.SpellList[i].Name);
			}
			MagicBook.UpdateSpellStats();
			MagicBook.ActiveSpell = Character.SpellList[(int)(SpellName)PlayerPrefs.GetInt("ActiveSpell")];  
			
		}
		
		
	}
	
}
