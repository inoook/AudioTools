using UnityEngine;
using System.Collections;

public class DrawWaveData : MonoBehaviour {

	[SerializeField]
	AudioAnalyzer audioAnalyzer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Draw (audioAnalyzer.GetWaveData ());
	}

	public float scaleX = 0.5f;
	public float depth = 10;
	public float red_scale = 5;

	public float r = 1.0f;
	public float vAmp = 1.0f;

	public void Draw(float[] waveData){

		if (waveData == null) { return; }

		// 音声の波形
		for (int i = 1; i < waveData.Length - 1; i++) {
			MeshLine.DrawLine (new Vector3 ((i - 1) * scaleX, waveData [i] * red_scale, depth), new Vector3 (i * scaleX, waveData [i + 1] * red_scale, depth), Color.red);// そのまま表示するならこれ
		}

		// 音声の波形を円状にしたもの
		float deltaRad = Mathf.PI * 2 / waveData.Length;
		for (int i = 0; i < waveData.Length; i++) {
			float d = deltaRad * i;
			float v = waveData [i];
			float x = Mathf.Cos (d);
			float y = Mathf.Sin (d);
			Vector3 dir = new Vector3 (x, y, 0);
			//
			float d1;
			float v1;
			if (i == 0) {
				d1 = deltaRad * (waveData.Length - 1);
				v1 = waveData [waveData.Length - 1];
			} else {
				d1 = deltaRad * (i - 1);
				v1 = waveData [i - 1];
			}
			float x1 = Mathf.Cos (d1);
			float y1 = Mathf.Sin (d1);

			Vector3 dir1 = new Vector3 (x1, y1, 0);

			MeshLine.DrawLine ((r + v1 * vAmp) * dir1, (r + v * vAmp) * dir, Color.red);
		}
	}
}
