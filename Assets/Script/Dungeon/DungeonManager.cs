using UnityEngine;
using System;			      //added to access the enum class
using System.Collections;
using System.Collections.Generic; // For List class;
using Pathfinding; 			      //To update the graph size

public class DungeonManager : MonoBehaviour{
	
	private GameManager _GameManager;
	
	//static Material _Material_BrickWall;
	//static TextureManager _TextureManager;
	private int[,] _dungeonMap;
	private int    _mapSizeX;
	private int    _mapSizeZ;
	private string _monsterToSpawn;
	 
	private int    _nbrMonsterSpawned;
	private int    _nbrMonsterLeft;
	private int    _nbrMonsterKilled;
	 
	private int    _disSafeZoneSpawnPlayer = 10;
	private int    _nbrOfSquareSpawned;
	private int    _curDungeonLevel;
	private GameObject _GO_Ground;
	private GameObject _GO_Player;	
	private AstarPath  _Astar;
	private PlayerHUD.DungeonParameters _CurDungeonParameters;
	private bool _isDungeonStarted = false;
	
	public GameObject _TEST_PET;
	public int[,] DungeonMap
	{
		get {return _dungeonMap; }
		set {_dungeonMap = value; }
	}
	
	public int MapSizeX
	{
		get {return _mapSizeX; }
		set {_mapSizeX = value; }
	}
	
	public int MapSizeZ
	{
		get {return _mapSizeZ; }
		set {_mapSizeZ = value; }
	}
	
	public int NbrMonsterKilled
	{
		get {return _nbrMonsterKilled; }
		set {_nbrMonsterKilled = value; }
	}
	
	public int NbrMonsterSpawned
	{
		get {return _nbrMonsterSpawned; }
		set {_nbrMonsterSpawned = value; }
	}
	
	void Start()
	{
		
		Camera.mainCamera.GetComponent<Skybox>().material = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<TextureManager>().Material_Skybox_Dungeon;
		_GameManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
		_GameManager.ChangeState("Play");
		
		// Find information about the dungeon to spawn
		_CurDungeonParameters = _GameManager.CurDungeonParameters;
		_curDungeonLevel = _CurDungeonParameters.level;

		// Find information about the level to spawn
		_mapSizeX = DungeonLevelPool.DungeonLevelList[_curDungeonLevel].SizeX;
		_mapSizeZ = DungeonLevelPool.DungeonLevelList[_curDungeonLevel].SizeY;
		_nbrOfSquareSpawned = DungeonLevelPool.DungeonLevelList[_curDungeonLevel].NbrSquareForSpawn;
		_monsterToSpawn     = DungeonLevelPool.DungeonLevelList[_curDungeonLevel].MonsterList;
		
		// Initialize Dungeon
		IniDungeon(_mapSizeX, _mapSizeZ);
		
		// Spawn Dungeon
		_dungeonMap = DungeonGenerator.SpawnDungeon(_mapSizeX,_mapSizeZ,_nbrOfSquareSpawned);
		
		// Initialize P_Pathfinding and Player
		IniPathfindGraph(_mapSizeX, _mapSizeZ);
		IniPlayer(_dungeonMap, _mapSizeX, _mapSizeZ);
		
		//Spawn all monster for the level
		SpawnAllMonster(_monsterToSpawn, _dungeonMap, _mapSizeX,_mapSizeZ);
		
		
		// Finalize the dungeon iniatiliation
		_GameManager.CurZone = DungeonLevelPool.DungeonLevelList[_curDungeonLevel].Name;
		_isDungeonStarted = true;
		GameObject.FindGameObjectWithTag("PlayerMaster").GetComponent<PlayerHUD>().InitializeMap(_dungeonMap, _mapSizeX, _mapSizeZ);
	}
	
