using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List class;

public class NGUI_HUD : MonoBehaviour {
	
	private int          _currentBuildingId     = -1;
	private int          _currentBuildingTypeId = 0;
	
	private List<string> _buildingTypeList = new List<string> {"Utility", "Storage", "Structural", "Decoration"};  //, "NoType(DEBUG)"}; 
	
	private string     _UIRoot2DName;
	private GameObject _UI_Root2D;
	
	private string     _panelTaskName;
	private GameObject _panelTask;
	
	private string     _panelSkillName;
	private GameObject _panelSkill;	
	
	private string     _panelBuildingName;
	private GameObject _panelBuilding;	
	private GameObject _BuildingTable;
	private GameObject _BuildingTypeLabel;
	private GameObject _BuildingInfoName;
	private GameObject _BuildingInfoRecipe;
	private GameObject _BuildingToTween;
	
	private PrefabManager _PrefabManager;
	private GameManager   _GameManager;
	
	private bool _isBuildingSelected = false;
	
	// Use this for initialization
	void Start () {
		_PrefabManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<PrefabManager>();
		_GameManager   = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
		
		// Name of GameObject used for NGUI
		_UIRoot2DName   = "UI Root (2D)";
		_panelTaskName  = _UIRoot2DName + "/Camera/Window/Anchor/PanelTask";
		_panelSkillName = _UIRoot2DName + "/Camera/Window/Anchor/PanelSkill";
		_panelBuildingName = _UIRoot2DName + "/Camera/Window/Anchor/PanelBuilding";
		
		// Find the GameObjects used for NGUI
		_UI_Root2D  = transform.FindChild(_UIRoot2DName).gameObject;
		_panelTask  = transform.FindChild(_panelTaskName).gameObject;
		_panelSkill = transform.FindChild(_panelSkillName).gameObject;
		_panelBuilding = transform.FindChild(_panelBuildingName).gameObject;
		
		_BuildingTable = _panelBuilding.transform.FindChild("Building List/SubPanelBuilding/BuildingTable").gameObject;
		if(_BuildingTable == null){Debug.LogWarning ("[NGUI_HUD.UpdateBuildList() - BuildingTable not found");}
		
		_BuildingTypeLabel = _panelBuilding.transform.FindChild("Building Type/Label - BuildingType").gameObject;
		if(_BuildingTypeLabel == null){Debug.LogWarning ("[NGUI_HUD.UpdateBuildList() - BuildingType not found");}
		
		_BuildingInfoName = _panelBuilding.transform.FindChild("Building Info/Label (Building Name)").gameObject;
		if(_BuildingInfoName == null){Debug.LogWarning ("[NGUI_HUD.BuildingType_Choose() - _BuildingInfoName not found");}
		
		_BuildingInfoRecipe = _panelBuilding.transform.FindChild("Building Info/Label (Recipe)").gameObject;
		if(_BuildingInfoRecipe == null){Debug.LogWarning ("[NGUI_HUD.BuildingType_Choose() - _BuildingInfoName not found");}
		
		_BuildingToTween = transform.FindChild("BuildingToTween/BuildingMesh").gameObject;
		if(_BuildingInfoRecipe == null){Debug.LogWarning ("[NGUI_HUD.DisplayBuildingTween() - _BuildingToTween not found");}
		
		// Initialize NGUI		
		UpdateAll();
		CloseAll ();
	}
	
	private void UpdateAll()
	{
		UpdateHUD ("TaskList");	
		UpdateHUD ("SkillList");	
		UpdateHUD ("BuildList");	
	}
	
	// NGUI - Update
	public void UpdateHUD(string _NewWindow)
	{
		switch(_NewWindow)
		{
			case "TaskList":
				UpdateTaskList();
				break;
			case "SkillList":
				UpdateSkillList();
				break;
			case "BuildList":
				UpdateBuildingList();
				break;
			default:
				_UI_Root2D.SetActive (false);
				Debug.LogWarning ("NGUI_HUD/Display() - Tried to opened an unknown window");
				break;
		}
	}
	
