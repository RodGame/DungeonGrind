using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List class;

static class DungeonGenerator {
	
	static PrefabManager _PrefabManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<PrefabManager>();
	static Material _Material_BrickWall;
	static TextureManager _TextureManager;
	static List<WallsToCreate> _ListOfWallsToCreate = new List<WallsToCreate>();
	static int _nbrRoom;
	
	static private int NbrRoom
	{
		get {return _nbrRoom; }
		set {_nbrRoom = value; }
	}
	private struct WallsToCreate
    {
        public Vector2 WallStart;
        public Vector2 WallEnd;
    };	
	
	static public int[,] SpawnDungeon(int _sizeX,int _sizeZ, int nbrSquareForGeneration)
	{
		 
		int[,] _RoomsMap     = new int[_sizeX,_sizeZ];
		
		_TextureManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<TextureManager>();
		_Material_BrickWall = _TextureManager.Material_Dungeon_BrickWall;
		
		int[,] _dungeonMap;
		
		_dungeonMap = CreateDungeonSquares(_sizeX,_sizeZ, nbrSquareForGeneration);
		_nbrRoom = FindRooms (_dungeonMap, _RoomsMap,_sizeX, _sizeZ);
		if(_nbrRoom > 1)
		{
			_dungeonMap = CreateDungeonHalls(_RoomsMap, _sizeX,_sizeZ,_nbrRoom);
		}
		
		//SpawnEnvironment(_dungeonMap, _sizeX, _sizeZ, _nbrRoom);
		
		_ListOfWallsToCreate = EvaluateWallToBuild(_RoomsMap, _sizeX, _sizeZ);
		InstantiateDungeonWalls(_ListOfWallsToCreate);
		
		return _dungeonMap;
	}
	
	static private void SpawnEnvironment(int [,]_dungeonMap, int _sizeX, int _sizeZ, int _nbrRoomCreated)
	{
		
		for(int i = 0; i < _nbrRoomCreated; i++)
		{
			Vector3 _newPos = GameObject.FindGameObjectWithTag("DungeonMaster").GetComponent<DungeonManager>().FindRandomRoomPosition(_dungeonMap, _sizeX, _sizeZ, 10);
			GameObject.Instantiate(_PrefabManager.Environment_SpiderEggs, _newPos, Quaternion.identity);
				
		}
	}
	
