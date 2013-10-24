using UnityEngine;
using System.Collections;

public class DungeonLevel {
	
	private string  _name = "NoName Dungeon";
	private string  _monsterList = "1*Spider";
	private string  _reward = "";
	private int     _sizeX = 100;
	private int     _sizeY = 100;
	private int     _nbrSquareForSpawn = 2;
	
	public string Name
	{
		get {return _name; }
		set {_name = value; }
	}
	
	public string MonsterList
	{
		get {return _monsterList; }
		set {_monsterList = value; }
	}
	
	public string Reward
	{
		get {return _reward; }
		set {_reward = value; }
	}
	
	public int SizeX
	{
		get {return _sizeX; }
		set {_sizeX = value; }
	}
	
	public int SizeY
	{
		get {return _sizeY; }
		set {_sizeY = value; }
	}
	
	public int NbrSquareForSpawn
	{
		get {return _nbrSquareForSpawn; }
		set {_nbrSquareForSpawn = value; }
	}
}
/*
// Enumeration of all Compound
public enum ItemName {
	None,
	Hammer,
	RockSword,
	RockAxe,
	RockPickaxe,
	RockSpear
}
*/