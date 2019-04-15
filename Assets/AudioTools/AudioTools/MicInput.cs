using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MicInput : MonoBehaviour
{
    public AudioSource[] audioSrcs;

	// Use this for initialization
	void Start ()
	{
		//if (audioSrc == null) {
		//	audioSrc = this.gameObject.GetComponent<AudioSource> ();
		//}
		//audioSrc.outputAudioMixerGroup = audioMixer.outputAudioMixerGroup;

		var sampleRate = AudioSettings.outputSampleRate;
        //AudioSettings.outputSampleRate = 8000;
        //AudioConfiguration config = AudioSettings.GetConfiguration();
        //config.sampleRate = 8000;
        //AudioSettings.Reset(config);
        Debug.Log("sampleRate: "+sampleRate);

		// Create a clip which is assigned to the default microphone.
        AudioClip micClip = Microphone.Start(null, true, 1, sampleRate);
        foreach(var audioSrc in audioSrcs){
            audioSrc.clip = micClip;
            audioSrc.loop = true;
        }
        if (micClip != null) {
			// Wait until the microphone gets initialized.
			int delay = 0;
			while (delay <= 0)
				delay = Microphone.GetPosition (null);

            // Start playing.
            foreach (var audioSrc in audioSrcs)
            {
                audioSrc.Play();
            }
		} else {
			Debug.LogWarning ("GenericAudioInput: Initialization failed.");
		}

	}
	
	// Update is called once per frame
	void Update ()
	{
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    foreach (var audioSrc in audioSrcs)
        //    {
        //        if (audioSrc.isPlaying)
        //        {
        //            audioSrc.Pause();
        //        }else{
        //            audioSrc.UnPause();
        //        }
        //    }
        //}
	}

    public void EnableMicOutput(bool enable) {
        foreach (var audioSrc in audioSrcs) {
            audioSrc.volume = enable ? 1 : 0;
        }
    }
}
