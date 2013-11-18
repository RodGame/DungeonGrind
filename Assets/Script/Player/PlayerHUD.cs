using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List class;
using System;					  // For Enum

public class PlayerHUD : MonoBehaviour {
	
	private GameManager _GameManager;
	private Ressource   _Ressource;
	
	private GameObject  _Player;
	private Camera      _PlayerCam;
	private GameObject  _PlayerMaster;
	private Weapon        _ItemClicked;
	private Vector2     _MousePosOnClick;
	private int 		_inventorySlotClicked;
	private TextureManager _TextureManager;
	private NGUI_HUD _NGUI_HUD;
	
	private const float _FLOATING_RECT_OPACITY = 0.8f;
	private const int _LINE_HEIGHT = 25;
	private const int _POSX = 25;
	private int _boxPosX  = (int)(_POSX*5.5f);
	private int _boxPosY  = (int)(_LINE_HEIGHT*4);
	private int _boxSizeX = (int)(Screen.width  * 0.60f);
	private int _boxSizeY = (int)(Screen.height * 0.50f);
	private int _buttonX  = 100;
	private int _progBarLength = Screen.width / 2;
	private int _chatLogMaxSize = 6;
	private bool _isDisplayFloatingMenu = false;
	private bool _isInteractiveDetected = false;
	private bool _isDiscussionActive    = false;
	private bool _isDamageLeveling  = true;
	private bool _isCdLeveling      = false;
	private bool _isRangeLeveling   = false;
	private bool _isManaLeveling    = false;
	private string _lastCompLeveled = "Damage";
	private float timeElapsedFloatingMenu = 0.0f;
	private float timeToDeletFloatingMenu = 4.0f;
	//private float timeElapsedDescriptMenu = 0.0f;
	//private float timeToDeletDescriptMenu = 4.0f;
	private float invSlot_sizeX;
	private	float invSlot_sizeY;
	private float invSlot_offset = 10;
	private bool[] _buttonToggle = new bool[7] {false, false, false, false, false, true, false};
	private int[,] _mapToDisplay;
	private bool _isUseNGUI = true;
	
	private string objectRaycastCollided;
	private string ToDisplayInBox;
	private string _mouseOver;
	private string _curDiscussion;
	private List<string> _chatLogListString;
	public bool   isDisplayCursor = true;
	
	public Texture textureProgSkill;
	public Texture textureProgSpell;
	public Texture textureProg;
	public Texture textureProgBackground;
	public Texture WhiteTexture;
	
	//private GameObject NGUI_TaskList;
	
	public DungeonParameters DungeonToGenerate;
	
	public struct DungeonParameters
	{
        public int  level;
     	public bool isHardcore;
		public bool isWave;
	};
	
	// Use this for initialization
	void Start () {
		_PlayerMaster = GameObject.FindGameObjectWithTag("PlayerMaster");
		_Player	      = GameObject.FindGameObjectWithTag("Player");
		_GameManager  = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
		_PlayerCam    = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<Camera>();
		_TextureManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<TextureManager>();
		_NGUI_HUD       = GameObject.FindGameObjectWithTag("NGUI").GetComponent<NGUI_HUD>();
		
		ToDisplayInBox = "TaskList";
		_chatLogListString = new List<string>();
			
		invSlot_sizeX  = _boxSizeX/15;
		invSlot_sizeY  = _boxSizeX/15;
	
		DungeonToGenerate.level = 1;
		DungeonToGenerate.isHardcore    = false;
		DungeonToGenerate.isWave        = false;
		
		//NGUI_TaskList =  GameObject.Find("Anchor(Task)").gameObject;
	}
	
	void Update()
	{
		if(Application.loadedLevelName == "Dungeon")
		{
			UpdateKnownMap();	
		}
		
	}
	
	void UpdateKnownMap()
	{
		// Update Known Map 
		int _playerSightRange = 4;
		
		int playerPosX = Mathf.RoundToInt(_Player.transform.position.x);
		int playerPosZ = Mathf.RoundToInt(_Player.transform.position.z);
		
		DungeonManager _DungeonManager = GameObject.FindGameObjectWithTag("DungeonMaster").GetComponent<DungeonManager>();
		
		for(int i = playerPosX - _playerSightRange; i < playerPosX + _playerSightRange; i++)
		{
			for(int j = playerPosZ - _playerSightRange; j < playerPosZ + _playerSightRange; j++)
			{
				if(i > 0 && i < _DungeonManager.MapSizeX - 1 && j > 0 && j < _DungeonManager.MapSizeZ - 1) // Make sure that the index are within the size of the map
				{
					if(_mapToDisplay[i,j] == 1)
					{
						_mapToDisplay[i,j] = 2;	
					}
				}
			}
		}	
	}
	
	public void InitializeMap(int[,] _dungeonMap, int _mapSizeX, int _mapSizeZ)
	{
		//DungeonManager _DungeonManager = GameObject.FindGameObjectWithTag("DungeonMaster").GetComponent<DungeonManager>();	
		_mapToDisplay = new int[_mapSizeX,_mapSizeZ];
		Array.Copy (_dungeonMap, _mapToDisplay, _mapSizeX * _mapSizeZ);
	}
	
	public void StartHUD()
	{
		if(ToDisplayInBox == "None")
		{
			ToDisplayInBox = "TaskList";	
		}
		ChangeBoxView(ToDisplayInBox);
		_GameManager.ChangeState("Menu");
	}
	
	void OnGUI()
	{
		GUI.skin.button.wordWrap = true;
		GUI.skin.box.wordWrap    = true;
		GUI.skin.label.wordWrap = true;
		
		if(_GameManager.CurState == "Menu")
		{
			DisplayHUD();
		}
		
		if(_isDisplayFloatingMenu == true)
		{
			DisplayFloatingMenu(_MousePosOnClick, _ItemClicked, _inventorySlotClicked);	
		}
		
		DisplayProgressBarAndHPMP();
		DisplayRessources();
		DisplayButtons();
		if(isDisplayCursor == true)
		{
			DisplayCursor();
		}
		DisplayFloatingBox();
		DisplayChatLog();
		DisplayEquippedWeapon();
		DisplayActiveSpell();
		//DisplayHP();
		DisplayActiveTask();
		DisplayTaskObjective();
		if(_isDisplayFloatingMenu)
		{
			if(_isDisplayFloatingMenu == true)
			{
				timeElapsedFloatingMenu += Time.deltaTime;
			}
			
			if(timeElapsedFloatingMenu > timeToDeletFloatingMenu)
			{
				timeElapsedFloatingMenu = 0.0f;
				CloseMenu(); 
			}
		}
		if(_isDiscussionActive == true)
		{
			DisplayDiscussion (_curDiscussion);
		}
	}
	
	private void DisplayHUD()
	{
		if(_isUseNGUI && (ToDisplayInBox == "TaskList" || ToDisplayInBox == "SkillList"))
		{
			//_NGUI_HUD.Display(ToDisplayInBox);
		}
		else
		{
			DisplayBox();
		}	
		
	}
	
	private void DisplayTaskObjective()
	{
		if(Application.loadedLevelName == "Dungeon")
		{
			DungeonManager _DungeonManager = GameObject.FindGameObjectWithTag("DungeonMaster").GetComponent<DungeonManager>();
			
			GUI.Label (new Rect(Screen.width*0.9f, 175 , 100 , _LINE_HEIGHT),"Kill count :  ");	
			GUI.Label (new Rect(Screen.width*0.9f, 200 , 100 , _LINE_HEIGHT), _DungeonManager.NbrMonsterKilled + "/" + _DungeonManager.NbrMonsterSpawned);
		}
		
		
	}
	
	private void DisplayActiveTask()
	{
		if(Character.ActiveTask != null)
		{
			GUI.Label (new Rect(_POSX, _boxPosY + _boxSizeY , 500 , _LINE_HEIGHT),"Active Task : " + Character.ActiveTask.Title);
		}
		else
		{
			GUI.Label (new Rect(_POSX, _boxPosY + _boxSizeY, 500 , _LINE_HEIGHT),"No active task");
		}
	}
	