	void Update()
	{
		
		if(_isDungeonStarted == true)
		{
			_nbrMonsterLeft = GameObject.FindGameObjectsWithTag("Monster").Length;
			_nbrMonsterKilled = _nbrMonsterSpawned - _nbrMonsterLeft;
			
			if(_nbrMonsterKilled >= _nbrMonsterSpawned)
			{
				Win ();
			}
		}
		
		if(_GO_Player.transform.position.y < -5)
		{
			Abandon ();	
		}
	}
	
	private void IniPlayer(int[,] _mapRooms, int _sizeX, int _sizeZ)
	{
		Vector2 _playerPos;
		
		Character.RefillMP();
		
		_GO_Player = GameObject.FindGameObjectWithTag("Player");
		_playerPos = FindRandomRoomPosition(_mapRooms,_sizeX, _sizeZ, 2.0f, 0.0f);
		_GO_Player.transform.position = new Vector3(_playerPos.x, 3.0f, _playerPos.y);
		
		//GameObject.Instantiate(_TEST_PET,_GO_Player.transform.position + new Vector3(1,0,1), Quaternion.identity);
		
	}
	
	// Find a random position in a room
	private Vector2 FindRandomRoomPosition_TOMERGE(int[,] _mapRooms, int _sizeX, int _sizeZ, float _distanceFromWall)
	{
		int _tryCounter = 0;
		int _tryCounterMax = 200;
		int _x = UnityEngine.Random.Range (1,_sizeX-1);
		int _z = UnityEngine.Random.Range (1,_sizeZ-1);
		int _distanceFromWallMap = Mathf.CeilToInt(_distanceFromWall);
		
		bool _isInRoom = false;
		
		while(_isInRoom == false && _tryCounter < _tryCounterMax)
		{
			_isInRoom = true;
			_x = UnityEngine.Random.Range (1,_sizeX-1);
			_z = UnityEngine.Random.Range (1,_sizeZ-1);
			
			for(int i = _x - _distanceFromWallMap; i <= _x + _distanceFromWallMap; i++)
			{
				for(int j = _z - _distanceFromWallMap; j <= _z + _distanceFromWallMap; j++)
				{
					if((i >= 0 && i <= _sizeX -1) && (j >= 0 && j <= _sizeZ -1))
					{
						if(_mapRooms[i,j] == 0)
						{
							_isInRoom = false;
						}
					}
				}
			}
			_tryCounter++;
		}
		
		return new Vector2(_x,_z);
	}
	
	// Overloaded function that take a safezone distance fot the player as in input
	public Vector2 FindRandomRoomPosition(int[,] _mapRooms, int _sizeX, int _sizeZ, float _distanceFromWall, float _distanceSafeZone)
	{
		int _tryCounter = 0;
		int _tryCounterMax = 500;
		bool _isPlayerTooClose;
		Vector2 _newPosition;
		
		//Calculate a first position
		_newPosition = FindRandomRoomPosition_TOMERGE(_mapRooms, _sizeX, _sizeZ, _distanceFromWall);
		
		//Only evaluate player distance if its above 0. This is used when the player is spawned.
		if(_distanceSafeZone > 0.0f)
		{
			Vector2 _playerPosition = new Vector2(_GO_Player.transform.position.x, _GO_Player.transform.position.z);
			
			_isPlayerTooClose = (Vector2.Distance (_newPosition,_playerPosition) <= _distanceSafeZone);
				
			while(_isPlayerTooClose == true && _tryCounter < _tryCounterMax)
			{
				_newPosition = FindRandomRoomPosition_TOMERGE(_mapRooms, _sizeX, _sizeZ, _distanceFromWall);
				_isPlayerTooClose = (Vector2.Distance (_newPosition,_playerPosition) <= _distanceSafeZone);
				_tryCounter++;
			}
		}
		return _newPosition;
	}
	