	// Update all the game object in the TaskList panel
	private void UpdateTaskList()
	{
		GameObject _TaskTable = _panelTask.transform.FindChild("Task Log/SubPanelTask/TaskTable").gameObject;
		if(_TaskTable == null){Debug.LogWarning ("[NGUI_HUD.UpdateTaskList() - TaskTable not found");}
		
		UITable _UITable = _TaskTable.GetComponent<UITable>();
		int _nbrTaskAdded = 0;
		string _titlePrefix;
		
		
		// Destroy all children from the table
		List<GameObject> children = new List<GameObject>();
		foreach (Transform child in _TaskTable.transform) children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));
		
		// Loop throught all task
		for(int i = 0; i < Character.TaskList.Length; i++)
		{
			// Add each unlocked task
			if(Character.TaskList[i].IsUnlocked == true)
			{
				// Create a new task and put it in the NGUI task window
				GameObject _NewTaskNGUI = Instantiate(_PrefabManager.NGUI_Task_TableTemplate, _panelTask.transform.position, _panelTask.transform.rotation) as GameObject;
				_NewTaskNGUI.transform.parent = _TaskTable.transform;
				_NewTaskNGUI.transform.localPosition = new Vector3(0.0f,0.0f - _nbrTaskAdded*38.0f,0.0f);
				_NewTaskNGUI.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
				
				// Setup the task title and description
				
				_titlePrefix = EvaluateTaskTitlePrefix (Character.TaskList[i]);
				if(Character.TaskList[i].IsFinished)
				{
					_NewTaskNGUI.transform.FindChild ("Label - Title").gameObject.GetComponent<UILabel>().text = (_titlePrefix + "[9E9E9E]"  + Character.TaskList[i].Title +"[-]");
					_NewTaskNGUI.transform.FindChild ("Tween/Label - Description").gameObject.GetComponent<UILabel>().text = "[COMPLETED] " + Character.TaskList[i].Description;
				}
				else
				{
					_NewTaskNGUI.transform.FindChild ("Label - Title").gameObject.GetComponent<UILabel>().text = (_titlePrefix + Character.TaskList[i].Title);
					_NewTaskNGUI.transform.FindChild ("Tween/Label - Description").gameObject.GetComponent<UILabel>().text = Character.TaskList[i].Description;
				}
				//_UITable.Reposition ();
				_nbrTaskAdded++;
			}
		}
	}
	
	// Update all the game object in the SkillList panel
	private void UpdateSkillList()
	{
		GameObject _SkillTable = _panelSkill.transform.FindChild("Skill List/SubPanelSkill/SkillTable").gameObject;
		if(_SkillTable == null){Debug.LogWarning ("[NGUI_HUD.UpdateSkillList() - SkillTable not found");}
		
		UITable _UITable = _SkillTable.GetComponent<UITable>();
		int _nbrSkillAdded = 0;
		string _titlePrefix;
		
		// Destroy all children from the table
		List<GameObject> children = new List<GameObject>();
		foreach (Transform child in _SkillTable.transform) children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));
		
		
		// Loop throught all Skill
		for(int i = 0; i < Character.SkillList.Length; i++)
		{
			// Add each unlocked Skillys
			if(Character.SkillList[i].IsUnlocked == true)
			{
				// Create a new skill object and put it in the NGUI task window
				GameObject _NewSkillNGUI = Instantiate(_PrefabManager.NGUI_Skill_SkillTemplate, _panelSkill.transform.position, _panelSkill.transform.rotation) as GameObject;
				_NewSkillNGUI.transform.parent = _SkillTable.transform;
				_NewSkillNGUI.transform.localPosition = new Vector3(0.0f,100.0f - _nbrSkillAdded*38.0f,0.0f);
				_NewSkillNGUI.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
				
				// Setup the skill information and progress
				
				// Skill name and level([66FA33][SkillName][-] LVL 1)
				_NewSkillNGUI.transform.FindChild ("Label - SkillName").gameObject.GetComponent<UILabel>().text = "[66FA33][" + Character.SkillList[i].Name + "][-]";
				_NewSkillNGUI.transform.FindChild ("Label - SkillLevel").gameObject.GetComponent<UILabel>().text = "[000000][" + "LVL" + Character.SkillList[i].Level + "][-]" ;
				
				_NewSkillNGUI.transform.FindChild ("Progress Bar").GetComponent<UISlider>().value = Character.SkillList[i].CurExp/100.0f;	
				_NewSkillNGUI.transform.FindChild ("Progress Bar/Label - Progress").GetComponent<UILabel>().text = "[000000]" + Character.SkillList[i].CurExp + " %[-]";
				
				/*_titlePrefix = EvaluateTitlePrefix (Character.SkillList[i]);
				if(Character.SkillList[i].IsFinished)
				{
					_NewSkillNGUI.transform.FindChild ("Label - Title").gameObject.GetComponent<UILabel>().text = (_titlePrefix + "[9E9E9E]"  + Character.SkillList[i].Title +"[-]");
					_NewSkillNGUI.transform.FindChild ("Tween/Label - Description").gameObject.GetComponent<UILabel>().text = "[COMPLETED] " + Character.SkillList[i].Description;
				}
				else
				{
					_NewSkillNGUI.transform.FindChild ("Label - Title").gameObject.GetComponent<UILabel>().text = (_titlePrefix + Character.SkillList[i].Title);
					_NewSkillNGUI.transform.FindChild ("Tween/Label - Description").gameObject.GetComponent<UILabel>().text = Character.SkillList[i].Description;
				}*/
				//_UITable.Reposition ();
				_nbrSkillAdded++;
			}
		}
	}
	
	// Update all the game object in the BuildingList panel
	private void UpdateBuildingList()
	{
		
		// Update Building Type Label
		_BuildingTypeLabel.GetComponent<UILabel>().text = "[000000]" + _buildingTypeList[_currentBuildingTypeId] + "[-]";
		
		// Update Building List Table
		UITable _UITable = _BuildingTable.GetComponent<UITable>();
		int _nbrBuildingAdded = 0;
		//string _titlePrefix;
		
		
		// Destroy all children from the table
		List<GameObject> children = new List<GameObject>();
		foreach (Transform child in _BuildingTable.transform) children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));
		
		// Loop throught all Building
		for(int i = 0; i < Inventory.BuildingList.Length; i++)
		{
			// Add each building of the current type
			if(Inventory.BuildingList[i].Type == _buildingTypeList[_currentBuildingTypeId])
			{
				if(_isBuildingSelected == false)
				{
					BuildingType_Choose(Inventory.BuildingList[i].Id);
					_isBuildingSelected = true;
				}
				GameObject _NewBuildingNGUI = NGUITools.AddChild (_BuildingTable,  _PrefabManager.NGUI_Build_BuildingTemplate);
				_NewBuildingNGUI.transform.FindChild ("Label (Building Name)").gameObject.GetComponent<UILabel>().text = Inventory.BuildingList[i].Name; //"[66FA33][" + Inventory.BuildingList[i].Name + "][-]";
				_NewBuildingNGUI.transform.FindChild ("SlicedSprite (Row Outline)").gameObject.GetComponent<OnClickBuildingType>().BuildingId = Inventory.BuildingList[i].Id;
				//Debug.Log(Inventory.BuildingList[i].Name);
			}
		}
		
		_UITable.Reposition ();
		
		// Display selected building
	}
	
	// Write the TaskTitle with the good colors
	private string EvaluateTaskTitlePrefix(Task _EvaluatedTask)
	{
		string _prefixOutput;
		int _MissionType = _EvaluatedTask.MissionType;
		
		if(_MissionType == 0) // Random
		{
			_prefixOutput = "[66FA33][R][-]";
		}
		else if(_MissionType == 1) // Main quest
		{
			_prefixOutput = "[0101DF][M" + _EvaluatedTask.MissionId + "][-]";
		}
		else if(_MissionType == 2) // Secondary quest
		{
			_prefixOutput = "[16AB35][S][-]";
		}
		else
		{
			_prefixOutput = "WrongType";	
		}
		
		return _prefixOutput;
	}
	
	// NGUI - DISPLAY
	public void Display(string _WindowToShow)
	{
		CloseAll ();
		_UI_Root2D.SetActive (true);
		switch(_WindowToShow)
		{
			case "TaskList":
				_panelTask.SetActive (true);
				break;
			case "SkillList":
				_panelSkill.SetActive (true);
				break;
			case "BuildList":
				_panelBuilding.SetActive (true);
				break;
			default:
				_UI_Root2D.SetActive (false);
				Debug.LogWarning ("NGUI_HUD/Display() - Tried to opened an unknown window");
				break;
		}
	}
		
	public void BuildingType_LeftClick()
	{
		if(_currentBuildingTypeId == 0)
		{
			_currentBuildingTypeId = _buildingTypeList.Count - 1;
		}
		else
		{
			_currentBuildingTypeId-- ;
		}
		_isBuildingSelected = false;
		UpdateBuildingList();
	}
	
	public void BuildingType_RightClick()
	{
		if(_currentBuildingTypeId == _buildingTypeList.Count - 1)
		{
			_currentBuildingTypeId = 0;
		}
		else
		{
			_currentBuildingTypeId++ ;
		}
		_isBuildingSelected = false;
		UpdateBuildingList();
	}
	
	public void BuildingType_Choose(int _buildingIdSelected)
	{
		 _currentBuildingId = _buildingIdSelected;
		_BuildingInfoName.GetComponent<UILabel>().text = Inventory.BuildingList[_buildingIdSelected].Name;
		 List<string> _ListStringRessource = Utility.parseStringToListString(Inventory.BuildingList[_buildingIdSelected].Recipe);
		string _Recipe = "";
		for(int i = 0; i < _ListStringRessource.Count; i++)
		{
			_Recipe	+= _ListStringRessource[i] + "\n";
		}
		_BuildingInfoRecipe.GetComponent<UILabel>().text = _Recipe;
	}
	
	private void DisplayBuildingTween()
	{
		//_currentBuildingId
		TweenPosition _BuildingTweenPosition = _BuildingToTween.GetComponent<TweenPosition>();
		_BuildingTweenPosition.PlayForward ();
	}
	
	public void BuildingType_Build()
	{
		if(BuildSystem.BuildState == 0)
		{
			CloseAll();
			BuildSystem.BuildBuilding (Inventory.BuildingList[_currentBuildingId]);
			_GameManager.ChangeState ("Build");
		}
	}
		
	
	
	// NGUI - CLOSE
	public void CloseAll()
	{
		_UI_Root2D.SetActive (false);
		_panelTask.SetActive (false);
		_panelSkill.SetActive (false);
		_panelBuilding.SetActive (false);
	}
	
	
}