	static private int[,] CreateDungeonHalls(int[,] _RoomsMap, int _sizeX,int _sizeZ, int nbrRoom)
	{
			//int _curRoomNbr = 0;
			int _x1;
			int _z1;
			int _x2;
			int _z2;
			int _diffX;
			int _diffZ;
			int debug_nbrCorrdiorCoordCreated = 0;
			int debug_nbrCorrdiorCreated = 0;
			
		
		for(int _curRoomNbr = 1; _curRoomNbr <= nbrRoom; _curRoomNbr++)
		{
			int nbrRoomsTry = 0;
			
			// Find a random coordinate in the room with number _corrdiorCreated + 1. 1 corridor will start form each room
			_x1 = Random.Range (1, _sizeX-1);	
			_z1 = Random.Range (1, _sizeZ-1);		
			while(_RoomsMap[_x1,_z1] != _curRoomNbr)
			{
				_x1 = Random.Range (1, _sizeX-1);	
				_z1 = Random.Range (1, _sizeZ-1);	
			}
			
			// Find a random coordinate in any room that isn't the first room
			_x2 = Random.Range (1, _sizeX-1);
			_z2 = Random.Range (1, _sizeZ-1);
			
			while((_RoomsMap[_x2,_z2] == 0 || _RoomsMap[_x2,_z2] == _curRoomNbr) && nbrRoomsTry < 100)
			{
				_x2 = Random.Range (1, _sizeX-1);	
				_z2 = Random.Range (1, _sizeZ-1);
				nbrRoomsTry++;	
			}
			
			// Difference between both coordinates on two axis
			
			_diffX = _x2 - _x1;
			_diffZ = _z2 - _z1;
			
			int _xDirection;
			int _zDirection;
			
			if(_diffX != 0)
			{
				_xDirection = _diffX/Mathf.Abs(_diffX); // 1 = -> ... -1 = <-
			}
			else
			{
				_xDirection = 0;
			}
			
			if(_diffZ != 0)
			{
				_zDirection = _diffZ/Mathf.Abs(_diffZ); // 1 = -> ... -1 = <-
			}
			else
			{
				_zDirection = 0;
			}
			
			
			
			//int _hallWidth  = 2 + Mathf.Abs (_diffZ)/10;
			//int _hallHeight = 2 + Mathf.Abs (_diffX)/10;
			
			int _hallWidth  = Random.Range (4,10);
			int _hallHeight = Random.Range (4,10);
			
			//Create vertical part of the hall
			for(int i = _x1; i != _x1 + _xDirection*_hallWidth; i += _xDirection)
			{
				for(int j = _z1; j != _z2; j += _zDirection)
				{
					if(i >= 0 && i < _sizeX && j >= 0 && j < +_sizeZ)
					{
						if(_RoomsMap[i,j] == 0)
						{
							_RoomsMap[i,j] = -1;
							debug_nbrCorrdiorCoordCreated++;
						}
					}
				}
			}
		
			//Create horizontal part of the hall
			for(int i = _x1; i != _x2; i += _xDirection)
			{
				for(int j = _z2; j != _z2 + _zDirection*_hallHeight; j += _zDirection)
				{
					if(i >= 0 && i < _sizeX && j >= 0 && j < +_sizeZ)
					{
						if(_RoomsMap[i,j] == 0)
						{
							_RoomsMap[i,j] = -1;
							debug_nbrCorrdiorCoordCreated++;
						}
					}
				}
			}
			debug_nbrCorrdiorCreated++;
		}	
		
		//Debug.Log ("Nbr corridor coord : " + debug_nbrCorrdiorCoordCreated);
		//Debug.Log ("Nbr corridor : " + debug_nbrCorrdiorCreated);
		
		return _RoomsMap;
	}
	
	static int FindRooms(int[,] _originalMap, int[,] _diffRoomMap, int _sizeX, int _sizeZ)
	{
		List<Vector2> ListCoordToTest = new List<Vector2>();
		
		int[,] _modifiedMap = new int[_sizeX,_sizeZ]; 
		int    _nbrRoomFound = 0;
		int _debug_nbrTimeAroundFound = 0;
		int _debug_nbrWhileDone   = 0;
		
		System.Array.Copy(_originalMap,_modifiedMap, _sizeX*_sizeZ); // Copy the originalMap in the modifiedMap
		for(int i = 1; i < _sizeX; i++)
		{
			for(int j = 1; j < _sizeZ; j++)
			{
				// If we find a room
				if(_modifiedMap[i,j] == 1)
				{
					ListCoordToTest.Add(new Vector2(i, j));
					
					//curNbrPosAround = CountRoomPosAround(_modifiedMap, i, j);
					
					while(ListCoordToTest.Count > 0)
					{
						int _x = (int)ListCoordToTest[0].x;
						int _z = (int)ListCoordToTest[0].y;
						ListCoordToTest.RemoveAt (0);
						
						_modifiedMap[_x,_z] = 0; // Remove the position from the room in the modified Map
						_diffRoomMap[_x,_z] = _nbrRoomFound + 1;
						
						for(int _xAround = _x - 1; _xAround <= _x + 1; _xAround++)
						{
							for(int _zAround = _z - 1 ; _zAround <= _z + 1; _zAround++)
							{
								//if(_modifiedMap[_x,_z] == 1 && (ListCoordToTest.Contains(new Vector2(_xAround,_zAround))) == false)
								if(_modifiedMap[_xAround,_zAround] == 1 && (ListCoordToTest.Contains(new Vector2(_xAround,_zAround))) == false)
								{
									_debug_nbrTimeAroundFound++;
									ListCoordToTest.Add(new Vector2(_xAround, _zAround));
								}
								//Debug.Log ("Room Found");
							}
						}
						_debug_nbrWhileDone++;
					}
					_nbrRoomFound++;
					
				}
			}
		}
		//Debug.Log (_nbrRoomFound + " Rooms found");
		//Debug.Log (_debug_nbrTimeAroundFound + " Times found a piece around");
		//Debug.Log (_debug_nbrWhileDone + " Times in While");
		return _nbrRoomFound;
	}
	
