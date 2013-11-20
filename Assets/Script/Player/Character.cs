using UnityEngine;
using System.Collections;
using System; // For Enum class;
static class Character {
	
	private static  TextureManager _TextureManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<TextureManager>();
	private static  GameManager    _GameManager    = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
	
	private static int _baseHP     = 80;
	private static int _maxHP      = _baseHP;
	private static int _curHP      = _maxHP;
	private static float _hpPerLevel = 7.5f;
	private static float _hpExposent = 1.35f;
	private static int _baseMP     = 100;
	private static int _maxMP      = _baseMP;
	private static int _curMP      = _maxMP;
	private static int _baseDamage = 8;
	private static int _curDamage  = _baseDamage;
	private static float _damagePerLevel = 1.0f;
	private static float _damageExposent = 1.35f;
	
	private static float _elapsedTimeManaRegen;
	private static float _regenTimer = 0.25f;
	private static int _regenValue = 2;
	private static int _influencePoints = 0;
	
	private static Task _activeTask;
	
	public static Stat[] StatList;
	public static Skill[] SkillList;
	public static Task[] TaskList;
	public static Spell[] SpellList;
	static public int CurHP
	{
		get {return _curHP; }
		set {_curHP = value; }
	}
	
	static public int MaxHP
	{
		get {return _maxHP; }
		set {_maxHP = value; }
	}
	
	static public int CurMP
	{
		get {return _curMP; }
		set {_curMP = value; }
	}
	
	static public int MaxMP
	{
		get {return _maxMP; }
		set {_maxMP = value; }
	}
	
	static public int CurDamage
	{
		get {return _curDamage; }
		set {_curDamage = value; }
	}
	
	static public int InfluencePoints
	{
		get {return _influencePoints; }
		set {_influencePoints = value; }
	}
	static public Task ActiveTask
	{
		get {return _activeTask; }
		set {_activeTask = value; }
	}
	
	
	//public static Task ActiveTask = TaskList[(int)TaskName.TaskIntro];
	
	
	public static void IniCharacter()
	{
		StatList  = new Stat [Enum.GetValues(typeof(StatName) ).Length];	
		SkillList = new Skill[Enum.GetValues(typeof(SkillName)).Length];
		TaskList  = new Task [Enum.GetValues(typeof(TaskName) ).Length];
		SpellList = new Spell[Enum.GetValues(typeof(SpellName)).Length];
		
		IniStatList();
		IniSkillList();
		IniTaskList();	
		IniSpellList();
	}
	
	private static void IniStatList()
	{
		for(int i = 0; i < StatList.Length; i++)
		{
			StatList[i] = new Stat();
			StatList[i].Name = ((StatName)i).ToString();
			if(StatList[i].Name == "Strength")
			{
				StatList[i].Level  = 0;
				StatList[i].CurExp = 0;
			}
			else if(StatList[i].Name == "Dexterity")
			{
				StatList[i].Level  = 1;
				StatList[i].CurExp = 1;
			}
			else if(StatList[i].Name == "Agility")
			{
				StatList[i].Level  = 1;
				StatList[i].CurExp = 5;
			}
			else if(StatList[i].Name == "Intelligence")
			{
				StatList[i].Level  = 5;
				StatList[i].CurExp = 1;
			}			
			
		}
	}
	
	private static void IniSkillList()
	{
		for(int i = 0; i < SkillList.Length; i++)
		{
			SkillList[i] = new Skill();
			SkillList[i].Name = ((SkillName)i).ToString();
			if(SkillList[i].Name == "Crafter")
			{
				SkillList[i].IsUnlocked = false;
				//SkillList[i].Level  = 0;
				SkillList[i].CurExp = 0;
			}
			else if(SkillList[i].Name == "Lumberjack")
			{
				SkillList[i].IsUnlocked = true;
				//SkillList[i].Level  = 0;
				SkillList[i].CurExp = 0;
			}
			else if(SkillList[i].Name == "Miner")
			{
				SkillList[i].IsUnlocked = true;
				//SkillList[i].Level  = 0;
				SkillList[i].CurExp = 0;
			}
			else if(SkillList[i].Name == "Fighter")
			{
				SkillList[i].IsUnlocked = true;
				//SkillList[i].Level  = 0;
				SkillList[i].CurExp = 0;
			}
			else if(SkillList[i].Name == "Constitution")
			{
				SkillList[i].IsUnlocked = true;
				//SkillList[i].Level  = 0;
				SkillList[i].CurExp = 0;
			}
			else if(SkillList[i].Name == "IceMage")
			{
				SkillList[i].Name     = "Ice Mage";
				SkillList[i].IsUnlocked = true;
				//SkillList[i].Level  = 0;
				SkillList[i].CurExp = 0;
			}
			else if(SkillList[i].Name == "FireMage")
			{
				SkillList[i].Name     = "Fire Mage";
				SkillList[i].IsUnlocked = true;
				//SkillList[i].Level  = 0;
				SkillList[i].CurExp = 0;
			}
		}
	}
	
