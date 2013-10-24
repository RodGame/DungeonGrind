using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List class;
using System;					  // For Enum

public class GameManager : MonoBehaviour {
	
	private PlayerHUD _PlayerHUD;
	private GameObject _Player;
	private GameObject _PlayerMaster;
	private GameObject _Spartan;
	private Camera _PlayerCam;
	private GameObject _CurObjectInteracted;
	
	private float _curProgress = 0;
	private int _curProgressSpeed = 120;
	private string _curAction = "None";
	private string _curZone   = "Camp";
	private string _curState; //Menu, Play, Talk
	private int    _distanceInteraction = 6;
	private int    _discussionStep = 0;
	private int    _dungeonLevelToSpawn = 0;
	
	private List<string> _discussionListString = new List<string>();
	
	private int _maxDungeonLevel = 1;
	
	public string CurState
	{
		get {return _curState; }
	}
	
	public GameObject NewCart;
	
	public float CurProgress
	{
		get {return _curProgress; }
		set {_curProgress = value; }
	}
	
	public string CurAction
	{
		get {return _curAction; }
		set {_curAction = value; }
	}
	
	public string CurZone
	{
		get {return _curZone; }
		set {_curZone = value; }
	}
	
	public int MaxDungeonLevel
	{
		get {return _maxDungeonLevel; }
		set {_maxDungeonLevel = value; }
	}
	
	public int DungeonLevelToSpawn
	{
		get {return _dungeonLevelToSpawn; }
		set {_dungeonLevelToSpawn = value; }
	}
	
	void Awake ()
	{
		Character.IniCharacter();
		Bestiary.IniBestiary();
		Inventory.IniInventory();
		
		_Player       = GameObject.FindGameObjectWithTag("Player");
		_PlayerCam    = GameObject.FindGameObjectWithTag("MainCamera").camera;
		_PlayerMaster = GameObject.FindGameObjectWithTag("PlayerMaster");
		_PlayerHUD    = _PlayerMaster.GetComponent<PlayerHUD>();
		
	}
	// Use this for initialization
	public void IniGame () 
	{
		
		SaveLoadSystem.Load();
		Character.SetActiveTask(Character.TaskList[(int)TaskName.Spartan0]);
		
		SkipTutorial();
		Camera.mainCamera.GetComponent<Skybox>().material = GetComponent<TextureManager>().Material_Skybox_Camp;
		ChangeState("Play");
	}
	
	// Update is called once per frame
	void Update () {
		if(_Spartan == null && GameObject.FindGameObjectWithTag("Spartan") != null)
		{
			_Spartan = GameObject.FindGameObjectWithTag("Spartan");	
		};
		if(_curAction != "None")
		{
			UpdateBar();
		}
		EvaluateZone();
		
		if(_curState == "Talk")
		{
			if(Input.GetMouseButtonDown(0))
			{
				if(_discussionStep < _discussionListString.Count)
				{
					_PlayerHUD.ShowDiscussion(_discussionListString[_discussionStep]);
					_discussionStep++;
				}
				else
				{
					_PlayerHUD.HideDiscussion();	
					ChangeState ("Play");
					_discussionStep = 0;
				}
			}
		}
		
		if(BuildSystem.BuildState != 0)
		{
			float rotaSpeed = 2.0f;
			if(Input.GetKey(KeyCode.Q))
			{
				BuildSystem.CreatedBuilding.transform.RotateAroundLocal(new Vector3(0.0f,1.0f,0.0f), rotaSpeed*Time.deltaTime);
			}
			else if(Input.GetKey(KeyCode.E))
			{
				BuildSystem.CreatedBuilding.transform.RotateAroundLocal(new Vector3(0.0f,1.0f,0.0f), -rotaSpeed*Time.deltaTime);
			} 
			
			if(Input.GetMouseButtonDown(0))
			{
				BuildSystem.CreatedBuilding.transform.parent = null;
				BuildSystem.BuildState = 0;
				_curState = "Play";
			}
		}
		
		//Regenerate Mana for character
		Character.RegenMP();
		
		//Loop trough all task to look for completion
		TestForTaskCompletion();
	}
	
	void UpdateBar() {
		
		if(_curAction != "Fighting" && _curAction != "Default")
		{
			if(Vector3.Distance(_CurObjectInteracted.transform.position, _Player.transform.position) < _distanceInteraction)
			{
				_curProgress = _curProgress + (_curProgressSpeed*Time.deltaTime);
				if(_curProgress >= 100)
				{
					_curProgress =  _curProgress - 100;
					CompleteBar();
					_curAction = "None";
					_curProgress = 0;
				}
			}
			else
			{
				_PlayerHUD.AddChatLog("[MISC] Action was not completed. Stay close to the object");
				_curAction = "None";
				_curProgress = 0;
			}
		}
		else
		{
			_curProgress = _curProgress + (_curProgressSpeed*Time.deltaTime);
			RotateEquippedItem();
			
			
			if(_curProgress >= 100)
			{
				_curProgress =  0;
				CompleteBar();
				_curAction = "None";
				_curProgress = 0;
			}
		}
	}
	
