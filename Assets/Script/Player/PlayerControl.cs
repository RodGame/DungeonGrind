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
			
		// RayCast if mouseRightButton is pressed
		if(Input.GetMouseButtonDown(mouseLeftButton))
		{
			_ray = _PlayerCam.ScreenPointToRay(new Vector3(_PlayerCam.pixelWidth/2,_PlayerCam.pixelHeight/2,0));
			if(Physics.Raycast (_ray,out _hitInfo))
			{
				_GameManager.RaycastAnalyze(_hitInfo.collider);
			}
		}
	}
	
	void getKeyboardInputs()
	{
		if(Input.GetKeyDown(KeyCode.Tab))
		{
			_GameManager.ToggleState();
		}
		else if(Input.GetKeyDown(KeyCode.F1)){_PlayerHUD.SwitchBoxFromKeys("F1");}
		else if(Input.GetKeyDown(KeyCode.F2)){_PlayerHUD.SwitchBoxFromKeys("F2");}
		else if(Input.GetKeyDown(KeyCode.F3)){_PlayerHUD.SwitchBoxFromKeys("F3");}
		else if(Input.GetKeyDown(KeyCode.F4)){_PlayerHUD.SwitchBoxFromKeys("F4");}
		else if(Input.GetKeyDown(KeyCode.F5)){_PlayerHUD.SwitchBoxFromKeys("F5");}
		else if(Input.GetKeyDown(KeyCode.F6)){_PlayerHUD.SwitchBoxFromKeys("F6");}
		else if(Input.GetKeyDown(KeyCode.F7)){_PlayerHUD.SwitchBoxFromKeys("F7");}
		else if(Input.GetKeyDown(KeyCode.F8)){_PlayerHUD.SwitchBoxFromKeys("F8");}
		//else if(Input.GetKeyDSwitchBoxFromKeysect.FindGameObjectWithTag("Spartan").animation.animation.CrossFade("run");} 
	}
}
