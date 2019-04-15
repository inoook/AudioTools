using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMicrophone : MonoBehaviour {

    [SerializeField] RecordVoice recordVoice;


    [SerializeField] Rect drawRect = new Rect(10,10,200,200);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [SerializeField] int count;

	private void OnGUI()
	{
        GUILayout.BeginArea(drawRect);
        count = recordVoice.recordAudioData.Count;
        GUILayout.Label("recordVoice.recordAudioData.Count: "+ count.ToString() + " / "+recordVoice.Get_dataBuffer());
        GUILayout.EndArea();
	}
}
