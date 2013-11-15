using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List class;
using System;					  // For Enum

public class SpartanInteract : MonoBehaviour {
	
	private Transform _PlayerTransform;
	private GameManager _GameManager;
	private TextureManager _TextureManager;
	public int  _taskState   = 0; // -1 = No task, 0 = Task available, 1 = Task given. 2 = Task completed, reward available. 
	public int  _currentTask = 0; // Current task number in SpartanInteract#
	
	public int TaskState
	{
		get {return _taskState; }
		set {_taskState = value; }
	}
	
	public int CurrentTask
	{
		get {return _currentTask; }
		set {_currentTask = value; }
	}
	
	// Use this for initialization
	void Awake () {
		_GameManager   = GameObject.FindGameObjectWithTag ("GameMaster").GetComponent<GameManager>();
		_TextureManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<TextureManager>();
		_currentTask = SaveLoadSystem.Spartan_CurrentTask;
		
		_taskState   = SaveLoadSystem.Spartan_TaskState;
		animation.animation.wrapMode = WrapMode.Loop;
	}
	
	// Update is called once per frame
	void Update () {
		
		GameObject _TaskIcon = GameObject.Find("TaskIcon"); //Todo: Make this more efficient. Avoid searching for the name + update texture every update
		
		// _taskState : -1 = No task, 0 = Task available, 1 = Task given. 2 = Task completed, reward available.
		if(_taskState == 0)
		{
			_TaskIcon.gameObject.SetActive(true);	
			_TaskIcon.GetComponent<UITexture>().mainTexture  = _TextureManager.GUI_TaskAvailable;
		}
		else if(_taskState == 1)
		{
			_TaskIcon.gameObject.SetActive(true);
			_TaskIcon.GetComponent<UITexture>().mainTexture = _TextureManager.GUI_TaskPending;
		}
		else if(_taskState == 2)
		{
			_TaskIcon.gameObject.SetActive(true);
			_TaskIcon.GetComponent<UITexture>().mainTexture = _TextureManager.GUI_TaskCompleted;
		}
		else if(_taskState == -1)
		{
			_TaskIcon.gameObject.SetActive(false);
		}
	}
	
	private void TurnToPlayer()
	{
		Vector3 _PlayerPositionAtEyeHeight;
		_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		_PlayerPositionAtEyeHeight = new Vector3(_PlayerTransform.position.x, transform.position.y ,_PlayerTransform.position.z);
		transform.LookAt(_PlayerPositionAtEyeHeight);
	}
	
	public void FinishCurTask()
	{	
		_taskState = 2;
	}
	
	public void UnlockNextTask()
	{
		_currentTask++;
		TaskName TaskIndex = (TaskName) Enum.Parse(typeof(TaskName), "Spartan" + _currentTask); 
		
		Character.TaskList[(int)TaskIndex].Unlock();
		Character.SetActiveTask(Character.TaskList[(int)TaskIndex]);
	}
	
	public void InteractWithPlayer()
	{
		TurnToPlayer();
		
		// _taskState : -1 = No task, 0 = Task available, 1 = Task given. 2 = Task completed, reward available. 
		if(_taskState == 0)
		{
			UnlockNextTask();
			DisplayTaskText();
			_taskState = 1;
		}
		else if(_taskState == 1)
		{
			DisplayTaskText();	
		}
		else if(_taskState == 2)
		{
			DisplayTaskText();
			TaskName TaskIndex = (TaskName) Enum.Parse(typeof(TaskName), "Spartan" + _currentTask); 
			Character.TaskList[(int)TaskIndex].Finish();
			
			_taskState = 0;
		}
		
		
	}
	
