using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List class;

public class NGUI_HUD : MonoBehaviour {
	
	private string     _UIRoot2DName;
	private GameObject _UI_Root2D;
	
	private string     _panelTaskName;
	private GameObject _panelTask;
	
	private string     _panelSkillName;
	private GameObject _panelSkill;	
	
	private PrefabManager _PrefabManager;
	
	// Use this for initialization
	void Start () {
		_PrefabManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<PrefabManager>();
		
		// Name of GameObject used for NGUI
		_UIRoot2DName   = "UI Root (2D)";
		_panelTaskName  = _UIRoot2DName + "/Camera/Window/Anchor/PanelTask";
		_panelSkillName = _UIRoot2DName + "/Camera/Window/Anchor/PanelSkill";
		
		// Find the GameObjects used for NGUI
		_UI_Root2D  = transform.FindChild(_UIRoot2DName).gameObject;
		_panelTask  = transform.FindChild(_panelTaskName).gameObject;
		_panelSkill = transform.FindChild(_panelSkillName).gameObject;
		UpdateAll();
		CloseAll ();
	}
	
	private void UpdateAll()
	{
		UpdateHUD ("TaskList");	
		UpdateHUD ("SkillList");	
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
			default:
				_UI_Root2D.SetActive (false);
				Debug.LogWarning ("NGUI_HUD/Display() - Tried to opened an unknown window");
				break;
		}
	}
	
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
				
				_titlePrefix = EvaluateTitlePrefix (Character.TaskList[i]);
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
				_NewSkillNGUI.transform.localPosition = new Vector3(0.0f,0.0f - _nbrSkillAdded*38.0f,0.0f);
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
	
	private string EvaluateTitlePrefix(Task _EvaluatedTask)
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
			default:
				_UI_Root2D.SetActive (false);
				Debug.LogWarning ("NGUI_HUD/Display() - Tried to opened an unknown window");
				break;
		}
	}
	
	/*private void DisplayTaskNGUI()
	{
		
	}
	
	private void DisplaySkillNGUI()
	{
		
		
	}*/
	
	// NGUI - CLOSE
	public void CloseAll()
	{
		_UI_Root2D.SetActive (false);
		_panelTask.SetActive (false);
		_panelSkill.SetActive (false);
	}
	
	
}