	private void RotateEquippedItem()
	{
		GameObject _GO_EquippedItem = GameObject.FindGameObjectWithTag("EquippedItem");
		Quaternion _originalRotation = Quaternion.identity;
		if(_GO_EquippedItem != null)
		{
			if(_curProgress == 0)
			{
				_originalRotation = _GO_EquippedItem.transform.rotation;	
			}
			
			if(_curProgress >= 100)
			{
				_GO_EquippedItem.transform.rotation = _originalRotation;
			}
			else
			{
				float _angleToRotate = 0.1f*Mathf.Sin(((Mathf.Deg2Rad*_curProgress/100*180) + Mathf.PI/2));
				_GO_EquippedItem.transform.RotateAroundLocal(new Vector3(1.0f,0.0f,0.0f), _angleToRotate);
			}
		}
	}
	
	void CompleteBar()
	{
		if(_curAction != "Fighting")
		{
			Character.ProcAction(_curAction);
			Inventory.ProcAction(_curAction);
		}
		
	}
	
	void SkipTutorial()
	{
		GameObject[] _Gates;
		Vector3      _NewRotation;
		
		_PlayerHUD.EnableButtonInHUD(0);
		_PlayerHUD.EnableButtonInHUD(1);
		_PlayerHUD.EnableButtonInHUD(2);
		_PlayerHUD.EnableButtonInHUD(3);
		_PlayerHUD.EnableButtonInHUD(4);
		_PlayerHUD.EnableButtonInHUD(5);
		//_PlayerHUD.EnableButtonInHUD(6);
		
		_Gates = GameObject.FindGameObjectsWithTag("Interactive");
		
		for(int i = 0; i < _Gates.Length; i++)
		{
			if(_Gates[i].name == "Gate")
			{
				_NewRotation = new Vector3(270, 180, 0);
				_Gates[i].transform.rotation = Quaternion.Euler(_NewRotation.x, _NewRotation.y, _NewRotation.z);
			}
		}
		//Inventory.BuildingList[(int)BuildingName.CraftingTable].IsUnlocked = true; // Unlock the building of the Crafting Table
		
		ItemInventory.EquipItem (Inventory.ItemList[(int)ItemName.RockSword]);
	}
	
	void TestForTaskCompletion()
	{
		for(int i = 0; i <  Character.TaskList.Length; i++)
		{
			if((Character.TaskList[i].IsUnlocked == true && Character.TaskList[i].IsFinished == false) && Character.TaskList[i].Requirement != "None") //Look for task that are Unlocked but not Finished
			{
				if(Character.TaskList[i].TestForCompletion() == true)
				{
					CompleteTask(Character.TaskList[i]);	
				}
			}
		}			
	}
		
	public void CompleteTask(Task _TaskToComplete)
	{
		ClaimReward (_TaskToComplete.Reward);
		_TaskToComplete.IsFinished = true;
	}
	
	public void ClaimReward(string _taskReward)
	{
		char _differentTypeDelimiter  = ',';
		string _curType;
		string _curReward;
			
		if( _taskReward != "None")
		{
			string[] _curRewardPart = _taskReward.Split (_differentTypeDelimiter); //Parse the Requirement string the _differentTypeDelimiter(,) char
			List<Utility.ParsedString> _RewardToGive  = new List<Utility.ParsedString>();	//Declare a list that contain all the ressource needed
			
			for(int i = 0; i < _curRewardPart.Length; i++)
			{
				
				_curType   = _curRewardPart[i].Substring(0,6);
				_curReward = _curRewardPart[i].Substring(6);
				if(_curType != "[DISS]")
				{
					_RewardToGive = Utility.parseString(_curReward);
				}
				
				switch(_curType)
				{
					case "[ULTA	]":
						
						for(int j = 0; j <  _RewardToGive.Count; j++)
						{
							TaskName TaskIndex = (TaskName) Enum.Parse(typeof(TaskName), _RewardToGive[j].type);  
							Character.TaskList[(int)TaskIndex].IsUnlocked = true;
						}
						break;
					
					case "[DISS]":
						if(_curReward == "Spartan")
						{
							Debug.Log ("Spartan Encountered");
							_Spartan.GetComponent<SpartanInteract>().IncreaseStateWithPlayer(); //Todo [MISC]RepairCart for requirement
							
						}
						break;
					
					case "[RESS]":
						_RewardToGive = Utility.parseString(_curReward);
						for(int j = 0; j <  _RewardToGive.Count; j++)
						{
							RessourceName RessourceIndex = (RessourceName) Enum.Parse(typeof(RessourceName), _RewardToGive[j].type); 
							Inventory.RessourceList[(int)RessourceIndex].CurValue += Convert.ToInt16(_RewardToGive[j].nbr);
						}
						break;
					
					case "[ITEM]":
						_RewardToGive = Utility.parseString(_curReward);
						for(int j = 0; j <  _RewardToGive.Count; j++)
						{
							ItemName ItemIndex = (ItemName) Enum.Parse(typeof(ItemName), _RewardToGive[j].type);  
							for(int k = 0; k < _RewardToGive[j].nbr; k++)
							{
								//Debug.Log ("Gave 1 : " + Inventory.ItemList[(int)ItemIndex].Name );
								ItemInventory.AddItem(Inventory.ItemList[(int)ItemIndex]);
							}
						
							//Inventory.ItemList[(int)ItemIndex].CurValue += Convert.ToInt16(_RewardToGive[j].nbr);
						}
						break;
				}
			}	
		}
	}
	
