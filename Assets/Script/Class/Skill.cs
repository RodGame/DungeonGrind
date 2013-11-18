using UnityEngine;
using System.Collections;

public class Skill {
	
	private string _name;
	private bool _isUnlocked;
	private int  _level = 1;
	private float  _curExp;
	
	
	public string Name
	{
		get {return _name; }
		set {_name = value; }
	}
	
	public bool IsUnlocked
	{
		get {return _isUnlocked; }
		set {_isUnlocked = value; }
	}
	
	public int Level
	{
		get {return _level; }
		set {_level = value; }
	}
	
	public float CurExp
	{
		get {return _curExp; }
		set {_curExp = value; }
	}
}

// Enumeration of all Skills
public enum SkillName {
	Constitution,
	Fighter,
	IceMage,
	FireMage,
	Lumberjack,
	Miner,
	Crafter
}