	private void DisplayDiscussion(string _stringToSay)
	{
		GUI.Box   (new Rect(Screen.width*0.5f,_boxPosY + Screen.width*0.02f ,300,_LINE_HEIGHT*(_stringToSay.Length/20.0f)),_stringToSay);	
		//GUILayout.Box   (new Rect(Screen.width*0.60f - 15,Screen.height*0.40f ,150,_LINE_HEIGHT*4),_stringToSay);	
	}
	
	public  void ShowDiscussion(string _whatToSay)
	{
		_isDiscussionActive = true;
		_curDiscussion = _whatToSay;
	}
	
	public  void HideDiscussion()
	{
		_isDiscussionActive = false;
	}
	
	private void DisplayEquippedWeapon()
	{
		if(ItemInventory.EquippedWeapon == null)
		{
			GUI.Button(new Rect(_POSX + _progBarLength + 25, 0.5f*_LINE_HEIGHT                     , invSlot_sizeX*1.5f, invSlot_sizeY*1.5f),"");
			GUI.Label (new Rect(_POSX + _progBarLength + 23, 0.5f*_LINE_HEIGHT + invSlot_sizeY*1.5f, invSlot_sizeX*2.0f, _LINE_HEIGHT      ),"No Item");
		}
		else 
		{
			   GUI.Label (new Rect(_POSX + _progBarLength + 20, 0.5f*_LINE_HEIGHT + invSlot_sizeY*1.5f, invSlot_sizeX*2.0f, _LINE_HEIGHT      ),ItemInventory.EquippedWeapon.Name);
			if(GUI.Button(new Rect(_POSX + _progBarLength + 25, 0.5f*_LINE_HEIGHT , invSlot_sizeX*1.5f, invSlot_sizeY*1.5f), new GUIContent(ItemInventory.EquippedWeapon.ItemIcon, "mouseOverOnEquippedWeapon")))
			{
				ItemInventory.UnequipWeapon();
			}
		}
		_mouseOver = GUI.tooltip;
	}
	
	private void DisplayActiveSpell()
	{
		if(MagicBook.ActiveSpell == null)
		{
			GUI.Button(new Rect(_POSX + _progBarLength + 25 + invSlot_sizeX*1.5f + 25, 0.5f*_LINE_HEIGHT                     , invSlot_sizeX*1.5f, invSlot_sizeY*1.5f),"");
			GUI.Label (new Rect(_POSX + _progBarLength + 23 + invSlot_sizeX*1.5f + 25, 0.5f*_LINE_HEIGHT + invSlot_sizeY*1.5f, invSlot_sizeX*2.0f, _LINE_HEIGHT      ),"No Spell");
		}
		else 
		{
			   GUI.Label (new Rect(_POSX + _progBarLength + 23 + invSlot_sizeX*1.5f + 25, 0.5f*_LINE_HEIGHT + invSlot_sizeY*1.5f, invSlot_sizeX*2.0f, _LINE_HEIGHT      ),MagicBook.ActiveSpell.Name);
			if(GUI.Button(new Rect(_POSX + _progBarLength + 25 + invSlot_sizeX*1.5f + 25, 0.5f*_LINE_HEIGHT                     , invSlot_sizeX*1.5f, invSlot_sizeY*1.5f),MagicBook.ActiveSpell.SpellIcon))
			{
				//UnEquipWeapon();
			}
			
		}
	}
	
	private void DisplayHP()
	{
		GUI.Label (new Rect(_POSX +_progBarLength + invSlot_sizeY*2.5f, 0.5f*_LINE_HEIGHT , invSlot_sizeX*2.0f, _LINE_HEIGHT      ),"HP : " + Character.CurHP);
	}
	
	private void DisplayChatLog()
	{
		float chatLogSizeY = Screen.height*0.25f;
		_chatLogMaxSize = (int)(Mathf.Floor (chatLogSizeY/_LINE_HEIGHT));
		
		GUI.Box(new Rect(_POSX, _boxPosY + _boxSizeY + Screen.width*0.02f, _boxSizeX, chatLogSizeY),"");
		for(int i = 0; i < _chatLogListString.Count ; i++)
		{
			GUI.Label (new Rect(_POSX, _boxPosY + _boxSizeY + Screen.width*0.02f + i*_LINE_HEIGHT, _boxSizeX, _LINE_HEIGHT),_chatLogListString[i]);	
		}
	}
	
	public  void AddChatLog(string _newLog)
	{
		_chatLogListString.Insert (0,_newLog);
		if(_chatLogListString.Count > _chatLogMaxSize)
		{
			_chatLogListString.RemoveAt ((int)_chatLogMaxSize - 1);	
		}
	}
	
	private List<string> CreateListInventoryItem(Weapon _WeaponHovered)
	{
		List<string> _StringToDisplay = new List<string>();
		
		_StringToDisplay.Add (_WeaponHovered.Name);
		_StringToDisplay.Add ("Level : " + _WeaponHovered.Level);
		_StringToDisplay.Add ("Exp : " + _WeaponHovered.CurExp);
		_StringToDisplay.Add ("Damage : " + _WeaponHovered.Damage);
		_StringToDisplay.Add ("Speed : " + _WeaponHovered.Speed);
		_StringToDisplay.Add ("Range : " + _WeaponHovered.Range);
		return _StringToDisplay;
	}
	
	private void DisplayFloatingBox()
	{
			
		// Test if a Weapon is being hovered
		for(int i = 0; i < Enum.GetValues(typeof(WeaponName)).Length ; i++)
		{
			if(_mouseOver == ("mouseOverOnItemToCraft_" + Inventory.WeaponList[i].Name))
			{
				DisplayFloatingLabels (Utility.parseStringToListString(Inventory.WeaponList[i].Recipe) );
			}
		}	
		
		// Test if an inventory slot is being hovered
		for(int i = 0; i < ItemInventory.InventorySize; i++)
		{
			if(_mouseOver == ("mouseOverOnInventorySlot_" + i.ToString()))
			{
				DisplayFloatingLabels (CreateListInventoryItem(ItemInventory.InventoryList[i].slotWeapon));
			}
		}
		
		// Test if the equipped weapon is being hovered
		if(_mouseOver == ("mouseOverOnEquippedWeapon"))
		{
			List<string> ListEquippedWeaponToDisplay = CreateListInventoryItem(ItemInventory.EquippedWeapon);
			if(ItemInventory.EquippedWeapon != null)
			{
				DisplayFloatingLabels(ListEquippedWeaponToDisplay);
			}
		}
		
		// Test if a Building is being hovered
		for(int i = 0; i < Enum.GetValues(typeof(BuildingName)).Length ; i++)
		{
			if(_mouseOver == ("mouseOverOnBuildingToBuild_" + Inventory.BuildingList[i].Name))
			{ 
				DisplayFloatingLabels (Utility.parseStringToListString(Inventory.BuildingList[i].Recipe) );
			}
		}		
		
		// Test if an upgrade is being hovered
		for(int i = 0; i < Enum.GetValues(typeof(DungeonUpgradeName)).Length ; i++)
		{
			if(_mouseOver == ("mouseOverOnDungeonUpgrade" + DungeonLevelPool.DungeonUpgradeList[i].Name))
			{
				List<string> UpgradeRessources = new List<string>();
				
				UpgradeRessources.Add("Cost : ");
				UpgradeRessources.Add("Influence : " + Character.InfluencePoints + "/" + DungeonLevelPool.DungeonUpgradeList[i].CostInfluence);
				UpgradeRessources.Add("Coin      : " + Inventory.RessourceList[(int)RessourceName.Coin].CurValue + "/" + DungeonLevelPool.DungeonUpgradeList[i].CostCoin);
				UpgradeRessources.Add("Description : ");
				UpgradeRessources.Add(DungeonLevelPool.DungeonUpgradeList[i].Description);
				
				DisplayFloatingLabels (UpgradeRessources);
			}
		}
	}
	
