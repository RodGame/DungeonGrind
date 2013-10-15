using UnityEngine;
using System.Collections;

public class Stat {
	
	private string _name;
	private int  _level;
	private float  _curExp;
	
	
	public string Name
	{
		get {return _name; }
		set {_name = value; }
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

// Enumeration of all Stats
public enum StatName {
	Strength,
	Dexterity,
	Agility,
	Intelligence
}