	static void InstantiateDungeonWalls(List<WallsToCreate> _lclListOfWalls) ///TODO: Fix notation
	{
		
		Vector2 _curVector1;
		Vector2 _curVector2;
		Vector3 _curVector1_3D;
		Vector3 _curVector2_3D;
		float _diffX;
		float _diffY;
		Vector3 _WallOffset    = new Vector3();
		Vector3 _WallWidthDir  = new Vector3();
		Vector3 _WallLengthDir = new Vector3();

		// Debug.Log (_lclListOfWalls.Count + " walls to create");
		for(int i = 0; i < _lclListOfWalls.Count; i++)
		{
			_curVector1 = _lclListOfWalls[i].WallStart;	
			_curVector2 = _lclListOfWalls[i].WallEnd;	
			
			_curVector1_3D = new Vector3(_curVector1.x,0,_curVector1.y);
			_curVector2_3D = new Vector3(_curVector2.x,0,_curVector2.y);
			
			_diffX = _curVector1.x - _curVector2.x;
			_diffY = _curVector1.y - _curVector2.y;
			_WallOffset = _curVector1_3D;
			_WallWidthDir = Vector3.up * 10.0f;
			_WallLengthDir = (_curVector2_3D - _curVector1_3D);
			CreateWall(_WallOffset, _WallWidthDir, _WallLengthDir, _Material_BrickWall);
			CreateOppositeWall(_WallOffset, _WallWidthDir, _WallLengthDir, _Material_BrickWall);
		}
		
	}
	
	static int[,] CreateDungeonSquares(int _sizeX, int _sizeZ, int _roomNbr)
	{
		int[,] _newMap = new int[_sizeX,_sizeZ];
		int    _roomSizeX = Random.Range (10,_sizeX/5);
		int    _roomSizeZ = Random.Range (10,_sizeZ/5);
		int    _roomPosX;
		int    _roomPosZ;
		for(int i = 0; i < _roomNbr; i++)
		{
			_roomPosX = Random.Range (1,_sizeX-_roomSizeX);
			_roomPosZ = Random.Range (1,_sizeZ-_roomSizeZ);
			for(int j = 0; j < _sizeX; j++)
			{
				for(int k = 0; k < _sizeZ; k++)
				{
					if(((j > _roomPosX) && (j < _roomPosX + _roomSizeX)) && ((k > _roomPosZ) && (k < _roomPosZ + _roomSizeZ)))
					{
						_newMap[j,k] = 1;
					}
							
				}	
			}
		}
		return _newMap;
	}
	
