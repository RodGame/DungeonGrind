using UnityEngine;
using System.Collections;
using System; // For Enum class;


static class DungeonLevelPool {
	
	private static int _dungeonLevelMax;
	public static DungeonLevel[] DungeonLevelList;
	public static DungeonUpgrade[] DungeonUpgradeList;
	
	//public static Monster[] MonsterList;
	//private static PrefabManager _PrefabManager;
	
	public static void IniDungeonLevelPool()
	{
		_dungeonLevelMax = 20;
		DungeonLevelList   = new DungeonLevel  [_dungeonLevelMax+1];	
		DungeonUpgradeList = new DungeonUpgrade[Enum.GetValues(typeof(DungeonUpgradeName)).Length];	
		//_PrefabManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<PrefabManager>();
		
		IniDungeonLevelList();
		IniDungeonUpgradeList();
	}
		
	
	private static void IniDungeonLevelList()
	{
		int _nbrPseudoSpider    = 1;
		int _nbrSpider          = 0;
		int _nbrSpiderQueen     = 0;
		int _nbrSkeletonToon    = 0;
		int _nbrSkeletonFighter = 0;
		int _nbrSkeletonKing    = 0;
		
		for(int level = 1; level <= _dungeonLevelMax; level++)
		{
			
			DungeonLevelList[level] = new DungeonLevel();
			
			// Spider Nest
			if(level <= 10)
			{
				DungeonLevelList[level].Name = "Spider Nest [" + level.ToString() + "]";
				
				_nbrPseudoSpider = 1;
				_nbrSpider = 0;
				_nbrSpiderQueen = 0;
				
				_nbrPseudoSpider = (int)Mathf.Round(level*1f);
				_nbrSpider       = (int)Mathf.Round(level*0.6f);
				if(level >= 7) {_nbrSpiderQueen = level - 6;}
				
								 
				DungeonLevelList[level].MonsterList = _nbrPseudoSpider + "*PseudoSpider+" + _nbrSpider + "*Spider+" + _nbrSpiderQueen + "*SpiderQueen";              //TODO [DROP] With droppable item
				//DungeonLevelList[level - 1].Reward = level.ToString + "*InfluencePoints";
				DungeonLevelList[level].SizeX = 40 + 8*level;
				DungeonLevelList[level].SizeY = 40 + 8*level;
				DungeonLevelList[level].NbrSquareForSpawn = 2 + (int)Mathf.Round(0.7f*level);
			}
			else if(level > 10 && level <= 20)
			{
				DungeonLevelList[level].Name = "Skeleton Crypt [" + level.ToString() + "]";
				
				_nbrPseudoSpider    = 11 - level;
				_nbrSpider          = 13 - level + UnityEngine.Random.Range (0,1);
				_nbrSpiderQueen     = Mathf.CeilToInt((15 - level)/2);
				_nbrSkeletonToon    = Mathf.RoundToInt((level - 10)*0.5f);
				_nbrSkeletonFighter = 1 + Mathf.RoundToInt((level - 11)*0.75f);
				_nbrSkeletonKing    = Mathf.RoundToInt((level - 15)*0.75f);
				
				
				DungeonLevelList[level].MonsterList = _nbrPseudoSpider + "*PseudoSpider+" + _nbrSpider + "*Spider+" + _nbrSpiderQueen + "*SpiderQueen+" + _nbrSkeletonToon + "*SkeletonToon+" + _nbrSkeletonFighter + "*SkeletonFighter+" + _nbrSkeletonKing + "*SkeletonKing";              //TODO [DROP] With droppable item
				//DungeonLevelList[level - 1].Reward = level.ToString + "*InfluencePoints";
				DungeonLevelList[level].SizeX = 30 + 7*level;
				DungeonLevelList[level].SizeY = 30 + 7*level;
				DungeonLevelList[level].NbrSquareForSpawn = 2 + (int)Mathf.Round(0.5f*level);
			}
		}
	}
	
	private static void IniDungeonUpgradeList()
	{
		for(int i = 0; i < DungeonUpgradeList.Length; i++)
		{
			
			DungeonUpgradeList[i] = new DungeonUpgrade();
			DungeonUpgradeList[i].Name = ((DungeonUpgradeName)i).ToString();
			if(DungeonUpgradeList[i].Name == "FirstUpgrade")
			{
			    DungeonUpgradeList[i].Name          = "First Upgrade";
				DungeonUpgradeList[i].Description	= "Give acces to Dungeon Upgrades";
				DungeonUpgradeList[i].IsUnlocked	= true;
	    		DungeonUpgradeList[i].CostInfluence = 5;
                DungeonUpgradeList[i].CostCoin      = 5;
			}
			else if(DungeonUpgradeList[i].Name == "SkeletonCrypt")
			{
			    DungeonUpgradeList[i].Name          = "Skeleton Crypt";
				DungeonUpgradeList[i].Description	= "Unlock lvl 11-20";
	    		DungeonUpgradeList[i].CostInfluence = 50;
                DungeonUpgradeList[i].CostCoin      = 50;
			}
			else if(DungeonUpgradeList[i].Name == "HardcoreMode")
			{
				DungeonUpgradeList[i].Name          = "Hardcore Mode";
			    DungeonUpgradeList[i].Description   = "Unlock Hardcore Mode";
	    		DungeonUpgradeList[i].CostInfluence = 100;
                DungeonUpgradeList[i].CostCoin      = 150;
			}
			else if(DungeonUpgradeList[i].Name == "WaveMode")
			{
			    DungeonUpgradeList[i].Name          = "Wave Mode";
				DungeonUpgradeList[i].Description	= "Infini monster";
	    		DungeonUpgradeList[i].CostInfluence = 5;
                DungeonUpgradeList[i].CostCoin      = 5;
			}
		}
	}
}