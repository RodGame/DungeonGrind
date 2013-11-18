using UnityEngine;
using System.Collections;

static class GameAnalyticsManager {
	
	static GameManager _GameManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
	
	static public void SendDatas()
	{
		Debug.Log ("Telemetry Sent");
		//Send skills level
		for(int i = 0; i < Character.SkillList.Length; i++)
		{
			GA.API.Design.NewEvent ("Skill:" + Character.SkillList[i].Name, Character.SkillList[i].Level);
		}
		
		//Send dungeon information
		for(int i = 0; i < Character.SkillList.Length; i++)
		{
			GA.API.Design.NewEvent ("Dungeon:MaxLevel" , _GameManager.MaxDungeonLevel);
			GA.API.Design.NewEvent ("Dungeon:Influence", Character.InfluencePoints);
		}
		
		// Send Killcount
		for(int i = 0; i < Bestiary.MonsterList.Length; i++)
		{
			GA.API.Design.NewEvent ("MonsterKills:" + Bestiary.MonsterList[i].Name,  Bestiary.MonsterList[i].NbrKilled);
		}
		
		//Send Building Information
		GA.API.Design.NewEvent("Building:TotalNumber",BuildSystem.CreatedBuildingList.Count);
		
		
	}
}
