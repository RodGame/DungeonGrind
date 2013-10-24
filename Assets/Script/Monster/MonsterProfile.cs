using UnityEngine;
using System.Collections;

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
	
	// Use this for initialization
	void Start () {
		_GameManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
	}
	
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
	
	public void IniMonsterType(Monster _MonsterInitialized)
	{
		_CurMonster = _MonsterInitialized;
		
		_maxHp       = _CurMonster.Hp; // Initialize HP
		_curHp       = _maxHp;
		_damage      = _CurMonster.Damage;
		_attackCd    = _CurMonster.AttackCd;
		_attackRange = _CurMonster.AttackRange;
		_sightRange  = _CurMonster.SightRange;
		_skillReward = _CurMonster.SkillReward;
	}
	
	public void AdjustHp(int _hpToAdd)
	{
		int hpBeforeUpdate = _curHp;
		_curHp += _hpToAdd;
		
		_GameManager.AddChatLogHUD(_CurMonster.Name + " HP : " + hpBeforeUpdate + "->" + _curHp); 
		if(_curHp > _CurMonster.Hp)
		{
			_curHp = _CurMonster.Hp;
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
		animation.Play("damage");
	}
	
	void KillMonster()
	{
		Destroy (gameObject);
		_GameManager.ClaimReward(_CurMonster.Loot);
	}
	
	/*void EvaluateState(int _state)
	{
		Vector3 _PlayerPos = _PlayerTransform.position;
		Vector3 _PlayerPosAtGround = new Vector3(_PlayerPos.x, Utility.FindTerrainHeight(_PlayerPos.x,_PlayerPos.z), _PlayerPos.z);
		
		timeToMonsterAttack -= Time.deltaTime;
		if(_state == 3)
		{
			rotateTo(_PlayerPosAtGround);
			
			if((timeToMonsterAttack <= 0.0f) && _distancePlayer <= _CurMonster.AttackRange)
			{
				timeToMonsterAttack = _CurMonster.AttackCd;
				AttackPlayer();
			}
			else
			{
					transform.animation.PlayQueued ("iddle");
			}
		}
		else if(_state == 2)
		{
			rotateTo(_PlayerPosAtGround);
			moveForward(_PlayerTransform.position, _CurMonster.MoveSpeed);
			transform.animation.CrossFade("walk");
		}
		else if(_state == 1)
		{
			rotateTo(_PlayerPosAtGround);
			transform.animation.CrossFade("awareness");
		}
		else
		{
			if(isMovingToTarget == false)
			{
				_idlingTarget = Utility.FindRandomPosition(transform.position,-8,8,-8,8);
				isMovingToTarget = true;
			}
			else
			{
				if(Vector3.Distance(transform.position, _idlingTarget) < 1.0f)
				{
					isMovingToTarget = false;		
				}
				else
				{
					rotateTo(_idlingTarget);
					moveForward(_idlingTarget, _CurMonster.MoveSpeed);
					
					transform.animation.CrossFade("walk");
				}
			}
			//rotateTo(idlingTarget);
			
		}
	}*/

	
	
	/*void rotateTo(Vector3 _positionToTarget)
	{
		float rotaSpeed = 5.0f;
		Vector3 _modifedTarget = new Vector3(_positionToTarget.x, Utility.FindTerrainHeight(transform.position.x,transform.position.z), _positionToTarget.z);
		
		Quaternion _quatFrom = transform.rotation;
		Quaternion _quatTo = Quaternion.LookRotation (_positionToTarget - transform.position);
		
		transform.rotation = Quaternion.Slerp(_quatFrom, _quatTo, rotaSpeed*Time.deltaTime);
		
		
		///// WORKING VERSION
		//Quaternion _quatFrom = transform.rotation;
		//Quaternion _quatTo = Quaternion.LookRotation (_positionToTarget - transform.position);
		//transform.rotation = Quaternion.Slerp(_quatFrom, _quatTo, rotaSpeed*Time.deltaTime);
	}
	
	void moveForward(Vector3 _positionToTarget, float _moveSpeed)
	{
		transform.position += transform.forward*_moveSpeed * Time.deltaTime;
	}*/

	
}


