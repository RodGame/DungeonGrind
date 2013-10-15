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
			if(MonsterList[i].Name == "Spider")
			{
				MonsterList[i].Loot          = "[ITEM]1*Hammer,[RESS]1*Coin";               //TODO [DROP] With droppable item
				MonsterList[i].Hp            = 10;
				MonsterList[i].Damage        = 1;
				MonsterList[i].SpawnCd       = 1.0f;
				MonsterList[i].AttackCd      = 2.0f;
				MonsterList[i].AttackRange   = 3.0f;
				MonsterList[i].SightRange    = 20;
				MonsterList[i].MonsterPrefab = _PrefabManager.Prefab_Monster_Spider;
			}
		}
	}
}
