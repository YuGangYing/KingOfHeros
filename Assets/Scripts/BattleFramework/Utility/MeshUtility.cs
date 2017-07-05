using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/**
 * create by yingyugang (232871714@qq.com) 
 * 2014.12.01
 **/

public enum MeshPivod{TOP,CENTER,BOTTOM};
public static class MeshUtility {

	static GameObject GetMeshGameObject(Material meshMaterial,Color color)
	{
		GameObject go = new GameObject ();
		MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
		if(meshMaterial==null)
		{
			Shader meshShader = Shader.Find("Diffuse");
			meshMaterial = new Material(meshShader);
		}
		meshMaterial.color = color;
		meshRenderer.material = meshMaterial;
		meshRenderer.sharedMaterial = meshMaterial;
		go.AddComponent<MeshFilter>();
		return go;
	}

	public static GameObject DrawSquareGameObject(Material meshMaterial,Color color,float edgeSize)
	{
		GameObject go = GetMeshGameObject (meshMaterial,color);
		Mesh mesh = new Mesh ();
		Vector3[] vertics = new Vector3[4];
		Vector2[] uvs = new Vector2[4];
		vertics [0] = new Vector3 (edgeSize/2,0,edgeSize/2);
		uvs [0] = new Vector2 (1,1);
		vertics [1] = new Vector3 (edgeSize/2,0,-edgeSize/2);
		uvs [1] = new Vector2 (1,0);
		vertics [2] = new Vector3 (-edgeSize/2,0,-edgeSize/2);
		uvs [2] = new Vector2 (0,0);
		vertics [3] = new Vector3 (-edgeSize/2,0,edgeSize/2);
		uvs [3] = new Vector2 (0,1);
		mesh.vertices = vertics;
		mesh.triangles = new int[]{0,1,2,2,3,0};
		mesh.uv = uvs;
		mesh.RecalculateNormals ();
		go.GetComponent<MeshFilter> ().mesh = mesh;
		return go;
	}

	public static GameObject DrawSectorGameObject(Material meshMaterial,Color color,float angle,float radiusLong,float radiusShort,float height)
	{
		radiusLong = Mathf.Abs (radiusLong);
		radiusShort = Mathf.Abs (radiusShort);
		if (radiusLong < radiusShort) {
			float temp = radiusShort;
			radiusShort = radiusLong;
			radiusLong = temp;
		}
		GameObject go = GetMeshGameObject (meshMaterial,color);
		return go;
	}


	public static GameObject DrawSectorGameObject(Material meshMaterial,Color color,float angle,float radiusLong,float radiusShort,bool isTwoSide)
	{
		radiusLong = Mathf.Abs (radiusLong);
		radiusShort = Mathf.Abs (radiusShort);
		if (radiusLong < radiusShort) {
			float temp = radiusShort;
			radiusShort = radiusLong;
			radiusLong = temp;
		}
		GameObject go = GetMeshGameObject (meshMaterial,color);
		Mesh mesh = DrawSectorMesh(radiusLong,radiusShort,angle,10,isTwoSide);
		go.GetComponent<MeshFilter>().mesh = mesh;
		return go;
	} 

	static List<Vector3> GetSectorPosList(float radius,float height,float angle)
	{
		Matrix4x4 matrix = GetYAxisMatrix (1);
		Vector3 pos = new Vector3 (radius,height,0);
		List<Vector3> verticsTmp = new List<Vector3> ();
		for(int i =0;i < (angle + 1) / 1;i++)
		{
			verticsTmp.Add(pos);
			pos = matrix.MultiplyVector(pos);
		}
		return verticsTmp;
	}

