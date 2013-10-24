using UnityEngine;
using System.Collections;
using System; // For Enum class;

static class Bestiary {

	public static Monster[] MonsterList;
	private static PrefabManager _PrefabManager;
	
	public static void IniBestiary()
	{
		MonsterList  = new Monster [Enum.GetValues(typeof(MonsterName) ).Length];	
		_PrefabManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<PrefabManager>();
		
		IniMonsterList();
	}
		
	
	private static void IniMonsterList()
	{
		for(int i = 0; i < MonsterList.Length; i++)
		{
			
			MonsterList[i] = new Monster();
			MonsterList[i].Name = ((MonsterName)i).ToString();
			if(MonsterList[i].Name == "PseudoSpider")
			{
				MonsterList[i].Loot          = "[RESS]1*Coin";               //TODO [DROP] With droppable item
				MonsterList[i].Hp            = 40;
				MonsterList[i].Damage        = 8;
				MonsterList[i].SpawnCd       = 1.0f;
				MonsterList[i].AttackCd      = 1.0f;
				MonsterList[i].AttackRange   = 3.0f;
				MonsterList[i].SightRange    = 25;
				MonsterList[i].SkillReward   = 1.0f;
				MonsterList[i].MonsterPrefab = _PrefabManager.Monster_PseudoSpider;
			}
			else if(MonsterList[i].Name == "Spider")
			{
				MonsterList[i].Loot          = "[RESS]1*Coin";               //TODO [DROP] With droppable item
				MonsterList[i].Hp            = 75;
				MonsterList[i].Damage        = 13;
				//MonsterList[i].SpawnCd       = 1.0f;
				MonsterList[i].AttackCd      = 1.5f;
				MonsterList[i].AttackRange   = 2.5f;
				MonsterList[i].SightRange    = 20;
				MonsterList[i].SkillReward   = 1.1f;
				MonsterList[i].MonsterPrefab = _PrefabManager.Monster_Spider;
			}
			else if(MonsterList[i].Name == "SpiderQueen")
			{
				MonsterList[i].Loot          = "[RESS]1*Coin";               //TODO [DROP] With droppable item
				MonsterList[i].Hp            = 120;
				MonsterList[i].Damage        = 30;
				//MonsterList[i].SpawnCd       = 1.0f;
				MonsterList[i].AttackCd      = 2.0f;
				MonsterList[i].AttackRange   = 5.0f;
				MonsterList[i].SightRange    = 10;
				MonsterList[i].SkillReward   = 1.2f;
				MonsterList[i].MonsterPrefab = _PrefabManager.Monster_SpiderQueen;
			}
		}
	}
	
	static public void SpawnMonster(Monster _MonsterToSpawn, Vector3 _PositionToSpawn)
	{
		GameObject _SpawnedMonster;
		
		_SpawnedMonster = GameObject.Instantiate(_MonsterToSpawn.MonsterPrefab, _PositionToSpawn, Quaternion.identity)as GameObject;
		_SpawnedMonster.name = _MonsterToSpawn.Name;
		_SpawnedMonster.GetComponent<MonsterProfile>().IniMonsterType(_MonsterToSpawn);
	}
	
}
