using UnityEngine;
using System.Collections;

public class MonsterAI : MonoBehaviour {
	
	
	private Transform _PlayerTransform;
	
	private int _curMonsterState = 0; //0 = idle/move randomly, 1 = Aware, 2 = Moving toward player, 3 = Attacking, 4 = Fleeing, -1 = Death
	private float _distancePlayer;
	private int _curHp;
	private Monster _CurMonster;
	public bool isMovingToTarget = false;
	public Vector3 _idlingTarget = new Vector3();
	private float timeToMonsterAttack = 0.0f;
	
	GameManager _GameManager;
	
	// Use this for initialization
	void Start () {
		animation["attack_Melee"].wrapMode = WrapMode.Once; 
		_GameManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
	}
	
	public void IniMonsterType(Monster _MonsterInitialized)
	{
		_CurMonster = _MonsterInitialized;
		_curHp = _CurMonster.Hp; // Initialize HP
	}
	
	// Update is called once per frame
	void Update () 
	{
		_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		_distancePlayer = Vector3.Distance (_PlayerTransform.position, transform.position);
		
		//gameObject.rigidbody.MovePosition
		
		if(_distancePlayer <= _CurMonster.AttackRange)
		{
			_curMonsterState = 3; // Attacking player
		}
		else if(_distancePlayer < _CurMonster.SightRange*0.75f)
		{
			_curMonsterState = 2; // Moving toward player
		}
		/*else if(_distancePlayer < _CurMonster.SightRange)
		{
			_curMonsterState = 1; // Aware of the player
		}*/
		else 
		{
			_curMonsterState = 0; // Idle
		}
		
		EvaluateState(_curMonsterState);
		
	}
	
	void EvaluateState(int _state)
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
	}

	void AttackPlayer()
	{
		animation.Play("attack_Melee");
		_GameManager.AddChatLogHUD("[FIGH] You lost hp");
		Character.LoseHp(_CurMonster.Damage);
	}
	
	void rotateTo(Vector3 _positionToTarget)
	{
		float rotaSpeed = 5.0f;
		Vector3 _modifedTarget = new Vector3(_positionToTarget.x, Utility.FindTerrainHeight(transform.position.x,transform.position.z), _positionToTarget.z);
		
		//transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(_modifedTarget - transform.position), rotaSpeed*Time.deltaTime);
		
		Quaternion _quatFrom = transform.rotation;
		Quaternion _quatTo = Quaternion.LookRotation (_positionToTarget - transform.position);
		//Quaternion _quatTo = Quaternion.FromToRotation(transform.position,_positionToTarget);
		
		transform.rotation = Quaternion.Slerp(_quatFrom, _quatTo, rotaSpeed*Time.deltaTime);
		
		
		///// WORKING VERSION
		//Quaternion _quatFrom = transform.rotation;
		//Quaternion _quatTo = Quaternion.LookRotation (_positionToTarget - transform.position);
		//transform.rotation = Quaternion.Slerp(_quatFrom, _quatTo, rotaSpeed*Time.deltaTime);
	}
	
	void moveForward(Vector3 _positionToTarget, float _moveSpeed)
	{
		//float _distanceToWalk
		//_distanceToWalk = Mathf.Min (_moveSpeed * Time.deltaTime,Vector3.Distance (transform.position, _positionToTarget));
		//transform.position += transform.forward*_distanceToWalk
		//_distanceToWalk = Mathf.Min (_moveSpeed * Time.deltaTime,Vector3.Distance (transform.position, _positionToTarget));
		//transform.position += transform.forward*_moveSpeed * Time.deltaTime;
		
		/*float _angleY = transform.rotation.y;
		float _posX   = transform.position.x;
		float _posZ   = transform.position.z;
		
		float _distMoveX = _moveSpeed*Mathf.Sin (_angleY*Mathf.Deg2Rad); 
		float _distMoveZ = _moveSpeed*Mathf.Cos (_angleY*Mathf.Deg2Rad);
		
		float _newPosX   = transform.position.x + _distMoveX;
		float _newPosZ   = transform.position.z + _distMoveZ;
		float _newPosY   = Utility.FindTerrainHeight(_newPosX,_newPosZ);
		transform.position = new Vector3(_newPosX, _newPosY, _newPosZ);*/
		
		
		//transform.position = new Vector3(transform.position.x, Utility.FindTerrainHeight(transform.position.x,transform.position.z), transform.position.z);
		
		//transform.position = new Vector3(transform.position.x, Utility.FindTerrainHeight(transform.position.x,transform.position.z), transform.position.z);
		
		///// WORKING VERSION
		transform.position += transform.forward*_moveSpeed * Time.deltaTime;
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
	
	public void DamageMonster(int _damageDealth)
	{
		AdjustHp(-_damageDealth);
	}
	
	void KillMonster()
	{
		Destroy (gameObject);
		_GameManager.ClaimReward(_CurMonster.Loot);
	}
}


