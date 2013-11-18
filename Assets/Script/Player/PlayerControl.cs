using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	
	
	private Camera _PlayerCam;
	private PlayerHUD _PlayerHUD;
	private GameManager _GameManager;
	
	public bool isCursorInteractive = true;
	
	
	void Start ()
	{
		_PlayerCam   = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<Camera>();
		_GameManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
		_PlayerHUD   = GameObject.FindGameObjectWithTag("PlayerMaster").GetComponent<PlayerHUD>();
	}
	
	// Update is called once per frame
	void Update () {
		if(_GameManager.CurState == "Play")
		{
			getMouseInput();
		}
		getKeyboardInputs();
		if(isCursorInteractive == true)
		{
			testInteractivAtMouse();
		}
	}
	
	void testInteractivAtMouse()
	{
		Ray _ray;
		RaycastHit _hitInfo;	
		_ray = _PlayerCam.ScreenPointToRay(new Vector3(_PlayerCam.pixelWidth/2,_PlayerCam.pixelHeight/2,0));
		if(Physics.Raycast (_ray,out _hitInfo))
		{
			_GameManager.RaycastAnalyzeAtMouse(_hitInfo.collider);
		}
		else
		{
			_GameManager.RaycastAnalyzeAtMouse(null);
		}
	}
	
	void getMouseInput()
	{
		Ray _ray;
		RaycastHit _hitInfo;	
		int mouseLeftButton = 0;
		int mouseRightButton = 1;	
		// RayCast if mouseRightButton is pressed
		if(Input.GetMouseButtonDown(mouseLeftButton))
		{
			_ray = _PlayerCam.ScreenPointToRay(new Vector3(_PlayerCam.pixelWidth/2,_PlayerCam.pixelHeight/2,0));
			if(Physics.Raycast (_ray,out _hitInfo))
			{
				_GameManager.RaycastAnalyze(_hitInfo.collider);
			}
			else
			{
				_GameManager.RaycastAnalyze(null);
			}
		}
		else if(Input.GetMouseButtonDown(mouseRightButton))
		{
			_ray = _PlayerCam.ScreenPointToRay(new Vector3(_PlayerCam.pixelWidth/2,_PlayerCam.pixelHeight/2,0));
			Physics.Raycast (_ray,out _hitInfo);
			_GameManager.RightClick(_hitInfo.collider);
		}
	}
	
	void getKeyboardInputs()
	{
		if(Input.GetKeyDown(KeyCode.Tab))
		{
			_GameManager.ToggleMenu();
		}
		else if(Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.I)){_PlayerHUD.SwitchBoxFromKeys("F1");}      //Item
		else if(Input.GetKeyDown(KeyCode.F2) || Input.GetKeyDown(KeyCode.C)){_PlayerHUD.SwitchBoxFromKeys("F2");}      //Stats
		else if(Input.GetKeyDown(KeyCode.F3) || Input.GetKeyDown(KeyCode.X)){_PlayerHUD.SwitchBoxFromKeys("F3");}      //Skills
		else if(Input.GetKeyDown(KeyCode.F4) || Input.GetKeyDown(KeyCode.P)){_PlayerHUD.SwitchBoxFromKeys("F4");}      //Spell
		else if(Input.GetKeyDown(KeyCode.F5) || Input.GetKeyDown(KeyCode.B)){_PlayerHUD.SwitchBoxFromKeys("F5");}      //Build
		else if(Input.GetKeyDown(KeyCode.F6) || Input.GetKeyDown(KeyCode.T)){_PlayerHUD.SwitchBoxFromKeys("F6");}      //Task
		else if(Input.GetKeyDown(KeyCode.F7) || Input.GetKeyDown(KeyCode.M)){_PlayerHUD.SwitchBoxFromKeys("F7");}      //Map
		else if(Input.GetKeyDown(KeyCode.F8))                               {_PlayerHUD.SwitchBoxFromKeys("F8");}      //Abandon
		else if(Input.GetKeyDown(KeyCode.F9) || Input.GetKeyDown(KeyCode.O)){_PlayerHUD.SwitchBoxFromKeys("F9");}      //Save
		else if(Input.GetKeyDown(KeyCode.F10) || Input.GetKeyDown(KeyCode.H)){_PlayerHUD.SwitchBoxFromKeys("F10");}
		else if(Input.GetKeyDown(KeyCode.F11)){_PlayerHUD.SwitchBoxFromKeys("F11");}
		else if(Input.GetKeyDown(KeyCode.F12)){_PlayerHUD.SwitchBoxFromKeys("F12");}
		//else if(Input.GetKeyDSwitchBoxFromKeysect.FindGameObjectWithTag("Spartan").animation.animation.CrossFade("run");} 
	}
}