	public static Mesh DrawSectorMesh(float radiusLong,float radiusShort,float angle,float height,bool isTwoSide){
		Mesh mesh = new Mesh ();
		List<Vector3> sectorLongTopPosList = GetSectorPosList (radiusLong, height / 2, angle);
		List<Vector3> sectorShortTopPosList = GetSectorPosList (radiusShort, height / 2, angle);
		List<Vector3> sectorLongBottomPosList = GetSectorPosList (radiusLong, -height / 2, angle);
		List<Vector3> sectorShortBottomPosList = GetSectorPosList (radiusShort, -height / 2, angle);
		List<Vector3> vertexList = new List<Vector3> ();
		//init small cylinder triangle vertics
		for (int i =1; i < sectorShortBottomPosList.Count; i ++) {
				vertexList.Add (sectorShortBottomPosList [i - 1]);
				vertexList.Add (sectorShortBottomPosList [i]);
				vertexList.Add (sectorShortTopPosList [i]);
				if (isTwoSide) {
						vertexList.Add (sectorShortBottomPosList [i]);
						vertexList.Add (sectorShortBottomPosList [i - 1]);
						vertexList.Add (sectorShortTopPosList [i]);
				}
		}


		for (int i =1; i < sectorShortTopPosList.Count; i ++) {
				vertexList.Add (sectorShortTopPosList [i - 1]);
				vertexList.Add (sectorShortTopPosList [i]);
				vertexList.Add (sectorShortBottomPosList [i - 1]);
				if (isTwoSide) {
						vertexList.Add (sectorShortTopPosList [i]);
						vertexList.Add (sectorShortTopPosList [i - 1]);
						vertexList.Add (sectorShortBottomPosList [i - 1]);
				}
		}
		//init large cylinder triangle vertics
		for (int i =1; i < sectorLongBottomPosList.Count; i ++) {
				vertexList.Add (sectorLongBottomPosList [i - 1]);
				vertexList.Add (sectorLongBottomPosList [i]);
				vertexList.Add (sectorLongTopPosList [i]);
				if (isTwoSide) {
						vertexList.Add (sectorLongBottomPosList [i]);
						vertexList.Add (sectorLongBottomPosList [i - 1]);
						vertexList.Add (sectorLongTopPosList [i]);
				}
		}
		for (int i =1; i < sectorLongTopPosList.Count; i ++) {
				vertexList.Add (sectorLongTopPosList [i - 1]);
				vertexList.Add (sectorLongTopPosList [i]);
				vertexList.Add (sectorLongBottomPosList [i - 1]);
				if (isTwoSide) {
						vertexList.Add (sectorLongTopPosList [i]);
						vertexList.Add (sectorLongTopPosList [i - 1]);
						vertexList.Add (sectorLongBottomPosList [i - 1]);
				}
		}

		//init top triangle vertics
		for (int i =1; i < sectorShortTopPosList.Count; i ++) {
			vertexList.Add (sectorShortTopPosList [i - 1]);
			vertexList.Add (sectorShortTopPosList [i]);
			vertexList.Add (sectorLongTopPosList [i]);
			if (isTwoSide) {
				vertexList.Add (sectorShortTopPosList [i]);
				vertexList.Add (sectorShortTopPosList [i - 1]);
				vertexList.Add (sectorLongTopPosList [i]);
			}
		}
		for (int i =1; i < sectorLongTopPosList.Count; i ++) {
			vertexList.Add (sectorLongTopPosList [i - 1]);
			vertexList.Add (sectorLongTopPosList [i]);
			vertexList.Add (sectorShortTopPosList [i - 1]);
			if (isTwoSide) {
				vertexList.Add (sectorLongTopPosList [i]);
				vertexList.Add (sectorLongTopPosList [i - 1]);
				vertexList.Add (sectorShortTopPosList [i - 1]);
			}
		}
		//init bottom triangle vertics
		for (int i =1; i < sectorShortBottomPosList.Count; i ++) {
			vertexList.Add (sectorShortBottomPosList [i - 1]);
			vertexList.Add (sectorShortBottomPosList [i]);
			vertexList.Add (sectorLongBottomPosList [i]);
			if (isTwoSide) {
				vertexList.Add (sectorShortBottomPosList [i]);
				vertexList.Add (sectorShortBottomPosList [i - 1]);
				vertexList.Add (sectorLongBottomPosList [i]);
			}
		}
		
		
		for (int i =1; i < sectorLongBottomPosList.Count; i ++) {
			vertexList.Add (sectorLongBottomPosList [i - 1]);
			vertexList.Add (sectorLongBottomPosList [i]);
			vertexList.Add (sectorShortBottomPosList [i - 1]);
			if (isTwoSide) {
				vertexList.Add (sectorLongBottomPosList [i]);
				vertexList.Add (sectorLongBottomPosList [i - 1]);
				vertexList.Add (sectorShortBottomPosList [i - 1]);
			}
		}
		//init left triangle vertics
		vertexList.Add (sectorLongBottomPosList [0]);
		vertexList.Add (sectorShortBottomPosList [0]);
		vertexList.Add (sectorShortTopPosList [0]);
		if (isTwoSide) {
			vertexList.Add (sectorShortBottomPosList [0]);
			vertexList.Add (sectorLongBottomPosList [0]);
			vertexList.Add (sectorShortTopPosList [0]);	
		}

		vertexList.Add (sectorLongBottomPosList [0]);
		vertexList.Add (sectorLongTopPosList [0]);
		vertexList.Add (sectorShortTopPosList [0]);
		if (isTwoSide) {
			vertexList.Add (sectorLongTopPosList [0]);
			vertexList.Add (sectorLongBottomPosList [0]);
			vertexList.Add (sectorShortTopPosList [0]);	
		}
		//init right triangle vertics
		vertexList.Add (sectorLongBottomPosList[sectorLongBottomPosList.Count - 1]);
		vertexList.Add (sectorShortBottomPosList [sectorShortBottomPosList.Count - 1]);
		vertexList.Add (sectorShortTopPosList [sectorShortTopPosList.Count - 1]);
		if (isTwoSide) {
			vertexList.Add (sectorShortBottomPosList [sectorShortBottomPosList.Count - 1]);
			vertexList.Add (sectorLongBottomPosList [sectorLongBottomPosList.Count - 1]);
			vertexList.Add (sectorShortTopPosList [sectorShortTopPosList.Count - 1]);	
		}
		
		vertexList.Add (sectorLongBottomPosList [sectorLongBottomPosList.Count - 1]);
		vertexList.Add (sectorLongTopPosList [sectorLongTopPosList.Count - 1]);
		vertexList.Add (sectorShortTopPosList [sectorShortTopPosList.Count - 1]);
		if (isTwoSide) {
			vertexList.Add (sectorLongTopPosList [sectorLongTopPosList.Count - 1]);
			vertexList.Add (sectorLongBottomPosList [sectorLongBottomPosList.Count - 1]);
			vertexList.Add (sectorShortTopPosList [sectorShortTopPosList.Count - 1]);	
		}
		
		
		int[] triangles = new int[vertexList.Count];
		for(int i =0; i < vertexList.Count;i ++)
		{
			triangles[i] = i;
		}
		mesh.vertices = vertexList.ToArray();
		mesh.triangles = triangles;
		mesh.uv = new Vector2[vertexList.Count];
		for(int i=0;i<vertexList.Count;i++)
		{
			mesh.uv[i] = new Vector2(vertexList[i].x,vertexList[i].y).normalized;
		}
		mesh.RecalculateNormals();
		return mesh;
	}