	public void SpawnAllMonster(string _stringMonsterToSpawn, int[,] _mapRooms, int _sizeX, int _sizeZ)
	{
		Vector3 _monsterPosition = new Vector3();
		Vector2 _spawnPos;
		_nbrMonsterSpawned = 0;
		List<Utility.ParsedString> _ListMonsterToSpawn  = new List<Utility.ParsedString>();	//Declare a list that contain all the monster to spawn
		
		_ListMonsterToSpawn = Utility.parseString (_stringMonsterToSpawn);
		
		for(int i = 0; i < _ListMonsterToSpawn.Count; i++)
		{
			MonsterName _MonsterIndex = (MonsterName) Enum.Parse(typeof(MonsterName), _ListMonsterToSpawn[i].type); 
			Monster MonsterToSpawn = Bestiary.MonsterList[(int)_MonsterIndex];
			for(int j = 0; j < _ListMonsterToSpawn[i].nbr; j++)
			{ 
				_nbrMonsterSpawned++;
				_spawnPos = FindRandomRoomPosition(_mapRooms, _sizeX, _sizeZ, MonsterToSpawn.Size*2.0f, _disSafeZoneSpawnPlayer + Mathf.RoundToInt(_curDungeonLevel * 0.5f));
				_monsterPosition.x = _spawnPos.x;
				_monsterPosition.z = _spawnPos.y;
				_monsterPosition.y = Utility.FindTerrainHeight(_monsterPosition.x,_monsterPosition.z, 0.25f);
			
				Bestiary.SpawnMonster(MonsterToSpawn,	_monsterPosition);
			}
		}
		_nbrMonsterKilled = 0;
	}
	
	public void IniDungeon(int _sizeX, int _sizeZ)
	{
		Transform _groundTransform;
		_GO_Ground = GameObject.FindGameObjectWithTag("Ground");
		_groundTransform = _GO_Ground.transform;
		
		
		_groundTransform.localScale = new Vector3(_sizeX  , _groundTransform.localScale.y, _sizeZ  );
		_groundTransform.position   = new Vector3(_sizeX/2, 0.0f                         , _sizeZ/2);	
	}
	
	// Initiate, resize and Scan the pathfind GridGraph
	public void IniPathfindGraph(int _sizeX, int _sizeZ)
	{
		_Astar = GameObject.FindGameObjectWithTag ("Astar").GetComponent<AstarPath>();
		
		GridGraph _Graph = AstarPath.active.astarData.gridGraph;
		
		_Graph.width = _sizeX;
		_Graph.depth = _sizeZ;
		_Graph.center = new Vector3(_sizeX/2, 0.0f, _sizeZ/2);	
		_Graph.UpdateSizeFromWidthDepth();
		_Graph.Scan();
		_Astar.Scan ();
		AstarPath.active.Scan ();
	}
	
	public void Abandon()
	{
		_GameManager.AddChatLogHUD ("[DUNG] You ABANDONNED the dungeon!!!");
		CloseDungeon();
	}
	
	public void Lose()
	{
		_GameManager.AddChatLogHUD ("[DUNG] You DIED in the dungeon!!!");
		CloseDungeon();
	}
	
	public void Win()
	{
		int _influenceGained;
		if(_CurDungeonParameters.isHardcore == false)
		{
			_influenceGained = _GameManager.CurDungeonParameters.level;
		}
		else
		{
			_influenceGained = 5+ Mathf.RoundToInt(_GameManager.CurDungeonParameters.level*1.5f);	
		}
		Character.GainInfluences(_influenceGained);
		
		if(_curDungeonLevel == _GameManager.MaxDungeonLevel)
		{
			_GameManager.MaxDungeonLevel++;
		}
		_GameManager.AddChatLogHUD ("[DUNG] You WON the dungeon!!!");
		CloseDungeon();
	}
	
	public void CloseDungeon()
	{
		_GameManager.CurZone = "Camp";
		Application.LoadLevel("Camp");
		_GO_Player.transform.position = new Vector3(-45.0f, 2.0f, -10.0f);
	
		
		
		_GO_Player.transform.position = new Vector3(-35.0f, 2.0f, -10.0f);
		Character.RefillHP();
		Character.RefillMP();
		SaveLoadSystem.Save ();
		Camera.mainCamera.GetComponent<Skybox>().material = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<TextureManager>().Material_Skybox_Camp;
	}
	
}
