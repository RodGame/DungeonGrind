using UnityEngine;
using System.Collections;

public class CloseMenuOnClick : MonoBehaviour {
	
	private PlayerHUD _PlayerHUD;
		
	
	// Use this for initialization
	void Start () {
		_PlayerHUD    = GameObject.FindGameObjectWithTag("PlayerMaster").GetComponent<PlayerHUD>();
	}
	
	void OnClick()
	{
		_PlayerHUD.CloseHUD();
	}
}
