using UnityEngine;
using System.Collections;

public class Building {
	
	private string  _name;
	private bool    _isBuildable;
	private bool    _isUnlocked;
	private int     _nbrBuilt;
	private string  _recipe;
	private GameObject _BuildingPrefab;
	
	
	public string Name
	{
		get {return _name; }
		set {_name = value; }
	}
	
	public bool IsBuildable
	{
		get {return _isBuildable; }
		set {_isBuildable = value; }
	}
	
	public bool IsUnlocked
	{
		get {return _isUnlocked; }
		set {_isUnlocked = value; }
	}
	
	public string Recipe
	{
		get {return _recipe; }
		set {_recipe = value; }
	}
	
		public int NbrBuilt
	{
		get {return _nbrBuilt; }
		set {_nbrBuilt = value; }
	}
	
	public GameObject BuildingPrefab
	{
		get {return _BuildingPrefab; }
		set {_BuildingPrefab = value; }
	}
}

// Enumeration of all Compound
public enum BuildingName {
	CraftingTable
}