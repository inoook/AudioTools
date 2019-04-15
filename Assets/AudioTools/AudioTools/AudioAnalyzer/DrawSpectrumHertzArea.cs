using UnityEngine;
using System.Collections;

public class DrawSpectrumHertzArea : MonoBehaviour {

	[SerializeField]
	AudioAnalyzer audioAnalyzer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Draw (audioAnalyzer.GetSpectrum (), audioAnalyzer.GetPitchHertz());
	}

	[System.Serializable]
	public class HertzArea
	{
		public string label;
//		public int hertz_min;
		public int hertz_max;

		public float value;

		public float value_max;
	}

	[SerializeField]
	HertzArea[] hertzAreas;

	void Draw(float[] spectrum, float pitchHertz)
	{
		if (spectrum == null) { return; }

		float s = (float)spectrum.Length / AudioSettings.outputSampleRate * 2.0f;

		int min = 0;
		for (int i = 0; i < hertzAreas.Length; i++) {
			HertzArea area = hertzAreas [i];
			if (i == 0) {
				min = 0;
			}else{
				min = hertzAreas [i - 1].hertz_max +1;
			}
			min = Mathf.Max (0, min);
//			area.hertz_min = min;
			int hertz_min = (int)(min * s);
			int hertz_max = (int)(area.hertz_max * s);

			area.value = GetAreaValue (spectrum, hertz_min, hertz_max);

			area.value_max = Mathf.Max (area.value, area.value_max);
		}

		// debug
		DebugDraw (spectrum, pitchHertz);
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

	#region API
	// 生のデータ
	public float GetRawAreaValueByIndex(int index)
	{
		index = CheckAndClampIndex (index);
		return hertzAreas [index].value;
	}
	// ampで増幅して 0 - 1 にclampした値
	public float GetAreaValueByIndex(int index, float amp)
	{
		float v = GetRawAreaValueByIndex (index);
		v = v * amp;
		return Mathf.Clamp01(v);
	}
	public string GetAreaNameByIndex(int index)
	{
		index = CheckAndClampIndex (index);
		return hertzAreas [index].label;
	}
	int CheckAndClampIndex(int index)
	{
		if (index < 0) {
			Debug.LogWarning ("error");
			index = 0;
		}else if(index > hertzAreas.Length-1){
			Debug.LogWarning ("error");
			index = hertzAreas.Length-1;
		}
		return index;
	}
	public int GetAreaCount()
	{
		return hertzAreas.Length;
	}

	public HertzArea GetHertzAreaByIndex(int index)
	{
		return hertzAreas[index];
	}

	public void Reset()
	{
		for (int i = 0; i < hertzAreas.Length; i++) {
			hertzAreas [i].value_max = 0;
		}
	}
	#endregion

	#region debugDraw
//	[SerializeField]
//	Rect drawRect = new Rect(10,10,100,100);
//	[SerializeField]
//	float limit = 1.0f;

//	void OnGUI()
//	{
//		GUILayout.BeginArea (drawRect);
//		GUILayout.Label ("DrawSpectrumHertzArea");
//		GUILayout.BeginHorizontal ();
//		for (int i = 0; i < hertzAreas.Length; i++) {
//			float v = hertzAreas[i].value;
//			GUILayout.VerticalSlider (v, limit, 0);
//		}
//		GUILayout.EndHorizontal ();
//		GUILayout.EndArea ();
//	}

	[SerializeField]
	Vector3 scale = Vector3.one;

	[SerializeField]
	Vector3 offsetPos = Vector3.zero;

	[SerializeField]
	Color color = Color.cyan;

	void DebugDraw(float[] spectrum, float pitchHertz)
	{
		// draw
		for (int i = 1; i < spectrum.Length; i++) {
//			MeshLine.DrawLine( new Vector3( Mathf.Log( i - 1 ), spectrum[i - 1] * scale.y, 0 ) + offsetPos, new Vector3( Mathf.Log( i ), spectrum[i] * scale.y, 0 ) + offsetPos, color );//音階はこれがわかりやすい。
			MeshLine.DrawLine( new Vector3( Log(i - 1 ), spectrum[i - 1] * scale.y, 0 ) + offsetPos, new Vector3( Log( i ), spectrum[i] * scale.y, 0 ) + offsetPos, color );//音階はこれがわかりやすい。
		}
		// area
		float s = (float)spectrum.Length / AudioSettings.outputSampleRate * 2.0f;
		for (int i = 0; i < hertzAreas.Length; i++) {
			HertzArea area = hertzAreas [i];

			float pitch_x = area.hertz_max * s;
			float log_pitch = Log (pitch_x);
			MeshLine.DrawLine (new Vector3 (log_pitch, 0, 0) + offsetPos, new Vector3 (log_pitch, 4, 0) + offsetPos, Color.yellow);
		}

		//
		float currentPitch = pitchHertz * s;
		float log_currentPitch = Log (currentPitch);
		MeshLine.DrawLine (new Vector3 (log_currentPitch, 0, 0) + offsetPos, new Vector3 (log_currentPitch, 6, 0) + offsetPos, Color.cyan);
	}

	float Log(float v)
	{
		// Mathf.Logの値で Invalid AABB a, IsFinite(outDistanceForSort) エラーが出るのが強引に。
		if (v == 0) {
			return 0;
		}
		return Mathf.Log (v);

		// Mathf.Logの値で Invalid AABB a, IsFinite(outDistanceForSort) エラーが出るのが強引に。
//		return Mathf.Log (v);
		//float value = (float)( Mathf.Ceil( Mathf.Log(v) * 1000000.0f )) / 1000000.0f;
		//return (value < 0) ? 0 : value;
	}
	#endregion
}
