// http://d.hatena.ne.jp/nakamura001/20120724/1343148980
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class Sinus : MonoBehaviour
{
	// un-optimized version
	public double frequency = 440;
	public double gain = 0.05;
	private double increment;
	private double phase;
	private double sampling_frequency = 48000;//44100 // 48000

	[SerializeField]
	private bool playing = false;


	void Start()
	{
		AudioSource audioSrc = this.gameObject.GetComponent<AudioSource> ();
		audioSrc.clip = null;
		audioSrc.Play ();

		sampling_frequency = AudioSettings.outputSampleRate;
	}
	
	void OnAudioFilterRead (float[] data, int channels)
	{
		if (!playing)
			return;
		// update increment in case frequency has changed
		increment = frequency * 2.0 * Math.PI / sampling_frequency;
		for (var i = 0; i < data.Length; i = i + channels) {
			phase = phase + increment;
			
			data [i] = (float)(gain * Math.Sin (phase));
			// if we have stereo, we copy the mono data to each channel
			if (channels == 2)
				data [i + 1] = data [i];
			if (phase > 2 * Math.PI)
				phase = 0;
		}
	}

} 