	private static void IniTaskList()
	{
		string _taskName;
		//int    _mainQuestNbr = 1;
		for(int i = 0; i < TaskList.Length; i++)
		{
			_taskName = ((TaskName)i).ToString();
			
			TaskList[i] = new Task();
			TaskList[i].MissionId = i;
			if(_taskName == "MainQuest0")
			{
				TaskList[i].Name = _taskName;	
				TaskList[i].Title = "Talk to Spartan";
				TaskList[i].Description = "Maybe it's the big sign over it's head, but I feel like he want me to talk to him";
				TaskList[i].Requirement = "None";				
				TaskList[i].Reward = "[ITEM]1*Hammer";
				TaskList[i].MissionType = 1;
			}
			else if(_taskName == "MainQuest1")
			{
				TaskList[i].Name = _taskName;
				TaskList[i].Title = "Repair the broken cart behind Spartan";
				TaskList[i].Description = "It seems like I need to repair that wreck. Why is he giving order to me?";
				TaskList[i].Requirement = "None";
				TaskList[i].Reward = "[RESS]50*Wood";
				TaskList[i].MissionType = 1;
			}
			else if(_taskName == "MainQuest2")
			{
				TaskList[i].Name = _taskName;
				TaskList[i].Title = "Build a crafting table";
				TaskList[i].Description = "Now he wants me to build a crafting table. Well, at least that seems cool and last mission gave me some woods!";
				TaskList[i].Requirement = "[BUIL]1*CraftingTable";
				TaskList[i].Reward = "[DISS]Spartan,[RESS]20*Coin";
				TaskList[i].MissionType = 1;
			}
			else if(_taskName == "MainQuest3")
			{
				TaskList[i].Name = _taskName;
				TaskList[i].Title = "Clear dungeon level 1";
				TaskList[i].Description = "This guy is cool, he gave me coins! Now what are they for?? I think I should equip my sword before entering the crypt...";
				TaskList[i].Requirement = "[DUNG]2*Level";
				TaskList[i].Reward = "[DISS]Spartan,[ITEM]1*RockAxe";
				TaskList[i].MissionType = 1;
			}
			else if(_taskName == "MainQuest4")
			{
				TaskList[i].Name = _taskName;
				TaskList[i].Title = "Kill 2 Spider Queen";
				TaskList[i].Description = "I already feel stronger from fighting those spiders. I'll keep fighting until I can kill these spiders at dungeon level 7!";
				TaskList[i].Requirement = "[KILL]2*SpiderQueen";
				TaskList[i].Reward = "[DISS]Spartan,[ITEM]1*RockPickaxe";
				TaskList[i].MissionType = 1;
			}
			else if(_taskName == "MainQuest5")
			{
				TaskList[i].Name = _taskName;
				TaskList[i].Title = "Kill 1 Skeleton Toon";
				TaskList[i].Description = "Spartan told me that I would need to upgrade the crypt to fight the Skeletons. I will look at the dungeon entrance for it.";
				TaskList[i].Requirement = "[KILL]1*SkeletonToon";
				TaskList[i].Reward = "[DISS]Spartan";
				TaskList[i].MissionType = 1;
			}
			/*else if(_taskName == "MainQuest6")
			{
				TaskList[i].Name = _taskName;
				TaskList[i].Title = "Kill 1 Skeleton Toon";
				TaskList[i].Description = "Spartan told me that I would need to upgrade the crypt to fight the Skeletons. I will look at the dungeon entrance for it.";
				TaskList[i].Requirement = "[KILL]1*SkeletonToon";
				TaskList[i].Reward = "[DISS]Spartan";
				TaskList[i].MissionType = 1;
			}
			else if(_taskName == "MainQuest7")
			{
				TaskList[i].Name = _taskName;
				TaskList[i].Title = "Kill 1 Skeleton Toon";
				TaskList[i].Description = "Spartan told me that I would need to upgrade the crypt to fight the Skeletons. I will look at the dungeon entrance for it.";
				TaskList[i].Requirement = "[KILL]1*SkeletonToon";
				TaskList[i].Reward = "[DISS]Spartan";
				TaskList[i].MissionType = 1;
			}
			else if(_taskName == "MainQuest8")
			{
				TaskList[i].Name = _taskName;
				TaskList[i].Title = "Kill 1 Skeleton Toon";
				TaskList[i].Description = "Spartan told me that I would need to upgrade the crypt to fight the Skeletons. I will look at the dungeon entrance for it.";
				TaskList[i].Requirement = "[KILL]1*SkeletonToon";
				TaskList[i].Reward = "[DISS]Spartan";
				TaskList[i].MissionType = 1;
			}
			else if(_taskName == "MainQuest9")
			{
				TaskList[i].Name = _taskName;
				TaskList[i].Title = "Kill 1 Skeleton Toon";
				TaskList[i].Description = "Spartan told me that I would need to upgrade the crypt to fight the Skeletons. I will look at the dungeon entrance for it.";
				TaskList[i].Requirement = "[KILL]1*SkeletonToon";
				TaskList[i].Reward = "[DISS]Spartan";
				TaskList[i].MissionType = 1;
			}
			else if(_taskName == "MainQuest10")
			{
				TaskList[i].Name = _taskName;
				TaskList[i].Title = "Kill 1 Skeleton Toon";
				TaskList[i].Description = "Spartan told me that I would need to upgrade the crypt to fight the Skeletons. I will look at the dungeon entrance for it.";
				TaskList[i].Requirement = "[KILL]1*SkeletonToon";
				TaskList[i].Reward = "[DISS]Spartan";
				TaskList[i].MissionType = 1;
			}
			else if(_taskName == "MainQuest10")
			{
				TaskList[i].Name = _taskName;
				TaskList[i].Title = "NOT FOUND";
				TaskList[i].Description = "NOT FOUND";
				TaskList[i].Requirement = "None";
				TaskList[i].Reward = "None";
				TaskList[i].MissionType = 1;
			}*/
		}
	}
	
