using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AvgFilter {

	public static float CalucAvg(List<float> values)
	{
		float v = 0;
		for (int i = 0; i < values.Count; i++) {
			v += values[i];
		}
		return v / values.Count;
	}

	List<float> avgList;
	public int bufferNum = 5;

	public AvgFilter()
	{
		avgList = new List<float>();
	}
	public float GetFilteredValue(float v){
		avgList.Add (v);
		if (avgList.Count > bufferNum) {
			avgList.RemoveRange (0, avgList.Count - bufferNum);
		}
		return CalucAvg (avgList);
	}

}
