using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List class;

static class DungeonGenerator {
	
	static PrefabManager _PrefabManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<PrefabManager>();
	static TextureManager _TextureManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<TextureManager>();
	static Material _Material_BrickWall = _TextureManager.Material_Dungeon_BrickWall;
	static List<WallsToCreate> _ListOfWallsToCreate = new List<WallsToCreate>();
	static int _nbrRoom;
	
	// Number of room found in the created dungeon
	static private int NbrRoom
	{
		get {return _nbrRoom; }
		set {_nbrRoom = value; }
	}
	
	// Structure that contain the position of the start/end of a wall to be created
	private struct WallsToCreate
    {
        public Vector2 WallStart;
        public Vector2 WallEnd;
    };	
	
	// Randomize configuration of dungeon and instantiate it
	static public int[,] SpawnDungeon(int _sizeX,int _sizeZ, int _nbrSquareForGeneration)
	{	
		int[,] _dungeonMap; // 2D Array of int that will hold the dungeon map information
		
		_dungeonMap = CreateDungeonSquares(_sizeX,_sizeZ, _nbrSquareForGeneration); // Fill the array with different squares of various size
		_nbrRoom = FindRooms (_dungeonMap,_sizeX, _sizeZ); // Loop through the dungeon map and find rooms created by touching squares
		if(_nbrRoom > 1)
		{
			_dungeonMap = CreateDungeonHalls(_dungeonMap, _sizeX,_sizeZ,_nbrRoom);  // Add halls between the created room
		}
		
		SpawnEnvironment(_dungeonMap, _sizeX, _sizeZ, _nbrRoom); // Create environment(Spider eggs for now)
		
		_ListOfWallsToCreate = EvaluateWallToBuild(_dungeonMap, _sizeX, _sizeZ); // Loop through the dungeon map to find all the wall that need to be created
		InstantiateDungeonWalls(_ListOfWallsToCreate); // Instantiate all the wall found by the EvaluateWallToBuild function
		
		return _dungeonMap;
	}
	
	// Create the inputted number of square on the map
	static int[,] CreateDungeonSquares(int _sizeX, int _sizeZ, int _squareNbr)
	{
		int[,] _newMap = new int[_sizeX,_sizeZ]; // Create empty map with imputted dimensions
		
		for(int i = 0; i < _squareNbr; i++)
		{
			// Calculate random room size and position. Make sure the room is inside the map.
			int _roomSizeX = Random.Range (10,_sizeX/5);
			int _roomSizeZ = Random.Range (10,_sizeZ/5);
			int _roomPosX  = Random.Range (2,_sizeX-_roomSizeX - 1);
			int _roomPosZ  = Random.Range (2,_sizeZ-_roomSizeZ - 1);
			
			// Add the square to the map. Avoid making square on the external values of the map.
			for(int j = _roomPosX; j < _roomPosX + _roomSizeX; j++)
			{
				for(int k = _roomPosZ; k < _roomPosZ + _roomSizeZ; k++)
				{
					_newMap[j,k] = 1;
				}	
			}
		}
		return _newMap;
	}
	
