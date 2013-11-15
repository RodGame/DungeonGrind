using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List class;
using System;

static class BuildSystem {
	
	static GameManager   _GameManager   = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
	static PrefabManager _PrefabManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<PrefabManager>();
	static GameObject    _Player        = GameObject.FindGameObjectWithTag("Player");
	static private GameObject _ActiveBuilding;
	static private int _buildState; //0 = No build, 1 = Currently Building, 2 = Built
	// Structure that contain the position of the start/end of a wall to be created
	static public List<GameObject> CreatedBuildingList = new List<GameObject>();
	
	static private Vector3 _CartPosition = new Vector3(1.0f, -1.25f, -12.5f);
	static private Quaternion _CartRotation = Quaternion.Euler (270.0f, 250.0f, 0.0f);
	
	static public GameObject ActiveBuilding
	{ 
		get {return _ActiveBuilding; }
	}
	
	static public int BuildState
	{ 
		get {return _buildState; }
		set {_buildState = value; }
	}
	
	// Evaluate the ressource needed, start the creation of the building and give skill points
	public static void BuildBuilding (Building _BuildingToBuild) 
	{	
		//_BuildingPosition = new Vector3(_PlayerTransform.transform.position.x + 5, _PlayerTransform.transform.position.y, _PlayerTransform.transform.position.z);
		
		List<Utility.ParsedString> _buildingRessourceNeeded  = new List<Utility.ParsedString>();	//Declare a list that contain all the ressource needed
		_buildingRessourceNeeded = Utility.parseString(_BuildingToBuild.Recipe);//TODO: Update with GO from object instead of hardcoded craftingtable
		
		if(CraftSystem.TestRessource(_buildingRessourceNeeded) == true) //Test if all ressources are available
		{
			EnterBuildMode(_BuildingToBuild);
			
			_BuildingToBuild.NbrBuilt++;
			CraftSystem.SpendRessource(_buildingRessourceNeeded);
			Character.GiveExpToSkill(Character.SkillList[(int)SkillName.Crafter],50);
		}
	}
	
	// Instantiate the building and initialize it
	private static void EnterBuildMode(Building _newBuilding)
	{
		Transform _PlayerTransform = _Player.transform;
		Vector3 _BuildingPosition;
		Quaternion _BuildingRotation = Quaternion.identity;
		Vector3 _OffsetToAdd;
		float	_distToBuild = 5.0f;;
		GameObject CreatedBuilding;
		
		_OffsetToAdd      = _PlayerTransform.forward * _distToBuild;
		_BuildingPosition = _PlayerTransform.position + _OffsetToAdd;
		_BuildingRotation = _PlayerTransform.rotation * Quaternion.Euler(0, -90, 0);
		
		// Instantiate and initialize the new object
		CreatedBuilding = SpawnBuilding(_newBuilding.Id, _BuildingPosition, _BuildingRotation);
		UpdateBuildingInfo(CreatedBuilding, CreatedBuildingList.Count, _newBuilding.Id);
		CreatedBuildingList.Add(CreatedBuilding);
		
		CreatedBuilding.GetComponent<BuildingManager>().ActivateBuilding ();
		
	}
	
	// Function called when the object is dropped
	public static void UpdateBuildingInfo(GameObject _BuildingToUpdate, int _id, int _buildingId)
	{
		if(_BuildingToUpdate.GetComponent<BuildingManager>() != null)
		{
			_BuildingToUpdate.GetComponent<BuildingManager>().Id = _id;
			_BuildingToUpdate.GetComponent<BuildingManager>().buildingId = _buildingId;
			
		}
		else
		{
			Debug.LogWarning("Wrong Gameobject was updated");	
		}
	}
	
	public static void SpawnCart()
	{
		if(Character.TaskList[(int)TaskName.Spartan1].IsFinished || (Character.TaskList[(int)TaskName.Spartan1].IsUnlocked && !Character.TaskList[(int)TaskName.Spartan1].IsFinished && SaveLoadSystem.Spartan_TaskState > 1))
		{
			SpawnCart("Cart");
		}
		else
		{
			SpawnCart("CartBroken");
		}
	}
	
	public static void SpawnCart(string _Type)
	{
		GameObject _GO;
		
		if(_Type == "Cart")
		{
			_GO = GameObject.Instantiate(_PrefabManager.Environment_Cart,_CartPosition,_CartRotation) as GameObject;
			_GO.name = "Cart";
		}
		else if(_Type == "CartBroken")
		{
			_GO = GameObject.Instantiate(_PrefabManager.Environment_CartBroken,_CartPosition,_CartRotation) as GameObject;
			_GO.name = "CartBroken";
		}
		else
		{
			Debug.LogWarning("Tried to spawn a wrong Cart in BuildSystem/SpawnCart");	
		}
	}
	
	
	// Function used when reentering the Camp scene to recreate all objects.
	public static void SpawnAllBuilding()
	{
		//List<GameObject> CreatedBuildingList = new List<GameObject>();
		SaveLoadSystem.LoadBuildings();
		//Debug.Log ("To Create " + SaveLoadSystem.LoadedBuildingInfo.Count + " Buildings");
		for(int i = 0; i < SaveLoadSystem.LoadedBuildingInfo.Count; i++)
		{
			GameObject _SpawnedBuilding = SpawnBuilding(SaveLoadSystem.LoadedBuildingInfo[i].buildingId, SaveLoadSystem.LoadedBuildingInfo[i].Position, SaveLoadSystem.LoadedBuildingInfo[i].Rotation) as GameObject;
			UpdateBuildingInfo(_SpawnedBuilding, SaveLoadSystem.LoadedBuildingInfo[i].id, SaveLoadSystem.LoadedBuildingInfo[i].buildingId);
			CreatedBuildingList.Add (_SpawnedBuilding);
		}
		//Debug.Log ("Created " + CreatedBuildingList.Count + " Buildings");
	}
	
	// Instantiate a building and a a BuildingManager component
	public static GameObject SpawnBuilding(int _id, Vector3 _Position, Quaternion _Rotation)
	{
		GameObject _NewBuilding = GameObject.Instantiate(Inventory.BuildingList[_id].BuildingPrefab, _Position, _Rotation) as GameObject;
		_NewBuilding.AddComponent<BuildingManager>();
		_NewBuilding.AddComponent<Rigidbody>();
		_NewBuilding.GetComponent<Rigidbody>().useGravity = false;
		_NewBuilding.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			
		return _NewBuilding;
	}
}