	public static Mesh DrawSectorMesh(float radiusLong,float radiusShort,float angle){
		Mesh mesh = new Mesh ();
		Matrix4x4 matrix = GetYAxisMatrix (1);
		Vector3 posShort = new Vector3 (radiusShort,0,0);
		Vector3 posLong = new Vector3 (radiusLong,0,0);
		List<Vector3> verticsTmp = new List<Vector3> ();
		Vector3 posShort0;
		Vector3 posLong0;
		verticsTmp.Add(posShort);
		verticsTmp.Add(posLong);
		for(int i =0;i < angle / 1;i++)
		{
			posShort0 = matrix.MultiplyVector(posShort);
			posLong0 = matrix.MultiplyVector(posLong);
			verticsTmp.Add(posShort0);
			verticsTmp.Add(posLong0);
			posShort = posShort0;
			posLong = posLong0;
		}
		List<Vector3> vertics = new List<Vector3> ();
		List<int> triangles = new List<int> ();
		for(int i = 2;i < verticsTmp.Count;i ++)
		{
			vertics.Add(verticsTmp[i-2]);
			vertics.Add(verticsTmp[i-1]);
			vertics.Add(verticsTmp[i]);
		}
		int triangleCount = vertics.Count / 3;
		for(int i = 0;i < triangleCount;i ++)
		{
			if(i % 2 == 0)
			{
				triangles.Add(i*3);
				triangles.Add(i*3 + 1);
				triangles.Add(i*3 + 2);
			}
			else
			{
				triangles.Add(i*3 + 2);
				triangles.Add(i*3 + 1);
				triangles.Add(i*3);
			}

		}
		mesh.vertices = vertics.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.uv = new Vector2[vertics.Count];
		for(int i=0;i<vertics.Count;i++)
		{
			mesh.uv[i] = new Vector2(vertics[i].x,vertics[i].y).normalized;
		}
		mesh.RecalculateNormals();
		return mesh;
	}
	