	// Loop through the dungeon map and find rooms created by touching squares
	static int FindRooms(int[,] _dungeonMap, int _sizeX, int _sizeZ)
	{
		
		List<Vector2> ListCoordToTest = new List<Vector2>(); // List of all coordinate that need to be evaluated for the current room.
		int[,] _modifiedMap  = new int[_sizeX,_sizeZ]; // This array will be a copy of _modifiedMap where all known room cell are set as 0 to be ignored by the next steps
		int    _nbrRoomFound = 0;
		
		System.Array.Copy(_dungeonMap,_modifiedMap, _sizeX*_sizeZ); // Create a copy of the _dungeonMap in _modifiedMap
		
		//Loop through all position of the map
		for(int i = 0; i < _sizeX; i++)
		{
			for(int j = 0; j < _sizeZ; j++)
			{
				// If a room is found
				if(_modifiedMap[i,j] == 1)
				{
					ListCoordToTest.Add(new Vector2(i, j)); // Add the coordinate to the list to test
					
					while(ListCoordToTest.Count > 0) // Loop while there are coordinates to test
					{	
						int _x = (int)ListCoordToTest[0].x; // Find x value of coordinate to test
						int _z = (int)ListCoordToTest[0].y; // Find y value of coordinate to test
						ListCoordToTest.RemoveAt (0); // Remove the currently tested coordinate
						
						_dungeonMap[_x,_z] = _nbrRoomFound + 1; // Update the _dungeonMap with the current number of room found
						
						// Look the 8 coordinate around the current coordinate to find for connected rooms
						for(int _xAround = _x - 1; _xAround <= _x + 1; _xAround++)
						{
							for(int _zAround = _z - 1 ; _zAround <= _z + 1; _zAround++)
							{
								// Test if evaluated coordinate is a square and need to be added to the room
								if(_modifiedMap[_xAround,_zAround] == 1)
								{
									ListCoordToTest.Add(new Vector2(_xAround, _zAround)); // Add it to the list of coordinates to test
									_modifiedMap[_xAround,_zAround] = 0; // Remove the room position from the modified map so we don't step on it again
								}
							}
						}
					}
					_nbrRoomFound++;
				}
			}
		}
		return _nbrRoomFound;
	}
	
// Create a hall starting from each room, connecting to another room or corridor
static private int[,] CreateDungeonHalls(int[,] _dungeonMap, int _sizeX,int _sizeZ, int nbrRoom)
{
	int _x1; // x coordinate of the starting position
	int _z1; // z coordinate of the starting position
	int _x2; // x coordinate of the ending position
	int _z2; // z coordinate of the ending position
		
	
	// Start a corridor from each room
	for(int _curRoomNbr = 1; _curRoomNbr <= nbrRoom; _curRoomNbr++)
	{
		int nbrRoomsTry = 0; // Counter is used to avoid looping forever if there is a bug in the program.
		int _nbrRoomsTryMax = 5000;
		
		// Find a random coordinate in the room with number _curRoomNbr
		_x1 = Random.Range (1, _sizeX-1);	
		_z1 = Random.Range (1, _sizeZ-1);
		
		// Find a random position with the current room number
		while(_dungeonMap[_x1,_z1] != _curRoomNbr && nbrRoomsTry < _nbrRoomsTryMax)
		{
			_x1 = Random.Range (1, _sizeX-1);	
			_z1 = Random.Range (1, _sizeZ-1);	
			nbrRoomsTry++;
		}
		nbrRoomsTry = 0;
		
		// Find a random coordinate in any room/corridor that isn't the first room
		_x2 = Random.Range (1, _sizeX-1);
		_z2 = Random.Range (1, _sizeZ-1);
		
		// Find a random position in a different room or corridor created from a previous room
		while((_dungeonMap[_x2,_z2] == 0 || _dungeonMap[_x2,_z2] == _curRoomNbr) && nbrRoomsTry < _nbrRoomsTryMax)
		{
			_x2 = Random.Range (1, _sizeX-1);	
			_z2 = Random.Range (1, _sizeZ-1);
			nbrRoomsTry++;	
		}
		
		// Difference between both coordinates on each axis. This is used to find direction of corridor to create
		int _diffX = _x2 - _x1;
		int _diffZ = _z2 - _z1;
		
		int _xDirection = 1; // Coefficient used to loop in the right direction on x axis(-1 or 1)
		int _zDirection = 1; // Coefficient used to loop in the right direction on z axis(-1 or 1)
		
		// Evaluate direction of the corridor on the X and Z axis
		if(_diffX != 0)
		{
			_xDirection = _diffX/Mathf.Abs(_diffX); // 1 = Left to Right (->) ... -1 =  Right to Left (<-)
		}
		else
		{
			_xDirection = 0; // No horizontal corridor if they are aligned on X axis
		}
		
		if(_diffZ != 0)
		{
			_zDirection = _diffZ/Mathf.Abs(_diffZ); // 1 = Top to Bottom( \/ ) ... -1 = Bottom to Top ( /\ )
		}
		else
		{
			_zDirection = 0; // No vertical corridor if they are aligned on Z axis
		}
		
		// randomize corridor portion and height
		int _hallWidth  = Random.Range (4,10);
		int _hallHeight = Random.Range (4,10);
		
		//Create vertical part of the hall
		for(int i = _x1; i != _x1 + _xDirection*_hallWidth; i += _xDirection)
		{
			for(int j = _z1; j != _z2; j += _zDirection)
			{
				if(i >= 0 && i < _sizeX && j >= 0 && j < +_sizeZ) // Make sure that the index is within the map range
				{
					if(_dungeonMap[i,j] == 0) // Only modify empty position, not rooms position
					{
						_dungeonMap[i,j] = -1; // Write corridor as -1 in the dungeonMap
					}
				}
			}
		}
	
		//Create horizontal portion of the hall
		for(int i = _x1; i != _x2; i += _xDirection)
		{
			for(int j = _z2; j != _z2 + _zDirection*_hallHeight; j += _zDirection)
			{
				if(i >= 0 && i < _sizeX && j >= 0 && j < +_sizeZ) // Make sure that the index is within the map range
				{
					if(_dungeonMap[i,j] == 0) // Only modify empty position, not rooms position
					{
						_dungeonMap[i,j] = -1; // Write corridor as -1 in the dungeonMap
					}
				}
			}
		}
	}	
	return _dungeonMap; // The _dungeonMap contains 0 for non-room, -1 for corridor and N for rooms
}
	
