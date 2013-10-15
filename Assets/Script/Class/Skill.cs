using UnityEngine;
using System.Collections;

public class Skill {
	
	private string _name;
	private bool _unlocked;
	private int  _level;
	private int  _curExp;
	
	
	public string Name
	{
		get {return _name; }
		set {_name = value; }
	}
	
	public bool Unlocked
	{
		get {return _unlocked; }
		set {_unlocked = value; }
	}
	
	public int Level
	{
		get {return _level; }
		set {_level = value; }
	}
	
	public int CurExp
	{
		get {return _curExp; }
		set {_curExp = value; }
	}
}

// Enumeration of all Skills
public enum SkillName {
	Crafter,
	Lumberjack,
	Miner,
	Fighter
}