	private void DisplayCursor()
	{
		Color _ColorToDisplay;
		
		if(_isInteractiveDetected == true)
		{
			_ColorToDisplay = Color.green;
			if(objectRaycastCollided == "Spartan")
			{
				GUI.Label (new Rect(Screen.width/2 - 15,Screen.height*0.52f,125,_LINE_HEIGHT),"Talk to Spartan");	
			}
			else if(objectRaycastCollided == "CraftingTable")
			{
				GUI.Label (new Rect(Screen.width/2 - 15,Screen.height*0.52f,125,_LINE_HEIGHT*2),"Use Crafting Table");	
			}
			else if(objectRaycastCollided == "CartBroken")
			{
				if(Character.TaskList[(int)TaskName.MainQuest1].IsUnlocked) // Only allow interaction with cart on first quest
				{
					if(ItemInventory.EquippedWeapon == Inventory.WeaponList[(int)WeaponName.Hammer])
					{
						GUI.Label (new Rect(Screen.width/2 - 30,Screen.height*0.52f,125,_LINE_HEIGHT*2),"Repair cart with Hammer");
					}
					else
					{
						GUI.Label (new Rect(Screen.width/2 - 30,Screen.height*0.52f,125,_LINE_HEIGHT*2),"Equip hammer to repair the cart");
					}	
				}
				else
				{
					_ColorToDisplay = Color.white;
				}
			}
			else if(objectRaycastCollided == "Monster")
			{
				GUI.Label (new Rect(Screen.width/2 - 15,Screen.height*0.52f,125,_LINE_HEIGHT),"Attack with " + ItemInventory.EquippedWeapon.Name);	
			}
			else if(objectRaycastCollided == "Tree")
			{
				GUI.Label (new Rect(Screen.width/2 - 15,Screen.height*0.52f,125,_LINE_HEIGHT),"Cut the tree");	
			}
			else if(objectRaycastCollided == "Rock")
			{
				GUI.Label (new Rect(Screen.width/2 - 15,Screen.height*0.52f,125,_LINE_HEIGHT),"Mine some rock");	
			}
			else
			{
				//GUI.Label (new Rect(Screen.width/2 - 15,Screen.height*0.52f,125,_LINE_HEIGHT),"Attack " + objectRaycastCollided);
			}
			
		}
		else
		{
			_ColorToDisplay = Color.white;
		}
		GUI.contentColor = _ColorToDisplay;
		GUI.Label (new Rect(Screen.width/2,Screen.height/2,_LINE_HEIGHT,_LINE_HEIGHT),"O");	
		GUI.contentColor = Color.white;
	}
	
	public  void DetectInteractive(string _actionInteractive)
	{
		objectRaycastCollided = _actionInteractive;
		if(_actionInteractive == "None")
		{
			_isInteractiveDetected = false;
		}
		else
		{
			_isInteractiveDetected = true;	
		}
	}
	
	private void DisplayBox()
	{
		GUI.Box(new Rect(_boxPosX, _boxPosY, _boxSizeX, _boxSizeY),"");
		DisplayInBox(ToDisplayInBox);
	}
	
	public  void EnableButtonInHUD(int _buttonPos)
	{
		_buttonToggle[_buttonPos] = true;
	}
	
	private void DisplayButtons()
	{
		int nbrLine = 4;
		
		if(!_buttonToggle[0]){GUI.enabled = false;}
		if(GUI.Button(new Rect(_POSX, (_LINE_HEIGHT)*nbrLine++, _buttonX, _LINE_HEIGHT), "F1 Items")) {ChangeBoxView("Inventory");}
		GUI.enabled = true;
		
		if(!_buttonToggle[1]){GUI.enabled = false;}
		if(GUI.Button(new Rect(_POSX, (_LINE_HEIGHT)*nbrLine++, _buttonX, _LINE_HEIGHT), "F2 Stats"))  {ChangeBoxView("StatList");}		
		GUI.enabled = true;
			
		if(!_buttonToggle[2]){GUI.enabled = false;}
		if(GUI.Button(new Rect(_POSX, (_LINE_HEIGHT)*nbrLine++, _buttonX, _LINE_HEIGHT), "F3 Skills")) {ChangeBoxView("SkillList");}		
		GUI.enabled = true;
		
		if(!_buttonToggle[3]){GUI.enabled = false;}
		if(GUI.Button(new Rect(_POSX, (_LINE_HEIGHT)*nbrLine++, _buttonX, _LINE_HEIGHT), "F4 Spells")) {ChangeBoxView("SpellBook");}		
		GUI.enabled = true;
	
		if(!_buttonToggle[4]){GUI.enabled = false;}
		if(GUI.Button(new Rect(_POSX, (_LINE_HEIGHT)*nbrLine++, _buttonX, _LINE_HEIGHT), "F5 Build"))  {ChangeBoxView("BuildList");}		
		GUI.enabled = true;
			
		if(!_buttonToggle[5]){GUI.enabled = false;}
		if(GUI.Button(new Rect(_POSX, (_LINE_HEIGHT)*nbrLine++, _buttonX, _LINE_HEIGHT), "F6 Tasks"))  {ChangeBoxView("TaskList");}
		GUI.enabled = true;
		
		nbrLine++;
		
		if(Application.loadedLevelName == "Dungeon"){
			if(GUI.Button(new Rect(_POSX, (_LINE_HEIGHT)*nbrLine++, _buttonX, _LINE_HEIGHT), "F7 Map"         ))  {ChangeBoxView("Map");}
			if(GUI.Button(new Rect(_POSX, (_LINE_HEIGHT)*nbrLine++, _buttonX, _LINE_HEIGHT), "F8 Abandon"))  {AbandonDungeon();}
		}
		
		nbrLine++;
		
		if(GUI.Button(new Rect(_POSX, (_LINE_HEIGHT)*14, _buttonX, _LINE_HEIGHT), "F9 Save Game"))  {SaveLoadSystem.Save ();Debug.Log ("Saved");}
		
		if(GUI.Button(new Rect(_POSX, (_LINE_HEIGHT)*15, _buttonX, _LINE_HEIGHT), "F10 Help"))  {ChangeBoxView("Help");}
	}
	
	// Change the menu tab to be displayed
	public  void ChangeBoxView(string _newBoxToShow)
	{
		// This prevent the opening of the HUD while building
		if(BuildSystem.BuildState == 0)
		{
			//Debug.Log (ToDisplayInBox);
			//Debug.Log (_GameManager.LastState);
			// If the player us a control to open the currently opened window, close it.
			if(ToDisplayInBox == _newBoxToShow && _GameManager.LastState == "Menu")
			{
				Debug.Log ("Window Closed");
				CloseHUD();
			} 
			else //Otherwise, open the window.
			{
				ToDisplayInBox = _newBoxToShow;
				if(ToDisplayInBox == "TaskList" || ToDisplayInBox == "SkillList")
				{
					_NGUI_HUD.UpdateHUD(ToDisplayInBox);
					_NGUI_HUD.Display(ToDisplayInBox);
				}
				else
				{
					_NGUI_HUD.CloseAll ();
				}
				_GameManager.ChangeState ("Menu");
			}
		}
	}
	
	public void CloseHUD()
	{
		_GameManager.ChangeState ("Play");
		_NGUI_HUD.CloseAll();
	}
	
	public  void SwitchBoxFromKeys(string _keyPressed)
	{
		switch(_keyPressed)
		{
			case "F1": if(_buttonToggle[0] == true){ChangeBoxView ("Inventory");} break;
			case "F2": if(_buttonToggle[2] == true){ChangeBoxView ("StatList") ;} break;
			case "F3": if(_buttonToggle[3] == true){ChangeBoxView ("SkillList");} break;
			case "F4": if(_buttonToggle[3] == true){ChangeBoxView ("SpellBook");} break;
			case "F5": if(_buttonToggle[4] == true){ChangeBoxView ("BuildList") ;} break;
			case "F6": if(_buttonToggle[5] == true){ChangeBoxView ("TaskList") ;} break;
			case "F7": if(Application.loadedLevelName == "Dungeon"){ChangeBoxView ("Map");} break;
			case "F8": if(Application.loadedLevelName == "Dungeon"){AbandonDungeon();} break;
			case "F9": SaveLoadSystem.Save (); break;
			case "F10": if(_buttonToggle[5] == true){ChangeBoxView ("Help") ;} break;
			
			default:break;
		}
	}
	
