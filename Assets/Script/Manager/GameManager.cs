using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List class;
using System;					  // For Enum

public class GameManager : MonoBehaviour {
	
	private PlayerHUD _PlayerHUD;
	private Camera _PlayerCam;
	private PrefabManager _PrefabManager;
	private GameObject _Player;
	private GameObject _PlayerMaster;
	private GameObject _Spartan;
	private GameObject _CurObjectInteracted;
	private PlayerHUD.DungeonParameters _CurDungeonParameters;
	private List<string> _discussionListString = new List<string>();
	private Quaternion _originalRotation;
	
	private string _curAction  = "None";  // Current action being executed by the player (Fighting,Mining,Woodcutting...)
	private string _curVersion = "0.39"; // Current Version
	private string _curZone    = "Camp";  // Current zone the player is in (Menu,Camp,Dungeon...)
	private string _curState   = "Menu";  // Current State of the game (Menu, Play, Talk, Build)
	private string _lastState  = "Menu";  // State at the end of the last frame
	private float _curProgress    = 0;    // Current progress value 
	private int   _discussionStep = 0;    // Current Discussion Step with Spartan 
	private int _maxDungeonLevel  = 1;    // Maximum Dungeon Level
	private bool _testForTaskCompletion = false; // Don't test task completion in the starting menu
	
	public string CurVersion
	{
		get {return _curVersion; }
		set {_curVersion = value; }
	}
	
	public string CurState
	{
		get {return _curState; }
	}
	
	public string LastState
	{
		get {return _lastState; }
	}
	
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
	
	public PlayerHUD.DungeonParameters CurDungeonParameters
	{
		get {return _CurDungeonParameters; }
		set {CurDungeonParameters = value; }
	}
	
	void Awake ()
	{
		Character.IniCharacter();
		Bestiary.IniBestiary();
		Inventory.IniInventory();
		ItemInventory.IniItemInventory();
		
		_Player       = GameObject.FindGameObjectWithTag("Player");
		_PlayerCam    = GameObject.FindGameObjectWithTag("MainCamera").camera;
		_PlayerMaster = GameObject.FindGameObjectWithTag("PlayerMaster");
		_PlayerHUD    = _PlayerMaster.GetComponent<PlayerHUD>();
		DungeonLevelPool.IniDungeonLevelPool();
		
	   _PrefabManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<PrefabManager>();
	}
	
	// Use this for initialization. This is called after "Camp" scene is loaded
	public void IniGame () 
	{
		// Initialize Game
		SaveLoadSystem.Load(); // Load game
		SkipTutorial();
		ChangeState("Play");
		_testForTaskCompletion = true;
		
		// Initialize Environment
		Camera.mainCamera.GetComponent<Skybox>().material = GetComponent<TextureManager>().Material_Skybox_Camp;  // Set Skybox
		BuildSystem.SpawnCart();
		
		// Initialize Character
		_Player.transform.position = new Vector3(0f,1.50f,-7.5f);
		_Player.transform.rotation.SetLookRotation(Vector3.back); // This line doesn't work
		Character.RefillHP (); // Make sure player has full HP
		Character.RefillMP ();// Make sure player has full HP
		
		if(!SaveLoadSystem.IsSaveExist) // If there are no existing save
		{
			NewGame(); // Setup a new game
		}
	}
	
	// Initialize a new game.
	private void NewGame()
	{
		Character.TaskList[(int)TaskName.MainQuest0].Unlock(); // Unlock the first quest
		Character.SetActiveTask(Character.TaskList[(int)TaskName.MainQuest0]); // Set the first quest as active task
		ItemInventory.EquipWeapon (Inventory.WeaponList[(int)WeaponName.RockSword]);
		ItemInventory.AddItem(Inventory.WeaponList[(int)WeaponName.Hammer]);
	}
	
	// Update is called once per frame
	void Update () {
		
		// Find the spartan if the current scene is the camp
		if(_Spartan == null && Application.loadedLevelName == "Camp")
		{
			_Spartan = GameObject.FindGameObjectWithTag("Spartan");	
		}
		
		// Update the bar when the is an action being done
		if(_curAction != "None")
		{
			UpdateProgress();
		}
		
		// Evaluate the position of the player to store the current zone in a variable
		EvaluateZone();
		
		// 
		if(_curState == "Talk")
		{
			DoDiscussion();
		}
		
		//Regenerate Mana for character
		Character.RegenMP();
		
		// Prevent from testing for task completion in menu
		if(_testForTaskCompletion)
		{
			TestForTaskCompletion(); //Loop trough all task to look for completion
		}
	}
	