	void EvaluateZone () 
	{
		
		string _lastZone = _curZone;
		if(Application.loadedLevelName == "Camp")
		{
			if((_Player.transform.position.x > -26 && _Player.transform.position.x < 13) && (_Player.transform.position.z > -30 && _Player.transform.position.z < 11))
			{	
				_curZone = "Camp";
			}
			else if((_Player.transform.position.x > -48 && _Player.transform.position.x < -40) && (_Player.transform.position.z > -11 && _Player.transform.position.z < -5))
			{
				EnterDungeon();
				_curZone = "Dungeon Warp";
			}
			else
			{
				_curZone = "No Man's Land";	
			}
			
			if(_lastZone != _curZone)
			{
				_PlayerMaster.GetComponent<PlayerHUD>().AddChatLog("[ZONE] You entered a new zone : " + _curZone);
				if(_lastZone == "Dungeon Warp")
				{
					ChangeState ("Play");	
				}
			}
		}
		
	}
	
	public void EnterDungeon()
	{
		if(_curState == "Play")
		{
			_PlayerHUD.ChangeBoxView ("DungeonMenu");
		}
	}
	
	public void ChangeDungeonState(string _newState)
	{
		DungeonManager _DungeonManager = GameObject.FindGameObjectWithTag("DungeonMaster").GetComponent<DungeonManager>();	
		
      	switch(_newState)
		{
		case "Win":
			_DungeonManager.Win ();
			break;
		case "Lose":
			_DungeonManager.Lose ();	
			break;
		case "Abandon":
			_DungeonManager.Abandon ();
			break;	
			
		}
		
		
	}
	
	public void ToggleState()
	{
		if(_curState == "Menu")
		{
			ChangeState("Play");
		}
		
		else if(_curState == "Play")
		{
			ChangeState("Menu");
		}
		
		
	}
	
	public void ChangeState(string _newState)
	{
		_PlayerHUD.isDisplayCursor = true;
		if(_newState == "Menu")
		{
			_curState = "Menu";
			_Player.GetComponent<CharacterController>().enabled = true;
			Screen.lockCursor = false;
			_Player.GetComponent<MouseLook>().enabled = false;
			_PlayerCam.GetComponent<MouseLook>().enabled = false;	
		}	
		else if(_newState == "Play")
		{
			_Player.GetComponent<CharacterController>().enabled = true;
			_curState = "Play";
			Screen.lockCursor = true;
			_Player.GetComponent<MouseLook>().enabled = true;
			_PlayerCam.GetComponent<MouseLook>().enabled = true;
		}
		else if(_newState == "Talk")
		{
			_curState = "Talk";
			Screen.lockCursor = true;
			_Player.GetComponent<CharacterController>().enabled = false;
		}
		else if(_newState == "Build")
		{
			_curState = "Build";
			Screen.lockCursor = true;
			_PlayerHUD.isDisplayCursor = false;
			_Player.GetComponent<CharacterController>().enabled = true;
		}
		else
		{
			ChangeState("Play");
		}
	}
	
	public void StartDiscussion(List<string> _ListStringToSay)
	{
		ChangeState("Talk");
		_discussionStep = 0;
		_discussionListString = _ListStringToSay;
	}
	
	
	
