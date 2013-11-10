using UnityEngine;
using System.Collections;

public class DungeonUpgrade {
	
	private string  _name = "NoName Upgrade";
	private string  _description = "NoDescription";
	private bool    _isUnlocked = false;
	private bool    _isEnabled  = false;
	private int     _costInfluence = 5;
	private int     _costCoin      = 5;
	
	public string Name
	{
		get {return _name; }
		set {_name = value; }
	}
	
	public string Description
	{
		get {return _description; }
		set {_description = value; }
	}
	
	public bool IsUnlocked
	{
		get {return _isUnlocked; }
		set {_isUnlocked = value; }
	}
	
	public bool IsEnabled
	{
		get {return _isEnabled; }
		set {_isEnabled = value; }
	}
	
	public int CostInfluence
	{
		get {return _costInfluence; }
		set {_costInfluence = value; }
	}
	
	public int CostCoin
	{
		get {return _costCoin; }
		set {_costCoin = value; }
	}	
}

//Enumeration of all Compound
public enum DungeonUpgradeName {
	FirstUpgrade,
	SkeletonCrypt,
	HardcoreMode,
	WaveMode
}