	static List<WallsToCreate> EvaluateWallToBuild(int[,] _map, int _sizeX, int _sizeZ)
	{
		List<WallsToCreate> _ListOfWalls = new List<WallsToCreate>();
		WallsToCreate _WallsToCreate = new WallsToCreate();
		
		int numConnectionsX = 0; // (-1) = [1 1 0], (0) = [0 1 0], (1) = [0, 1, 1], 2 = [1 1 1]
		int numConnectionsY = 0;
		int vectorToAdd     = 0;  //0 = AddStart, 1 = AddEnd
		
		bool _isWallInsideStartX  = false;
		bool _isWallInsideEndX    = false;
		bool _isWallOutsideStartX = false;
		bool _isWallOutsideEndX   = false;
		
		bool _isWallInsideStartY  = false;
		bool _isWallInsideEndY    = false;
		bool _isWallOutsideStartY = false;
		bool _isWallOutsideEndY   = false;
		
		// Change all room values to 1 THIS MIGHT BE CHANGED FOR FASTER RESULTS
		for(int j = 0; j < _sizeZ; j++)
		{
			for(int i = 0; i < _sizeX; i++)
			{
				if(_map[i,j] != 0)
				{
					_map[i,j] = 1;
				}
			}
		}
		
		// Find all horizontal walls
		for(int j = 1; j < _sizeZ - 1; j++) //Those conditions might be reevaluated
		{
			for(int i = 1; i < _sizeX - 1; i++) //Those conditions might be reevaluated
			{
				if(_map[i,j] == 1)
				{
					//Calculate numConnections
					numConnectionsX = CalculacteNumConnectionX(_map, i, j); // 0 = no X connections, 1 = 1 X connections, 2 = 2 X connections
					numConnectionsY = CalculacteNumConnectionY(_map, i, j); // 0 = no Y connections, 1 = 1 Y connections, 2 = 2 Y connections
					
					_isWallInsideStartX = (numConnectionsX ==  1 && (numConnectionsY == 1 || numConnectionsY == -1));
					_isWallInsideEndX   = (numConnectionsX == -1 && (numConnectionsY == 1 || numConnectionsY == -1));
					
					_isWallOutsideStartX = (numConnectionsX == 2 & numConnectionsY == 2) && ((_map[i + 1, j + 1] == 1 && _map[i + 1, j - 1 ] == 0) || (_map[i + 1,j + 1] == 0 && _map[i + 1,j - 1 ] == 1));
					_isWallOutsideEndX   = (numConnectionsX == 2 & numConnectionsY == 2) && ((_map[i - 1, j + 1] == 1 && _map[i - 1, j - 1 ] == 0) || (_map[i - 1,j + 1] == 0 && _map[i - 1,j - 1 ] == 1));
					
					// Identify start/end of horizontal walls
					if(_isWallInsideStartX || _isWallOutsideStartX) // Start/ending of a wall
					{
						//Debug.Log ("WalLX started");
						_WallsToCreate.WallStart = new Vector2(i,j);
					}
					else if(_isWallInsideEndX || _isWallOutsideEndX)
					{
						//Debug.Log ("WalLX ended");
						_WallsToCreate.WallEnd = new Vector2(i,j);
						_ListOfWalls.Add (_WallsToCreate);
						//Debug.Log ("Horizontal Wall : " + _WallsToCreate.WallStart + "To" + _WallsToCreate.WallEnd);
					
					}	
				}		
			}
		}
		
		if(vectorToAdd == 1)
		{
			vectorToAdd = 0; // Reinitialize vectorToAdd
			Debug.LogWarning ("DungeonGenerator : Wall started but not ended");
		}
		
		// Find all verticall walls
		for(int i = 1; i < _sizeX - 1 ; i++)
		{
			for(int j = 1; j < _sizeZ - 1; j++)
			{
				if(_map[i,j] != 0)
				{
					//Calculate numConnections
					numConnectionsX = CalculacteNumConnectionX(_map, i, j); // 0 = no X connections, 1 = 1 X connections, 2 = 2 X connections
					numConnectionsY = CalculacteNumConnectionY(_map, i, j); // 0 = no Y connections, 1 = 1 Y connections, 2 = 2 Y connections
					
					_isWallInsideStartY = (numConnectionsY ==  1 && (numConnectionsX == 1 || numConnectionsX == -1));
					_isWallInsideEndY   = (numConnectionsY == -1 && (numConnectionsX == 1 || numConnectionsX == -1));
					
					_isWallOutsideStartY = (numConnectionsX == 2 & numConnectionsY == 2) && ((_map[i - 1,j + 1] == 1 && _map[i + 1,j + 1 ] == 0) || (_map[i - 1,j + 1] == 0 && _map[i + 1,j + 1 ] == 1));
					_isWallOutsideEndY   = (numConnectionsX == 2 & numConnectionsY == 2) && ((_map[i + 1,j - 1] == 1 && _map[i - 1,j - 1 ] == 0) || (_map[i + 1,j - 1] == 0 && _map[i - 1,j - 1 ] == 1));
					
					// Identify start/end of vertical walls
					if(_isWallInsideStartY || _isWallOutsideStartY) // Start/ending of a wall
					{
						if(vectorToAdd == 0)
						{
							//Debug.Log ("WalLY started");
							_WallsToCreate.WallStart = new Vector2(i,j);
						}
					}
					else if(_isWallInsideEndY || _isWallOutsideEndY)
					{
						//Debug.Log ("WalLY ended");
						_WallsToCreate.WallEnd = new Vector2(i,j);
						_ListOfWalls.Add (_WallsToCreate);
						//Debug.Log ("Vertical Wall : " + _WallsToCreate.WallStart + "To" + _WallsToCreate.WallEnd);
					}	
				}		
			}
		}
		//Debug.Log (_ListOfWalls.Count + "Walls to create");
		return _ListOfWalls;
	
	}
	