	private void DisplayInBox(string _toDisplay)
	{
		
		switch(_toDisplay)
		{
		
		case "CraftList":
			for(int i = 0; i <  Inventory.WeaponList.Length; i++)
			{
				if(Inventory.WeaponList[i].IsUnlocked == true)
				{
					if(GUI.Button(new Rect(_boxPosX + 25, _boxPosY + (i+1) * (_LINE_HEIGHT+10)    , _buttonX, _LINE_HEIGHT), new GUIContent(Inventory.WeaponList[i].Name, "mouseOverOnItemToCraft_" + Inventory.WeaponList[i].Name)))
					{
						CraftSystem.CraftItem (Inventory.WeaponList[i]);
						CloseHUD();
					}
				}
			}
			_mouseOver = GUI.tooltip;
			break;	
			
		case "BuildList":
			for(int i = 0; i <  Inventory.BuildingList.Length; i++)
			{
				if(Inventory.BuildingList[i].IsUnlocked == true)
				{
					if(GUI.Button(new Rect(_boxPosX + 25, _boxPosY + (i+1) * (_LINE_HEIGHT+10)    , _buttonX*1.75f, _LINE_HEIGHT), new GUIContent(Inventory.BuildingList[i].Name, "mouseOverOnBuildingToBuild_" + Inventory.BuildingList[i].Name)))
					{	
						if(BuildSystem.BuildState == 0)
						{
							BuildSystem.BuildBuilding (Inventory.BuildingList[i]);
							_GameManager.ChangeState ("Build");
						}
					}
				}
			}
			_mouseOver = GUI.tooltip;
			break;
			
		case "StatList":
			int _totalSkillLvl = Character.CalculateSkillLevel();
			GUI.Label(new Rect(_boxPosX + 25, _boxPosY + 1*_LINE_HEIGHT + 10, 300, _LINE_HEIGHT), "Total skill level : " + _totalSkillLvl);
			GUI.Label(new Rect(_boxPosX + 25, _boxPosY + 2*_LINE_HEIGHT     , 300, _LINE_HEIGHT), "HP : " + Character.CurHP + "/" + Character.MaxHP);
			GUI.Label(new Rect(_boxPosX + 25, _boxPosY + 3*_LINE_HEIGHT + 10, 300, _LINE_HEIGHT), "Damage : " + _GameManager.calculateDamage());
			GUI.Label(new Rect(_boxPosX + 25, _boxPosY + 4*_LINE_HEIGHT + 10, 300, _LINE_HEIGHT), "Character Damage : " + Character.CurDamage);
			if(ItemInventory.EquippedWeapon != null)
			{
				GUI.Label(new Rect(_boxPosX + 25, _boxPosY + 5*_LINE_HEIGHT + 10, 300, _LINE_HEIGHT), "Weapon Damage    : " + ItemInventory.EquippedWeapon.Damage);
			}
			else
			{
				GUI.Label(new Rect(_boxPosX + 25, _boxPosY + 5*_LINE_HEIGHT + 10, 300, _LINE_HEIGHT), "Weapon Damage    : 0(No weapon equipped)");
			}
			
			
			GUI.Label(new Rect(_boxPosX + 25, _boxPosY + 6*_LINE_HEIGHT + 10, 300, _LINE_HEIGHT), "Dungeon Level : " + _GameManager.MaxDungeonLevel);
			GUI.Label(new Rect(_boxPosX + 25, _boxPosY + 7*_LINE_HEIGHT + 10, 300, _LINE_HEIGHT), "Influence : " + Character.InfluencePoints);
			
			
			break;	
			
		case "SkillList":
			int _boxBarLength = (int)(_boxSizeX*0.60f);
			int _skillRowNbr = 0;
			float _barRatio;
			for(int i = 0; i <  Character.SkillList.Length; i++)
			{
				if(Character.SkillList[i].IsUnlocked == true)
				{
					_barRatio = Character.SkillList[i].CurExp/100.0f;
					GUI.Label 	   (new Rect(_boxPosX + 25 ,  _boxPosY + (_skillRowNbr+1)  * (_LINE_HEIGHT+10) , 300 , _LINE_HEIGHT),Character.SkillList[i].Name);
					GUI.Label 	   (new Rect(_boxPosX + 100,  _boxPosY + (_skillRowNbr+1)  * (_LINE_HEIGHT+10) , 300 , _LINE_HEIGHT),"LVL : " + Character.SkillList[i].Level);
					GUI.DrawTexture(new Rect(_boxPosX + 150,  _boxPosY + (_skillRowNbr+1)  * (_LINE_HEIGHT+10) , _boxBarLength, _LINE_HEIGHT), textureProgBackground);	
					GUI.DrawTexture(new Rect(_boxPosX + 150,  _boxPosY + (_skillRowNbr+1)  * (_LINE_HEIGHT+10) , _boxBarLength*_barRatio, _LINE_HEIGHT), textureProgSkill,ScaleMode.ScaleAndCrop);
					GUI.Label 	   (new Rect(_boxPosX + 150 + _boxBarLength , _boxPosY + (_skillRowNbr+1)  * (_LINE_HEIGHT+10) , 300 , _LINE_HEIGHT),_barRatio*100 + "%");
					_skillRowNbr++;
				}
			}
			break;	
		
		case "SpellBook":
			DisplaySpellBook(); //Display the inventory with all slots/item
			break;	
			
		case "Inventory":
			DisplayBoxWeaponList(); //Display the inventory with all slots/item
			break;
			
		case "TaskList":
			DisplayTaskList(); //Display all the task
			break;
			
		case "Map":
			if(Application.loadedLevelName == "Dungeon")
			{
				DisplayDungeonMap();	
			}
			else
			{
				ChangeBoxView("StatList");
			}
			break;
			
		case "DungeonAbandon":
			AbandonDungeon();	
			break;
			
		case "DungeonMenu":
			if(_GameManager.CurZone == "Dungeon Warp")
			{
				DisplayDungeonMenu();
			}
			else if(Application.loadedLevelName == "Dungeon")
			{
				ChangeBoxView("Map");
			}
			else
			{
				ChangeBoxView("Inventory");	
			}
			break;
			
		case "DungeonUpgrade":
			DisplayDungeonUpgrade();
			break;
		case "Help":
			DisplayHelp();
			break;
		default:
			Debug.LogWarning ("Wrong Display in PlayerHUD :" + _toDisplay);
			break;
		}
	}
	
	private void DisplayHelp()
	{
		Debug.Log ("Help");
	}
	
