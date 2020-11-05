using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorControllerDebug : MonoBehaviour {

    [SerializeField] AudioRecorder audioRecorder = null;

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
        count = audioRecorder.GetRecData().Count;
        GUILayout.Label("recordVoice.recordAudioData.Count: "+ count.ToString() + " / "+audioRecorder.Get_dataBuffer());
        GUILayout.EndArea();
	}
}