	private static void IniSpellList()
	{
		for(int i = 0; i < SpellList.Length; i++)
		{
			SpellList[i] = new Spell();
			SpellList[i].Name = ((SpellName)i).ToString();
			if(SpellList[i].Name == "IceBolt")
			{
				SpellList[i].Name = "IceBolt";
				SpellList[i].Category  = "Ice";
				SpellList[i].IsUnlocked = true;
				SpellList[i].SpellIcon  = _TextureManager.Icon_Spell_IceBolt;
				
				SpellList[i].DamageBase      = 3;
				SpellList[i].DamagePerLevel  = 2.0f;
				SpellList[i].CdBase          = 0.45f;
				SpellList[i].CdPerLevel      = -0.01f;
				//SpellList[i].RangeBase       = 5.0f;
				//SpellList[i].RangePerLevel   = 0.25f;
				SpellList[i].ManaBase        = 15;
				SpellList[i].ManaPerLevel    = -0.4f;
				
			}
			else if(SpellList[i].Name == "FireBat")
			{
				SpellList[i].Name = "FireBat";
				SpellList[i].Category  = "Fire";
				SpellList[i].IsUnlocked = true;
				SpellList[i].SpellIcon  = _TextureManager.Icon_Spell_FireBat;
				
				SpellList[i].DamageBase      = 6;
				SpellList[i].DamagePerLevel  = 1.6f;
				SpellList[i].CdBase          = 0.55f;
				SpellList[i].CdPerLevel      = -0.015f;
				//SpellList[i].RangeBase       = 5.0f;
				//SpellList[i].RangePerLevel   = 0.25f;
				SpellList[i].ManaBase        = 30;
				SpellList[i].ManaPerLevel    = -0.75f;
			}
			
			
		}
	}
	
	public static void ProcAction(string _actionProced)
	{
		switch(_actionProced)
		{
			case "Woodcutting": 
				GiveExpToSkill(SkillList[(int)SkillName.Lumberjack],25.0f);
				break;
			case "Mining":
				GiveExpToSkill(SkillList[(int)SkillName.Miner],25.0f);
				break;
			case "Default":
				break;
		}
	}
	
	public static void GiveExpToSkill(Skill _SkillToInc, float _expToGive) // TODO: Move this in each skill
	{
		_SkillToInc.CurExp = _SkillToInc.CurExp + _expToGive;	
		if(_SkillToInc.CurExp >= 100.0f)
		{
			 LevelupSkill(_SkillToInc);
		}
	}
	
