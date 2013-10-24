using UnityEngine;
using System.Collections;
using System; // For Enum class;

static class DungeonLevelPool {
	
	private static int _dungeonLevelMax;
	public static DungeonLevel[] DungeonLevelList;
	//public static Monster[] MonsterList;
	//private static PrefabManager _PrefabManager;
	
	public static void IniDungeonLevelPool()
	{
		_dungeonLevelMax = 20;
		DungeonLevelList  = new DungeonLevel [_dungeonLevelMax+1];	
		//_PrefabManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<PrefabManager>();
		
		IniDungeonLevelList();
	}
		
	
	private static void IniDungeonLevelList()
	{
		for(int level = 1; level <= _dungeonLevelMax; level++)
		{
			
			DungeonLevelList[level] = new DungeonLevel();
			
			// Spider Nest
			if(level <= 20)
			{
				
				int _nbrPseudoSpider = 1;
				int _nbrSpider = 0;
				int _nbrSpiderQueen = 0;
				
				_nbrPseudoSpider = (int)Mathf.Round(level*1f);
				_nbrSpider       = (int)Mathf.Round(level*0.7f);
				if(level >= 7) {_nbrSpiderQueen = level - 3;}
				
				DungeonLevelList[level].Name = "Spider Nest" + level.ToString();
								 
				DungeonLevelList[level].MonsterList = _nbrPseudoSpider + "*PseudoSpider+" + _nbrSpider + "*Spider+" + _nbrSpiderQueen + "*SpiderQueen";              //TODO [DROP] With droppable item
				//DungeonLevelList[level - 1].Reward = level.ToString + "*InfluencePoints";
				DungeonLevelList[level].SizeX = 35 + 12*level;
				DungeonLevelList[level].SizeY = 35 + 12*level;
				DungeonLevelList[level].NbrSquareForSpawn = 2 + (int)Mathf.Round(0.7f*level);
			}
		}
	}
}
