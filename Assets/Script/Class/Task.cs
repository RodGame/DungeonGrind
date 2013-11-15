using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List class;
using System;					  // For Enum

public class Task {
	
	private string  _name;
	private string  _definition;
	private string  _reward;
	private string  _requirement;
	private bool  _isUnlocked = false;
	private bool  _isFinished = false;
	private GameManager _GameManager = GameObject.FindGameObjectWithTag("GameMaster"  ).GetComponent<GameManager>();
	
	public string Name
	{
		get {return _name; }
		set {_name = value; }
	}
	
	public string Definition
	{
		get {return _definition; }
		set {_definition = value; }
	}
	
	public string Reward
	{
		get {return _reward; }
		set {_reward = value; }
	}
	
	public string Requirement
	{
		get {return _requirement;}
		set {_requirement = value;}
	}
	
	public bool IsUnlocked
	{
		get {return _isUnlocked; }
	}
	
	public bool IsFinished
	{
		get {return _isFinished; }
	}
	
	public bool TestForCompletion()
	{
		bool _isTaskCompleted = true;
		char _differentTypeDelimiter  = ',';
		/*char _typeDelimiter1  = '[';
		char _typeDelimiter2  = ']';
		char _delimiter1 = '+';
		char _delimiter2 = '*';*/
		string _curType;
		string _curReq;
		string[] _curRequirementPart = Requirement.Split (_differentTypeDelimiter); //Parse the Requirement string the _differentTypeDelimiter(,) char
		List<Utility.ParsedString> _ExpenditureNeeded  = new List<Utility.ParsedString>();	//Declare a list that contain all the ressource needed
		
		
		for(int i = 0; i < _curRequirementPart.Length; i++)
		{
			_curType = _curRequirementPart[i].Substring(0,6);
			_curReq  = _curRequirementPart[i].Substring(6);
			_ExpenditureNeeded = Utility.parseString(_curReq);
			switch(_curType)
			{
				case "[BUIL]":
					for(int j = 0; j <  _ExpenditureNeeded.Count; j++)
					{
						BuildingName BuildingIndex = (BuildingName) Enum.Parse(typeof(BuildingName), _ExpenditureNeeded[j].type);  
						Debug.Log (_ExpenditureNeeded[j].nbr + "/" + Inventory.BuildingList[(int)BuildingIndex].NbrBuilt + " " +  _ExpenditureNeeded[j].type);
						if( Inventory.BuildingList[(int)BuildingIndex].NbrBuilt  < _ExpenditureNeeded[j].nbr)
						{
							_isTaskCompleted = false;
						}
					}
					break;
				
				case "[RESS]":
					if(CraftSystem.TestRessource(_ExpenditureNeeded) == false) //Test if all ressources are available
					{
							_isTaskCompleted = false;
					}
					break;
				
				case "[CRAF]":
				{
					for(int j = 0; j <  _ExpenditureNeeded.Count; j++)
					{
						WeaponName WeaponIndex = (WeaponName) Enum.Parse(typeof(WeaponName), _ExpenditureNeeded[j].type); 
						if( Inventory.WeaponList[(int)WeaponIndex].NbrCrafted  < _ExpenditureNeeded[j].nbr)
						{
							_isTaskCompleted = false;
						}
					}
					break;
				}
				default:
				{
					Debug.Log ("WARNING in Task.cs - Trying to test for an unknown condition: " + _curType);
					break;
				}
			}
		}
		return _isTaskCompleted;
	}
	
	public void Unlock()
	{
		this._isUnlocked = true;
		GA.API.Design.NewEvent("Task:" + this._name + ":IsUnlocked", Character.SkillList[(int)SkillName.Fighter].Level + Character.SkillList[(int)SkillName.IceMage].Level);
	}
	
	public void Finish()
	{
		_GameManager.ClaimReward (this.Reward);
		this._isFinished = true;
		GA.API.Design.NewEvent("Task:" + this._name + ":IsFinished", Character.SkillList[(int)SkillName.Fighter].Level + Character.SkillList[(int)SkillName.IceMage].Level);
	}
}

// Enumeration of all Tasks
public enum TaskName {
	Spartan4,
	Spartan3,
	Spartan2,
	Spartan1,
	TaskIntro
}