	static int CalculacteNumConnectionX(int[,] _array, int _i, int _j)
	{
		int _numConnectionsX;
		
		_numConnectionsX = -(_array[_i - 1,_j    ]) + _array[_i + 1,_j    ]; // 0 = no X connections, 1 = 1 X connections, 2 = 2 X connections
		if(_numConnectionsX == 0 && _array[_i - 1,_j] == 1){_numConnectionsX = 2;}
		
		return _numConnectionsX;
	}
	
	static int CalculacteNumConnectionY(int[,] _array, int _i, int _j)
	{
		int _numConnectionsY;
		
		_numConnectionsY = -(_array[_i ,_j - 1 ]) + _array[_i    ,_j + 1]; // 0 = no Y connections, 1 = 1 Y connections, 2 = 2 Y connections
		if(_numConnectionsY == 0 && _array[_i,_j - 1] == 1){_numConnectionsY = 2;}
		
		return _numConnectionsY;
	}
	
	static void CreateRoom(float _sizeX,float _sizeY,float _sizeZ)
	{
		Vector3 _wallPosition = new Vector3();
		Vector3 _lengthDir    = new Vector3();
		Vector3 _widthDir     = new Vector3();
		
		
		// Right Wall
		_wallPosition = new Vector3(_sizeX,0,_sizeZ);
		_lengthDir    = _sizeZ*Vector3.back;
		_widthDir     = _sizeY*Vector3.up;
		CreateWall (_wallPosition, _lengthDir, _widthDir, _Material_BrickWall);
		
		// Left Wall
		_wallPosition = new Vector3(0,0,0);
		_lengthDir    = _sizeZ*Vector3.forward;
		_widthDir     = _sizeY*Vector3.up;
		CreateWall (_wallPosition, _lengthDir, _widthDir, _Material_BrickWall);
		
		// Back Wall
		_wallPosition = new Vector3(_sizeX,0,0);
		_lengthDir    = _sizeZ*Vector3.left;
		_widthDir     = _sizeY*Vector3.up;
		CreateWall (_wallPosition, _lengthDir, _widthDir, _Material_BrickWall);
		
		// Front Wall
		_wallPosition = new Vector3(0,0,_sizeZ);
		_lengthDir    = _sizeZ*Vector3.right;
		_widthDir     = _sizeY*Vector3.up;
		CreateWall (_wallPosition, _lengthDir, _widthDir, _Material_BrickWall);
	}
	
