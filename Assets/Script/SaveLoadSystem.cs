using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List class;
using System;					  // For Enum


static class SaveLoadSystem {
	
	public static List<BuildingInfo> LoadedBuildingInfo = new List<BuildingInfo>();
	static GameManager _GameManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
	
	static public int Spartan_TaskState   = 0;
	static public int Spartan_CurrentTask = 0;
	
	
	public struct BuildingInfo
	{
		public int id;
		public int buildingId;
		public Vector3 Position;
		public Quaternion Rotation;
	}
	
	static public void Save()
	{
		int _dummyForBool = 0;
		
		_GameManager.AddChatLogHUD ("[SAVE] Game was saved.");
		
		//Set Save Bool as true
		PlayerPrefs.SetInt ("IsSaveExist",1);
		
		//Save GameManager
		PlayerPrefs.SetInt ("MaxDungeonLevel",_GameManager.MaxDungeonLevel);
		
		//Save Character
		PlayerPrefs.SetInt ("InfluencePoints", Character.InfluencePoints);
		
		// Save Inventory
		for(int i = 0; i < ItemInventory.InventoryList.Length; i++)
		{
			if(ItemInventory.InventoryList[i].slotWeapon != null)
			{
				PlayerPrefs.SetInt   ("ItemSlot_isFull_" + i.ToString (), 1);
				PlayerPrefs.SetInt   ("ItemSlot_ID_" + i.ToString (), ItemInventory.InventoryList[i].slotWeapon.IdWeapon);
				PlayerPrefs.SetInt   ("ItemSlot_Level_" + i.ToString (), ItemInventory.InventoryList[i].slotWeapon.Level);
				PlayerPrefs.SetFloat ("ItemSlot_CurExp_" + i.ToString (), ItemInventory.InventoryList[i].slotWeapon.CurExp);
			}
			else
			{
				PlayerPrefs.SetInt   ("ItemSlot_isFull_" + i.ToString (), 0);
				//PlayerPrefs.SetInt ("ItemInventorySlot_" + i.ToString (), 0);
			}
		}
		
		// Save Equipped Item
		if(ItemInventory.EquippedWeapon != null)
		{
			PlayerPrefs.SetInt   ("EquippedWeapon_isFull", 1);
			PlayerPrefs.SetInt   ("EquippedWeapon_ID"    , ItemInventory.EquippedWeapon.IdWeapon);
			PlayerPrefs.SetInt   ("EquippedWeapon_Level" , ItemInventory.EquippedWeapon.Level);
			PlayerPrefs.SetFloat ("EquippedWeapon_CurExp", ItemInventory.EquippedWeapon.CurExp);
		}
		else
		{
			PlayerPrefs.SetInt   ("EquippedWeapon_isFull_", 0);
		}
		
		
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
		
		// Save Upgrades
		for(int i = 0; i < DungeonLevelPool.DungeonUpgradeList.Length; i++)
		{
			PlayerPrefs.SetInt   ("DungeonUpgradeIsUnlocked_" + DungeonLevelPool.DungeonUpgradeList[i].Name, Convert.ToInt32(DungeonLevelPool.DungeonUpgradeList[i].IsUnlocked));
			PlayerPrefs.SetInt   ("DungeonUpgradeIsEnabled_"  + DungeonLevelPool.DungeonUpgradeList[i].Name, Convert.ToInt32(DungeonLevelPool.DungeonUpgradeList[i].IsEnabled));
		}
		
		// Save Killcount
		for(int i = 0; i < Bestiary.MonsterList.Length; i++)
		{
			PlayerPrefs.SetInt   ("MonsterKillcount_" + Bestiary.MonsterList[i].Name, Bestiary.MonsterList[i].NbrKilled);	
		}
		
		// Save Current Dungeon Setting
		PlayerPrefs.SetInt   ("CurDungeonLevel", _GameManager.CurDungeonParameters.level);	
		if(_GameManager.CurDungeonParameters.isHardcore)
		{
			_dummyForBool = 1;
		}
		PlayerPrefs.SetInt   ("CurDungeonIsHardcore", _dummyForBool);
		
		if(_GameManager.CurDungeonParameters.isWave)
		{
			_dummyForBool = 1;
		}
		PlayerPrefs.SetInt   ("CurDungeonIsWave", _dummyForBool);	
		
		if(Application.loadedLevelName == "Camp")
		{
			SaveBuildings();
		}
		
		//Save Tasks
		for(int i = 0; i < Character.TaskList.Length; i++)
		{
			PlayerPrefs.SetInt   ("TaskIsUnlocked_" + i, Convert.ToInt32(Character.TaskList[i].IsUnlocked));
			PlayerPrefs.SetInt   ("TaskIsEnabled_"  + i, Convert.ToInt32(Character.TaskList[i].IsFinished));
		}
		
		//Save Spartan State
		if(Application.loadedLevelName == "Camp")
		{
			SpartanInteract _SpartanInteract = GameObject.FindGameObjectWithTag("Spartan").GetComponent<SpartanInteract>();
			PlayerPrefs.SetInt("Spartan_TaskState", _SpartanInteract._taskState);
			PlayerPrefs.SetInt("Spartan_CurrentTask",_SpartanInteract._currentTask);
		}
		
		GameAnalyticsManager.SendDatas();
	}
	
