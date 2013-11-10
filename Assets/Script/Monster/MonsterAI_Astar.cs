using UnityEngine;
using System.Collections;
using Pathfinding;

public class MonsterAI_Astar : MonoBehaviour {
	
	 //The point to move to
    public Vector3 targetPosition;
	private Vector3 playerPosition;
    private Seeker seeker;
    private CharacterController controller;
    private Path path; //The calculated path
	public int monsterState = 0; // 0 == Waiting for order, 1 = Moving around, 2 = Chase player, 3 = Attack Player
	public float distanceToPlayer;
    private float speed = 150; //The AI's speed per second
	private float rotaSpeed = 10;
    private float nextWaypointDistance = 3;  //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public int currentWaypoint = 0;//The waypoint we are currently moving towards
 	public Vector3 dir;
	private float timeLastPathUpdate = 0.0f;
	private float timeToUpdatePath	 = 0.5f;
	private float timeToMonsterAttack     = 0.0f;
	private GameManager _GameManager;
	private MonsterProfile _MonsterProfile;
	
    public void Start () {
		
		// GetComponent of the monster
        seeker 	   = GetComponent<Seeker>(); // Get the seeker component of the AI GameObject
        controller = GetComponent<CharacterController>();
		_GameManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
		_MonsterProfile = GetComponent<MonsterProfile>();
		
		
		animation[_MonsterProfile.CurMonster.AnimAttkName].wrapMode = WrapMode.Once; 
    }
    
    public void OnPathComplete (Path p) {
        //Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
        if (!p.error) {
            path = p;
            //Reset the waypoint counter
            currentWaypoint = 0;
        }
    }
 
     public void FixedUpdate () {
		
		playerPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		
		EvaluateMonsterState();
		
		if (path == null) {
            //We have no path to move after yet
            return;
        }
        
        if (currentWaypoint >= path.vectorPath.Count) {
			TargetEntered();
			return;
        }
		else
		{
	        //Direction to the next waypoint
	        MoveMonster();
	        if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
	            currentWaypoint++;
				return;
	        }
		}
    }
	
	void MoveMonster()
	{
		speed = _MonsterProfile.MoveSpeed;
		dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		//dir.y = transform.position.y;
		//dir.Normalize ();
        dir *= speed * Time.fixedDeltaTime;
		//transform.position += dir;
		
		Vector3 _PlayerPosAtGround   = new Vector3(playerPosition.x, Utility.FindTerrainHeight(playerPosition.x,playerPosition.z, 0.0f), playerPosition.z);
		//Vector3 _PlayerPosAtGround   = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);
		//Vector3 _WaypointPosAtGround = new Vector3(path.vectorPath[currentWaypoint].x, Utility.FindTerrainHeight(path.vectorPath[currentWaypoint].x,path.vectorPath[currentWaypoint].z, 0.0f), path.vectorPath[currentWaypoint].z);
		Quaternion _quatFrom = transform.rotation;
		Quaternion _quatTo = Quaternion.LookRotation (_PlayerPosAtGround - transform.position);
	
		transform.rotation = Quaternion.Slerp(_quatFrom, _quatTo, rotaSpeed*Time.deltaTime);
		GetComponent<CharacterController>().SimpleMove(dir);

	}
	
	void EvaluateMonsterState()
	{
		distanceToPlayer = Vector3.Distance(transform.position,playerPosition);
		
		timeToMonsterAttack -= Time.deltaTime;
		if(monsterState == 0 && distanceToPlayer < _MonsterProfile.SightRange)
		{
			TargetPlayer();
			monsterState = 2;
			
		}
		else if(monsterState == 2)
		{
			
			timeLastPathUpdate += Time.deltaTime;
			transform.animation.CrossFade(_MonsterProfile.CurMonster.AnimWalkName);
			
			if(timeLastPathUpdate >= timeToUpdatePath)
			{
				timeLastPathUpdate = timeLastPathUpdate - timeToUpdatePath;
				if(distanceToPlayer <= _MonsterProfile.AttackRange)
				{
					monsterState = 3;
				}	
				else
				{
					TargetPlayer();
				}
			}	
		}
		else if(monsterState == 3)
		{
			if((timeToMonsterAttack <= 0.0f) && distanceToPlayer <= _MonsterProfile.AttackRange)
			{
				timeToMonsterAttack = _MonsterProfile.AttackCd;
				AttackPlayer();
			}
			else if((timeToMonsterAttack <= 0.0f) && distanceToPlayer > _MonsterProfile.AttackRange)
			{
				monsterState = 0;
			}
			else
			{
					transform.animation.PlayQueued (_MonsterProfile.CurMonster.AnimIdleName);
			}
		}
		else
		{
			transform.animation.PlayQueued (_MonsterProfile.CurMonster.AnimIdleName);	
		}
	}
	
	void AttackPlayer()
	{
		animation.Play(_MonsterProfile.CurMonster.AnimAttkName);
		_GameManager.AddChatLogHUD("[FIGH] You lost hp");
		Character.LoseHp(_MonsterProfile.Damage);
	}
	
	public void TargetPlayer()
	{
		//Debug.Log ("Targetting Player");
		targetPosition = FindPlayerPositionAtMonsterHeight();   //
        seeker.StartPath (transform.position, targetPosition , OnPathComplete);
		currentWaypoint = 0;
		//AstarPath.StartPath(path);
	}
	
	public void TargetEntered()
	{
		//Debug.Log ("Target Entered");
		TargetPlayer();
	}
	
	public Vector3 FindPlayerPositionAtMonsterHeight()
	{
		Vector3 _UpdatedPos = GameObject.FindGameObjectWithTag ("Player").transform.position;
		_UpdatedPos = new Vector3(_UpdatedPos.x, transform.position.y, _UpdatedPos.z);
		return _UpdatedPos;	
	}
}