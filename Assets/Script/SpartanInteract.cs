using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List class;

public class SpartanInteract : MonoBehaviour {
	
	private Transform _PlayerTransform;
	private GameManager _GameManager;
	private Inventory   _Inventory;
	
	private int  _stateWithPlayer = 1;
	private bool _isStateInteractedOnce = false;
	
	// Use this for initialization
	void Start () {
		_GameManager = GameObject.FindGameObjectWithTag ("GameMaster").GetComponent<GameManager>();
		_Inventory   = GameObject.FindGameObjectWithTag ("PlayerMaster").GetComponent<Inventory>(); 
		animation.animation.wrapMode = WrapMode.Loop;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void TurnToPlayer()
	{
		Vector3 _PlayerPositionAtEyeHeight;
		_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		
		_PlayerPositionAtEyeHeight = new Vector3(_PlayerTransform.position.x, transform.position.y ,_PlayerTransform.position.z);
		
		
		transform.LookAt(_PlayerPositionAtEyeHeight);
		
	}
	
	public void IncreaseStateWithPlayer()
	{
		_stateWithPlayer++;	
		_isStateInteractedOnce = false;
	}
	
	public void InteractWithPlayer()
	{
		List<string> ListOfStrings = new List<string>();
		Debug.Log ("_stateWithPlayer : " + _stateWithPlayer);
		TurnToPlayer();
		switch(_stateWithPlayer)
		{
			case 1:
				if(!_isStateInteractedOnce)
				{
					_GameManager.CompleteTask(Character.TaskList[(int)TaskName.Spartan0]);
					Character.UnlockTask(Character.TaskList[(int)TaskName.Spartan1]);
						
				
					ListOfStrings.Add("Hi there adventurer. My name is Spartan ↓↓");
					ListOfStrings.Add("I'm here to help you become a hero.\n\n Prove me you're not useless and repair the cart behind me ↓↓");
					ListOfStrings.Add("Oh I forgot, take that hammer and make sure to equip it!");
					//ItemInventory.AddItem (_Inventory.ItemList[(int)ItemName.Hammer])
					
					_GameManager.ActivateButtonInHUD(4); //Button4 is Item
					_GameManager.ActivateButtonInHUD(5); //Button4 is Task
					_isStateInteractedOnce = true;
				}
				else
				{
					ListOfStrings.Add("I told you to equipe the hammer in your inventory and repair the cart!!");
				}
				break;
			case 2:
				if(!_isStateInteractedOnce)
				{
					_GameManager.CompleteTask(Character.TaskList[(int)TaskName.Spartan1]);
					Character.UnlockTask(Character.TaskList[(int)TaskName.Spartan2]);
				
					ListOfStrings.Add("Good job! Maybe I can do something with you finally.. ↓↓");
					ListOfStrings.Add("I have plan for a crafting table here. Can you make one ?");
					Debug.Log ("Activate Buttons");
					_GameManager.ActivateButtonInHUD(0); //Button0 is Build
					_Inventory.BuildingList[(int)BuildingName.CraftingTable].IsUnlocked = true; // Unlock the building of the Crafting Table //TODO: Move this in a real reward
					_isStateInteractedOnce = true;
					
				}
				else
				{
					ListOfStrings.Add("You repaired the cart... BUILDING a table shouldn't be too hard");
				}
				break;
			case 3:
				if(!_isStateInteractedOnce)
				{
					Character.UnlockTask(Character.TaskList[(int)TaskName.Spartan3]);
				
					ListOfStrings.Add("Excellent! Now craft a axe and pickaxe↓↓");
					ListOfStrings.Add("And show me that you can gather 10 Wood and 10 Rock");
				
					_isStateInteractedOnce = true;
					
				}
				else
				{
					ListOfStrings.Add("Plz bring the ressources!");
				}
				break;
			case 4:
				if(!_isStateInteractedOnce)
				{
					Character.UnlockTask(Character.TaskList[(int)TaskName.Spartan4]);
				
					ListOfStrings.Add("Now buy a sword");
				
					_isStateInteractedOnce = true;
					
				}
				else
				{
					ListOfStrings.Add("Plz bring the ressources!");
				}
				break;
			default:
				Debug.LogWarning ("WARNING : Tried to have an unkwown discussion with Spartan");
				break;
		}
		_GameManager.StartDiscussion(ListOfStrings);
	}
}
