using UnityEngine;
using System.Collections;

public class Spell {
	
	private string _name;
	private string _category;
	private bool   _isUnlocked;
	private int    _damage         = 1;
	private int    _damageBase     = 1;
	private float  _damagePerLevel = 1;
	private int    _damageLevel    = 0;
	private float  _damageCurExp   = 0;
	private float  _cd             = 1.0f;
	private float  _cdBase         = 1;
	private float  _cdPerLevel     = 0;
	private int    _cdLevel        = 1;
	private float  _cdCurExp       = 0;
	private float  _range          = 5.0f;
	private float  _rangeBase      = 1;
	private float  _rangePerLevel  = 0;
	private int    _rangeLevel     = 1;
	private float  _rangeCurExp    = 0;
	private int    _mana           = 5;
	private int  _manaBase         = 1;
	private float  _manaPerLevel   = 0;
	private int    _manaLevel      = 1;
	private float  _manaCurExp     = 0;
	
	private Texture _SpellIcon;
	//private int  _splashLevel  = 1;
	//private int  _splashCurExp = 1;
	
	
	public string Name
	{
		get {return _name; }
		set {_name = value; }
	}
	
	public string Category
	{
		get {return _category; }
		set {_category = value; }
	}
	
	public bool IsUnlocked
	{
		get {return _isUnlocked; }
		set {_isUnlocked = value; }
	}
	
	
	// Damage Attributes
	public int Damage
	{
		get {return _damage; }
		set {_damage = value; }
	}
	
	public int DamageBase
	{
		get {return _damageBase; }
		set {_damageBase = value; }
	}
	
	public float DamagePerLevel
	{
		get {return _damagePerLevel; }
		set {_damagePerLevel = value; }
	}
	
	public int DamageLevel
	{
		get {return _damageLevel; }
		set {_damageLevel = value; }
	}
	
	public float DamageCurExp
	{
		get {return _damageCurExp; }
		set {_damageCurExp = value; }
	}
	
	
	// CoolDown Attributes
	public float Cd
	{
		get {return _cd; }
		set {_cd = value; }
	}
	
	public float CdBase
	{
		get {return _cdBase; }
		set {_cdBase = value; }
	}
	
	public float CdPerLevel
	{
		get {return _cdPerLevel; }
		set {_cdPerLevel = value; }
	}
	public int CdLevel
	{
		get {return _cdLevel; }
		set {_cdLevel = value; }
	}
	
	public float CdCurExp
	{
		get {return _cdCurExp; }
		set {_cdCurExp = value; }
	}
	
	public float Range
	{
		get {return _range; }
		set {_range = value; }
	}
	
	public float RangeBase
	{
		get {return _rangeBase; }
		set {_rangeBase = value; }
	}
	
	public float RangePerLevel
	{
		get {return _rangePerLevel; }
		set {_rangePerLevel = value; }
	}
	
	public int RangeLevel
	{
		get {return _rangeLevel; }
		set {_rangeLevel = value; }
	}
	
	public float RangeCurExp
	{
		get {return _rangeCurExp; }
		set {_rangeCurExp = value; }
	}
	
	public int Mana
	{
		get {return _mana; }
		set {_mana = value; }
	}
	
	public int ManaBase
	{
		get {return _manaBase; }
		set {_manaBase = value; }
	}
	
	public float ManaPerLevel
	{
		get {return _manaPerLevel; }
		set {_manaPerLevel = value; }
	}
	
	public int ManaLevel
	{
		get {return _manaLevel; }
		set {_manaLevel = value; }
	}
	
	public float ManaCurExp
	{
		get {return _manaCurExp; }
		set {_manaCurExp = value; }
	}
	
	public Texture SpellIcon
	{
		get {return _SpellIcon; }
		set {_SpellIcon = value; }
	}
}

// Enumeration of all Skills
public enum SpellName {
	IceBolt,
	FireBat
}