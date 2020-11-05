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
		if (INSTANCE == null)
		{
			INSTANCE = this;
		}
	}

	// Use this for initialization
	void Start () {
		Setup();
	}

	void Setup()
    {
		if(mf != null) { return; }

		mf = this.GetComponent<MeshFilter>();

		mesh = new Mesh();
		mesh.name = "LineMesh";
		mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

		mf.mesh = mesh;

		vertexList = new List<Vector3>();
		colorList = new List<Color>();
	}

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

	void _Render ()
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

    static void Create()
	{
		if (INSTANCE == null)
		{
			Debug.LogWarning("Create: MeshLine");
			var gameObject = new GameObject("MeshLine");
			var ml = gameObject.AddComponent<MeshLine>();

			var mr = gameObject.GetComponent<MeshRenderer>();
			mr.sharedMaterial = new Material(Shader.Find("Sprites/Default"));

			INSTANCE = ml;

			INSTANCE.Setup();
		}
	}


	#region draw
	public static void DrawLine(Vector3 a, Vector3 b, Color color)
	{
		Create();
		INSTANCE._DrawLine (a, b, color);
	}
	public static void DrawRay(Vector3 a, Vector3 b, Color color)
	{
		Create();
		INSTANCE._DrawLine(a, a + b, color);
	}
	#endregion
}
