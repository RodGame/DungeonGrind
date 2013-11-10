using UnityEngine;
using System.Collections;

public class Monster{
	
	private string  _name = "None";
	private string  _loot = "None";
	private string  _animAttkName;
	private string  _animWalkName;
	private string  _animIdleName;
	private string  _animKnkcName;
	private int     _hp          = 5;
	private int     _damage      = 1;
	private int     _nbrKilled   = 0;
	private float   _spawnCd     = 0.0f;
	private float   _attackCd    = 0.0f;
	private float   _attackRange = 2.0f;
	private float   _sightRange  = 10.0f;
	private float   _moveSpeed   = 100.00f;
	private float   _skillReward = 1.0f;
	private float   _size        = 1.0f;
	
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
	
	public string AnimAttkName
	{
		get {return _animAttkName; }
		set {_animAttkName = value; }
	}
	
	public string AnimWalkName
	{
		get {return _animWalkName; }
		set {_animWalkName = value; }
	}
	
	public string AnimIdleName
	{
		get {return _animIdleName; }
		set {_animIdleName = value; }
	}
	
	public string AnimKnkcName
	{
		get {return _animKnkcName; }
		set {_animKnkcName = value; }
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
	
	public float Size
	{
		get {return _size; }
		set {_size = value; }
	}
	
	public float Height
	{
		get {return _size; }
		set {_size = value; }
	}
}

// Enumeration of all Compound
public enum MonsterName {
	PseudoSpider,
	Spider,
	SpiderQueen,
	SkeletonToon,
	SkeletonFighter,
	SkeletonKing
}