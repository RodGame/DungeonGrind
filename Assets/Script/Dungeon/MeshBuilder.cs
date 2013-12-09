using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshBuilder
{
	private List<Vector3> _Vertices = new List<Vector3>();
	private List<Vector3> _Normals  = new List<Vector3>();
	private List<Vector2> _UVs      = new List<Vector2>();
	private List<int>     _Indices  = new List<int>();	
	private List<Color32> _Colors   = new List<Color32>();
	
	public List<Vector3> Vertices { get { return _Vertices; } }
	public List<Vector2> UVs { get { return _UVs; } }
	public List<Vector3> Normals { get { return _Normals; } }
	public List<Color32> Colors { get { return _Colors; } set { _Colors = value;} }

	public void AddTriangle(int index0, int index1, int index2)
	{
		_Indices.Add(index0);
		_Indices.Add(index1);
		_Indices.Add(index2);
	}

	public Mesh CreateMesh()
	{
		Mesh mesh = new Mesh();
		mesh.name = "planeMesh";
		mesh.vertices  = _Vertices.ToArray();
		mesh.triangles = _Indices.ToArray();
		
		
		//Normals are optional. Only use them if we have the correct amount:
		if (_Normals.Count == _Vertices.Count)
			mesh.normals = _Normals.ToArray();

		//UVs are optional. Only use them if we have the correct amount:
		if (_UVs.Count == _Vertices.Count)
			mesh.uv = _UVs.ToArray();
		
		//Colors are optional. Only use them if we have the correct amount:
		if (_Colors.Count == _Vertices.Count)
			mesh.colors32  = _Colors.ToArray ();
		
		mesh.RecalculateBounds(); // This might be unnecessary because "Assigning triangles will automatically Recalculate the bounding volume." from Unity3D docs

		return mesh;
	}
}