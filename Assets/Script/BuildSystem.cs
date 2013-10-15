using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List class;
using System;

static class BuildSystem {
	
	static GameManager   _GameManager   = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
	static PrefabManager _PrefabManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<PrefabManager>();
	static GameObject    _Player        = GameObject.FindGameObjectWithTag("Player");
	
	static int _buildState; //0 = No build, 1 = Currently Building, 2 = Built
	
	public static void BuildBuilding (Building _BuildingToBuild) 
	{	
		//_BuildingPosition = new Vector3(_PlayerTransform.transform.position.x + 5, _PlayerTransform.transform.position.y, _PlayerTransform.transform.position.z);
		
		List<CraftSystem.ExpenditureNeeded> _buildingRessourceNeeded  = new List<CraftSystem.ExpenditureNeeded>();	//Declare a list that contain all the ressource needed
		_buildingRessourceNeeded = CraftSystem.parseString(_BuildingToBuild.Recipe);//TODO: Update with GO from object instead of hardcoded craftingtable
		
		if(CraftSystem.TestRessource(_buildingRessourceNeeded) == true) //Test if all ressources are available
		{
			EnterBuildMode(_BuildingToBuild);
			_GameManager.ChangeState("Play");	
			
			
			_BuildingToBuild.NbrBuilt++;
			CraftSystem.SpendRessource(_buildingRessourceNeeded);
			 _GameManager.AddChatLogHUD("[BUIL] " + _BuildingToBuild.Name + " has been built");
			Character.GiveExpToSkill(Character.SkillList[(int)SkillName.Crafter],50);
		}
	}
	
	private static void EnterBuildMode(Building _newBuilding)
	{
		Transform _PlayerTransform = _Player.transform;
		Vector3 _BuildingPosition;
		Quaternion _BuildingOrientation = Quaternion.identity;
		
		Vector3 _OffsetToAdd;
		float	_distToBuild = 5.0f;;
		
		
		_buildState = 1;
		_GameManager.ChangeState("Build");	
		
		_OffsetToAdd      = _PlayerTransform.forward * _distToBuild;
		_BuildingPosition = _PlayerTransform.position + _OffsetToAdd;
		
		_BuildingOrientation = _PlayerTransform.rotation * Quaternion.Euler(0, -90, 0);
		GameObject.Instantiate(_PrefabManager.Prefab_Crafting_Table.gameObject, _BuildingPosition, _BuildingOrientation);
	}
}