	public static void LevelupSkill(Skill _SkillToLevel)
	{
		
		int _valuePreLevel = 0;
		int _valuPostLevel = 0;
		
		string _UpdatedStat = "StatNotFound";
		
		_SkillToLevel.CurExp = 0;	
		_SkillToLevel.Level++;
		if(_SkillToLevel.Name == "Fighter")
		{
			_valuePreLevel = Character.CurDamage;
			UpdateStats();
			_valuPostLevel = Character.CurDamage;
			_UpdatedStat = "Damage";
			_GameManager.AddChatLogHUD ("[SKIL] " + _SkillToLevel.Name + " is now level " + _SkillToLevel.Level + " ! " + _UpdatedStat + " " + _valuePreLevel + " -> " + _valuPostLevel);	
		}
		else if(_SkillToLevel.Name == "Constitution")
		{
			_valuePreLevel = Character._maxHP;
			UpdateStats();
			_valuPostLevel = Character._maxHP;
			_curHP += _valuPostLevel - _valuePreLevel;
			_UpdatedStat = "HP";
			_GameManager.AddChatLogHUD ("[SKIL] " + _SkillToLevel.Name + " is now level " + _SkillToLevel.Level + " ! " + _UpdatedStat + " " + _valuePreLevel + " -> " + _valuPostLevel);	
		}
		else if(_SkillToLevel.Name == "IceMage")
		{
			MagicBook.UpdateSpellStats();
			_GameManager.AddChatLogHUD ("[SKIL] " + _SkillToLevel.Name + " is now level " + _SkillToLevel.Level + " ! Damage increased" );	
		}
		else if(_SkillToLevel.Name == "FireMage")
		{
			MagicBook.UpdateSpellStats();
			_GameManager.AddChatLogHUD ("[SKIL] " + _SkillToLevel.Name + " is now level " + _SkillToLevel.Level + " ! Damage increased" );	
		}
		
		
	}
	
	public static void UpdateStats()
	{
		_maxHP     = _baseHP     + (int)(_hpPerLevel      * Mathf.Pow (SkillList[(int)SkillName.Constitution].Level, _hpExposent));
		_curDamage = _baseDamage + (int)( _damagePerLevel * Mathf.Pow(SkillList[(int)SkillName.Fighter      ].Level, _damageExposent));
		
	}
	
	public static void ActivateTask(Task _TaskToUnlock)
	{
		_activeTask = _TaskToUnlock;
	}
	
	/*public static void FinishTask(Task _TaskToFinish)
	{
		_TaskToFinish.IsFinished = true;
		if(_activeTask == _TaskToFinish)
		{
			_activeTask = null;
		}
		_activeTask = _TaskToFinish;
	}*/
	
	public static void SetActiveTask(Task _TaskToActive)
	{
		_activeTask = _TaskToActive;
	}
	
	public static void LoseHp(int _hpToLose)
	{
		_curHP -= _hpToLose;
		Character.GiveExpToSkill(Character.SkillList[(int)SkillName.Constitution],_hpToLose/(Mathf.Pow (Character.SkillList[(int)SkillName.Constitution].Level, 1.4f)));	
		if(_curHP <= 0)
		{
			_curHP = 0;
			Die ();
		}
	}
	
	public static void RefillHP()
	{
		_curHP = _maxHP;
	}
	
	public static void GiveMp(int _mpToGive)
	{
		_curMP += _mpToGive;
		if(_curMP >= _maxMP)
		{
			_curMP = _maxMP;
		}
	}
	
	public static void LoseMp(int _mpToLose)
	{
		_curMP -= _mpToLose;
		if(_curMP <= 0)
		{
			_curMP = 0;
		}
	}
	
	public static void RefillMP()
	{
		_curMP = _maxMP;
	}
	
	public static void RegenMP()
	{
		_elapsedTimeManaRegen += Time.deltaTime;
		
		if(_elapsedTimeManaRegen >= _regenTimer)
		{
			_elapsedTimeManaRegen -= _regenTimer;
			GiveMp(_regenValue);
		}
	}
	
	public static void Die()
	{
		_GameManager.AddChatLogHUD("[DIED] You died.");
		_GameManager.ChangeDungeonState("Lose");	
	}
	
	public static void GainInfluences(int _pointToAdd)
	{
		_influencePoints += _pointToAdd;
	}
	
	public static int CalculateSkillLevel()
	{
		int _totalSkillLvl = 0;
		for(int i = 0; i <  Character.SkillList.Length; i++)
		{
			if(Character.SkillList[i].IsUnlocked == true)
			{
				_totalSkillLvl += Character.SkillList[i].Level;
			}
		}	
		return _totalSkillLvl;
	}
	
	public static int CalculateSavedSkillLevel()
	{
		int _totalSkillLvl = 0;
		for(int i = 0; i <  Character.SkillList.Length; i++)
		{
			if(Character.SkillList[i].IsUnlocked == true)
			{
				_totalSkillLvl += PlayerPrefs.GetInt ("SkillLvl_" + Character.SkillList[i].Name);
			}
		}	
		return _totalSkillLvl;
	}
}