	static void CreateOppositeWall(Vector3 _offset, Vector3 _widthDir, Vector3 _lengthDir, Material _MaterialToSet)
	{
		//Debug.Log ("WdithDir : " + _widthDir);
		//Debug.Log ("_lengthDir : " + _lengthDir);
		CreateWall(_offset + _lengthDir, _widthDir, -_lengthDir, _MaterialToSet);	
	}
	
	static void CreateWall(Vector3 _offset, Vector3 _widthDir, Vector3 _lengthDir, Material _MaterialToSet)	
	{
		MeshBuilder _WallMeshBuilder = new MeshBuilder();
		Mesh        _WallMesh        = new Mesh();
		GameObject  _GO_Created      = new GameObject("ProcGenWall");
		
		//Add Mesh component to the created object
		_GO_Created.AddComponent<MeshFilter>();
		_GO_Created.AddComponent<MeshCollider>();
		_GO_Created.AddComponent<MeshRenderer>();
		_GO_Created.layer = LayerMask.NameToLayer ("Obstacles");
		
		MeshFilter   _MeshFilter   = _GO_Created.GetComponent<MeshFilter>();
		MeshCollider _MeshCollider = _GO_Created.GetComponent<MeshCollider>();
		MeshRenderer _MeshRenderer = _GO_Created.GetComponent<MeshRenderer>();
		
		// Create the wallMesh by using BuildQuad
		_WallMesh = ProceduralGen.BuildQuad(_WallMeshBuilder, _offset, _widthDir, _lengthDir); //Create a Mesh that contain a quad

		_MeshFilter.mesh          = _WallMesh;
		_MeshCollider.sharedMesh  = _WallMesh;
		_MeshRenderer.renderer.material = _MaterialToSet; // Apply the material
		AstarPath.active.UpdateGraphs(_GO_Created.collider.bounds);
	}
	
	
}


	/*static private int[,] CreateDungeonHalls(int[,] _mapWithRooms, int _sizeX,int _sizeZ,int nbrRoom)
	{
		int[,] _updatedDungeonMap = _mapWithRooms;
		int    _curRoomNbr = 0;
		int _x1;
		int _z1;
		int _x2;
		int _z2;
		int _dummyTemp;
		
		bool _isCorrdirVertical = true;
		bool _isCoord1Room;
		bool _isCoord2Room;
		
		while (_curRoomNbr < nbrRoom)
		{
			// Find a random Vertical/Horizontal wall 
			if(_isCorrdirVertical == true)
			{
				_isCorrdirVertical = false;
				_x1 = Random.Range (1, _sizeX-1);	
				_z1 = Random.Range (1, _sizeZ-1);		
				
				_x2 = _x1;
				_z2 = Random.Range (1, _sizeZ-1);
				if(Mathf.Min (_z1, _z2) == _z2)
				{
					_dummyTemp = _z1;
					_z1 = _z2;
					_z2 = _dummyTemp;
				}
			}
			else
			{
				_isCorrdirVertical = true;
				_x1 = Random.Range (1, _sizeX-1);	
				_z1 = Random.Range (1, _sizeZ-1);		
				
				_x2 = Random.Range (1, _sizeX-1);		
				_z2 = _z1;
				
				// Make sure _x1 is the smallest value
				if(Mathf.Min (_x1, _x2) == _x2)
				{
					_dummyTemp = _x1;
					_x1 = _x2;
					_x2 = _dummyTemp;
				}
				
			}
			
			// Test if both coordinate are in a room
			_isCoord1Room = (_mapWithRooms[_x1,_z1] == 1);
			_isCoord2Room = (_mapWithRooms[_x1,_z1] == 1);
			
			if (_isCoord1Room && _isCoord2Room)
			{
				_curRoomNbr++;
				Debug.Log ("Corridor Created");
				for(int i = _x1 - 1; i <= _x2 + 1; i++)
				{
					for(int j = _z1 - 1; j <= _z2 + 1; j++)
					{
						_updatedDungeonMap[i,j] = 1;
					}
					
				}
				
			}
			
			
		}
		return _updatedDungeonMap;
	}
	*/
