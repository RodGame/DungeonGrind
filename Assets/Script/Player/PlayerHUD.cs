using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List class;
using System;					  // For Enum

public class PlayerHUD : MonoBehaviour {
	
	private GameManager _GameManager;
	private Ressource   _Ressource;
	private Inventory   _Inventory;
	
	private GameObject  _Player;
	private Camera      _PlayerCam;
	private GameObject  _PlayerMaster;
	private Item        _ItemClicked;
	private Vector2     _MousePosOnClick;
	private int 		_inventorySlotClicked;
	
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
	private float timeElapsedFloatingMenu = 0.0f;
	private float timeToDeletFloatingMenu = 4.0f;
	//private float timeElapsedDescriptMenu = 0.0f;
	//private float timeToDeletDescriptMenu = 4.0f;
	private float invSlot_sizeX;
	private	float invSlot_sizeY;
	private bool[] _buttonToggle = new bool[7] {false, false, false, false, false, true, false};
	
	
	private string objectRaycastCollided;
	private string ToDisplayInBox;
	private string _mouseOver;
	private string _curDiscussion;
	private List<string> _chatLogListString;
	public bool   isDisplayCursor = true;
	
	public Texture textureSkill;
	public Texture textureProg;
	public Texture textureProgBackground;
	public Texture WhiteTexture;
	
	
	
	// Use this for initialization
	void Start () {
		_PlayerMaster = GameObject.FindGameObjectWithTag("PlayerMaster");
		_Player	      = GameObject.FindGameObjectWithTag("Player");
		_GameManager  = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
		_Inventory    = _PlayerMaster.GetComponent<Inventory>();
		_PlayerCam    = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<Camera>();
		ToDisplayInBox = "None";
		_chatLogListString = new List<string>();
			
		invSlot_sizeX  = _boxSizeX/15;
		invSlot_sizeY  = _boxSizeX/15;
		
	}
	
	void OnGUI()
	{
		GUI.skin.button.wordWrap = true;
		GUI.skin.box.wordWrap    = true;
		GUI.skin.label.wordWrap = true;		
		
		
		if(ToDisplayInBox != "None" && _GameManager.CurState == "Menu")
		{
			DisplayBox();
		}
		
		if(_isDisplayFloatingMenu == true)
		{
			DisplayFloatingMenu(_MousePosOnClick, _ItemClicked, _inventorySlotClicked);	
		}
		
		DisplayProgressBar();
		DisplayRessources();
		DisplayButtons();
		if(isDisplayCursor == true)
		{
			DisplayCursor();
		}
		DisplayFloatingBox();
		DisplayChatLog();
		DisplayEquippedItem();
		DisplayHP();
		DisplayActiveTask();
		
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
		
		
		/*if(_isDiscussionActive == true && (timeElapsedDescriptMenu < timeToDeletDescriptMenu))
		{
			timeElapsedDescriptMenu += Time.deltaTime;
			DisplayDiscussion (_curDiscussion);
		}
		else
		{
			timeElapsedDescriptMenu = 0.0f;
			_isDiscussionActive = false;
		}*/
	}
	