	private void DisplaySpellBook()
	{
		
		
		float spellSlot_posX   = _boxPosX;
		float spellSlot_posY   = _boxPosX;
		float _barRatio;
		float _barLength = _boxSizeX/4;
		float _barHeight = _LINE_HEIGHT;
		
		for(int i = 0; i < Character.SpellList.Length; i++)
		{	
			spellSlot_posY = _boxPosY+(i*invSlot_sizeY)+(i*invSlot_offset) + invSlot_offset;
			GUI.Label(new Rect(spellSlot_posX, spellSlot_posY + invSlot_sizeY/2, invSlot_sizeX, invSlot_sizeY), Character.SpellList[i].Category);
			if(GUI.Button(new Rect(spellSlot_posX + 50, spellSlot_posY, invSlot_sizeX, invSlot_sizeY), Character.SpellList[i].SpellIcon))
			{
				MagicBook.ActiveSpell = Character.SpellList[i];
			}
		}
		
		float activeSpellSlot_posX  = _boxPosX + _boxSizeX*0.4f;
		float activeSpellSlot_posY  = _boxPosX;
		float activeSpellSlot_sizeX = invSlot_sizeX*2;
		float activeSpellSlot_sizeY = invSlot_sizeY*2;
		
		// Display active spell and its name
		GUI.Label(new Rect(activeSpellSlot_posX, activeSpellSlot_posY, activeSpellSlot_sizeX, 20), MagicBook.ActiveSpell.Name);
		GUI.Label(new Rect(activeSpellSlot_posX, activeSpellSlot_posY + 20, activeSpellSlot_sizeX, activeSpellSlot_sizeY), MagicBook.ActiveSpell.SpellIcon);
		
		int   _lineNumber = 0;
		// Display spell stats
		GUI.Label(new Rect(activeSpellSlot_posX + activeSpellSlot_sizeX*1.5f, activeSpellSlot_posY + _lineNumber++ * _LINE_HEIGHT, _barLength*1.5f, 20), "Damage : " + MagicBook.ActiveSpell.Damage + "(" + MagicBook.ActiveSpell.DamagePerLevel + "/LVL)");
		GUI.Label(new Rect(activeSpellSlot_posX + activeSpellSlot_sizeX*1.5f, activeSpellSlot_posY + _lineNumber++ * _LINE_HEIGHT, _barLength*1.5f, 20), "Cooldown : " + MagicBook.ActiveSpell.Cd + " sec" + "(" + MagicBook.ActiveSpell.CdPerLevel + "/LVL)");
		GUI.Label(new Rect(activeSpellSlot_posX + activeSpellSlot_sizeX*1.5f, activeSpellSlot_posY + _lineNumber++ * _LINE_HEIGHT, _barLength*1.5f, 20), "Mana Cost : " + MagicBook.ActiveSpell.Mana + " MP" + "(" + MagicBook.ActiveSpell.ManaPerLevel + "/LVL)");
		
		// Display text to choose component to level
		GUI.Label      (new Rect(activeSpellSlot_posX + _barLength*0.15f, activeSpellSlot_posY + activeSpellSlot_sizeY, _barLength*2, _barHeight), "Choose skill component to level");
		_lineNumber = 1;
		//Display DamageLevel bar
		_barRatio = MagicBook.ActiveSpell.DamageCurExp/100.0f;
		if(GUI.Toggle  (new Rect(activeSpellSlot_posX + _barLength*0.00f, activeSpellSlot_posY + activeSpellSlot_sizeY + _lineNumber*_barHeight*1.2f, _barHeight*1.5f     , _barHeight*1.5f), _isDamageLeveling,"")){ToggleActivateMe("Damage");}
	    GUI.Label      (new Rect(activeSpellSlot_posX + _barLength*0.15f, activeSpellSlot_posY + activeSpellSlot_sizeY + _lineNumber*_barHeight*1.2f, _barLength*0.75f    , _barHeight), "Damage LVL : " + MagicBook.ActiveSpell.DamageLevel);
		GUI.DrawTexture(new Rect(activeSpellSlot_posX + _barLength*0.90f, activeSpellSlot_posY + activeSpellSlot_sizeY + _lineNumber*_barHeight*1.2f, _barLength          , _barHeight), textureProgBackground);	
		GUI.DrawTexture(new Rect(activeSpellSlot_posX + _barLength*0.90f, activeSpellSlot_posY + activeSpellSlot_sizeY + _lineNumber*_barHeight*1.2f, _barLength*_barRatio, _barHeight), textureProgSpell,ScaleMode.ScaleAndCrop);
		GUI.Label      (new Rect(activeSpellSlot_posX + _barLength*1.90f, activeSpellSlot_posY + activeSpellSlot_sizeY + _lineNumber*_barHeight*1.2f, _barLength*0.5f     , _barHeight), MagicBook.ActiveSpell.DamageCurExp.ToString () + "%");
		_lineNumber++;
		
		//Display Cooldown bar
		_barRatio = MagicBook.ActiveSpell.CdCurExp/100.0f;
		if(GUI.Toggle  (new Rect(activeSpellSlot_posX + _barLength*0.00f, activeSpellSlot_posY + activeSpellSlot_sizeY + _lineNumber*_barHeight*1.2f, _barHeight*1.5f     , _barHeight*1.5f), _isCdLeveling,"")){ToggleActivateMe("Cd");}
		GUI.Label      (new Rect(activeSpellSlot_posX + _barLength*0.15f, activeSpellSlot_posY + activeSpellSlot_sizeY + _lineNumber*_barHeight*1.2f, _barLength*0.75f    , _barHeight), "Cooldown LVL : " + MagicBook.ActiveSpell.CdLevel);
		GUI.DrawTexture(new Rect(activeSpellSlot_posX + _barLength*0.90f, activeSpellSlot_posY + activeSpellSlot_sizeY + _lineNumber*_barHeight*1.2f, _barLength, _barHeight), textureProgBackground);	
		GUI.DrawTexture(new Rect(activeSpellSlot_posX + _barLength*0.90f, activeSpellSlot_posY + activeSpellSlot_sizeY + _lineNumber*_barHeight*1.2f, _barLength*_barRatio, _barHeight), textureProgSpell,ScaleMode.ScaleAndCrop);
		GUI.Label      (new Rect(activeSpellSlot_posX + _barLength*1.90f, activeSpellSlot_posY + activeSpellSlot_sizeY + _lineNumber*_barHeight*1.2f, _barLength*0.5f     , _barHeight), MagicBook.ActiveSpell.CdCurExp.ToString () + "%");
		_lineNumber++;
		
		//Display Mana bar
		_barRatio = MagicBook.ActiveSpell.ManaCurExp/100.0f;
		if(GUI.Toggle  (new Rect(activeSpellSlot_posX + _barLength*0.00f, activeSpellSlot_posY + activeSpellSlot_sizeY + _lineNumber*_barHeight*1.2f, _barHeight*1.5f     , _barHeight*1.5f), _isManaLeveling,"")){ToggleActivateMe("Mana");}
	    GUI.Label      (new Rect(activeSpellSlot_posX + _barLength*0.15f, activeSpellSlot_posY + activeSpellSlot_sizeY + _lineNumber*_barHeight*1.2f, _barLength*0.75f    , _barHeight), "Mana Cost LVL : " + MagicBook.ActiveSpell.ManaLevel);
		GUI.DrawTexture(new Rect(activeSpellSlot_posX + _barLength*0.90f, activeSpellSlot_posY + activeSpellSlot_sizeY + _lineNumber*_barHeight*1.2f, _barLength, _barHeight), textureProgBackground);	
		GUI.DrawTexture(new Rect(activeSpellSlot_posX + _barLength*0.90f, activeSpellSlot_posY + activeSpellSlot_sizeY + _lineNumber*_barHeight*1.2f, _barLength*_barRatio, _barHeight), textureProgSpell,ScaleMode.ScaleAndCrop);
		GUI.Label      (new Rect(activeSpellSlot_posX + _barLength*1.90f, activeSpellSlot_posY + activeSpellSlot_sizeY + _lineNumber*_barHeight*1.2f, _barLength*0.5f     , _barHeight), MagicBook.ActiveSpell.ManaCurExp.ToString () + "%");
		_lineNumber++;
		
		/*//Display Range bar
		_barRatio = MagicBook.ActiveSpell.CdCurExp/100.0f;
		GUI.DrawTexture(new Rect(activeSpellSlot_posX - activeSpellSlot_sizeX, activeSpellSlot_posY + invSlot_sizeY/2 + _lineNumber*_LINE_HEIGHT, _barLength, _barHeight), textureProgBackground);	
		GUI.DrawTexture(new Rect(activeSpellSlot_posX - activeSpellSlot_sizeX, activeSpellSlot_posY + invSlot_sizeY/2 + _lineNumber*_LINE_HEIGHT, _barLength*_barRatio, _barHeight), textureProgSpell,ScaleMode.ScaleAndCrop);
		_lineNumber++;
		*/
		
		// GUI.DrawTexture(new Rect(_boxPosX + 150,  _boxPosY + (_skillRowNbr+1)  * (_LINE_HEIGHT+10) , _boxBarLength, _LINE_HEIGHT), textureProgBackground);	
		// GUI.DrawTexture(new Rect(_boxPosX + 150,  _boxPosY + (_skillRowNbr+1)  * (_LINE_HEIGHT+10) , _boxBarLength*_barRatio, _LINE_HEIGHT), textureProgSkill,ScaleMode.ScaleAndCrop);
		
	}
	
