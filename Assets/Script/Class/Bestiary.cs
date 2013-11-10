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
				MonsterList[i].Loot          = "None";               //TODO [DROP] With droppable item
				MonsterList[i].Hp            = 40;
				MonsterList[i].Damage        = 6;
				MonsterList[i].SpawnCd       = 1.0f;
				MonsterList[i].AttackCd      = 1.0f;
				MonsterList[i].AttackRange   = 3.0f;
				MonsterList[i].SightRange    = 20;
				MonsterList[i].MoveSpeed     = 325;
				MonsterList[i].SkillReward   = 1.0f;
				MonsterList[i].Size          = 1.0f;
				//MonsterList[i].Height        = 0.55f;
					
				MonsterList[i].AnimAttkName = "attack_Melee";
				MonsterList[i].AnimWalkName = "walk";
				MonsterList[i].AnimIdleName = "iddle";
				MonsterList[i].AnimKnkcName = "damage";
				MonsterList[i].MonsterPrefab = _PrefabManager.Monster_PseudoSpider;
			}
			else if(MonsterList[i].Name == "Spider")
			{
				MonsterList[i].Loot          = "[RESS]1*Coin";               //TODO [DROP] With droppable item
				MonsterList[i].Hp            = 90;
				MonsterList[i].Damage        = 15;
				//MonsterList[i].SpawnCd       = 1.0f;
				MonsterList[i].AttackCd      = 1.5f;
				MonsterList[i].AttackRange   = 3.0f;
				MonsterList[i].SightRange    = 20;
				MonsterList[i].MoveSpeed     = 250;
				MonsterList[i].SkillReward   = 1.1f;
				MonsterList[i].Size          = 1.5f;
				//MonsterList[i].Height        = 0.55f;
					
				MonsterList[i].AnimAttkName = "attack_Melee";
				MonsterList[i].AnimWalkName = "walk";
				MonsterList[i].AnimIdleName = "iddle";
				MonsterList[i].AnimKnkcName = "damage";
				MonsterList[i].MonsterPrefab = _PrefabManager.Monster_Spider;
			}
			else if(MonsterList[i].Name == "SpiderQueen")
			{
				MonsterList[i].Loot          = "[RESS]1*Coin";               //TODO [DROP] With droppable item
				MonsterList[i].Hp            = 175;
				MonsterList[i].Damage        = 35;
				//MonsterList[i].SpawnCd       = 1.0f;
				MonsterList[i].AttackCd      = 2.0f;
				MonsterList[i].AttackRange   = 6.0f;
				MonsterList[i].SightRange    = 25;
				MonsterList[i].MoveSpeed     = 275;
				MonsterList[i].SkillReward   = 1.25f;
				MonsterList[i].Size          = 2.0f;
				//MonsterList[i].Height        = 1.00f;
					
				MonsterList[i].AnimAttkName = "attack_Melee";
				MonsterList[i].AnimWalkName = "walk";
				MonsterList[i].AnimIdleName = "iddle";
				MonsterList[i].AnimKnkcName = "damage";
				MonsterList[i].MonsterPrefab = _PrefabManager.Monster_SpiderQueen;
			}
			else if(MonsterList[i].Name == "SkeletonToon")
			{
				MonsterList[i].Loot          = "[RESS]2*Coin";               //TODO [DROP] With droppable item
				MonsterList[i].Hp            = 150;
				MonsterList[i].Damage        = 35;
				//MonsterList[i].SpawnCd       = 1.0f;
				MonsterList[i].AttackCd      = 2.0f;
				MonsterList[i].AttackRange   = 4.0f;
				MonsterList[i].SightRange    = 20;
				MonsterList[i].MoveSpeed     = 250;
				MonsterList[i].SkillReward   = 1.35f;
				MonsterList[i].Size          = 0.75f;
				//MonsterList[i].Height        = 1.5f;
					
				MonsterList[i].AnimAttkName = "Attack";
				MonsterList[i].AnimWalkName = "Walk";
				MonsterList[i].AnimIdleName = "Idle";
				MonsterList[i].AnimKnkcName = "Knock_back";
				MonsterList[i].MonsterPrefab = _PrefabManager.Monster_SkeletonToon;
			}
			else if(MonsterList[i].Name == "SkeletonFighter")
			{
				MonsterList[i].Loot          = "[RESS]2*Coin";               //TODO [DROP] With droppable item
				MonsterList[i].Hp            = 225;
				MonsterList[i].Damage        = 45;
				//MonsterList[i].SpawnCd       = 1.0f;
				MonsterList[i].AttackCd      = 1.5f;
				MonsterList[i].AttackRange   = 4.5f;
				MonsterList[i].SightRange    = 20;
				MonsterList[i].MoveSpeed     = 350;
				MonsterList[i].SkillReward   = 1.55f;
				MonsterList[i].Size          = 1.5f;
				//MonsterList[i].Height        = 2.00f;
					
				MonsterList[i].AnimAttkName = "Attack";
				MonsterList[i].AnimWalkName = "Charge";
				MonsterList[i].AnimIdleName = "Idle";
				MonsterList[i].AnimKnkcName = "Damage";
				MonsterList[i].MonsterPrefab = _PrefabManager.Monster_SkeletonFighter;
			}
			else if(MonsterList[i].Name == "SkeletonKing")
			{
				MonsterList[i].Loot          = "[RESS]3*Coin";               //TODO [DROP] With droppable item
				MonsterList[i].Hp            = 275;
				MonsterList[i].Damage        = 60;
				//MonsterList[i].SpawnCd       = 1.0f;
				MonsterList[i].AttackCd      = 2.0f;
				MonsterList[i].AttackRange   = 6.0f;
				MonsterList[i].SightRange    = 20;
				MonsterList[i].MoveSpeed     = 300;
				MonsterList[i].SkillReward   = 1.75f;
				MonsterList[i].Size          = 2.0f;
				//MonsterList[i].Height        = 2.50f;
					
				MonsterList[i].AnimAttkName = "Attack";
				MonsterList[i].AnimWalkName = "Run";
				MonsterList[i].AnimIdleName = "Idle";
				MonsterList[i].AnimKnkcName = "Damage";
				MonsterList[i].MonsterPrefab = _PrefabManager.Monster_SkeletonKing;
			}
		}
	}
	
	static public void SpawnMonster(Monster _MonsterToSpawn, Vector3 _PositionToSpawn)
	{
		
		// UnityEngine.Random.Range(0,1)
		GameObject _SpawnedMonster;
		
		
		_SpawnedMonster = GameObject.Instantiate(_MonsterToSpawn.MonsterPrefab, _PositionToSpawn, Quaternion.AngleAxis(UnityEngine.Random.Range(0.0f,359.0f), Vector3.up))as GameObject;
		_SpawnedMonster.name = _MonsterToSpawn.Name;
		
		//Initialize the monster type with according buffs
		_SpawnedMonster.GetComponent<MonsterProfile>().IniMonsterType(_MonsterToSpawn, GameObject.FindGameObjectWithTag ("GameMaster").GetComponent<GameManager>().CurDungeonParameters.isHardcore);
	}
	
}
