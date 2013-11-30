using UnityEngine;
using System.Collections;

public class OnClickBuildingType : MonoBehaviour {
	
	public int _buildingId;
	private NGUI_HUD _NGUI_HUD;
	
	public int BuildingId
	{
		get {return _buildingId; }
		set {_buildingId = value; }
	}
	
	void Start()
	{
		_NGUI_HUD = GameObject.Find("NGUI_Menu").GetComponent<NGUI_HUD>();
		
	}
	
	void OnClick()
	{
		_NGUI_HUD.BuildingType_Choose(_buildingId);
	}
}