	// Loop through the dungeon map to find all the wall that need to be created
	static List<WallsToCreate> EvaluateWallToBuild(int[,] _map, int _sizeX, int _sizeZ)
	{
		List<WallsToCreate> _ListOfWalls = new List<WallsToCreate>();
		WallsToCreate _WallsToCreate = new WallsToCreate();
		
		int _numConnectionsX = 0; // (-1) = [1 1 0], (0) = [0 1 0], (1) = [0, 1, 1], 2 = [1 1 1]
		int _numConnectionsZ = 0;
		int vectorToAdd     = 0;  //0 = AddStart, 1 = AddEnd
		
		bool _isWallInsideStartX  = false;
		bool _isWallInsideEndX    = false;
		bool _isWallOutsideStartX = false;
		bool _isWallOutsideEndX   = false;
		
		bool _isWallInsideStartZ  = false;
		bool _isWallInsideEndZ    = false;
		bool _isWallOutsideStartZ = false;
		bool _isWallOutsideEndZ   = false;
		
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
		for(int j = 0; j < _sizeZ; j++) //Those conditions might be reevaluated
		{
			for(int i = 0; i < _sizeX; i++) //Those conditions might be reevaluated
			{
				if(_map[i,j] == 1)
				{
					//Calculate numConnections
					_numConnectionsX = CalculacteNumConnectionX(_map, _sizeX, i, j); // -1 = Left connection only, 0 = no X connections, 1 = Right  connection only, 2 = 2 X connections
					_numConnectionsZ = CalculacteNumConnectionZ(_map, _sizeZ, i, j); // -1 = Top  connection only, 0 = no Y connections, 1 = Bottom connection only, 2 = 2 Y connections
					
					_isWallInsideStartX = (_numConnectionsX ==  1 && (_numConnectionsZ == 1 || _numConnectionsZ == -1));
					_isWallInsideEndX   = (_numConnectionsX == -1 && (_numConnectionsZ == 1 || _numConnectionsZ == -1));
					
					_isWallOutsideStartX = (_numConnectionsX == 2 & _numConnectionsZ == 2) && ((_map[i + 1, j + 1] == 1 && _map[i + 1, j - 1 ] == 0) || (_map[i + 1,j + 1] == 0 && _map[i + 1,j - 1 ] == 1));
					_isWallOutsideEndX   = (_numConnectionsX == 2 & _numConnectionsZ == 2) && ((_map[i - 1, j + 1] == 1 && _map[i - 1, j - 1 ] == 0) || (_map[i - 1,j + 1] == 0 && _map[i - 1,j - 1 ] == 1));
					
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
					_numConnectionsX = CalculacteNumConnectionX(_map, _sizeX, i, j); // -1 = Left connection only, 0 = no X connections, 1 = Right  connection only, 2 = 2 X connections
					_numConnectionsZ = CalculacteNumConnectionZ(_map, _sizeZ, i, j); // -1 = Top  connection only, 0 = no Y connections, 1 = Bottom connection only, 2 = 2 Y connections
					
					_isWallInsideStartZ = (_numConnectionsZ ==  1 && (_numConnectionsX == 1 || _numConnectionsX == -1));
					_isWallInsideEndZ   = (_numConnectionsZ == -1 && (_numConnectionsX == 1 || _numConnectionsX == -1));
					
					_isWallOutsideStartZ = (_numConnectionsZ == 2 & _numConnectionsX == 2) && ((_map[i + 1, j + 1] == 1 && _map[i - 1, j + 1 ] == 0) || (_map[i + 1,j + 1] == 0 && _map[i - 1,j + 1 ] == 1));
					_isWallOutsideEndZ   = (_numConnectionsZ == 2 & _numConnectionsX == 2) && ((_map[i + 1, j - 1] == 1 && _map[i - 1, j - 1 ] == 0) || (_map[i + 1,j - 1] == 0 && _map[i - 1,j - 1 ] == 1));
					
					// Identify start/end of vertical walls
					if(_isWallInsideStartZ || _isWallOutsideStartZ) // Start/ending of a wall
					{
						if(vectorToAdd == 0)
						{
							//Debug.Log ("WalLY started");
							_WallsToCreate.WallStart = new Vector2(i,j);
						}
					}
					else if(_isWallInsideEndZ || _isWallOutsideEndZ)
					{
						//Debug.Log ("WalLY ended");
						_WallsToCreate.WallEnd = new Vector2(i,j);
						if(_WallsToCreate.WallStart.x == _WallsToCreate.WallEnd.x || _WallsToCreate.WallStart.y == _WallsToCreate.WallEnd.y)
						{
							_ListOfWalls.Add (_WallsToCreate);
						}
						else
						{
							CancelDungeonCreation();
						}
						//Debug.Log ("Vertical Wall : " + _WallsToCreate.WallStart + "To" + _WallsToCreate.WallEnd);
					}	
				}		
			}
		}
		//Debug.Log (_ListOfWalls.Count + "Walls to create");
		return _ListOfWalls;
	
	}
	
