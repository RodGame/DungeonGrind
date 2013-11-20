using UnityEngine;
using System.Collections;

public class BringThisObjectInLevels : MonoBehaviour
{
	void Start()
	{
		DontDestroyOnLoad(this);	
	}
}