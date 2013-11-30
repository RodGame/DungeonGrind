using UnityEngine;
using System.Collections;

public class OnClickMenu : MonoBehaviour {
	
	private PlayerHUD _PlayerHUD;
	private NGUI_HUD _NGUI_HUD;	
	
	// Use this for initialization
	void Start () {
		_PlayerHUD = GameObject.FindGameObjectWithTag("PlayerMaster").GetComponent<PlayerHUD>();
		_NGUI_HUD  = GameObject.Find("NGUI_Menu").GetComponent<NGUI_HUD>();
	}
	
	void OnClick()
	{
		if(transform.gameObject.name == "Button - Build")
		{
			_NGUI_HUD.BuildingType_Build();
		}
		else if(transform.gameObject.name == "Button - Close")
		{
			_PlayerHUD.CloseHUD();
		}
	}
}