	static public void SaveBuildings()
	{
		Debug.Log ("Saved " + BuildSystem.CreatedBuildingList.Count + " Buildings");
		
		// Save Building created
		PlayerPrefs.SetInt("BuildingCreated_numberBuilding", BuildSystem.CreatedBuildingList.Count);
		for(int i = 0; i < BuildSystem.CreatedBuildingList.Count; i++)
		{
			BuildingManager _BuildingManager = BuildSystem.CreatedBuildingList[i].GetComponent<BuildingManager>();
			
			PlayerPrefs.SetInt("BuildingCreated_" + i + "_id", _BuildingManager.buildingId);
			PlayerPrefs.SetFloat("BuildingCreated_" + i + "_positionX", BuildSystem.CreatedBuildingList[i].transform.position.x);
			PlayerPrefs.SetFloat("BuildingCreated_" + i + "_positionY", BuildSystem.CreatedBuildingList[i].transform.position.y);
			PlayerPrefs.SetFloat("BuildingCreated_" + i + "_positionZ", BuildSystem.CreatedBuildingList[i].transform.position.z);
			PlayerPrefs.SetFloat("BuildingCreated_" + i + "_rotationX", BuildSystem.CreatedBuildingList[i].transform.rotation.x);
			PlayerPrefs.SetFloat("BuildingCreated_" + i + "_rotationY", BuildSystem.CreatedBuildingList[i].transform.rotation.y);
			PlayerPrefs.SetFloat("BuildingCreated_" + i + "_rotationZ", BuildSystem.CreatedBuildingList[i].transform.rotation.z);
			PlayerPrefs.SetFloat("BuildingCreated_" + i + "_rotationW", BuildSystem.CreatedBuildingList[i].transform.rotation.w);	
		}
		
		// Save Number of building created
		for(int i = 0; i < Inventory.BuildingList.Length; i++)
		{
			PlayerPrefs.SetInt("Building_NbrCreated_" + i, Inventory.BuildingList[i].NbrBuilt);
		}
		
	}
	