	private void DisplayTaskText()
	{
		List<string> ListOfStrings = new List<string>();
		
		switch(_currentTask)
		{
			case 1:
				switch(_taskState)
				{
					case 0:
						ListOfStrings.Add("Hi there adventurer. I'm here to help you become stronger and upgrade the camp...");
						ListOfStrings.Add("\n\n Prove me you're not useless and repair the cart behind me by equipping the hammer in your inventory.");
						break;
					case 1:
						ListOfStrings.Add("I told you to equipe the hammer in your inventory and repair the cart!!");
						break;
					case 2:
						ListOfStrings.Add("Thanks for repairing the cart! It might be useful later.");
						break;
				}
				break;
			case 2:
				switch(_taskState)
				{
					case 0:
						ListOfStrings.Add("I have plan for a crafting table here. Can you make one ?");
						break;
					case 1:
						ListOfStrings.Add("Equip your hammer and Left click to build a Crafting Table");
						break;
					case 2:
						ListOfStrings.Add("Excellent! This crafting table will allow you to craft better gear.");
						ListOfStrings.Add("When you equip the hammer, you can move any object you've created...");
						ListOfStrings.Add("By right-clicking it. Use Q/E for rotation, R/F for for the distance and Z to delete...");
						break;
				}
				break;
			case 3:
				switch(_taskState)
				{
					case 0:
						ListOfStrings.Add("You're better than I tought. I think you're read to try the first dungeon level...");
						ListOfStrings.Add("Enter the crypt outside of the camp and enter the first level... Beware, those pesky spider can hurt!!");
						break;
					case 1:
						ListOfStrings.Add("I can clear the dungeon level 1 for you if you are too weak..");
						break;
					case 2:
						ListOfStrings.Add("I knew you could do it!");
						break;
				}
				break;
			default:
				Debug.LogWarning("SpartanInteract.cs : Trying to access text of an unknown Task.");
				break;
		}
		
		_GameManager.StartDiscussion(ListOfStrings);
			
		
		/*//First quest, Talk to the spartan
		if(Character.TaskList[(int)TaskName.Spartan0].IsUnlocked == true && Character.TaskList[(int)TaskName.Spartan0].IsFinished == false)
		{
			Character.TaskList[(int)TaskName.Spartan0].Finish();
			Character.TaskList[(int)TaskName.Spartan1].Unlock();	
			Character.SetActiveTask(Character.TaskList[(int)TaskName.Spartan1]);
			_isTaskAvailable = true;
		}
		
		// Second quest, repair the cart
		if(Character.TaskList[(int)TaskName.Spartan1].IsUnlocked == true && Character.TaskList[(int)TaskName.Spartan1].IsFinished == false)
		{
			if(_isTaskCompleted)
			{
				
			}
			else
			{
				if(!_isStateInteractedOnce)
				{
					
					
					_isStateInteractedOnce = true;
				}
				else
				{
					
				}
			}
		}
		
		// 3rd Quest : Build a crafting table
		else if(Character.TaskList[(int)TaskName.Spartan2].IsUnlocked == true && Character.TaskList[(int)TaskName.Spartan2].IsFinished == false)
		{
			if(_isTaskCompleted)
			{
				Character.TaskList[(int)TaskName.Spartan2].Finish();
				Character.TaskList[(int)TaskName.Spartan3].Unlock();
				Character.SetActiveTask(Character.TaskList[(int)TaskName.Spartan3]);
				_isTaskCompleted = false;
				_isTaskAvailable = true;
			}
			else
			{
				if(!_isStateInteractedOnce)
				{
					
					Inventory.BuildingList[(int)BuildingName.CraftingTable].IsUnlocked = true; // Unlock the building of the Crafting Table //TODO: Move this in a real reward
					_isStateInteractedOnce = true;
				}
				else
				{
				}
			}
		}
		
		// Fourth Quest, 
		else if(Character.TaskList[(int)TaskName.Spartan3].IsUnlocked == true && Character.TaskList[(int)TaskName.Spartan3].IsFinished == false)
		{
			if(!_isStateInteractedOnce)
			{
			

				_isStateInteractedOnce = true;
				_isTaskAvailable       = true;
				
			}
			else
			{
				}
			}
		}
		/*if(Character.TaskList[(int)TaskName.Spartan3].IsUnlocked == true)
		{
			if(!_isStateInteractedOnce)
			{
				Character.UnlockTask(Character.TaskList[(int)TaskName.Spartan4]);
			
				ListOfStrings.Add("Now go try the dungeon in the crypt!");
				ListOfStrings.Add("Each time you win, the dungeon get bigger and more monster appears!!");
			
				_isStateInteractedOnce = true;
				
			}
			else
			{
				ListOfStrings.Add("Plz bring the ressources!");
			}
		}*/
	}
}