	public static GameObject DrawCircleGameObject(Material meshMaterial,float radius){
		GameObject go = GetMeshGameObject (meshMaterial,Color.red);
		Mesh mesh = DrawCircleMesh(radius);
		go.GetComponent<MeshFilter>().mesh = mesh;
		return go;
	}

	public static Mesh DrawCircleMesh(float radius){
		Mesh mesh = new Mesh ();
		Matrix4x4 matrix = GetYAxisMatrix (1);
		Vector3 pos = new Vector3 (radius,0,0);
		Vector3 centerPos = Vector3.zero;

		List<Vector3> vertics = new List<Vector3> ();
		List<int> triangles = new List<int> ();
		for(int i =0;i < 360 / 1;i++)
		{
			Vector3 pos0 = matrix.MultiplyVector(pos);
			triangles.Add(vertics.Count);
			vertics.Add(centerPos);
			triangles.Add(vertics.Count);
			vertics.Add(pos);
			triangles.Add(vertics.Count);
			vertics.Add(pos0);
			pos = pos0;
		}
		mesh.vertices = vertics.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.uv = new Vector2[vertics.Count];
		for(int i=0;i<vertics.Count;i++)
		{
			mesh.uv[i] = Vector2.zero;
		}
		mesh.RecalculateNormals();
		return mesh; 
	}

	public static Matrix4x4 GetYAxisMatrix(float angle){
		Matrix4x4 matrix = new Matrix4x4 ();
		matrix.m00 = Mathf.Cos (angle / 180 * Mathf.PI);
		matrix.m20 = -Mathf.Sin(angle/180 * Mathf.PI);
		matrix.m11 = 1;
		matrix.m02 = Mathf.Sin (angle/180 * Mathf.PI);;
		matrix.m22 = Mathf.Cos (angle / 180 * Mathf.PI);
		return matrix;
	}

	public static Matrix4x4 GetZAxisMatrix(float angle){
		Matrix4x4 matrix = new Matrix4x4 ();
		matrix.m00 = Mathf.Cos (angle / 180 * Mathf.PI);
		matrix.m10 = Mathf.Sin(angle/180 * Mathf.PI);
		matrix.m01 = -Mathf.Sin(angle/180 * Mathf.PI);
		matrix.m11 = Mathf.Cos (angle/180 * Mathf.PI);;
		matrix.m22 = 1;
		return matrix;
	}