	static public void Load()
	{
		
		if(PlayerPrefs.GetInt ("IsSaveExist") == 1) //If save exist
		{
			
			// Load GameManager
			_GameManager.MaxDungeonLevel = PlayerPrefs.GetInt ("MaxDungeonLevel");
			
			// Load Character
			Character.InfluencePoints = PlayerPrefs.GetInt ("InfluencePoints");
			
			int   _curWeaponId;
			int   _curWeaponLevel;
			float _curWeaponCurExp;
				
			// Load Inventory
			for(int i = 0; i < ItemInventory.InventoryList.Length; i++)
			{
				if(PlayerPrefs.GetInt   ("ItemSlot_isFull_" + i.ToString ()) == 1) // If there is an item saved in the slot
				{
					_curWeaponId     = PlayerPrefs.GetInt   ("ItemSlot_ID_"    + i.ToString ());
					_curWeaponLevel  = PlayerPrefs.GetInt   ("ItemSlot_Level_" + i.ToString ());
					_curWeaponCurExp = PlayerPrefs.GetFloat ("ItemSlot_CurExp_" + i.ToString ());
					ItemInventory.AddItem (_curWeaponId, _curWeaponLevel, _curWeaponCurExp);
				}
				else
				{
					ItemInventory.InventoryList[i].isSlotFull = false;
					ItemInventory.InventoryList[i].slotWeapon = null;
					ItemInventory.InventoryList[i].slotCount  = 0;
				}
			}
			
			// Load Equipped Item
			if(PlayerPrefs.GetInt("EquippedWeapon_isFull") == 1) // If there is a saved equipped item
			{
				PlayerPrefs.GetInt   ("EquippedWeapon_isFull", 1);
				_curWeaponId     = PlayerPrefs.GetInt   ("EquippedWeapon_ID");
				_curWeaponLevel  = PlayerPrefs.GetInt   ("EquippedWeapon_Level");
				_curWeaponCurExp = PlayerPrefs.GetFloat ("EquippedWeapon_CurExp");
				ItemInventory.EquipWeapon (_curWeaponId, _curWeaponLevel, _curWeaponCurExp);
				
			}
			else
			{
				ItemInventory.EquipWeapon(null);
			}
			
			
			// Load Skills
			for(int i = 0; i < Character.SkillList.Length; i++)
			{
				Character.SkillList[i].Level  = PlayerPrefs.GetInt ("SkillLvl_" + Character.SkillList[i].Name);
				Character.SkillList[i].CurExp = PlayerPrefs.GetFloat ("SkillExp_" + Character.SkillList[i].Name);
			}
			Character.UpdateStats();
			
			// Load Ressource
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
			
			// Load Upgrades
			for(int i = 0; i < DungeonLevelPool.DungeonUpgradeList.Length; i++)
			{
				DungeonLevelPool.DungeonUpgradeList[i].IsUnlocked = (PlayerPrefs.GetInt("DungeonUpgradeIsUnlocked_" + DungeonLevelPool.DungeonUpgradeList[i].Name) == 1);
				DungeonLevelPool.DungeonUpgradeList[i].IsEnabled  = (PlayerPrefs.GetInt("DungeonUpgradeIsEnabled_"  + DungeonLevelPool.DungeonUpgradeList[i].Name) == 1);
			}
			
			// Load Killcount
			for(int i = 0; i < Bestiary.MonsterList.Length; i++)
			{
				Bestiary.MonsterList[i].NbrKilled = PlayerPrefs.GetInt   ("MonsterKillcount_" + Bestiary.MonsterList[i].Name);	
			}
			/*
			// Load Curren Dungeon Setting(Not working perfectly)
			 PlayerHUD.DungeonParameters _SavedDungeonParameters;
			_SavedDungeonParameters.level      = PlayerPrefs.GetInt   ("CurDungeonLevel");
			_SavedDungeonParameters.isHardcore = (PlayerPrefs.GetInt   ("CurDungeonIsHardcore") == 1);
			_SavedDungeonParameters.isWave     = (PlayerPrefs.GetInt   ("CurDungeonIsWave") == 1);
			
			_GameManager.CurDungeonParameters = _SavedDungeonParameters;
			*/
			
			//Load Tasks
			for(int i = 0; i < Character.TaskList.Length; i++)
			{
				if(PlayerPrefs.GetInt("TaskIsUnlocked_" + i) == 1)
				{
					Character.TaskList[i].Unlock();	
				}
				
				if(PlayerPrefs.GetInt("TaskIsEnabled_"  + i) == 1)
				{
					Character.TaskList[i].Finish(); 
				}
			}
			

			
			// Load Nbr building created
			for(int i = 0; i < Inventory.BuildingList.Length; i++)
			{
				Inventory.BuildingList[i].NbrBuilt = PlayerPrefs.GetInt("Building_NbrCreated_" + i);
			}
				
			
			// Load buildings
			LoadBuildings();
			
		}
	}
	
	static public void LoadSpartanState()
	{
		//Load Spartan State
		Spartan_TaskState   = PlayerPrefs.GetInt("Spartan_TaskState");
		Spartan_CurrentTask = PlayerPrefs.GetInt("Spartan_CurrentTask");	
	}
	
	static public void LoadBuildings()
	{
		if(PlayerPrefs.GetInt ("IsSaveExist") == 1) //If save exist
		{
			// Load Building created
			BuildingInfo _DummyBuildingInfo = new BuildingInfo();
			LoadedBuildingInfo = new List<BuildingInfo>();
			
			int _numberBuilding = PlayerPrefs.GetInt("BuildingCreated_numberBuilding");
			//Debug.Log ("Loaded " + _numberBuilding + " Buildings");
			for(int i = 0; i < _numberBuilding; i++)
			{
				int   _dummyId   = PlayerPrefs.GetInt("BuildingCreated_" + i + "_id");
				
				float _dummyPosX = PlayerPrefs.GetFloat("BuildingCreated_" + i + "_positionX");
				float _dummyPosY = PlayerPrefs.GetFloat("BuildingCreated_" + i + "_positionY");
				float _dummyPosZ = PlayerPrefs.GetFloat("BuildingCreated_" + i + "_positionZ");
				
				float _dummyRotX = PlayerPrefs.GetFloat("BuildingCreated_" + i + "_rotationX");
				float _dummyRotY = PlayerPrefs.GetFloat("BuildingCreated_" + i + "_rotationY");
				float _dummyRotZ = PlayerPrefs.GetFloat("BuildingCreated_" + i + "_rotationZ");
				float _dummyRotW = PlayerPrefs.GetFloat("BuildingCreated_" + i + "_rotationW");
				
				_DummyBuildingInfo.id = i;
				_DummyBuildingInfo.buildingId = _dummyId;
				_DummyBuildingInfo.Position = new Vector3(_dummyPosX, _dummyPosY, _dummyPosZ);
				_DummyBuildingInfo.Rotation = new Quaternion(_dummyRotX, _dummyRotY, _dummyRotZ, _dummyRotW);
				
				
				LoadedBuildingInfo.Add (_DummyBuildingInfo);
			}
		}
	}
}
