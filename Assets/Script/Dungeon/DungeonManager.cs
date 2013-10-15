using UnityEngine;
using System.Collections;

public class DungeonManager : MonoBehaviour {
	
	private GameManager _GameManager;
	
	//static Material _Material_BrickWall;
	//static TextureManager _TextureManager;
	static int[,] _dungeonMap;
	static int    _mapSizeX;
	static int    _mapSizeZ;
	static int    _monsterToSpawn;
	static int    _nbrOfRoom;
	static private GameObject _GO_Ground;
	static private GameObject _GO_Player;	
	
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
	
	void Start()
	{
		_mapSizeX = 50;
		_mapSizeZ = 50;
		_monsterToSpawn = 2;
		_nbrOfRoom = 0;
		IniDungeon(_mapSizeX, _mapSizeZ);
		_dungeonMap = DungeonGenerator.SpawnDungeon(_mapSizeX,_mapSizeZ,_nbrOfRoom);
		for(int i = 0; i < _monsterToSpawn; i++)
		{
			SpawnMonster(_mapSizeX,_mapSizeZ);
		}
		
	}
	
	public void SpawnMonster(int _sizeX, int _sizeZ)
	{
		Vector3 _monsterPosition = new Vector3();
		_monsterPosition.x = Random.Range(0,_sizeX);
		_monsterPosition.z = Random.Range(0,_sizeZ);
		_monsterPosition.y = Utility.FindTerrainHeight(_monsterPosition.x,_monsterPosition.z);
		_GameManager.GetComponent<MonsterManager>().SpawnMonster(Bestiary.MonsterList[(int)MonsterName.Spider],	_monsterPosition);
	}
	
	public void IniDungeon(int _sizeX, int _sizeZ)
	{
		Transform _playerTransform;
		Transform _groundTransform;
		
		_GameManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
		//_GO_MeshHolder = new GameObject();
		//_TextureManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<TextureManager>();
		//_Material_BrickWall = _TextureManager.Material_Dungeon_BrickWall;
		_GO_Ground = GameObject.FindGameObjectWithTag("Ground");
		_GO_Player = GameObject.FindGameObjectWithTag("Player");
		
		
		_groundTransform = _GO_Ground.transform;
		_playerTransform = _GO_Player.transform;
		
		_groundTransform.localScale = new Vector3(_sizeX  , _groundTransform.localScale.y, _sizeZ  );
		_groundTransform.position   = new Vector3(_sizeX/2, 0.0f                         , _sizeZ/2);
		_playerTransform.position   = new Vector3(_sizeX/2, 3.0f                         , _sizeZ/2);
		
		
	}
	
	public void Abandon()
	{
		Application.LoadLevel("Camp");
		_GO_Player.transform.position = new Vector3(-35.0f, 2.0f, -10.0f);
		_GameManager.AddChatLogHUD ("[DUNG] You abandonned the dungeon");
	}
	
	public void Lose()
	{
		Application.LoadLevel("Camp");
		_GO_Player.transform.position = new Vector3(-35.0f, 2.0f, -10.0f);
		_GameManager.AddChatLogHUD ("[DUNG] You failed to finish the dungeon");
	}
	
	public void Win()
	{
		Application.LoadLevel("Camp");
		_GO_Player.transform.position = new Vector3(-35.0f, 2.0f, -10.0f);
		_GameManager.AddChatLogHUD ("[DUNG] You finished the dungeon");
	}
	
}
