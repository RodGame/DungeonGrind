using UnityEngine;
using System.Collections;

static class Utility {
	
	static float _monsterHeight = 0.0f;
	
	public static Vector3 FindRandomPosition(Vector3 _originPosition, float _minX, float _maxX, float _minZ, float _maxZ)
	{
		Vector3 _RandomPosition;
		
		float _posX = _originPosition.x + Random.Range (_minX, _maxX);
		float _posZ = _originPosition.z + Random.Range (_minZ, _maxZ);
		float _posY = FindTerrainHeight(_posX, _posZ);
		
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
	
	public static float FindTerrainHeight(float _x, float _y)
	{
		GameObject _GO_Ground = GameObject.FindGameObjectWithTag("Ground");
		
		if(_GO_Ground.name == "Terrain")
		{
			return Terrain.activeTerrain.SampleHeight(new Vector3(_x,_monsterHeight,_y));	
		}
		else if(_GO_Ground.name == "DungeonGround")
		{
			return (_GO_Ground.transform.position.y + _GO_Ground.transform.localScale.y/2);
		}
		else
		{
			return(2.0f);	
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
	
	
}
