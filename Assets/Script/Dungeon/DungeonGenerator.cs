using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List class;

static class DungeonGenerator {

	static Material _Material_BrickWall;
	static TextureManager _TextureManager;
	static List<WallsToCreate> _ListOfWallsToCreate = new List<WallsToCreate>();
	
	private struct WallsToCreate
    {
        public Vector2 WallStart;
        public Vector2 WallEnd;
    };	
	
	static public int[,] SpawnDungeon(int _SizeX,int _SizeZ, int nbrRoom)
	{
		_TextureManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<TextureManager>();
		_Material_BrickWall = _TextureManager.Material_Dungeon_BrickWall;
		
		int[,] _dungeonMap;
		
		_dungeonMap = CreateDungeonMap(_SizeX,_SizeZ, nbrRoom);
		_ListOfWallsToCreate = EvaluateWallToBuild(_dungeonMap, _SizeX, _SizeZ);
		CreateDungeonWalls(_ListOfWallsToCreate);
		
		return _dungeonMap;
	}
	
	static void FindRooms(int[,] _originalMap,int _sizeX, int _sizeZ)
	{
		List<int[]> ListCoordRoom = new List<int[]>();
		
		int[,] _modifiedMap = _originalMap;
		int[,] _roomMap = new int[_sizeX, _sizeZ];
		int[] _curIndex;
		//int nbrCoordRoomLeft = 0;
		
		for(int i = 1; i < _sizeX; i++)
		{
			for(int j = 1; j < _sizeZ; j++)
			{
				// If we find a room
				if(_modifiedMap[i,j] == 1)
				{
					ListCoordRoom.Add(new int[2] { i, j});
					
					//curNbrPosAround = CountRoomPosAround(_modifiedMap, i, j);
					
					while(ListCoordRoom.Count > 0)
					{
						int _x = ListCoordRoom[0][0];
						int _y = ListCoordRoom[0][1];
						ListCoordRoom.RemoveAt (0);
						
						_modifiedMap[_x,_y] = 0; // Remove the position from the room in the modified Map
						_roomMap[_x,_y]     = 1;
						for(int _xAround = _x - 1; _xAround < _x + 1; _xAround++)
						{
							for(int _yAround = _y - 1 ; _yAround < _y + 1; _yAround++)
							{
								_curIndex = new int[2] { _xAround, _yAround};
								//ListCoordRoom.Add(_curIndex);
								//Debug.Log ("Room Found");
							}
						}
					}
				}
			}
		}
	}
	
	static void CreateDungeonWalls(List<WallsToCreate> _lclListOfWalls) ///TODO: Fix notation
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

		
		for(int i = 0; i < _lclListOfWalls.Count; i++)
		{
			_curVector1 = _lclListOfWalls[i].WallStart;	
			_curVector2 = _lclListOfWalls[i].WallEnd;	
			
			_curVector1_3D = new Vector3(_curVector1.x,0,_curVector1.y);
			_curVector2_3D = new Vector3(_curVector2.x,0,_curVector2.y);
			
			_diffX = _curVector1.x - _curVector2.x;
			_diffY = _curVector1.y - _curVector2.y;
			
			if(Mathf.Max(Mathf.Abs (_diffX), Mathf.Abs (_diffY)) == _diffX) // if _diffX has the biggest difference, Horizontal wall
			{
				
			}
			_WallOffset = _curVector1_3D;
			_WallWidthDir = Vector3.up * 10.0f;
			_WallLengthDir = (_curVector2_3D - _curVector1_3D);
			CreateWall(_WallOffset, _WallWidthDir, _WallLengthDir, _Material_BrickWall);
			CreateOppositeWall(_WallOffset, _WallWidthDir, _WallLengthDir, _Material_BrickWall);
		}
		
	}
	
	static int[,] CreateDungeonMap(int _sizeX, int _sizeZ, int _roomNbr)
	{
		int[,] _newMap = new int[_sizeX,_sizeZ];
		int    _roomSizeX = Random.Range (10,_sizeX/4);
		int    _roomSizeZ = Random.Range (10,_sizeZ/4);
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
		
		// Find all horizontal walls
		for(int j = 1; j < _sizeZ; j++)
		{
			for(int i = 1; i < _sizeX; i++)
			{
				if(_map[i,j] == 1)
				{
					//Calculate numConnections
					numConnectionsX = CalculacteNumConnectionX(_map, i, j); // 0 = no X connections, 1 = 1 X connections, 2 = 2 X connections
					numConnectionsY = CalculacteNumConnectionY(_map, i, j); // 0 = no Y connections, 1 = 1 Y connections, 2 = 2 Y connections
					
					_isWallInsideStartX = (numConnectionsX ==  1 && (numConnectionsY == 1 || numConnectionsY == -1));
					_isWallInsideEndX   = (numConnectionsX == -1 && (numConnectionsY == 1 || numConnectionsY == -1));
					
					_isWallOutsideStartX = (numConnectionsX == 2 & numConnectionsY == 2) && ((_map[i + 1,j + 1] == 1 && _map[i + 1,j - 1 ] == 0) || (_map[i + 1,j + 1] == 0 && _map[i + 1,j - 1 ] == 1));
					_isWallOutsideEndX   = (numConnectionsX == 2 & numConnectionsY == 2) && ((_map[i - 1,j + 1] == 1 && _map[i - 1,j - 1 ] == 0) || (_map[i - 1,j + 1] == 0 && _map[i - 1,j - 1 ] == 1));
					
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
		for(int i = 1; i < _sizeX; i++)
		{
			for(int j = 1; j < _sizeZ; j++)
			{
				if(_map[i,j] == 1)
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
		
		
		MeshFilter   _MeshFilter   = _GO_Created.GetComponent<MeshFilter>();
		MeshCollider _MeshCollider = _GO_Created.GetComponent<MeshCollider>();
		MeshRenderer _MeshRenderer = _GO_Created.GetComponent<MeshRenderer>();
		
		// Create the wallMesh by using BuildQuad
		_WallMesh = ProceduralGen.BuildQuad(_WallMeshBuilder, _offset, _widthDir, _lengthDir); //Create a Mesh that contain a quad

		_MeshFilter.mesh          = _WallMesh;
		_MeshCollider.sharedMesh  = _WallMesh;
		_MeshRenderer.renderer.material = _MaterialToSet; // Apply the material
	}
}