	private void ToggleActivateMe(string _boolToActivate)
	{
		if(_lastCompLeveled != _boolToActivate)
		{
			_isDamageLeveling = false;
			_isCdLeveling     = false;
			_isManaLeveling   = false;
			_isRangeLeveling   = false;
			
			_lastCompLeveled = _boolToActivate;
			switch(_boolToActivate)
			{
				case "Damage":	
					_isDamageLeveling = true;
					break;
				case "Cd":	
					_isCdLeveling = true;
					break;
				case "Mana":	
					_isManaLeveling = true;
					break;
			}
			Debug.Log (_boolToActivate);
			MagicBook.CompToLevelOnSpell = _boolToActivate;
		}
		
	}

	
	private void DisplayDungeonMenu()
	{
		
		float _offset = 10.0f;
		float _textLength = 200.0f;
		float _button1posX = _boxSizeX + _textLength;
		float _curPosX = _boxPosX + _offset;
		float _curPosY = _boxPosY + _offset;
		
		// Menu
		GUI.Label  (new Rect(_curPosX, _curPosY, _textLength , _LINE_HEIGHT),"Dungeon Menu" ); _curPosY += _LINE_HEIGHT;
		GUI.Label  (new Rect(_curPosX, _curPosY, _textLength , _LINE_HEIGHT),"------------" ); _curPosY += _LINE_HEIGHT;
		GUI.Label  (new Rect(_curPosX, _curPosY, _textLength , _LINE_HEIGHT),"Dungeon Level : " ); _curPosY += _LINE_HEIGHT;
		
		GUI.Label  (new Rect(_curPosX, _curPosY, _textLength , _LINE_HEIGHT),"Influences Points : "  + Character.InfluencePoints.ToString ()); _curPosY += _LINE_HEIGHT;
		GUI.Label  (new Rect(_curPosX, _curPosY, _textLength , _LINE_HEIGHT),"Coin : " + Inventory.RessourceList[(int)RessourceName.Coin].CurValue); _curPosY += _LINE_HEIGHT;
		GUI.Label  (new Rect(_curPosX, _curPosY, _textLength , _LINE_HEIGHT),"Dungeon Max Level : " + _GameManager.MaxDungeonLevel.ToString()); _curPosY += _LINE_HEIGHT*1.25f;
		if(GUI.Button (new Rect(_curPosX, _curPosY, invSlot_sizeX*0.5f , invSlot_sizeY*0.5f),"-" ))
		{
			if(DungeonToGenerate.level > 1)
			{
				DungeonToGenerate.level--;
			}
		}
		
		_curPosX += invSlot_sizeX*0.5f + _offset;	
		
		GUI.Button (new Rect(_curPosX, _curPosY-_LINE_HEIGHT*0.5f, invSlot_sizeX       , invSlot_sizeY      ),DungeonToGenerate.level.ToString()); _curPosX += invSlot_sizeX + _offset;
		if(GUI.Button (new Rect(_curPosX, _curPosY, invSlot_sizeY*0.5f , invSlot_sizeY*0.5f),"+" ))
		{
			if(DungeonToGenerate.level < _GameManager.MaxDungeonLevel && DungeonToGenerate.level < 20)
			{
				if(DungeonToGenerate.level < 10 || DungeonLevelPool.DungeonUpgradeList[(int)DungeonUpgradeName.SkeletonCrypt].IsEnabled == true)
				{
					DungeonToGenerate.level++;
				}
			}
		}
		
		_curPosX = _boxPosX + _offset;
		
		//Button for Hardcore Mode
		if(DungeonLevelPool.DungeonUpgradeList[(int)DungeonUpgradeName.HardcoreMode].IsEnabled == true)
		{
			if(GUI.Button (new Rect(_curPosX, _boxPosY + _LINE_HEIGHT*6.0f, invSlot_sizeX*1.0f , invSlot_sizeY*0.5f),DungeonToGenerate.isHardcore.ToString())){ToggleMode("Hardcore");} _curPosX+=invSlot_sizeX + _offset;
			GUI.Label (new Rect(_curPosX, _boxPosY + _LINE_HEIGHT*6.0f, _textLength , invSlot_sizeY*0.5f),"Hardcore Mode");
		}
		
		// Button for Wave Mode
		// _curPosX = _boxPosX + _offset;
		// if(GUI.Button (new Rect(_curPosX, _boxPosY + _LINE_HEIGHT*7.0f, invSlot_sizeX*1.0f , invSlot_sizeY*0.5f),DungeonToGenerate.isWave.ToString())){ToggleMode("Wave");} _curPosX+=invSlot_sizeX + _offset;
		// GUI.Label (new Rect(_curPosX, _boxPosY + _LINE_HEIGHT*7.0f, _textLength , invSlot_sizeY*0.5f),"Wave Mode");
		
		
		if(GUI.Button (new Rect(_curPosX, _boxPosY + _LINE_HEIGHT*10.0f, invSlot_sizeX*2.0f , invSlot_sizeY*1.0f),"Enter Dungeon" ))
		{
			_GameManager.StartDungeon (DungeonToGenerate);
		}
		
		
		
		_curPosX += invSlot_sizeX*2.0f + _offset;
		
		// First upgrade or DungeonUpgrade
		if(DungeonLevelPool.DungeonUpgradeList[(int)DungeonUpgradeName.FirstUpgrade].IsEnabled == false)
		{
			if(GUI.Button (new Rect(_curPosX, _boxPosY + _LINE_HEIGHT*10.0f, invSlot_sizeX*3.0f , invSlot_sizeY*0.75f), new GUIContent("Upgrade to upgrade", "mouseOverOnDungeonUpgrade" + DungeonLevelPool.DungeonUpgradeList[(int)DungeonUpgradeName.FirstUpgrade].Name)))
			{
				_GameManager.UpgradeDungeon (DungeonLevelPool.DungeonUpgradeList[(int)DungeonUpgradeName.FirstUpgrade]);
			}
			
			_mouseOver = GUI.tooltip;
		}
		else
		{
			if(GUI.Button (new Rect(_curPosX, _boxPosY + _LINE_HEIGHT*10.0f, invSlot_sizeX*3.0f , invSlot_sizeY*0.75f),"Upgrade Menu" ))
			{
				ToDisplayInBox = "DungeonUpgrade";
			}
		}
		
		
		// Display selected dungeon info
		_curPosX = _boxPosX + _boxSizeX/3;
		_curPosY = _boxPosY + _offset;
		GUI.Label (new Rect(_curPosX, _curPosY, _textLength , _LINE_HEIGHT), DungeonLevelPool.DungeonLevelList[DungeonToGenerate.level].Name);_curPosY += _LINE_HEIGHT;
		GUI.Label (new Rect(_curPosX, _curPosY, _textLength , _LINE_HEIGHT), "------------------------");_curPosY += _LINE_HEIGHT;
		
		List<Utility.ParsedString> _ListMonsterToSpawn  = new List<Utility.ParsedString>();	//Declare a list that contain all the monster to spawn
		
		_ListMonsterToSpawn = Utility.parseString (DungeonLevelPool.DungeonLevelList[DungeonToGenerate.level].MonsterList);
		
		for(int i = 0; i < _ListMonsterToSpawn.Count; i++)
		{
			MonsterName _MonsterIndex = (MonsterName) Enum.Parse(typeof(MonsterName), _ListMonsterToSpawn[i].type); 
			Monster MonsterToSpawn = Bestiary.MonsterList[(int)_MonsterIndex];
			
			if(_ListMonsterToSpawn[i].nbr > 0)
			{
				GUI.Label (new Rect(_curPosX, _curPosY, _textLength , _LINE_HEIGHT), _ListMonsterToSpawn[i].nbr + "*" + _ListMonsterToSpawn[i].type + "(" + MonsterToSpawn.NbrKilled + ")");
				_curPosY += _LINE_HEIGHT;
			}
		}
	}
		
