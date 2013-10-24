using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {
	
	private GameManager _GameManager;
	
	private int _buttonPosX  = (int)(Screen.width  * 0.165f);
	private int _buttonPosY  = (int)(Screen.height * 0.10f);
	private int _buttonSizeX = (int)(Screen.width  * 0.66f);
	private int _buttonSizeY = (int)(Screen.height * 0.10f);
	private int _offsetY     = (int)(Screen.height * 0.05f);

	// Use this for initialization
	void Start () {
		_GameManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
		_GameManager.ChangeState("Menu");
	}
	
	void OnGUI()
	{
		float _boxPosX;
		float _boxPosY;
		
		if(GUI.Button(new Rect(_buttonPosX, _buttonPosY, _buttonSizeX, _buttonSizeY), "New Game (Erase Save)"))
		{
			PlayerPrefs.DeleteAll();
			StartGame();
			
		}
		if(PlayerPrefs.GetInt ("IsSaveExist") == 0) //If save exist
		{
			GUI.enabled = false;
		}
		
		if(GUI.Button(new Rect(_buttonPosX, _buttonPosY + _buttonSizeY + _offsetY, _buttonSizeX, _buttonSizeY), "Load Game"))
		{
			StartGame();
		}
		GUI.enabled = true;
		
		
		// Saved stats Display
		_boxPosX = _buttonPosX;
		_boxPosY = _buttonPosY + 2 * _buttonSizeY + 2 *_offsetY;
		GUI.Box(new Rect(_buttonPosX, _boxPosY, _buttonSizeX, _buttonSizeY*3),"");
		
		if(PlayerPrefs.GetInt ("IsSaveExist") == 0) //If save exist
		{
			GUI.Label(new Rect(_buttonPosX, _boxPosY + 0.5f*_offsetY, _buttonSizeX, 25.0f),"Save not found. Start a new game.");
		}								   
		else							
		{								   
			GUI.Label(new Rect(_buttonPosX, _boxPosY           , _buttonSizeX, 25.0f),"==> SAVE FOUND <== ");
			GUI.Label(new Rect(_buttonPosX, _boxPosY + 1f*25.0f, _buttonSizeX, 25.0f),"Total Skill Level : " + Character.CalculateSavedSkillLevel());
			GUI.Label(new Rect(_buttonPosX, _boxPosY + 2f*25.0f, _buttonSizeX, 25.0f),"Dungeon level : " + PlayerPrefs.GetInt ("MaxDungeonLevel"));
			GUI.Label(new Rect(_buttonPosX, _boxPosY + 3f*25.0f, _buttonSizeX, 25.0f),"Influence : "     + PlayerPrefs.GetInt ("InfluencePoints"));
		}
	}
	
	void StartGame()
	{
		Application.LoadLevel("Camp");
		GameObject.FindGameObjectWithTag("PlayerMaster").GetComponent<PlayerHUD>().enabled = true;
		GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>().IniGame ();
		GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(-20.0f,0.75f,-10.0f);
		//GameObject.FindGameObjectWithTag("Player").transform.rotation.SetLookRotation(Vector3.back);
	}
}
