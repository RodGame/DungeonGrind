using UnityEngine;
using System.Collections;

public class Monster{
	
	private string  _name;
	private string  _loot = "None";
	private int     _hp          = 5;
	private int     _damage      = 1;
	private int     _nbrKilled   = 0;
	private float   _spawnCd     = 0.0f;
	private float   _attackCd    = 0.0f;
	private float   _attackRange = 2.0f;
	private float   _sightRange  = 10.0f;
	private float   _moveSpeed   = 3.00f;
	private float   _skillReward = 1.0f;
	
	private GameObject _MonsterPrefab;
	
	
	public string Name
	{
		get {return _name; }
		set {_name = value; }
	}
	
	public string Loot
	{
		get {return _loot; }
		set {_loot = value; }
	}
	
	public int Hp
	{ 
		get {return _hp; }
		set {_hp = value; }
	}
	
	public int Damage
	{
		get {return _damage; }
		set {_damage = value; }
	}
	
	public int NbrKilled
	{
		get {return _nbrKilled; }
		set {_nbrKilled = value; }
	}
	
	public float SpawnCd
	{
		get {return _spawnCd; }
		set {_spawnCd = value; }
	}
	
	public float AttackCd
	{
		get {return _attackCd; }
		set {_attackCd = value; }
	}
	
	public float AttackRange
	{
		get {return _attackRange; }
		set {_attackRange = value; }
	}
	
	public float MoveSpeed
	{
		get {return _moveSpeed; }
		set {_moveSpeed = value; }
	}
	
	public float SightRange
	{
		get {return _sightRange; }
		set {_sightRange = value; }
	}
	
	public GameObject MonsterPrefab
	{
		get {return _MonsterPrefab; }
		set {_MonsterPrefab = value; }
	}
	public float SkillReward
	{
		get {return _skillReward; }
		set {_skillReward = value; }
	}
	
}

// Enumeration of all Compound
public enum MonsterName {
	PseudoSpider,
	Spider,
	SpiderQueen
}