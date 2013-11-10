using UnityEngine;
using System.Collections;
using Pathfinding;

public class PetAI_Astar : MonoBehaviour {
	
	 //The point to move to
    public Vector3 targetPosition;
	private Vector3 playerPosition;
    private Seeker seeker;
    private CharacterController controller;
    private Path path; //The calculated path
	public int monsterState = 2; // 0 == Waiting for order, 1 = Moving around, 2 = Chase player, 3 = Attack Player(not used), 4 = Chase Target, 5 = Attack Target
	public float distanceToPlayer;
	public float distanceToClosestMonster;
    private float speed = 150; //The AI's speed per second
	private float rotaSpeed = 10;
    private float nextWaypointDistance = 3;  //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public int currentWaypoint = 0;//The waypoint we are currently moving towards
 	private GameObject _ClosestMonster;
		
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
		
		//Setup animation 
		animation["attack_Melee"].wrapMode = WrapMode.Once; 
		
		
        
    }
    
    public void OnPathComplete (Path p) {
        //Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
        if (!p.error) {
            path = p;
            //Reset the waypoint counter
            currentWaypoint = 0;
        }
		
		
		
		
    }
	
	void Update ()
	{
		_ClosestMonster = FindClosestMonster();
		
		distanceToPlayer = Vector3.Distance(transform.position,playerPosition);
		distanceToClosestMonster = Vector3.Distance (transform.position, _ClosestMonster.transform.position);
		
		timeToMonsterAttack -= Time.deltaTime;
		/*if(monsterState == 0 && distanceToPlayer < _MonsterProfile.SightRange)
		{
			TargetPlayer();	
			monsterState = 2;
			
		}*/
		if(monsterState == 2)
		{
			
			timeLastPathUpdate += Time.deltaTime;
			transform.animation.CrossFade("walk");
			
			if(distanceToClosestMonster < 15)
			{
				monsterState = 4;
			}
			else
			{
				if(timeLastPathUpdate >= timeToUpdatePath)
				{
					timeLastPathUpdate = timeLastPathUpdate - timeToUpdatePath;
					{
						TargetPlayer ();
					}
				}	
			}
		}
		else if(monsterState == 4)
		{
			
			timeLastPathUpdate += Time.deltaTime;
			transform.animation.CrossFade("walk");
			
			if(distanceToClosestMonster < 15)
			{
				if(timeLastPathUpdate >= timeToUpdatePath)
				{
					timeLastPathUpdate = timeLastPathUpdate - timeToUpdatePath;
					if(distanceToClosestMonster < _MonsterProfile.AttackRange)
					{
						monsterState = 5;
					}	
					else
					{
						TargetClosestMonster ();
					}
				}	
			}
			else
			{
				monsterState = 2;	
			}
			
			
		}
		else if(monsterState == 5)
		{	
			if(distanceToClosestMonster < 15)
			{
				if((timeToMonsterAttack <= 0.0f) && distanceToClosestMonster <= _MonsterProfile.AttackRange)
				{
					timeToMonsterAttack = _MonsterProfile.AttackCd;
					AttackTarget();
				}
				else if((timeToMonsterAttack <= 0.0f) && distanceToClosestMonster > _MonsterProfile.AttackRange)
				{
					monsterState = 4;
				}
				else
				{
					transform.animation.PlayQueued ("iddle");
				}
			}
			else
			{
				monsterState = 2;
			}
		}
	}
 
     public void FixedUpdate () {
		
		playerPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		
		
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
	        Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
			
	        dir *= speed * Time.fixedDeltaTime;
	        
			
			//Vector3 _PlayerPosAtGround = new Vector3(playerPosition.x, Utility.FindTerrainHeight(playerPosition.x,playerPosition.z), playerPosition.z);
			Vector3 _WaypointPosAtGround = new Vector3(path.vectorPath[currentWaypoint].x, Utility.FindTerrainHeight(path.vectorPath[currentWaypoint].x,path.vectorPath[currentWaypoint].z,0.0f), path.vectorPath[currentWaypoint].z);
			Quaternion _quatFrom = transform.rotation;
			Quaternion _quatTo = Quaternion.LookRotation (_WaypointPosAtGround - transform.position);
		
			transform.rotation = Quaternion.Slerp(_quatFrom, _quatTo, rotaSpeed*Time.deltaTime);
			controller.SimpleMove (dir);
			
	        //Check if we are close enough to the next waypoint
	        //If we are, proceed to follow the next waypoint
	        if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
	            currentWaypoint++;
				return;
	        }
		}
    }
	
	void AttackPlayer()
	{
		animation.Play("attack_Melee");
		_GameManager.AddChatLogHUD("[FIGH] You lost hp");
		//Character.LoseHp(_MonsterProfile.Damage);
	}
	
	void AttackTarget()
	{
		animation.Play("attack_Melee");
		//_GameManager.AddChatLogHUD("[FIGH] You lost hp");
		//Character.LoseHp(_MonsterProfile.Damage);
	}
	
	public void TargetClosestMonster()
	{
		GameObject _GO_Target;
		_GO_Target = FindClosestMonster();
		targetPosition = _GO_Target.transform.position;
        seeker.StartPath (transform.position,targetPosition, OnPathComplete);
		currentWaypoint = 0;
	}
	
	public GameObject FindClosestMonster()
	{
		GameObject[] Monsters;
	    GameObject _FoundClosestMonster;
		Monsters = GameObject.FindGameObjectsWithTag("Monster");
		_FoundClosestMonster = Monsters[0];
		float _distanceClosestMonster = Vector3.Distance(transform.position, _FoundClosestMonster.transform.position);
		for(int i = 0; i < Monsters.Length; i++)
		{
			if(Vector3.Distance(transform.position, Monsters[i].transform.position) > _distanceClosestMonster)
			{
				_FoundClosestMonster = Monsters[i];
			}
			
		}
		
		return _FoundClosestMonster;
	}
	
	public void TargetPlayer()
	{
		targetPosition = FindPlayerPosition();   //
        seeker.StartPath (transform.position,targetPosition, OnPathComplete);
		currentWaypoint = 0;
	}
	
	public void TargetEntered()
	{
		TargetClosestMonster();
	}
	
	public Vector3 FindPlayerPosition()
	{
		Vector3 _UpdatedPos = GameObject.FindGameObjectWithTag ("Player").transform.position;
		_UpdatedPos = new Vector3(_UpdatedPos.x, transform.position.y, _UpdatedPos.z);
		return _UpdatedPos;	
	}
	
	
	
	
	////// MODIFIED V1
    /*public Vector3 targetPosition;
	public Seeker seeker;
		
    public void Start () {
        //Get a reference to the Seeker component we added earlier
        seeker = GetComponent<Seeker>();
		
		
		//targetPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		//Start a new path to the targetPosition, return the result to the OnPathComplete function
        seeker.StartPath (transform.position,FindPlayerPosition(), OnPathComplete);
    }
    
	
	public void Update()
	{
		
		
		
	}
    public void OnPathComplete (Path p) {
		//targetPosition = 
		//Start a new path to the targetPosition, return the result to the OnPathComplete function
        seeker.StartPath (transform.position,FindPlayerPosition(), OnPathComplete);
        Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
    }*/
	
	
	
}
