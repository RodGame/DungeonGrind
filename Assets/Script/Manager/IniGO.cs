using UnityEngine;
using System.Collections;

public class IniGO : MonoBehaviour 
{
	public GameObject _GameMaster;
	public GameObject _PlayerMaster;
	public GameObject _Player;
	
	void Start()
	{
		if(GameObject.FindGameObjectWithTag ("GameMaster") == null)
		{
			Instantiate(_GameMaster);
		}
		
		if(GameObject.FindGameObjectWithTag ("PlayerMaster") == null)
		{
			Instantiate(_PlayerMaster);
		}

		if(GameObject.FindGameObjectWithTag ("Player") == null)
		{
			Instantiate(_Player,new Vector3(0,2,0),Quaternion.identity);
		}
	}
	
}