	private void DisplayDungeonUpgrade()
	{
		float _offset = 10.0f;
		float _curPosX = _boxPosX + _offset;
		float _curPosY = _boxPosY + _offset;
		float _textLength = 200.0f;
		
		GUI.Label  (new Rect(_curPosX, _curPosY, _textLength , _LINE_HEIGHT),"Dungeon Upgrade Menu" )    ; _curPosY += _LINE_HEIGHT;
		GUI.Label  (new Rect(_curPosX, _curPosY, _textLength , _LINE_HEIGHT),"--------------------" )   ; _curPosY += _LINE_HEIGHT;
		GUI.Label  (new Rect(_curPosX, _curPosY, _textLength , _LINE_HEIGHT),"Influences Points : "  + Character.InfluencePoints.ToString ()); _curPosY += _LINE_HEIGHT;
		GUI.Label  (new Rect(_curPosX, _curPosY, _textLength , _LINE_HEIGHT),"Coin : " + Inventory.RessourceList[(int)RessourceName.Coin].CurValue); _curPosY += _LINE_HEIGHT;
		
		for(int i = 0; i <  DungeonLevelPool.DungeonUpgradeList.Length; i++)
		{
			if(DungeonLevelPool.DungeonUpgradeList[i].IsUnlocked && !DungeonLevelPool.DungeonUpgradeList[i].IsEnabled && DungeonLevelPool.DungeonUpgradeList[i].Name != "First Upgrade")
			{				
				if(GUI.Button (new Rect(_curPosX, _curPosY, invSlot_sizeX*2.0f , invSlot_sizeY*1.0f), new GUIContent(DungeonLevelPool.DungeonUpgradeList[i].Name, "mouseOverOnDungeonUpgrade" + DungeonLevelPool.DungeonUpgradeList[i].Name)))
				{
					_GameManager.UpgradeDungeon (DungeonLevelPool.DungeonUpgradeList[i]);
				}
				_curPosY += invSlot_sizeY + _offset;
			}
		}
		
		if(GUI.Button (new Rect(_curPosX, _boxPosY + _LINE_HEIGHT*10.0f, invSlot_sizeX*2.0f , invSlot_sizeY*1.0f),"RETURN" ))
		{
			ToDisplayInBox = "DungeonMenu";
		}
		
		_mouseOver = GUI.tooltip;
	}

	private void ToggleMode(string _modeToToggle)
	{
		if(_modeToToggle == "Hardcore")
		{
			DungeonToGenerate.isHardcore = !DungeonToGenerate.isHardcore;
		}
		
		
		else if(_modeToToggle == "Wave")
		{
			DungeonToGenerate.isWave = !DungeonToGenerate.isWave;
		}
	}

	private void AbandonDungeon() //TODO: Probably to move somewhere else
	{
		DungeonManager _DungeonManager = GameObject.FindGameObjectWithTag("DungeonMaster").GetComponent<DungeonManager>();	
		_DungeonManager.Abandon();
	}
	
	private void DisplayDungeonMap()
	{
		DungeonManager _DungeonManager = GameObject.FindGameObjectWithTag("DungeonMaster").GetComponent<DungeonManager>();
		
		int playerPosX = Mathf.RoundToInt(_Player.transform.position.x);
		int playerPosZ = Mathf.RoundToInt(_Player.transform.position.z);
		
		// Display Map
		float _offsetX  = 150.0f;
		float _offsetZ  = 120.0f;
		float _curPosX =  _offsetX;
		float _curPosZ =  _offsetZ;
		
		string _toPrint;
		
		float sizeSlotX = 15.0f;
		float sizeSlotZ = 15.0f;
		
		
		int nbrSlotX = Mathf.CeilToInt (_boxSizeX/sizeSlotX) - 5;
		int nbrSlotZ = Mathf.CeilToInt (_boxSizeY/sizeSlotZ) - 5;
		
		int _mapToDisplayPosXmin = 0;
		int _mapToDisplayPosZmin = 0;
		
		int _mapToDisplayPosXmax = nbrSlotX - 1;
		int _mapToDisplayPosZmax = nbrSlotZ - 1;
		
		if(_mapToDisplayPosXmax > _DungeonManager.MapSizeX) {_mapToDisplayPosXmax = _DungeonManager.MapSizeX - 1;}
		if(_mapToDisplayPosZmax > _DungeonManager.MapSizeZ) {_mapToDisplayPosZmax = _DungeonManager.MapSizeZ - 1;}
		
		
		if(playerPosX > _mapToDisplayPosXmax)
		{
			_mapToDisplayPosXmin = 	playerPosX - nbrSlotX/2;
			_mapToDisplayPosXmax = 	playerPosX + nbrSlotX/2;	
		}
		
		if(playerPosZ > _mapToDisplayPosZmax)
		{
			_mapToDisplayPosZmin = 	playerPosZ - nbrSlotZ/2;
			_mapToDisplayPosZmax = 	playerPosZ + nbrSlotZ/2;	
		}
		
		if(_mapToDisplayPosXmax > _DungeonManager.MapSizeX) {_mapToDisplayPosXmax = _DungeonManager.MapSizeX - 1 ;}
		if(_mapToDisplayPosZmax > _DungeonManager.MapSizeZ) {_mapToDisplayPosZmax = _DungeonManager.MapSizeZ - 1;}
		
		
		
		//Debug.Log ("x(" + _mapToDisplayPosXmin + "," + _mapToDisplayPosXmax + ") - z(" + _mapToDisplayPosZmin + "," + _mapToDisplayPosZmax + ")");
		//Debug.Log ("Size(" + nbrSlotX + "," + nbrSlotZ + ")");
		
		
		// Display map rotation difference
		GUI.Label (new Rect(_curPosX, _curPosZ, _boxSizeX/2, _LINE_HEIGHT), "Map rotation : " + _Player.transform.rotation.eulerAngles.y  + "° (0° for map aligned)"); _curPosZ += _LINE_HEIGHT;
		
		// Display map
		float _mapToDisplayStartPosZ = _curPosZ;
		for(int i = _mapToDisplayPosXmin; i < _mapToDisplayPosXmax - 1  ; i++)
		{
			for(int j = _mapToDisplayPosZmax - 1; j >= _mapToDisplayPosZmin ; j--)
			{
				if(i == Mathf.Round(_Player.transform.position.x) && j == Mathf.Round(_Player.transform.position.z))
				{
					GUI.color = Color.blue;	
					_toPrint = "X";
				}
				else if(_mapToDisplay[i,j] == 1) //Unknown room
				{
					GUI.color = Color.gray;	
					_toPrint = "#";
				}
				else if(_mapToDisplay[i,j] == 2) //Known room
				{
					GUI.color = Color.white;	
					_toPrint = "#";
				}
				else // Outside
				{
					_toPrint = "";
				}
				
				GUI.Label(new Rect(_curPosX, _curPosZ, sizeSlotX, sizeSlotZ), _toPrint);
				GUI.color = Color.white;
				_curPosZ += sizeSlotZ;
				
			}	
			_curPosZ = _mapToDisplayStartPosZ;
			_curPosX += sizeSlotX;
		}
		
	}
		
	private void DisplayTaskList()	
	{
		int _taskRowNbr = 0;
		for(int i = 0; i <  Character.TaskList.Length; i++)
		{
			if(Character.TaskList[i].IsUnlocked == true)
			{
				if(Character.TaskList[i].IsFinished == true)
				{
					GUI.enabled = false;
				}
					GUI.Button(new Rect(_boxPosX + 25 ,  _boxPosY + (++_taskRowNbr+1)  * (_LINE_HEIGHT+10) , 500 , _LINE_HEIGHT),Character.TaskList[i].Description);
					GUI.enabled = true;
			}
		}	
	}
		
