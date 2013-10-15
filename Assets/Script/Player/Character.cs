using UnityEngine;
using System.Collections;
using System; // For Enum class;
static class Character {
	
	private static int _curHP     = 15;
	private static int _curDamage = 5;
	private static GameManager _GameManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
	public static Stat[] StatList;
	public static Skill[] SkillList;
	public static Task[] TaskList;
	
	static public int CurHP
	{
		get {return _curHP; }
		set {_curHP = value; }
	}
	
	static public int CurDamage
	{
		get {return _curDamage; }
		set {_curDamage = value; }
	}
	
	//public static Task ActiveTask = TaskList[(int)TaskName.Spartan0];
	public static Task ActiveTask;
	
	public static void IniCharacter()
	{
		StatList  = new Stat [Enum.GetValues(typeof(StatName) ).Length];	
		SkillList = new Skill[Enum.GetValues(typeof(SkillName)).Length];
		TaskList  = new Task [Enum.GetValues(typeof(TaskName) ).Length];
		
		IniStatList();
		IniSkillList();
		IniTaskList();	
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
				SkillList[i].Unlocked = false;
				SkillList[i].Level  = 0;
				SkillList[i].CurExp = 0;
			}
			else if(SkillList[i].Name == "Lumberjack")
			{
				SkillList[i].Unlocked = false;
				SkillList[i].Level  = 0;
				SkillList[i].CurExp = 0;
			}
			else if(SkillList[i].Name == "Miner")
			{
				SkillList[i].Unlocked = false;
				SkillList[i].Level  = 0;
				SkillList[i].CurExp = 0;
			}
			else if(SkillList[i].Name == "Fighter")
			{
				SkillList[i].Unlocked = false;
				SkillList[i].Level  = 0;
				SkillList[i].CurExp = 0;
			}
		}
	}
	
	private static void IniTaskList()
	{
		string _taskName;
		for(int i = 0; i < TaskList.Length; i++)
		{
			_taskName = ((TaskName)i).ToString();
			
			TaskList[i] = new Task();
			
			if(_taskName == "Spartan0")
			{
				TaskList[i].Name = _taskName;	
				TaskList[i].Definition = "Talk to Spartan to obtain your first quest.";
				TaskList[i].Requirement = "None";				
				TaskList[i].Reward = "[ITEM]1*Hammer";
				TaskList[i].IsUnlocked = true;
				TaskList[i].IsFinished = false;
			}
			else if(_taskName == "Spartan1")
			{
				TaskList[i].Name = _taskName;
				TaskList[i].Definition = "Repair the wooden cart behind Spartan";
				TaskList[i].Requirement = "None"; //TODO Add Misc requirements
				TaskList[i].Reward = "[DISS]Spartan";
				TaskList[i].IsUnlocked = false;
				TaskList[i].IsFinished = false;
			}
			else if(_taskName == "Spartan2")
			{
				TaskList[i].Name = _taskName;
				TaskList[i].Definition = "Build a crafting table";
				TaskList[i].Requirement = "[BUIL]1*CraftingTable";
				TaskList[i].Reward = "[DISS]Spartan";
				TaskList[i].IsUnlocked = false;
				TaskList[i].IsFinished = false;
			}
			else if(_taskName == "Spartan3")
			{
				TaskList[i].Name = _taskName;
				TaskList[i].Definition = "Craft an Axe and Pickaxe and gather 10 Rock and 10 Wood";
				TaskList[i].Requirement = "[CRAF]1*RockAxe+1*RockPickaxe,[RESS]10*Rock+10*Wood";
				TaskList[i].Reward = "[DISS]Spartan,[RESS]15*Coin";
				TaskList[i].IsUnlocked = false;
				TaskList[i].IsFinished = false;
			}
			else if(_taskName == "Spartan4")
			{
				TaskList[i].Name = _taskName;
				TaskList[i].Definition = "Buy a Sword";
				TaskList[i].Requirement = "[RESC]15*Coin";
				TaskList[i].Reward = "[ITEM]1*RockSword";
				TaskList[i].IsUnlocked = false;
				TaskList[i].IsFinished = false;
			}
			
		}
	}
	
	public static void ProcAction(string _actionProced)
	{
		switch(_actionProced)
		{
			case "Woodcutting": 
				GiveExpToSkill(SkillList[(int)SkillName.Lumberjack],25);
				break;
			case "Mining":
				GiveExpToSkill(SkillList[(int)SkillName.Lumberjack],25);
				break;
			case "Default": 
				Debug.LogWarning ("No Action in GameManager");
				break;
		}
	}
	
	public static void GiveExpToSkill(Skill _SkillToInc, int _expToGive)
	{
		_SkillToInc.CurExp = _SkillToInc.CurExp + _expToGive;	
		if(_SkillToInc.CurExp >= 100)
		{
			_SkillToInc.CurExp = 0;	
			_SkillToInc.Level++;
		}
	}
	
	public static void UnlockTask(Task _TaskToUnlock)
	{
		_TaskToUnlock.IsUnlocked = true;
		Character.ActiveTask = _TaskToUnlock;
	}
	
	public static void FinishTask(Task _TaskToFinish)
	{
		_TaskToFinish.IsFinished = true;
		if(Character.ActiveTask == _TaskToFinish)
		{
			Character.ActiveTask = null;
		}
		Character.ActiveTask = _TaskToFinish;
	}
	
	public static void SetActiveTask(Task _TaskToActive)
	{
		Character.ActiveTask = _TaskToActive;
	}
	
	public static void LoseHp(int _hpToLose)
	{
		_curHP -= _hpToLose;
		if(_curHP <= 0)
		{
			_curHP = 0;
			Die ();
		}
	}
	
	public static void Die()
	{
		_GameManager.AddChatLogHUD("[DIED] You died.");
		if(_GameManager.CurZone == "Dungeon")
		{
			_GameManager.ChangeDungeonState("Lose");	
		}
	}
}
