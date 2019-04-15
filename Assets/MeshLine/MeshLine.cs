using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshLine : MonoBehaviour {

	static MeshLine INSTANCE;

	MeshFilter mf;
	Mesh mesh;
	
	List<Vector3> vertexList;
	List<Color> colorList;

	[SerializeField]
	bool debugDrawLine = false;

	void Awake()
	{
		INSTANCE = this;
	}

	// Use this for initialization
	void Start () {
		mf = this.GetComponent<MeshFilter> ();

		mesh = new Mesh ();
		mesh.name = "LineMesh";


		mf.mesh = mesh;
		
		vertexList = new List<Vector3> ();
		colorList = new List<Color> ();
	}

	// test
//	void Update()
//	{
//		MeshLine.DrawLine (Vector3.zero, new Vector3 (0, 1, 0), Color.green);
//		MeshLine.DrawLine (new Vector3 (0, 1, 0), new Vector3 (0, 1, 1), Color.white);
//	}

	// Update is called once per frame
	void LateUpdate () {
		_Render ();
	}


	void _DrawLine(Vector3 a, Vector3 b, Color color)
	{
		if (debugDrawLine) {
			Debug.DrawLine (a, b, color);
		}

		vertexList.Add (a);
		vertexList.Add (b);

		colorList.Add (color);
		colorList.Add (color);


	}

	void _Render()
	{
		mesh.Clear ();
		mesh.MarkDynamic ();
		mesh.UploadMeshData (false);

		mesh.vertices = vertexList.ToArray();
		mesh.colors = colorList.ToArray ();
		
//		Vector3[] normals = new Vector3[vertexList.Count];
		int[] indices = new int[vertexList.Count];
		for (int i = 0; i < vertexList.Count; i++) {
			indices [i] = i;
//			normals [i] = Vector3.one;
		}
//		mesh.normals = normals;
		mesh.SetIndices (indices, MeshTopology.Lines, 0);

		mesh.RecalculateBounds ();

		vertexList.Clear ();
		colorList.Clear ();
	}

//	static Material lineMaterial;
//	static void CreateLineMaterial() {
//		if( !lineMaterial ) {
//			lineMaterial = new Material(Shader.Find("Hidden/Lines/Colored Blended"));
//			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
//			lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
//		}
//	}
//
//	void _Render__()
//	{
//		Debug.Log (vertexList.Count);
//		CreateLineMaterial();
//
//		UnityEngine.GL.PushMatrix();
//		lineMaterial.SetPass(0);
//		UnityEngine.GL.LoadOrtho();
//		UnityEngine.GL.Begin(UnityEngine.GL.LINES);
//		for (int i = 0; i < vertexList.Count; i++) {
//			UnityEngine.GL.Color (colorList[i]);
//			UnityEngine.GL.Vertex (vertexList[i]);
//		}
//		UnityEngine.GL.End();
//		UnityEngine.GL.PopMatrix();
//
//		vertexList.Clear ();
//		colorList.Clear ();
//	}

	#region API
	public static void DrawLine(Vector3 a, Vector3 b, Color color)
	{
		INSTANCE._DrawLine (a, b, color);
	}
	#endregion
}
