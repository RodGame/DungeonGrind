using UnityEngine;
using System.Collections;

public class Weapon : Item {
	 
	private int    _idWeapon;
	
	private int   _damage = 2;
	private float _speed = 125;
	private float _range = 6.0f;
	
	private float  _damagePerLevel = 1.5f;
	private float  _speedPerLevel = 7;
	private float  _rangePerLevel = 0.25f;

	protected int    _baseDamage = 5;
	protected float  _baseSpeed = 125.0f; // _curProgressSpeed    when implementing item system
	protected float  _baseRange = 6.0f; //   _distanceInteraction when implementing item system
	
	public int IdWeapon
	{
		get {return _idWeapon; }
		set {_idWeapon = value; }
	}
	
	public int Damage
	{
		get {return _damage; }
		set {_damage = value; }
	}
	
	public float Speed
	{
		get {return _speed; }
		set {_speed = value; }
	}
	
	public float Range
	{
		get {return _range; }
		set {_range = value; }
	}
	
	public float DamagePerLevel
	{
		get {return _damagePerLevel; }
		set {_damagePerLevel = value; }
	}
	
	public float SpeedPerLevel
	{
		get {return _speedPerLevel; }
		set {_speedPerLevel = value; }
	}
	
	public float RangePerLevel
	{
		get {return _rangePerLevel; }
		set {_rangePerLevel = value; }
	}
	
	public int BaseDamage
	{
		get {return _baseDamage; }
		set {_baseDamage = value; }
	}
	
	public float BaseSpeed
	{
		get {return _baseSpeed; }
		set {_baseSpeed = value; }
	}
	
	public float BaseRange
	{
		get {return _baseRange; }
		set {_baseRange = value; }
	}
	
	void Awake()
	{
		IdItem = (int)ItemType.Weapon;
	}
	
	public void UpdateStats()
	{
		_baseDamage  = Inventory.WeaponList[IdWeapon]._baseDamage;
		_baseSpeed   = Inventory.WeaponList[IdWeapon]._baseSpeed;
		_baseRange   = Inventory.WeaponList[IdWeapon]._baseRange;
		
		_damagePerLevel = Inventory.WeaponList[IdWeapon].DamagePerLevel;
		_speedPerLevel  = Inventory.WeaponList[IdWeapon].SpeedPerLevel;
		_rangePerLevel  = Inventory.WeaponList[IdWeapon].RangePerLevel;
		
		_damage = _baseDamage + Mathf.RoundToInt (Level*_damagePerLevel);
		_speed  =  _baseSpeed + Mathf.RoundToInt (Level*_speedPerLevel);
		_range  =  _baseRange + Mathf.RoundToInt (Level*_rangePerLevel);
	}
	
	public void InitializeItem()
	{
		Name        = Inventory.WeaponList[_idWeapon].Name;        	
		IsCraftable = Inventory.WeaponList[_idWeapon].IsCraftable; 	
		IsUnlocked  = Inventory.WeaponList[_idWeapon].IsUnlocked; 	
		Recipe      = Inventory.WeaponList[_idWeapon].Recipe;      	
		ItemIcon    = Inventory.WeaponList[_idWeapon].ItemIcon;    	
		ItemPrefab  = Inventory.WeaponList[_idWeapon].ItemPrefab;  	
	}
	
	public void GiveExp(float _expToGive)
	{
		CurExp = CurExp + _expToGive;	
		if(CurExp >= 100.0f)
		{
			 LevelUpWeapon();
		}
	}
	
	public void LevelUpWeapon()
	{
		CurExp = 0;	
		Level++;
		UpdateStats();
	}
	
}

// Enumeration of all Compound
public enum WeaponName {
	None,
	Hammer,
	RockSword,
	RockAxe,
	RockPickaxe,
	RockSpear
}