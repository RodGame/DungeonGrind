using UnityEngine;
using System.Collections;

static class ProceduralGen {
	
	//float _shapeLength = 1.0f;
	//float _shapeWidth  = 1.0f;
	
	static MeshBuilder TestMesh = new MeshBuilder();
	static TextureManager _TextureManager;
	
	//BuildQuad with vectors
	static public Mesh BuildQuad(MeshBuilder meshBuilder, Vector3 offset, Vector3 widthDir, Vector3 lengthDir)
	{
		Vector3 normal = Vector3.Cross(lengthDir, widthDir).normalized;
	
		meshBuilder.Vertices.Add(offset);
		meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
		meshBuilder.Normals.Add(normal);
	
		meshBuilder.Vertices.Add(offset + lengthDir);
		meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
		meshBuilder.Normals.Add(normal);
	
		meshBuilder.Vertices.Add(offset + lengthDir + widthDir);
		meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
		meshBuilder.Normals.Add(normal);
	
		meshBuilder.Vertices.Add(offset + widthDir);
		meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
		meshBuilder.Normals.Add(normal);
	
		int baseIndex = meshBuilder.Vertices.Count - 4;
	
		meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
		meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
		
		Mesh _CreatedMesh = meshBuilder.CreateMesh();
		_CreatedMesh.RecalculateNormals();
		return _CreatedMesh;
	}
	
	//BuildQuad from float Length
	/*static public void BuildQuad(MeshBuilder meshBuilder, Vector3 offset, float _shapeLength, float _shapeWidth)
	{
		meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, 0.0f));
		meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
		meshBuilder.Normals.Add(Vector3.up);
	
		meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, _shapeLength));
		meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
		meshBuilder.Normals.Add(Vector3.up);
	
		meshBuilder.Vertices.Add(new Vector3(_shapeWidth, 0.0f, _shapeLength));
		meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
		meshBuilder.Normals.Add(Vector3.up);
	
		meshBuilder.Vertices.Add(new Vector3(_shapeWidth, 0.0f, 0.0f));
		meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
		meshBuilder.Normals.Add(Vector3.up);
	
		int baseIndex = meshBuilder.Vertices.Count - 4;
	
		meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
		meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
		
		//Create the mesh:
		MeshFilter filter = GetComponent<MeshFilter>();
		
		if (filter != null)
		{
			filter.sharedMesh = meshBuilder.CreateMesh();
		}
		
		
		Debug.Log ("Vertices : " + meshBuilder.Vertices.Count);
		Debug.Log ("UVs : " + meshBuilder.UVs.Count);
		Debug.Log ("Normals : " + meshBuilder.Normals.Count);
		
	}*/
	
	/*static void BuildCube(MeshBuilder meshBuilder, float _shapeLength, float _shapeWidth, float _shapeHeight, Material _MaterialToSet)
	{
		Vector3 upDir = Vector3.up * _shapeHeight;
		Vector3 rightDir = Vector3.right * _shapeWidth;
		Vector3 forwardDir = Vector3.forward * _shapeLength;
		
		Vector3 nearCorner = Vector3.zero;
		Vector3 farCorner = upDir + rightDir + forwardDir;
		
		BuildQuad(meshBuilder, nearCorner, forwardDir, rightDir);
		BuildQuad(meshBuilder, nearCorner, rightDir, upDir);
		BuildQuad(meshBuilder, nearCorner, upDir, forwardDir);
		
		BuildQuad(meshBuilder, farCorner, -rightDir, -forwardDir);
		BuildQuad(meshBuilder, farCorner, -upDir, -rightDir);
		BuildQuad(meshBuilder, farCorner, -forwardDir, -upDir);
		
		Mesh mesh = meshBuilder.CreateMesh();
		
		//Create the mesh:
		MeshFilter filter = GetComponent<MeshFilter>();
		MeshCollider collider = GetComponent<MeshCollider>();
		Mesh _CreatedMesh = meshBuilder.CreateMesh();
		
		renderer.material = _MaterialToSet;
		collider.sharedMesh = _CreatedMesh;
		if (filter != null)
		{
			filter.sharedMesh = _CreatedMesh;
		}
	}*/
	
	/*static void BuildCube(MeshBuilder meshBuilder, float _shapeLength, float _shapeWidth, float _shapeHeight)
	{
		Vector3 upDir = Vector3.up * _shapeHeight;
		Vector3 rightDir = Vector3.right * _shapeWidth;
		Vector3 forwardDir = Vector3.forward * _shapeLength;
		
		Vector3 nearCorner = Vector3.zero;
		Vector3 farCorner = upDir + rightDir + forwardDir;
		
		BuildQuad(meshBuilder, nearCorner, forwardDir, rightDir);
		BuildQuad(meshBuilder, nearCorner, rightDir, upDir);
		BuildQuad(meshBuilder, nearCorner, upDir, forwardDir);
		
		BuildQuad(meshBuilder, farCorner, -rightDir, -forwardDir);
		BuildQuad(meshBuilder, farCorner, -upDir, -rightDir);
		BuildQuad(meshBuilder, farCorner, -forwardDir, -upDir);
		
		Mesh mesh = meshBuilder.CreateMesh();
		mesh.colors = new Color[] { new Color(0,0,0), 
                            new Color(0,0,0), 
                            new Color(0,0,0) };
		//Create the mesh:
		MeshFilter filter = GetComponent<MeshFilter>();
		if (filter != null)
		{
			filter.sharedMesh = meshBuilder.CreateMesh();
		}
	}*/
}
