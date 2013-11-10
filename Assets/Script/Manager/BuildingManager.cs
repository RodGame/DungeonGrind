using UnityEngine;
using System.Collections;

public class BuildingManager : MonoBehaviour {
	
	public int Id;
	public int buildingId;
	private GameManager _GameManager;
	private GameObject _Player;
	private bool _isObjectColliding = false;
	
	// Use this for initialization
	void Awake () {
		_GameManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
		_Player	     = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		_isObjectColliding = false;
	}	
	
	void OnCollisionStay(Collision _Collision)
	{
		if(_Collision.gameObject.name != "Terrain" && _Collision.gameObject.tag != "EquippedItem" && _Collision.gameObject.tag != "Player")
		{
			_isObjectColliding = true;
		}
	}
	
	public void ControlBuilding()
	{
		float _rotaSpeed = 2.0f;
		float _moveSpeed = 0.5f;
		float _newPosX = BuildSystem.ActiveBuilding.transform.position.x;
		float _newPosZ = BuildSystem.ActiveBuilding.transform.position.z;
		float _newPosY = Utility.FindTerrainHeight(_newPosX, _newPosZ,-0.5f);
		
		transform.position = new Vector3(_newPosX, _newPosY, _newPosZ);
		
		if(_isObjectColliding == true)
		{
			gameObject.renderer.material.color = Color.red;	
		}
		else
		{
			gameObject.renderer.material.color = Color.green;	
		}
		
		
		// Test for inputs
		if(Input.GetKey(KeyCode.Q))
		{
			transform.RotateAroundLocal(new Vector3(0.0f,1.0f,0.0f), _rotaSpeed*Time.deltaTime);
		}
		else if(Input.GetKey(KeyCode.E))
		{
			transform.RotateAroundLocal(new Vector3(0.0f,1.0f,0.0f), -_rotaSpeed*Time.deltaTime);
		} 
		else if(Input.GetKey(KeyCode.R))
		{
			if(Vector3.Distance (_Player.transform.position, transform.position) < 10)
			{
				transform.position += _moveSpeed*_Player.transform.forward;
			}
		} 
		else if(Input.GetKey(KeyCode.F))
		{
			if(Vector3.Distance (_Player.transform.position, transform.position) > 4)
			{
				transform.position -= _moveSpeed*_Player.transform.forward;
			}
		} 
		else if(Input.GetKey(KeyCode.Z))
		{
			DestroyBuilding();
		} 
		
		// Test for drop of building
		if(Input.GetMouseButtonDown(0))
		{
			if(_isObjectColliding == false)
			{
				gameObject.renderer.material.color = Color.white;
				transform.parent = null;
				BuildSystem.BuildState = 0;
				_GameManager.ChangeState("Play");
				
			}
			else
			{
				_GameManager.AddChatLogHUD("[BUIL] Building can't be placed there");	
			}
			
		}
	}
	
	private void DestroyBuilding()
	{
		Debug.Log ("Destroyed Item");
		BuildSystem.CreatedBuildingList.Remove(gameObject);
		BuildSystem.BuildState = 0;
		BuildSystem.ActiveBuilding = null;
		GameObject.Destroy(gameObject);
	}
	
	
	
}