	private void DoDiscussion()
	{
		// If Left click is pressed
		if(Input.GetMouseButtonDown(0))
			{
				//Display all discussion step stored one by one
				if(_discussionStep < _discussionListString.Count)
				{
					_PlayerHUD.ShowDiscussion(_discussionListString[_discussionStep]); // Display the frame and the text
					_discussionStep++;
				}
				else
				{
					_PlayerHUD.HideDiscussion(); // Hid the discussion when all line have been displayed	
					ChangeState ("Play");
					_discussionStep = 0;
				}
			}	
	}
	
	// Called at the end of the frame update
	void LateUpdate()
	{
		_lastState = _curState;	// Store the last state
	}
	
	// Update the current progress
	void UpdateProgress() {
		
		float _progressSpeed = 100.0f;
                
                if(_curAction != "Fighting" && _curAction != "Default" && _curAction != "Woodcutting" && _curAction != "Mining")
                {
                        if(ItemInventory.EquippedWeapon != null)
                        {
                                _progressSpeed = ItemInventory.EquippedWeapon.Speed;
                        }
                        
                        if(Vector3.Distance(_CurObjectInteracted.transform.position, _Player.transform.position) < ItemInventory.EquippedWeapon.Range)
                        {
                                _curProgress = _curProgress + (_progressSpeed*Time.deltaTime);
                                if(_curProgress >= 100)
                                {
                                        _curProgress =  _curProgress - 100;
                                        CompleteAction();
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
                        if(ItemInventory.EquippedWeapon != null)
                        {
                                _progressSpeed = ItemInventory.EquippedWeapon.Speed;
                        }
                        
                        _curProgress = _curProgress + (_progressSpeed*Time.deltaTime);
                        if(ItemInventory.EquippedWeapon != null)
                        {
                                RotateEquippedWeapon();
                        }
                        
                        
                        if(_curProgress >= 100)
                        {
                                _curProgress =  0;
                               CompleteAction();
                                _curAction = "None";
                                _curProgress = 0;
                        }
                }
        }
	
	// Complete the current action
	void CompleteAction()
	{
		if(_curAction != "Fighting")
		{
			Character.ProcAction(_curAction);
			Inventory.ProcAction(_curAction);
			_curAction = "None";
			_curProgress =  0;
		}
		
	}
	
	// Basic animation of the Weapon on attack (90° Rotation back-and-forth)
	private void RotateEquippedWeapon()
	{
		GameObject _GO_EquippedItem = GameObject.FindGameObjectWithTag("EquippedItem");
		
		if(_GO_EquippedItem != null)
		{
			if(_curProgress >= 100)
			{
				_GO_EquippedItem.transform.localRotation = Quaternion.AngleAxis(90.0f, new Vector3(0.0f, 1.0f, 0.0f));
			}
			else
			{
				float _angleToRotate = 7.25f*Mathf.Sin(((Mathf.Deg2Rad*_curProgress/100*180) + Mathf.PI/2))*Time.deltaTime;
				_GO_EquippedItem.transform.RotateAroundLocal(new Vector3(1.0f,0.0f,0.0f), _angleToRotate);
			}
		}
	}
	
	// Skip Tutorial and enable/give everthing that would've happen in tutorial (Function isn't really helpful/functionning in its current state)
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
		
		
	}
	
	// Verify the requirement(kills,ressource..) for each unlocked task and finish them
	void TestForTaskCompletion() //Todo, simplify this logic
	{
		for(int i = 0; i <  Character.TaskList.Length; i++)
		{
			if((Character.TaskList[i].IsUnlocked == true && Character.TaskList[i].IsFinished == false) && Character.TaskList[i].Requirement != "None") //Look for task that are Unlocked but not Finished
			{
				if(Character.TaskList[i].MissionType != 1 || _Spartan != null)
				{
					if(Character.TaskList[i].TestForCompletion() == true)
					{
						Character.TaskList[i].Finish();	
					}	
				}
			}
		}			
	}
	
	// Claim Reward(Item,Ressource, Unlocked Task) for completion of tasks
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
					case "[ULTA]":
						
						for(int j = 0; j <  _RewardToGive.Count; j++)
						{
							TaskName TaskIndex = (TaskName) Enum.Parse(typeof(TaskName), _RewardToGive[j].type);  
							Character.TaskList[(int)TaskIndex].Unlock();
						}
						break;
					
					case "[DISS]":
						if(_curReward == "Spartan")
						{
							//Debug.Log (_Spartan.GetComponent<SpartanInteract>().CurrentTask);
							//Debug.Log (_Spartan.GetComponent<SpartanInteract>().TaskState);
							if(_Spartan != null)
							{
								if(_Spartan.GetComponent<SpartanInteract>().TaskState != 2)
								{
									_Spartan.GetComponent<SpartanInteract>().FinishCurTask();
								}
							}
							else
							{
								Debug.LogWarning ("GameManager.cs - ClaimReward(). Spartan Not Found");
							}
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
							WeaponName WeaponIndex = (WeaponName) Enum.Parse(typeof(WeaponName), _RewardToGive[j].type);  
							for(int k = 0; k < _RewardToGive[j].nbr; k++)
							{
								//Debug.Log ("Gave 1 : " + Inventory.WeaponList[(int)WeaponIndex].Name );
								ItemInventory.AddItem(Inventory.WeaponList[(int)WeaponIndex]);
							}
						
							//Inventory.WeaponList[(int)WeaponIndex].CurValue += Convert.ToInt16(_RewardToGive[j].nbr);
						}
						break;
				}
			}	
		}
	}
	
	// Evaluate the position of the player to set _curZone
	void EvaluateZone () 
	{
		
		string _lastZone = _curZone;
		if(Application.loadedLevelName == "Camp")
		{
			if((_Player.transform.position.x > -26 && _Player.transform.position.x < 13) && (_Player.transform.position.z > -30 && _Player.transform.position.z < 11))
			{	
				_curZone = "Camp";
			}
			else if((_Player.transform.position.x > -49 && _Player.transform.position.x < -40) && (_Player.transform.position.z > -11 && _Player.transform.position.z < -5))
			{
				OpenDungeonMenu();
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
	
	// Open Dungeon Menu
	public void OpenDungeonMenu()
	{
		if(_curState == "Play")
		{
			_PlayerHUD.ChangeBoxView("DungeonMenu");
		}
	}
	
	// Start the dungeon by saving game and loading the Dungeon scene
	public void StartDungeon(PlayerHUD.DungeonParameters _DungeonParametersSelected)
	{
		_CurDungeonParameters = _DungeonParametersSelected;
		SaveLoadSystem.Save();
		BuildSystem.CreatedBuildingList = new List<GameObject>(); // Todo: This probably need to go somewhere else
		Application.LoadLevel("Dungeon");
	}
	
	// Change the dungeon state to Win/Lose/Abandon
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
	
	// Toggle the menu between Menu/Play
	public void ToggleMenu()
	{
		if(_curState == "Menu")
		{
			_PlayerHUD.CloseHUD();
		}
		
		else if(_curState == "Play")
		{
			_PlayerHUD.StartHUD();
		}
		
		
	}
	
	// Change state, show/hide mouse and activate/deactivate control
	public void ChangeState(string _newState)
	{
		if(_newState == "Menu")
		{
			_curState = "Menu";
			_Player.GetComponent<CharacterMotor>().enabled = true;
			Screen.lockCursor = false;
			_PlayerHUD.isDisplayCursor = false;
			_Player.GetComponent<MouseLook>().enabled = false;
			_PlayerCam.GetComponent<MouseLook>().enabled = false;	
		}	
		else if(_newState == "Play")
		{
			_Player.GetComponent<CharacterMotor>().enabled = true;
			_curState = "Play";
			Screen.lockCursor = true;
			_PlayerHUD.isDisplayCursor = true;
			_Player.GetComponent<MouseLook>().enabled = true;
			_PlayerCam.GetComponent<MouseLook>().enabled = true;
		}
		else if(_newState == "Talk")
		{
			_curState = "Talk";
			Screen.lockCursor = true;
			_PlayerHUD.isDisplayCursor = false;
			_Player.GetComponent<CharacterMotor>().enabled = false;
			_Player.GetComponent<MouseLook>().enabled = true;
			_PlayerCam.GetComponent<MouseLook>().enabled = true;
		}
		else if(_newState == "Build")
		{
			_curState = "Build";
			Screen.lockCursor = true;
			_PlayerHUD.isDisplayCursor = false;
			_Player.GetComponent<CharacterMotor>().enabled = true;
			_Player.GetComponent<MouseLook>().enabled = true;
			_PlayerCam.GetComponent<MouseLook>().enabled = true;
		}
		else
		{
			ChangeState("Play");
		}
	}
	
	// Start a new discussion with Spartan
	public void StartDiscussion(List<string> _ListStringToSay)
	{
		ChangeState("Talk");
		_discussionStep = 0;
		_discussionListString = _ListStringToSay;
	}
	
	// Evaluated the raycast's collided object and start an action in consequence (Todo: This function should be split in different part. It's too big)
	public void RaycastAnalyze(Collider _collidedObj)
	{
		//Debug.Log ("BuildState : " + BuildSystem.BuildState + ", _curState : " + _curState);
		//Debug.Log ("_collidedObj : " + _collidedObj.name    + ", Weapon : " + (ItemInventory.EquippedWeapon.IdWeapon == Inventory.WeaponList[(int)WeaponName.Hammer].IdWeapon));
		
		int _idEquippedWeapon;
		if(ItemInventory.EquippedWeapon == null){_idEquippedWeapon = -1;} else {_idEquippedWeapon = ItemInventory.EquippedWeapon.IdWeapon;}
		
		if(BuildSystem.BuildState == 0 && _collidedObj != null)
		{
			// Verify that there is an EquippedWeapon
			
			
			// If equipped item is an hammer, start the building creation. Else, interact with the object.
			if(_idEquippedWeapon == Inventory.WeaponList[(int)WeaponName.Hammer].IdWeapon && _curState == "Play" && Application.loadedLevelName == "Camp" && _collidedObj.tag != "Interactive" && _collidedObj.tag != "Spartan" )
			{
				_PlayerHUD.ChangeBoxView("BuildList");
			}
			else
			{
				float _distanceInteraction = 5.0f;
				if(ItemInventory.EquippedWeapon != null)
				{
					_distanceInteraction = ItemInventory.EquippedWeapon.Range;
				}
				else
				{
					_distanceInteraction = 4.5f;
				}
				
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
								case "CartBroken":
									if(ItemInventory.EquippedWeapon == Inventory.WeaponList[(int)WeaponName.Hammer] && Character.TaskList[(int)TaskName.MainQuest1].IsUnlocked == true)
									{
										Debug.Log("Repaired");
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
		}
		else
		{
			if(_idEquippedWeapon == Inventory.WeaponList[(int)WeaponName.Hammer].IdWeapon && _curState == "Play" && Application.loadedLevelName == "Camp")
			{
				_PlayerHUD.ChangeBoxView("BuildList");
			}
			else
			{
				DoAction("Default");	
			}
		}
	}
	
	// Evaluated the raycast's collided object to display information at mouse
	public void RaycastAnalyzeAtMouse(Collider _collidedObj)
	{
		float _distanceInteraction = 5.0f;
		if(ItemInventory.EquippedWeapon != null)
		{
			_distanceInteraction = ItemInventory.EquippedWeapon.Range;
		}
		
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
					case "wan":
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
	
	// Start a new action
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
					if(ItemInventory.EquippedWeapon != null)
					{
						_curAction = _newAction;
						_Player.GetComponent<PlayerSound>().PlaySound("SwordSwing");
					}
					break;
				default:
					Debug.LogWarning ("No Item in GameManager");
					break;
			}
		}
	}
	
	// On RightClick, cast a spell or take control of a building
	public void RightClick(Collider _collidedObj)
	{
		float distanceBuild = 7.0f;
		BuildingManager _BuildingManager = null;
		
		// Verify that there is an EquippedWeapon
		int _idEquippedWeapon;
		if(ItemInventory.EquippedWeapon == null){_idEquippedWeapon = -1;} else {_idEquippedWeapon = ItemInventory.EquippedWeapon.IdWeapon;}
		
		// If the player use an hammer in the camp
		if(_idEquippedWeapon == Inventory.WeaponList[(int)WeaponName.Hammer].IdWeapon && Application.loadedLevelName == "Camp")
		{
			// Test for a BuildingManager component to identify building.
			if(_collidedObj != null)
			{
				Debug.Log("Looking for Building Manager");
				if(_collidedObj.gameObject.GetComponent<BuildingManager>() != null)
				{
					_BuildingManager = _collidedObj.gameObject.GetComponent<BuildingManager>();
				}
				else if (_collidedObj.transform.parent.GetComponent<BuildingManager>() != null)
				{
					_BuildingManager = _collidedObj.transform.parent.GetComponent<BuildingManager>();
				}
				else
				{
					Debug.LogWarning ("[GameManager.RightClick() - BuildingManager not found");	// Not necessary warning worthy but will help for now. To verify
				}
				
				if(_BuildingManager != null && Vector3.Distance (_Player.transform.position, _collidedObj.transform.position) < distanceBuild)
				{
					_BuildingManager.ActivateBuilding();
				}
			}			
		}
		else
		{
			MagicBook.CastSpell(MagicBook.ActiveSpell);	
		}
	}
	
	// Attack a target : Damage it and give exp to the player
	public void AttackTarget()
	{
		int _damageDealt = calculateDamage();
		
		// Look if there is a MonsterProfile on the target. Otherwise take it's parent
		if(_CurObjectInteracted.GetComponent<MonsterProfile>() != null)
		{
			_CurObjectInteracted.GetComponent<MonsterProfile>().DamageMonster(_damageDealt);
		}
		else
		{
			_CurObjectInteracted.transform.parent.GetComponent<MonsterProfile>().DamageMonster(_damageDealt);
		}
		
		Character.GiveExpToSkill(Character.SkillList[(int)SkillName.Fighter],_damageDealt/Mathf.Pow (Character.SkillList[(int)SkillName.Fighter].Level,1.4f));
		if(ItemInventory.EquippedWeapon != null)
		{
			ItemInventory.EquippedWeapon.GiveExp(_damageDealt/Mathf.Pow (ItemInventory.EquippedWeapon.Level, 1.4f));
		}
	}
	
	// Calculate Damage dealt from the Weapon/Fighter skill
	public int calculateDamage()
	{
		int _damageDealt;
		int _damageFromWeapon;
		if(ItemInventory.EquippedWeapon == null)
		{
			_damageFromWeapon = 0;
		}
		else
		{
			_damageFromWeapon = ItemInventory.EquippedWeapon.Damage;
		}
		
		_damageDealt = Character.CurDamage + _damageFromWeapon;
		return _damageDealt;
	}
	
	// Unique Action are particular action that shouldn't happen regularly
	public void DoUniqueAction(string _actionToDo)
	{
		if(_curState == "Play")
		{
			switch(_actionToDo)
			{
				case "RepairCart":
					Destroy(_CurObjectInteracted); // Destroy the current Broken Cart
					BuildSystem.SpawnCart("Cart"); // Create a new Cart
				
					_Spartan.GetComponent<SpartanInteract>().FinishCurTask();
					//ActivateButtonInHUD(3); // Button3 is Skill Button
					//Character.SkillList[(int)SkillName.Crafter].Unlocked = true;
				
					Character.GiveExpToSkill(Character.SkillList[(int)SkillName.Crafter],50.0f); 
					break;
				Default: 
					Debug.LogWarning ("UNIQUE ACTION NOT FOUND");
					break;
			}
		}
	}
	
	// Activate menu button, this allow player to click them
	public void ActivateButtonInHUD(int _buttonPos)
	{
		_PlayerHUD.EnableButtonInHUD(_buttonPos);
	}
	
	// Display a line of text on the HUD
	public void AddChatLogHUD(string _stringToAddChatLog)
	{
	
		_PlayerHUD.AddChatLog(_stringToAddChatLog);
		
	}
	
	// Enable an upgrade on the dungeon
	public void UpgradeDungeon(DungeonUpgrade _UpgradeToEnable)
	{
		if(Inventory.RessourceList[(int)RessourceName.Coin].CurValue >= _UpgradeToEnable.CostCoin && Character.InfluencePoints >= _UpgradeToEnable.CostInfluence)
		{
			Inventory.RessourceList[(int)RessourceName.Coin].CurValue -= _UpgradeToEnable.CostCoin;
			Character.InfluencePoints -= _UpgradeToEnable.CostInfluence;
			_UpgradeToEnable.IsEnabled = true;
		}
		
		if(_UpgradeToEnable.Name == "First Upgrade")
		{
			DungeonLevelPool.DungeonUpgradeList[(int)DungeonUpgradeName.HardcoreMode].IsUnlocked = true;	
			DungeonLevelPool.DungeonUpgradeList[(int)DungeonUpgradeName.SkeletonCrypt].IsUnlocked = true;	
		}
	}
	
	
}
