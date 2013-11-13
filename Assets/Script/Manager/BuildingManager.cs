using UnityEngine;
using System.Collections;

public class BuildingManager : MonoBehaviour {
	
	public int Id;
	public int buildingId;
	private GameManager _GameManager;
	private GameObject _Player;
	public bool _isObjectColliding = false;
	public bool _isObjectActive    = false;
	
	// Use this for initialization
	void Awake () {
		_GameManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
		_Player	     = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		_isObjectColliding = false;
		
		if(_isObjectActive == true)
		{
			
		}
	}	
	
	void Update()
	{
		if(_isObjectActive == true)
		{
			ControlBuilding();
			ColorBuilding();
			TestInput();
		}
	}
	
	void ColorBuilding()
	{
		if(_isObjectColliding == true)
		{
			gameObject.renderer.material.color = Color.red;	
			Debug.Log ("Red Colliding");
		}
		else
		{
			gameObject.renderer.material.color = Color.green;	
		}	
	}
	
	void TestInput()
	{
		// Test for drop of building
		if(Input.GetMouseButtonDown(0))
		{
			Debug.Log ("On click, Collision  : " + _isObjectColliding);
			if(_isObjectColliding == false)
			{
				ReleaseBuilding();
				_GameManager.AddChatLogHUD("[BUIL] " + name + " has been built");
			}
			else
			{
				_GameManager.AddChatLogHUD("[BUIL] Building can't be placed there");	
			}
		}	
	}
	
	void OnCollisionStay(Collision _Collision)
	{
		if(_Collision.gameObject.name != "Terrain" && _Collision.gameObject.tag != "EquippedItem" && _Collision.gameObject.tag != "Player")
		{
			_isObjectColliding = true;
		}
	}
	
	public void ActivateBuilding()
	{
		transform.parent = _Player.transform;
		_GameManager.ChangeState("Build");	
		BuildSystem.BuildState = 1;
		_isObjectActive = true;
	}
	
	public void ControlBuilding()
	{
		float _rotaSpeed = 2.0f;
		float _moveSpeed = 0.3f;
		float _newPosX = transform.position.x;
		float _newPosZ = transform.position.z;
		float _newPosY = Utility.FindTerrainHeight(_newPosX, _newPosZ,-0.75f);
		
		transform.position = new Vector3(_newPosX, _newPosY, _newPosZ);	
		
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
	}
	
	private void ReleaseBuilding()
	{
		Debug.Log ("Object Released");
		_isObjectActive = false;
		gameObject.renderer.material.color = Color.white;
		transform.parent = null;
		BuildSystem.BuildState = 0;
		_GameManager.ChangeState("Play");
		
	}
	
	private void DestroyBuilding()
	{
		Debug.Log ("Destroyed Item");
		BuildSystem.CreatedBuildingList.Remove(gameObject);
		BuildSystem.BuildState = 0;
		GameObject.Destroy(gameObject);
		_GameManager.ChangeState("Play");
	}
	
	
	
}
