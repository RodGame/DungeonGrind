using UnityEngine;
using System.Collections;
using System;              // For Enum

public class MonsterProfile : MonoBehaviour {
	
	private GameManager _GameManager;
	private Monster _CurMonster;
	
	
	private int _curHp;
	private int _maxHp;
	private int _damage; 
	private float _attackCd;
	private float _attackRange;
	private float _sightRange;
	private float _skillReward;
	private float _moveSpeed;
	
	public int CurHp
	{
		get {return _curHp; }
		set {_curHp = value; }
	}
	
	public int MaxHp
	{
		get {return _maxHp; }
		set {_maxHp = value; }
	}
	
	public int Damage
	{
		get {return _damage; }
		set {_damage = value; }
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
	
	public float SightRange
	{
		get {return _sightRange; }
		set {_sightRange = value; }
	}
	
	public float SkillReward
	{
		get {return _skillReward; }
		set {_skillReward = value; }
	}
	
	public float MoveSpeed
	{
		get {return _moveSpeed; }
		set {_moveSpeed = value; }
	}
	
	public Monster CurMonster
	{
		get {return _CurMonster; }
		set {_CurMonster = value; }
	}
	
	// Use this for initialization
	void Start () {
		_GameManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
		
		
		//MonsterName _MonsterIndex = (MonsterName) Enum.Parse(typeof(MonsterName), transform.name);
		//IniMonsterType(Bestiary.MonsterList[(int)_MonsterIndex]);
	}
	
	public void IniMonsterType(Monster _MonsterInitialized, bool _isHardcore)
	{
		_CurMonster = _MonsterInitialized;
		
		float _ratioMaxHp  = 1.0f;
		float _ratioDamage = 1.0f;
		float _ratioSpeed  = 1.0f;
		float _ratioReward  = 1.0f;
		
		if(_isHardcore)
		{
			_ratioMaxHp  = 6.0f;
			_ratioDamage = 10.0f;
			_ratioSpeed  = 1.5f;
			_ratioReward = 1.5f;
		}
	
		_maxHp       = Mathf.RoundToInt(_CurMonster.Hp*_ratioMaxHp); // Initialize HP
		_curHp       = _maxHp;
		_damage      = Mathf.RoundToInt(_CurMonster.Damage*_ratioDamage);
		_attackCd    = _CurMonster.AttackCd;
		_attackRange = _CurMonster.AttackRange;
		_sightRange  = _CurMonster.SightRange;
		_skillReward = Mathf.RoundToInt(_CurMonster.SkillReward*_ratioReward);
		_moveSpeed   = _CurMonster.MoveSpeed*_ratioSpeed;
	}
	
	public void AdjustHp(int _hpToAdd)
	{
		int hpBeforeUpdate = _curHp;
		_curHp += _hpToAdd;
		
		_GameManager.AddChatLogHUD(_CurMonster.Name + " HP : " + hpBeforeUpdate + "->" + _curHp); 
		if(_curHp > _maxHp)
		{
			_curHp = _maxHp;
		}
		else if(_curHp <= 0)
		{
			_curHp = 0;
			KillMonster();
			_GameManager.AddChatLogHUD(_CurMonster.Name + " was killed."); 
		} 
	}
	
	public void DamageMonster(int _damageTaken)
	{
		AdjustHp(-_damageTaken);
		GetComponent<MonsterHealthbar>().IsHealthbarEnabled = true;
		animation.Play(_CurMonster.AnimKnkcName);
	}
	
	void KillMonster()
	{
		Destroy (gameObject);
		_GameManager.ClaimReward(_CurMonster.Loot);
		_CurMonster.NbrKilled++;
	}
}


