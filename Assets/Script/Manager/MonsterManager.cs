using UnityEngine;
using System.Collections;

public class MonsterManager : MonoBehaviour {
	
	private GameManager _GameManager;
	private PrefabManager _PrefabManager;
	private Vector3   _SpawnPosition;
	private int _maxNbrSpider = 3;
	private int _curNbrSpider = 0;
	
	private float _spawnPosX;
	private float _spawnPosY;
	private float _spawnPosZ;
	private GameObject _SpawnedMonster;
	private GameObject[] _Spawners;
	// Use this for initialization
	void Start () {
		_GameManager   = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
		_PrefabManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<PrefabManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(_GameManager.CurZone != "Dungeon")
		{
		
			_Spawners = GameObject.FindGameObjectsWithTag("Spawner");
			
			for(int i = 0; i < _Spawners.Length; i++)
			{
				if(_curNbrSpider < _maxNbrSpider)
				{
					_SpawnPosition = Utility.FindRandomPosition(_Spawners[i].transform.position,-10,10,-15,15);
					
					SpawnMonster(Bestiary.MonsterList[(int)MonsterName.Spider], _SpawnPosition);
					_curNbrSpider++;
				}
			}
		}
	}
	
	public void SpawnMonster(Monster _MonsterToSpawn, Vector3 _PositionToSpawn)
	{
		_SpawnedMonster = GameObject.Instantiate(_MonsterToSpawn.MonsterPrefab, _PositionToSpawn, Quaternion.identity)as GameObject;
		_SpawnedMonster.name = _MonsterToSpawn.Name;
		_SpawnedMonster.GetComponent<MonsterAI>().IniMonsterType(_MonsterToSpawn);
	}
}
