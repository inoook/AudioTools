using UnityEngine;
using System.Collections;

public class DrawSpectrum : MonoBehaviour {

	[SerializeField]
	AudioAnalyzer audioAnalyzer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Draw (audioAnalyzer.GetSpectrum (), audioAnalyzer.pitch);
	}

	[SerializeField]
	int resolution = 64;

	[SerializeField]
	Vector3 scale = Vector3.one;

	[SerializeField]
	Vector3 scale2 = Vector3.one;

	[SerializeField]
	Vector3 scale_raw = Vector3.one;

	void Draw(float[] spectrum, float pitchHertz)
	{
		if (spectrum == null) { return; }
			
		float max = Mathf.Log (spectrum.Length);
		var deltaFreq = max / resolution;

		float[] datas = new float[resolution];
		for (int n = 0; n < datas.Length; n++) {
			datas [n] = 0;
		}

		// log分割
		for (var n = 1; n < resolution; n++) {
			var freqMin = deltaFreq * (n-1);
			var freqMax = deltaFreq * n;
			for (var i = 0; i < spectrum.Length; i++) {
				float v = Mathf.Log (i);
				if (v > freqMin && v <= freqMax) {
					datas [n] += spectrum[i];
				}
			}
		}

		// 通常分割
//		int delta = spectrum.Length / resolution;
//		for (var n = 0; n < resolution; n++) {
//			for (var i = delta*n; i < delta*(n+1); i++) {
//				datas [n] += spectrum[i];
//			}
//		}

		// graph --------------------------
		// draw
		for (int i = 1; i < resolution; i++) {
			var freq0 = deltaFreq * (i-1);
			var freq1 = deltaFreq * i;
			Debug.DrawLine (new Vector3 (freq0 * scale.x, datas [i-1] * scale.y, 0), new Vector3 (freq1 * scale.x, datas [i] * scale.y, 0), Color.magenta);
		}
		// vertical line
//		for (var n = 0; n < resolution; n++) {
//			var freq = deltaFreq * n;
//			Debug.DrawLine (new Vector3 (freq * scale.x, 0, 0), new Vector3 (freq * scale.x, 1, 0), Color.gray);
//		}

		// log spectrum
		for (int i = 1; i < spectrum.Length; i++) {
//			Debug.DrawLine (new Vector3 ((i-1) * scale.x, spectrum [i-1] * scale.y, 0), new Vector3 (i * scale.x, spectrum [i] * scale.y, 0), Color.cyan);
			Debug.DrawLine( new Vector3( Mathf.Log( i - 1 ), spectrum[i - 1] * scale2.y, 0 ), new Vector3( Mathf.Log( i ), spectrum[i] * scale2.y, 0 ), Color.cyan );//音階はこれがわかりやすい。
		}

		// raw spectrum
		for (int i = 1; i < spectrum.Length; i++) {
			Debug.DrawLine (new Vector3 ((i-1) * scale_raw.x, spectrum [i-1] * scale_raw.y, 0), new Vector3 (i * scale_raw.x, spectrum [i] * scale_raw.y, 0), Color.yellow);
		}

		// draw current Pitch pos
		float s = (float)spectrum.Length / AudioSettings.outputSampleRate * 2.0f;
		float pitch_x = pitchHertz * s;
		Debug.DrawLine (new Vector3 (pitch_x * scale_raw.x, 0, 0), new Vector3 (pitch_x * scale_raw.x, 4, 0), Color.yellow);

		Debug.DrawLine (new Vector3 (Mathf.Log(pitch_x), 0, 0), new Vector3 (Mathf.Log(pitch_x), 6, 0), Color.cyan);

		// debug
		draw_datas = datas;

		// 
		DividAreas(datas);
	}


	[System.Serializable]
	class IndexArea
	{
		public string label = "";
		public int min = 0;
		public int max = 0;

		public float value = 0;
	}

	[SerializeField]
	IndexArea[] areas;
	void DividAreas(float[] values)
	{
		for (int i = 0; i < areas.Length; i++) {
			IndexArea area = areas [i];
			area.value = GetAreaValue (values, area.min, area.max);
		}
	}
	float GetAreaValue(float[] values, int min, int max)
	{
		float v = 0;
		max = Mathf.Min (values.Length-1, max);// clamp
		for (int i = min; i <= max; i++) {
			v += values[i];
		}
		return v;
	}


	#region debug
	float[] draw_datas;
	[SerializeField]
	Rect drawRect = new Rect(10,10,100,100);
	[SerializeField]
	float limit = 1.0f;

	[SerializeField]
	Rect drawRect_areaData = new Rect(10,10,100,100);
	[SerializeField]
	float limit_area = 1.0f;


	void OnGUI()
	{
		GUILayout.BeginArea (drawRect);
		GUILayout.Label ("DrawSpectrum");
		GUILayout.BeginHorizontal ();
		for (int i = 0; i < draw_datas.Length; i++) {
			float v = draw_datas[i];
			GUILayout.VerticalSlider (v, limit, 0);
		}
		GUILayout.EndHorizontal ();
		GUILayout.EndArea ();

		GUILayout.BeginArea (drawRect_areaData);
		GUILayout.Label ("DrawArea");
		GUILayout.BeginHorizontal ();
		for (int i = 0; i < areas.Length; i++) {
			float v = areas[i].value;
			GUILayout.VerticalSlider (v, limit_area, 0);
		}
		GUILayout.EndHorizontal ();
		GUILayout.EndArea ();
	}
	#endregion
}