	public void RaycastAnalyze(Collider _collidedObj)
	{
		if(BuildSystem.BuildState == 0)
		{
			if(_collidedObj != null)
			{
				if(Vector3.Distance(_collidedObj.transform.position, _Player.transform.position) <= _distanceInteraction)
				{
					_CurObjectInteracted = _collidedObj.gameObject;
					switch(_collidedObj.tag)
					{
						case "Tree":
							DoAction("Woodcutting");
							break;
						case "Rock":
							DoAction("Mining");
							break;
						case "Monster":
							DoAction("Fighting");
							break;
						case "Spartan":
							_Spartan.GetComponent<SpartanInteract>().InteractWithPlayer();
							break;
						case "CraftingTable": 
							_PlayerHUD.ChangeBoxView("CraftList");
							break;
						case "Interactive":
							switch(_collidedObj.name)
							{
								case "Gate":
									_PlayerMaster.GetComponent<PlayerHUD>().AddChatLog("[MISC] The door won't open. Maybe spartan know something about this.");	
									break;
								case "Cart03":	
									if(ItemInventory.EquippedItem == Inventory.ItemList[(int)ItemName.Hammer])
									{
										DoUniqueAction("RepairCart");
									}
									break;
								default:
									break;
							}
							break;
						default:
							DoAction("Default");
							break;
					}
				}
				else
				{
					DoAction("Default");
				}
			}
			else
			{
				DoAction("Default");	
			}
		}
	}
	
	public void RaycastAnalyzeAtMouse(Collider _collidedObj)
	{
		
		if(_collidedObj != null)
		{
			if(Vector3.Distance (_collidedObj.transform.position, _Player.transform.position) < _distanceInteraction)
			{
				switch(_collidedObj.tag)
				{
					case "Tree":
						_PlayerHUD.DetectInteractive(_collidedObj.tag);
						break;
					case "Rock":
						_PlayerHUD.DetectInteractive(_collidedObj.tag);
						break;
					case "Spartan":
						_PlayerHUD.DetectInteractive(_collidedObj.tag);
						break;
					case "CraftingTable":
						_PlayerHUD.DetectInteractive(_collidedObj.tag);
						break;
					case "Interactive":
						_PlayerHUD.DetectInteractive(_collidedObj.name);
						break;
					case "Monster":
						_PlayerHUD.DetectInteractive(_collidedObj.name);
						break;
					default:
						_PlayerHUD.DetectInteractive("None");
						break;
				}
			}
			else
			{
			_PlayerHUD.DetectInteractive("None");
			}
		}
		else
		{
			_PlayerHUD.DetectInteractive("None");
		}
	}
	
	public void DoAction(string _newAction)
	{
		if(_curState == "Play")
		{
			switch(_newAction)
			{
				case "Woodcutting": 
					_curAction = _newAction;
					break;
				case "Mining": 
					_curAction = _newAction;
					break;
				case "Fighting":
					if(_curProgress == 0)
					{
						AttackTarget();
						_curAction = _newAction;
						_Player.GetComponent<PlayerSound>().PlaySound("SwordHit");
					}
					break;
				case "Default": 
					_curAction = _newAction;
					_Player.GetComponent<PlayerSound>().PlaySound("SwordSwing");
					break;
				default:
					Debug.LogWarning ("No Item in GameManager");
					break;
			}
		}
	}
	
	public void AttackTarget()
	{
		_CurObjectInteracted.transform.parent.GetComponent<MonsterProfile>().DamageMonster(Character.CurDamage);
		Character.GiveExpToSkill(Character.SkillList[(int)SkillName.Fighter],10.0f/Character.SkillList[(int)SkillName.Fighter].Level);
		//_Player.GetComponent<PlayerSound>().PlaySound("SwordHit");
		
	}
	
	public void DoUniqueAction(string _actionToDo)
	{
		if(_curState == "Play")
		{
			switch(_actionToDo)
			{
				case "RepairCart":
					GameObject.Instantiate(NewCart,_CurObjectInteracted.transform.position,_CurObjectInteracted.transform.rotation);
					Destroy(_CurObjectInteracted);
					Character.TaskList[(int)TaskName.Spartan1].CompleteTask();
					//_Spartan.GetComponent<SpartanInteract>().IncreaseStateWithPlayer(); //Todo [MISC]RepairCart for requirement
					ActivateButtonInHUD(3); // Button3 is Skill Button
					Character.SkillList[(int)SkillName.Crafter].Unlocked = true;
					Character.GiveExpToSkill(Character.SkillList[(int)SkillName.Crafter],50.0f); 
					break;
				Default: 
					Debug.LogWarning ("UNIQUE ACTION NOT FOUND");
					break;
			}
		}
	}
	
	public void ActivateButtonInHUD(int _buttonPos)
	{
		_PlayerHUD.EnableButtonInHUD(_buttonPos);
	}
	
	public void AddItemToInventory(Item _ItemToAdd)
	{
		ItemInventory.AddItem(_ItemToAdd);
	}
	
	public void AddChatLogHUD(string _stringToAddChatLog)
	{
	
		_PlayerHUD.AddChatLog(_stringToAddChatLog);
		
	}
	
	public void StartDungeon(int _dungeonLevel)
	{
		_dungeonLevelToSpawn = _dungeonLevel;
		Application.LoadLevel("Dungeon");	
	}
}
