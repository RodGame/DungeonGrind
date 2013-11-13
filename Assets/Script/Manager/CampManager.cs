using UnityEngine;
using System.Collections;

public class CampManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		BuildSystem.SpawnAllBuilding();
		BuildSystem.BuildState = 0;
		GameObject.FindGameObjectWithTag ("GameMaster").GetComponent<GameManager>().ChangeState("Play");
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