	private void DisplayActiveTask()
	{
		if(Character.ActiveTask != null)
		{
			GUI.Label (new Rect(_POSX, _boxPosY + _boxSizeY , 500 , _LINE_HEIGHT),"Active Task : " + Character.ActiveTask.Definition);
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
	
	private void DisplayEquippedItem()
	{
		if(ItemInventory.EquippedItem == null)
		{
			GUI.Button(new Rect(_POSX + _progBarLength + 25, 0.5f*_LINE_HEIGHT                     , invSlot_sizeX*1.5f, invSlot_sizeY*1.5f),"");
			GUI.Label (new Rect(_POSX + _progBarLength + 23, 0.5f*_LINE_HEIGHT + invSlot_sizeY*1.5f, invSlot_sizeX*2.0f, _LINE_HEIGHT      ),"No Item");
		}
		else 
		{
			GUI.Label (new Rect(_POSX + _progBarLength + 20, 0.5f*_LINE_HEIGHT + invSlot_sizeY*1.5f, invSlot_sizeX*2.0f, _LINE_HEIGHT      ),ItemInventory.EquippedItem.Name);
			if(GUI.Button(new Rect(_POSX + _progBarLength + 25, 0.5f*_LINE_HEIGHT , invSlot_sizeX*1.5f, invSlot_sizeY*1.5f),ItemInventory.EquippedItem.ItemTexture))
			{
				UnEquipItem();
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
	
	private void DisplayFloatingBox()
	{
	// Test if a Process is being hovered
		for(int i = 0; i < Enum.GetValues(typeof(ItemName)).Length ; i++)
		{
			if(_mouseOver == ("mouseOverOnItemToCraft" + _Inventory.ItemList[i].Name))
			{
				DisplayFloatingLabels (CraftSystem.parseStringToListString(_Inventory.ItemList[i].Recipe) );
			}
		}	
	}
	
	private void DisplayCursor()
	{
		if(_isInteractiveDetected == true)
		{
			if(objectRaycastCollided == "Spartan")
			{
				GUI.Label (new Rect(Screen.width/2 - 15,Screen.height*0.52f,125,_LINE_HEIGHT),"Talk to Spartan");	
			}
			else if(objectRaycastCollided == "CraftingTable")
			{
				GUI.Label (new Rect(Screen.width/2 - 15,Screen.height*0.52f,125,_LINE_HEIGHT*2),"Use Crafting Table");	
			}
			else if(objectRaycastCollided == "Cart03")
			{
				if(ItemInventory.EquippedItem == _Inventory.ItemList[(int)ItemName.Hammer])
				{
					GUI.Label (new Rect(Screen.width/2 - 30,Screen.height*0.52f,125,_LINE_HEIGHT*2),"Repair cart with Hammer");
				}
				else
				{
					GUI.Label (new Rect(Screen.width/2 - 30,Screen.height*0.52f,125,_LINE_HEIGHT*2),"Equip hammer to repair the cart");
				}	
			}
			else if(objectRaycastCollided == "Monster")
			{
				GUI.Label (new Rect(Screen.width/2 - 15,Screen.height*0.52f,125,_LINE_HEIGHT),"Attack with " + ItemInventory.EquippedItem.Name);	
			}
			else
			{
				if(ItemInventory.EquippedItem == _Inventory.ItemList[(int)ItemName.None])
				{
					GUI.Label (new Rect(Screen.width/2 - 15,Screen.height*0.52f,125,_LINE_HEIGHT),"Use hand");	
					GUI.Label (new Rect(Screen.width/2 - 15,Screen.height*0.52f + _LINE_HEIGHT+0.80f,125,_LINE_HEIGHT),"on " + objectRaycastCollided);	
				}
				else
				{
					GUI.Label (new Rect(Screen.width/2 - 15,Screen.height*0.52f,125,_LINE_HEIGHT),"Use " + ItemInventory.EquippedItem.Name);	
					GUI.Label (new Rect(Screen.width/2 - 15,Screen.height*0.52f + _LINE_HEIGHT*0.80f,125,_LINE_HEIGHT),"on " + objectRaycastCollided);	
				}
			}
			GUI.contentColor = Color.green;
		}
		else
		{
			GUI.contentColor = Color.white;
		}
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
		DisplayInBox (ToDisplayInBox);
	}
	
	public  void EnableButtonInHUD(int _buttonPos)
	{
		_buttonToggle[_buttonPos] = true;
	}
	
	private void DisplayButtons()
	{
		//if(GUI.Button(new Rect(_POSX, (_LINE_HEIGHT)*4, _buttonX, _LINE_HEIGHT), "F1 Craft")) {ChangeBoxView("CraftList");}
		
		if(!_buttonToggle[0]){GUI.enabled = false;}
		if(GUI.Button(new Rect(_POSX, (_LINE_HEIGHT)*5, _buttonX, _LINE_HEIGHT), "F1 Build")) {ChangeBoxView("BuildList");}
		GUI.enabled = true;
		
		if(!_buttonToggle[1]){GUI.enabled = false;}
		if(GUI.Button(new Rect(_POSX, (_LINE_HEIGHT)*6, _buttonX, _LINE_HEIGHT), "F2 Train")) {ChangeBoxView("TrainList");}	
		GUI.enabled = true;
		
		if(!_buttonToggle[2]){GUI.enabled = false;}
		if(GUI.Button(new Rect(_POSX, (_LINE_HEIGHT)*8, _buttonX, _LINE_HEIGHT), "F3 Stats"))  {ChangeBoxView("StatList");}		
		GUI.enabled = true;
			
		if(!_buttonToggle[3]){GUI.enabled = false;}
		if(GUI.Button(new Rect(_POSX, (_LINE_HEIGHT)*9, _buttonX, _LINE_HEIGHT), "F4 Skills")) {ChangeBoxView("SkillList");}		
		GUI.enabled = true;
			
		if(!_buttonToggle[4]){GUI.enabled = false;}
		if(GUI.Button(new Rect(_POSX, (_LINE_HEIGHT)*10, _buttonX, _LINE_HEIGHT), "F5 Items"))  {ChangeBoxView("ItemList");}		
		GUI.enabled = true;
			
		if(!_buttonToggle[5]){GUI.enabled = false;}
		if(GUI.Button(new Rect(_POSX, (_LINE_HEIGHT)*11, _buttonX, _LINE_HEIGHT), "F6 Task"))  {ChangeBoxView("TaskList");}
		GUI.enabled = true;
		
		if(_GameManager.CurZone == "Dungeon"){
			if(GUI.Button(new Rect(_POSX, (_LINE_HEIGHT)*12, _buttonX, _LINE_HEIGHT), "F7 Map"         ))  {ChangeBoxView("Map");}
			if(GUI.Button(new Rect(_POSX, (_LINE_HEIGHT)*13, _buttonX, _LINE_HEIGHT), "F8 Abandon"))  {AbandonDungeon();}
		}
		
	}
	
	public  void ChangeBoxView(string _newBoxToShow)
	{
		if(ToDisplayInBox == _newBoxToShow)
		{
			_GameManager.ChangeState ("Play");
			ToDisplayInBox = "None";
		} 
		else 
		{
			_GameManager.ChangeState ("Menu");
			ToDisplayInBox = _newBoxToShow;
		}
	}
	
	public  void SwitchBoxFromKeys(string _keyPressed)
	{
		switch(_keyPressed)
		{
			case "F1": if(_buttonToggle[0] == true){ChangeBoxView ("BuildList");} break;
			case "F2": if(_buttonToggle[1] == true){ChangeBoxView ("TrainList");} break;
			
			case "F3": if(_buttonToggle[2] == true){ChangeBoxView ("StatList") ;} break;
			case "F4": if(_buttonToggle[3] == true){ChangeBoxView ("SkillList");} break;
			case "F5": if(_buttonToggle[4] == true){ChangeBoxView ("ItemList") ;} break;
			case "F6": if(_buttonToggle[5] == true){ChangeBoxView ("TaskList") ;} break;
			case "F7": if(_GameManager.CurZone == "Dungeon"){ChangeBoxView ("Map")   ; Debug.Log("Pressed F7");} break;
			case "F8": if(_GameManager.CurZone == "Dungeon"){AbandonDungeon();} break;
			
			default:break;
		}
	}
	
	private void DisplayInBox(string _toDisplay)
	{
		switch(_toDisplay)
		{
		
		case "CraftList":
			for(int i = 0; i <  _Inventory.ItemList.Length; i++)
			{
				if(_Inventory.ItemList[i].IsUnlocked == true)
				{
					if(GUI.Button(new Rect(_boxPosX + 25, _boxPosY + (i+1) * (_LINE_HEIGHT+10)    , _buttonX, _LINE_HEIGHT), new GUIContent(_Inventory.ItemList[i].Name, "mouseOverOnItemToCraft" + _Inventory.ItemList[i].Name)))
					{
						CraftSystem.CraftItem (_Inventory.ItemList[i]);
					}
				}
			}
			_mouseOver = GUI.tooltip;
			break;	
			
		case "BuildList":
			for(int i = 0; i <  _Inventory.BuildingList.Length; i++)
			{
				if(_Inventory.BuildingList[i].IsUnlocked == true)
				{
					if(GUI.Button(new Rect(_boxPosX + 25, _boxPosY + (i+1) * (_LINE_HEIGHT+10)    , _buttonX, _LINE_HEIGHT), new GUIContent(_Inventory.BuildingList[i].Name, "mouseOverOnItemToBuild" + _Inventory.BuildingList[i].Name)))
					{
						BuildSystem.BuildBuilding (_Inventory.BuildingList[i]);
					}
				}
			}
			_mouseOver = GUI.tooltip;
			break;
		
		case "TrainList":
			if(GUI.Button(new Rect(_boxPosX + 25, _boxPosY + 1*_LINE_HEIGHT     , _buttonX, _LINE_HEIGHT), "Fighting")){}
			if(GUI.Button(new Rect(_boxPosX + 25, _boxPosY + 2*_LINE_HEIGHT + 10, _buttonX, _LINE_HEIGHT), "Crafting")){}
			break;	
			
			
		case "StatList":
			if(GUI.Button(new Rect(_boxPosX + 25, _boxPosY + 1*_LINE_HEIGHT     , _buttonX, _LINE_HEIGHT), "Strength")){}
			if(GUI.Button(new Rect(_boxPosX + 25, _boxPosY + 2*_LINE_HEIGHT + 10, _buttonX, _LINE_HEIGHT), "Wisdom")){}
			break;	
			
		case "SkillList":
			int _boxBarLength = (int)(_boxSizeX*0.60f);
			int _skillRowNbr = 0;
			float _barRatio;
			for(int i = 0; i <  Character.SkillList.Length; i++)
			{
				if(Character.SkillList[i].Unlocked == true)
				{
					_barRatio = Character.SkillList[i].CurExp/100.0f;
					GUI.Label 	   (new Rect(_boxPosX + 25 ,  _boxPosY + (_skillRowNbr+1)  * (_LINE_HEIGHT+10) , 300 , _LINE_HEIGHT),Character.SkillList[i].Name);
					GUI.Label 	   (new Rect(_boxPosX + 100,  _boxPosY + (_skillRowNbr+1)  * (_LINE_HEIGHT+10) , 300 , _LINE_HEIGHT),"LVL : " + Character.SkillList[i].Level);
					GUI.DrawTexture(new Rect(_boxPosX + 150,  _boxPosY + (_skillRowNbr+1)  * (_LINE_HEIGHT+10) , _boxBarLength, _LINE_HEIGHT), textureProgBackground);	
					GUI.DrawTexture(new Rect(_boxPosX + 150,  _boxPosY + (_skillRowNbr+1)  * (_LINE_HEIGHT+10) , _boxBarLength*_barRatio, _LINE_HEIGHT), textureSkill,ScaleMode.ScaleAndCrop);
					GUI.Label 	   (new Rect(_boxPosX + 150 + _boxBarLength , _boxPosY + (i+1) * (_LINE_HEIGHT+10) , 300 , _LINE_HEIGHT),_barRatio*100 + "%");
					_skillRowNbr++;
				}
			}
			break;	
			
		case "ItemList":
			DisplayBoxItemList(); //Display the inventory with all slots/item
			break;
			
		case "TaskList":
			DisplayTaskList(); //Display all the task
			break;
			
		case "Map":
			DisplayDungeonMap();
			break;
			
		case "DungeonAbandon":
			AbandonDungeon();	
			break;
		default:
			Debug.LogWarning ("Wrong Display in PlayerHUD :" + _toDisplay);
			break;
		}	
	}
		
	private void AbandonDungeon() //TODO: Probably to move somewhere else
	{
		
		DungeonManager _DungeonManager = GameObject.FindGameObjectWithTag("DungeonMaster").GetComponent<DungeonManager>();	
		_DungeonManager.Abandon();
	}
	
	private void DisplayDungeonMap()
	{
		int[,] _map;
		DungeonManager _DungeonManager = GameObject.FindGameObjectWithTag("DungeonMaster").GetComponent<DungeonManager>();	
		_map = _DungeonManager.DungeonMap;
		Debug.Log (Mathf.Round(_PlayerMaster.transform.position.x));
		Debug.Log (Mathf.Round(_PlayerMaster.transform.position.z));
		for(int j = _DungeonManager.MapSizeZ - 1; j >= 0  ; j--)
		{
			for(int i = 0; i < _DungeonManager.MapSizeX ; i++)
			{
				if(i == Mathf.Round(_Player.transform.position.x) && j == Mathf.Round(_Player.transform.position.z))
				{
					GUI.color = Color.blue;	
					GUI.Label(new Rect(_boxPosX + i*10, _boxPosY + 15 * j, 15, 15), "P");		
					GUI.color = Color.white;	
					Debug.Log ("Player found at : " + i + "," + j);
				}
				else
				{
					GUI.Label(new Rect(_boxPosX + i*10, _boxPosY + 15 * j, 15, 15), _map[i,j].ToString());		
				}
			}	
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
					GUI.Button(new Rect(_boxPosX + 25 ,  _boxPosY + (++_taskRowNbr+1)  * (_LINE_HEIGHT+10) , 500 , _LINE_HEIGHT),Character.TaskList[i].Definition);
					GUI.enabled = true;
			}
		}	
	}
		
	private void DisplayBoxItemList()
	{
		float invSlot_offset = 10;
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
				if(ItemInventory.InventoryList[_curSlot].slotItem != null)
				{
					if(GUI.Button(new Rect(invSlot_posX, invSlot_posY, invSlot_sizeX, invSlot_sizeY),ItemInventory.InventoryList[_curSlot].slotItem.ItemTexture))
					{
						_inventorySlotClicked = (_curSlot);
		       			if (Event.current.button == 1)
						{
							GUI.Label 	   (new Rect(_POSX, _LINE_HEIGHT*3.5f  , 300 , _LINE_HEIGHT),"BL:ALBALBA" );
							_ItemClicked  = ItemInventory.InventoryList[_curSlot].slotItem;
							_MousePosOnClick = 	new Vector2(Event.current.mousePosition.x,Event.current.mousePosition.y);
							
							_isDisplayFloatingMenu = true;
						}
						else if (Event.current.button == 0)
						{
							EquipItem(ItemInventory.InventoryList[_curSlot].slotItem, _curSlot);
						}
						
					}
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
		for(int i = 0; i <  _Inventory.RessourceList.Length; i++)
		{
			GUI.Label (new Rect(Screen.width*0.9f, 15 + i*_LINE_HEIGHT , 100 , _LINE_HEIGHT),_Inventory.RessourceList[i].Name + " : " + _Inventory.RessourceList[i].CurValue );
		}
		//GUI.Label (new Rect(Screen.width*0.9f, 15 + 0*_LINE_HEIGHT , 100 , _LINE_HEIGHT),"Coin : " + _Ressources.CoinNbr );
		//GUI.Label (new Rect(Screen.width*0.9f, 15 + 1*_LINE_HEIGHT , 100 , _LINE_HEIGHT),"Wood : " + _Ressources.WoodNbr );
		//GUI.Label (new Rect(Screen.width*0.9f, 15 + 2*_LINE_HEIGHT , 100 , _LINE_HEIGHT),"Rock : " + _Ressources.RockNbr );
	}
	
	private void DisplayProgressBar()
	{
		
		float _ratio = ((float)_GameManager.CurProgress / (float)100);
		
		GUI.DrawTexture(new Rect(_POSX, _LINE_HEIGHT*0.5f, _progBarLength, _LINE_HEIGHT), textureProgBackground);	
		GUI.DrawTexture(new Rect(_POSX, _LINE_HEIGHT*0.6f, _ratio*_progBarLength, 0.8f*_LINE_HEIGHT), textureProg,ScaleMode.ScaleAndCrop);
		GUI.Label 	   (new Rect(_POSX, _LINE_HEIGHT*1.5f  , 300 , _LINE_HEIGHT),"Action : " + _GameManager.CurAction );
		GUI.Label 	   (new Rect(_POSX, _LINE_HEIGHT*2.3f  , 300 , _LINE_HEIGHT),"Zone   : " + _GameManager.CurZone );
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
			GUI.Label (new Rect(_posX, _posY + i*_LINE_HEIGHT , _sizeX, _LINE_HEIGHT), _toDisplay[i]);
		}
	}
	
	// Display a floating menu to interact with
	private void DisplayFloatingMenu(Vector2 _position, Item _ItemRightClicked, int _inventorySlotNumber)
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
			EquipItem(_ItemRightClicked, _inventorySlotNumber);
			CloseMenu ();
		}
			
		if(GUI.Button (new Rect(_posX, _posY + _lineCnt++*_LINE_HEIGHT , _sizeX, _LINE_HEIGHT),"Delete"))
		{
			RemoveItemFromInventory(_inventorySlotNumber);
			CloseMenu ();
		}
		
		if(GUI.Button (new Rect(_posX, _posY + _lineCnt++*_LINE_HEIGHT , _sizeX, _LINE_HEIGHT),"Cancel"))      
		{
			CloseMenu ();
		}
	}
	
	private void EquipItem(Item _ItemToEquip, int _slotNbr)
	{	
		RemoveItemFromInventory(_inventorySlotClicked);
		
		if(ItemInventory.EquippedItem != _Inventory.ItemList[(int)ItemName.None])
		{
			UnEquipItem();
		}
		
		ItemInventory.EquipItem(_ItemToEquip);
		AddChatLog("[ITEM] You equipped " + _ItemToEquip.Name);
	}
	
	private void UnEquipItem()
	{
		int _emptySlot;
		_emptySlot = ItemInventory.FindFirstEmptySlot();
		
		ItemInventory.InventoryList[_emptySlot].slotItem = ItemInventory.EquippedItem;
		ItemInventory.InventoryList[_emptySlot].slotCount = 1;
		ItemInventory.InventoryList[_emptySlot].isSlotFull = true;
		ItemInventory.UnequipItem();
	}
	
	private void RemoveItemFromInventory(int _slotNbr)
	{
		ItemInventory.InventoryList[_slotNbr].slotItem = null;
		ItemInventory.InventoryList[_slotNbr].slotCount = 0;
		ItemInventory.InventoryList[_slotNbr].isSlotFull = false;
	}
	
	private void CloseMenu()
	{
		_isDisplayFloatingMenu = false;
	}
}