	static void CancelDungeonCreation()
	{
		Debug.Log ("CancelDungeon");
		Application.LoadLevel("Dungeon");
	}
	
	// Instantiate all the wall found by the EvaluateWallToBuild function
	static void InstantiateDungeonWalls(List<WallsToCreate> _lclListOfWalls) ///TODO: Fix notation
	{
		
		Vector2 _curVector1;
		Vector2 _curVector2;
		Vector3 _curVector1_3D;
		Vector3 _curVector2_3D;
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
			_WallOffset = _curVector1_3D;
			_WallWidthDir = Vector3.up * 10.0f;
			_WallLengthDir = (_curVector2_3D - _curVector1_3D);
			CreateWall(_WallOffset, _WallWidthDir, _WallLengthDir, _Material_BrickWall);
			CreateOppositeWall(_WallOffset, _WallWidthDir, _WallLengthDir, _Material_BrickWall);
		}
		
	}
	
	// Create environment(Spider eggs for now)
	static private void SpawnEnvironment(int [,]_dungeonMap, int _sizeX, int _sizeZ, int _nbrRoomCreated)
	{
		
		for(int i = 0; i < Mathf.RoundToInt(0.75f*_nbrRoomCreated); i++)
		{
			Vector2 _posXZ = GameObject.FindGameObjectWithTag("DungeonMaster").GetComponent<DungeonManager>().FindRandomRoomPosition(_dungeonMap, _sizeX, _sizeZ, 1.0f, 0.0f); 
			Vector3 _newPos = new Vector3(_posXZ.x, 0.75f, _posXZ.y);
			GameObject _GO_Created = GameObject.Instantiate(_PrefabManager.Environment_SpiderEggs, _newPos, Quaternion.identity) as GameObject;
			_GO_Created.transform.localScale *= 1.25f;
			_GO_Created.layer = _GO_Created.layer = LayerMask.NameToLayer ("Obstacles"); 
		 	AstarPath.active.UpdateGraphs(_GO_Created.collider.bounds);
		}
	}
	
	// Calculate the number of connections on the X axis for a particular map index
	static int CalculacteNumConnectionX(int[,] _array, int _sizeX, int _i, int _j)
	{
		int __numConnectionsX; // Output of the function, number of connections
		int _connectionLeft;  // (0/1) Connection left
		int _connectionRight; // (0/1) Connection right
		
		if(_i <= 0)
		{
			_connectionLeft = 0;
			_connectionRight = _array[_i + 1, _j]; // Value at the right of the index
		}
		else if(_i > _sizeX-1)
		{
			_connectionLeft  = _array[_sizeX - 1, _j]; // Value at the left of the index
			_connectionRight = 0;	
		}
		else
		{
			_connectionLeft  = _array[_i - 1, _j]; // Value at the left of the index
			_connectionRight = _array[_i + 1, _j]; // Value at the right of the index
		}
		
		__numConnectionsX = -_connectionLeft + _connectionRight; // -1 = Only left, 0 = no connections or both way, 1 = Only right
		if(__numConnectionsX == 0 && _connectionRight == 1){__numConnectionsX = 2;} // if __numConnectionsX == 0 and right value == 1, it means both side are 1 hence, __numConnectionsX = 2
		
		return __numConnectionsX;
	}
	
	// Calculate the number of connections on the Y axis for a particular map index
	static int CalculacteNumConnectionZ(int[,] _array, int _sizeZ, int _i, int _j)
	{
		int __numConnectionsZ; // Output of the function, number of connections
		int _connectionTop;   // (0/1) Connection top
		int _connectionDown;  // (0/1) Connection bottom
		
		if(_j <= 0)
		{
			_connectionTop = 0;
			_connectionDown = _array[_i, _j + 1]; // Value under the index
		}
		else if(_j >= _sizeZ-1)
		{
			_connectionTop  = _array[_i, _sizeZ-1]; // Value above the index
			_connectionDown = 0;	
		}
		else
		{
			_connectionTop  = _array[_i, _j - 1]; // Value above the index
			_connectionDown = _array[_i, _j + 1]; // Value under the index
			//Debug.Log ("SizeZ : " + _sizeZ + ", j :" + _j);
		}
		
		__numConnectionsZ = -_connectionTop + _connectionDown; // -1 = Only top, 0 = no connections or both way, 1 = Only down
		if(__numConnectionsZ == 0 && _connectionDown == 1){__numConnectionsZ = 2;} // if __numConnectionsZ == 0 and down value == 1, it means both side are 1 hence, __numConnectionsZ = 2
		
		return __numConnectionsZ;
	}
	
	
	
	
	/*static int CalculacteNumConnectionZ(int[,] _array, int _sizeZ, int _i, int _j)
	{
		int __numConnectionsZ;
		
		__numConnectionsZ = -(_array[_i ,_j - 1 ]) + _array[_i    ,_j + 1]; // 0 = no Y connections, 1 = 1 Y connections, 2 = 2 Y connections
		if(__numConnectionsZ == 0 && _array[_i,_j - 1] == 1){__numConnectionsZ = 2;}
		
		return __numConnectionsZ;
	}*/
	
	// Instantiate a wall
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
		_GO_Created.tag   = "Wall";
		
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
	
	// Instantiate the mirror of the create wall
	static void CreateOppositeWall(Vector3 _offset, Vector3 _widthDir, Vector3 _lengthDir, Material _MaterialToSet)
	{
		CreateWall(_offset + _lengthDir, _widthDir, -_lengthDir, _MaterialToSet);	
	}
	
	
	
	
}