	public static GameObject Draw6EdgeGameObject(Material meshMaterial,float gridSize)
	{
		if(meshMaterial==null)
		{
			Shader meshShader = Shader.Find("Diffuse");
			meshMaterial = new Material(meshShader);
		}
		GameObject go = new GameObject ();
		go.transform.eulerAngles = new Vector3 (-90,0,0);
		MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
		meshRenderer.material = meshMaterial;
		meshRenderer.sharedMaterial = meshMaterial;
		MeshFilter meshFilter = go.AddComponent<MeshFilter>();
		Mesh mesh = Draw6DirectionMesh(gridSize);
		meshFilter.mesh = mesh;
		go.transform.localEulerAngles = Vector3.zero;
		return go;
	}

	/// <summary>
	/// Draw 6 edges mesh.
	/// </summary>
	/// <param name="meshMaterial">Mesh material.</param>
	/// <param name="gridSize">Grid size.</param>
	public static Mesh Draw6DirectionMesh(float gridSize)
	{
		Mesh mesh = new Mesh();
		float sin60 = Mathf.Sin (60 * Mathf.PI / 180);
		int[] triangles = new int[18];
		Vector3[] vertics = new Vector3[7];
		vertics [0] = new Vector3 (0, 0, 0);
		vertics [1] = new Vector3 (0, gridSize ,0);
		vertics [2] = new Vector3 (sin60*gridSize,0.5f*gridSize,0);
		vertics [3] = new Vector3 (sin60*gridSize,-0.5f*gridSize,0);
		vertics [4] = new Vector3 (0, -gridSize,0);
		vertics [5] = new Vector3 (-sin60*gridSize,-0.5f*gridSize,0);
		vertics [6] = new Vector3 (-sin60*gridSize,0.5f*gridSize,0);
		triangles[0] = 0;
		triangles[1] = 1;
		triangles [2] = 2;
		triangles [3] = 0;
		triangles [4] = 2;
		triangles [5] = 3;
		triangles [6] = 0;
		triangles [7] = 3;
		triangles [8] = 4;
		triangles [9] = 0;
		triangles [10] = 4;
		triangles [11] = 5;
		triangles [12] = 0;
		triangles [13] = 5;
		triangles [14] = 6;
		triangles [15] = 0;
		triangles [16] = 6;
		triangles [17] = 1;
		mesh.vertices = vertics;
		mesh.triangles = triangles;
		Vector2 vertices0 =  new Vector2 (0,0);
		Vector2 vertices1 = new Vector2(0,1);
		Vector2 vertices2 = new Vector2(sin60,0.5f);
		Vector2 vertices3 = new Vector2(sin60,-0.5f);
		Vector2 vertices4 = new Vector2(0,-1);
		Vector2 vertices5 = new Vector2(-sin60,-0.5f);
		Vector2 vertices6 = new Vector2(-sin60,0.5f);
		mesh.uv = new Vector2[]{vertices0 , vertices1 , vertices2 , vertices3,vertices4,vertices5,vertices6};
		mesh.RecalculateNormals();
		return mesh;
	}

	public static List<GameObject> Draw6DirectionGrid(int width,int height,Material meshMaterial,float gridSize)
	{
		List<GameObject> allGo = new List<GameObject>();
		float sin60 = Mathf.Sin (60 * Mathf.PI / 180);
		for(int i=0;i < height;i++)
		{
			float y = gridSize + gridSize * 1.5f * i;
			for(int j=0;j < width;j++)
			{
				float x = (sin60 + sin60 * 2 * j) * gridSize;
				if(i % 2 != 0)x += sin60;
				GameObject go = Draw6EdgeGameObject(meshMaterial,gridSize);
				go.transform.localScale = Vector3.one * 0.9f;
				go.transform.position = new Vector2(x,y);
				allGo.Add(go);
			}
		}
		return allGo;
	}

}
