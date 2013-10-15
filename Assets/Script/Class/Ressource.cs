using UnityEngine;
using System.Collections;

public class Ressource {
	
	private string _name;
	private int _curValue;
	private int _maxValue;

	
	public string Name
	{
		get {return _name; }
		set {_name = value; }
	}
	
	public int CurValue
	{
		get {return _curValue; }
		set {_curValue = value; }
	}
	
		public int MaxValue
	{
		get {return _maxValue; }
		set {_maxValue = value; }
	}
	
}

// Enumeration of all Compound
public enum RessourceName {
	Coin,
	Wood,
	Rock,
	Gold
}