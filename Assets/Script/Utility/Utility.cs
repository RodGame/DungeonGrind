using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List class;

static class Utility {
	
	static float _monsterHeight = 0.0f;
	
	public struct ParsedString
    {
        public int nbr;
        public string type;
    };	
	
	public static Vector3 FindRandomPosition(Vector3 _originPosition, float _minX, float _maxX, float _minZ, float _maxZ)
	{
		Vector3 _RandomPosition;
		
		float _posX = _originPosition.x + Random.Range (_minX, _maxX);
		float _posZ = _originPosition.z + Random.Range (_minZ, _maxZ);
		float _posY = FindTerrainHeight(_posX, _posZ, 0.0f);
		
		if(Terrain.activeTerrain != null)
		{
			_RandomPosition = new Vector3(_posX, _posY + Terrain.activeTerrain.transform.position.y, _posZ);
		}
		else
		{
			_RandomPosition = new Vector3(_posX, _monsterHeight, _posZ);
		}
		
		
		return _RandomPosition;
	}
	
	public static float FindTerrainHeight(float _x, float _y, float _offset)
	{
		GameObject _GO_Ground = GameObject.FindGameObjectWithTag("Ground");
		
		if(_GO_Ground.name == "Terrain")
		{
			return Terrain.activeTerrain.SampleHeight(new Vector3(_x,0.0f,_y)) + _offset;	
		}
		else if(_GO_Ground.name == "DungeonGround")
		{
			return (_GO_Ground.transform.position.y + _GO_Ground.transform.localScale.y/2 +_offset);
		}
		else
		{
			return(2.0f + _offset);	
		}
	}
	
	public static Vector3 FindOffsetPosition(Transform _baseTransform, float _offsetMag, float _offsetPhase)
	{
		Vector3 OffsetedPosition;
		float _playerOrientationAngleY;
		
		float _offsetX;
		float _offsetZ;
		
		_playerOrientationAngleY = _baseTransform.rotation.y + _offsetPhase;
		
		_offsetX = _offsetMag * Mathf.Sin (Mathf.Deg2Rad*_playerOrientationAngleY);
		_offsetZ = _offsetMag * Mathf.Cos (Mathf.Deg2Rad*_playerOrientationAngleY);
		
		OffsetedPosition = new Vector3(_offsetX, 0.0f, _offsetZ);
		
		return OffsetedPosition;
	}
	
	public static List<ParsedString> parseString(string __string)
	{
		List<ParsedString> __curList = new List<ParsedString>();
		char __delimiter  = '+';
		char __delimiter2 = '*';
		ParsedString __ParsedString;
		
		string[] __compoundsString = __string.Split (__delimiter); //Parse the input __string with the __delimiter char
		
			for(int i = 0; i < __compoundsString.Length; i++)
			{
				
				string[] __nbrAndCompound = __compoundsString[i].Split (__delimiter2); //Parse the input __string with the __delimiter char
				//Debug.Log ("Length :" + __compoundsString.Length);
				//Debug.Log (" 0 : " + __nbrAndCompound[0]);
				//Debug.Log (" 1 : " + __nbrAndCompound[1]);
			
				__ParsedString.nbr = int.Parse(__nbrAndCompound[0]);
				__ParsedString.type = __nbrAndCompound[1];
				__curList.Add(__ParsedString);
			}
		return __curList;
	}
	
	public static List<string> parseStringToListString(string __string)
	{
		List<string>		   ListString = new List<string>();
		List<ParsedString> ListRessourceToListString  = new List<ParsedString>();	//Declare a list that contain all the ressource needed
		ListRessourceToListString = parseString(__string);
		
		for(int i = 0; i < ListRessourceToListString.Count; i++)
		{
			ListString.Add (ListRessourceToListString[i].type + " : " + ListRessourceToListString[i].nbr);	
		}
		
		return ListString;
		
		
	}
	
}