	private void DisplayBoxWeaponList()
	{
		float invSlot_posX   = _boxPosX;
		float invSlot_posY   = _boxPosX;
		
		int _curSlot;
		
		if(_isDisplayFloatingMenu == true)
		{
			GUI.enabled = false;	
		}
		
		//ItemInventory;
		for(int i = 0; i < 10; i++)
		{
			for(int j = 0; j < 5; j++)
			{
				_curSlot = (i + 10*j);
				invSlot_posX = _boxPosX+(i*invSlot_sizeX)+(i*invSlot_offset) + invSlot_offset;
				invSlot_posY = _boxPosY+(j*invSlot_sizeY)+(j*invSlot_offset) + invSlot_offset;
				if(ItemInventory.InventoryList[_curSlot].slotWeapon != null)
				{
					if(GUI.Button(new Rect(invSlot_posX, invSlot_posY, invSlot_sizeX, invSlot_sizeY), new GUIContent(ItemInventory.InventoryList[_curSlot].slotWeapon.ItemIcon, "mouseOverOnInventorySlot_" + _curSlot.ToString())   ))
					{
						_inventorySlotClicked = (_curSlot);
		       			if (Event.current.button == 1)
						{
							GUI.Label 	   (new Rect(_POSX, _LINE_HEIGHT*3.5f  , 300 , _LINE_HEIGHT),"Menu" );
							_ItemClicked  = ItemInventory.InventoryList[_curSlot].slotWeapon;
							_MousePosOnClick = 	new Vector2(Event.current.mousePosition.x,Event.current.mousePosition.y);
							
							_isDisplayFloatingMenu = true;
						}
						else if (Event.current.button == 0)
						{
							EquipWeapon(ItemInventory.InventoryList[_curSlot].slotWeapon, _curSlot);
						}
						
					}
					
					_mouseOver = GUI.tooltip;
				}
				else
				{
					GUI.Button(new Rect(invSlot_posX, invSlot_posY, invSlot_sizeX, invSlot_sizeY),"");	
				}
			}
			
		}
		GUI.enabled = true;
	}
	
	private void DisplayRessources()
	{
		for(int i = 0; i <  Inventory.RessourceList.Length; i++)
		{
			GUI.Label (new Rect(Screen.width*0.9f, 15 + i*_LINE_HEIGHT , 100 , _LINE_HEIGHT),Inventory.RessourceList[i].Name + " : " + Inventory.RessourceList[i].CurValue );
		}
	}
	
	private void DisplayProgressBarAndHPMP()
	{
		
		float _ratioProgress = ((float)_GameManager.CurProgress / (float)100);
		float _ratioHP = (float)Character.CurHP/(float)Character.MaxHP;
		float _ratioMP = (float)Character.CurMP/(float)Character.MaxMP;
		
		float _hpmpBarLength = _progBarLength * 0.66f;
		
		GUI.DrawTexture(new Rect(_POSX, _LINE_HEIGHT*0.5f, _progBarLength, _LINE_HEIGHT), textureProgBackground);	
		GUI.DrawTexture(new Rect(_POSX, _LINE_HEIGHT*0.6f, _ratioProgress*_progBarLength, 0.8f*_LINE_HEIGHT), textureProg,ScaleMode.ScaleAndCrop);
		
		GUI.DrawTexture(new Rect(_POSX, _LINE_HEIGHT*1.6f, _hpmpBarLength, _LINE_HEIGHT*0.66f), textureProgBackground);	
		GUI.DrawTexture(new Rect(_POSX, _LINE_HEIGHT*1.7f, _ratioHP*_hpmpBarLength, 0.8f*_LINE_HEIGHT*0.66f), _TextureManager.Texture_HP,ScaleMode.ScaleAndCrop);
		GUI.Label(new Rect(_POSX, _LINE_HEIGHT*1.55f, _hpmpBarLength + 20.0f, _LINE_HEIGHT), "HP " + Character.CurHP + "/" + Character.MaxHP);
		
		GUI.DrawTexture(new Rect(_POSX, _LINE_HEIGHT*2.3f, _hpmpBarLength, _LINE_HEIGHT*0.66f), textureProgBackground);	
		GUI.DrawTexture(new Rect(_POSX, _LINE_HEIGHT*2.4f, _ratioMP*_hpmpBarLength, 0.8f*_LINE_HEIGHT*0.66f), _TextureManager.Texture_MP,ScaleMode.ScaleAndCrop);
		GUI.Label(new Rect(_POSX, _LINE_HEIGHT*2.30f, _hpmpBarLength + 20.0f, _LINE_HEIGHT), "MP " + Character.CurMP + "/" + Character.MaxMP);
		
		GUI.Label 	   (new Rect(_POSX, _LINE_HEIGHT*3.2f  , 300 , _LINE_HEIGHT),"Zone   : " + _GameManager.CurZone );
	}
	
	private void DisplayTransparentBackground(float _posX, float _posY, float _sizeX, float _sizeY, float _opacity)
	{
		Rect _objRect= new Rect(_posX, _posY, _sizeX, _sizeY);
		
		// Display semi-transparent background for processes and set color back to white
		GUI.color = new Color(0.0f, 0.0f, 0.0f, _opacity);
		GUI.DrawTexture(_objRect, WhiteTexture);
		GUI.color = Color.white;
	}
	
	private void DisplayFloatingLabels(List<string> _toDisplay) 
	{
		float _posX  = Input.mousePosition.x; float _posY  = Mathf.Abs(Input.mousePosition.y - _PlayerCam.pixelHeight) + _LINE_HEIGHT;
		float _sizeX = Screen.width/8;   float _sizeY = _toDisplay.Count*_LINE_HEIGHT;
		float _opacity = _FLOATING_RECT_OPACITY;
		
		DisplayTransparentBackground(_posX, _posY, _sizeX, _sizeY, _opacity);
		
		// Display string[] in FloatingLabels
		for(int i = 0; i < _toDisplay.Count; i++)
		{
			GUI.Label (new Rect(_posX, _posY + i*_LINE_HEIGHT , _sizeX, _LINE_HEIGHT*0.8f), _toDisplay[i]);
		}
	}
	
	// Display a floating menu to interact with
	private void DisplayFloatingMenu(Vector2 _position, Weapon _ItemRightClicked, int _inventorySlotNumber)
	{;
		float _posX  = _position.x;        float _posY  = _position.y;
		float _sizeX = _boxSizeX*0.20f ;   float _sizeY = 4*_LINE_HEIGHT;
		float _opacity = _FLOATING_RECT_OPACITY;
		int _lineCnt = 0;
		
		DisplayTransparentBackground(_posX, _posY, _sizeX, _sizeY, _opacity);
		
		// Display buttons in FloatingMenu
		GUI.Label  (new Rect(_posX, _posY + _lineCnt++*_LINE_HEIGHT , _sizeX, _LINE_HEIGHT)   ,_ItemRightClicked.Name);
		if(GUI.Button (new Rect(_posX, _posY + _lineCnt++*_LINE_HEIGHT , _sizeX, _LINE_HEIGHT)   ,"Equip"))
		{
			EquipWeapon(_ItemRightClicked, _inventorySlotNumber);
			CloseMenu ();
		}
			
		if(GUI.Button (new Rect(_posX, _posY + _lineCnt++*_LINE_HEIGHT , _sizeX, _LINE_HEIGHT),"Delete"))
		{
			ItemInventory.RemoveItemFromInventory(_inventorySlotNumber);
			CloseMenu ();
		}
		
		if(GUI.Button (new Rect(_posX, _posY + _lineCnt++*_LINE_HEIGHT , _sizeX, _LINE_HEIGHT),"Cancel"))      
		{
			CloseMenu ();
		}
	}
	
	private void EquipWeapon(Weapon _WeaponToEquip, int _slotNbr)
	{	
		ItemInventory.RemoveItemFromInventory(_inventorySlotClicked);
		ItemInventory.EquipWeapon(_WeaponToEquip);
		AddChatLog("[ITEM] You equipped " + _WeaponToEquip.Name);
	}
	
	private void CloseMenu()
	{
		_isDisplayFloatingMenu = false;
	}
}
