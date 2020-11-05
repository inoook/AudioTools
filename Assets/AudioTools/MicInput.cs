using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

/// <summary>
/// マイクによる入力を audioSrcs へセットする。
/// mute する場合は、audioSrcs の audioMixer で volume を 0 にする。
/// </summary>
public class MicInput : MonoBehaviour
{
    [SerializeField] AudioSource[] audioSrcs;
    [SerializeField] string microphoneDeviceName = "";

    string activeDeviceName = "";

    [SerializeField] bool enableOnStart = true;

    // Use this for initialization
    void Start ()
	{
        if (enableOnStart)
        {
            StartMicrophone();
        }
    }

    bool IsValidDeviceName(string name)
    {
        string[] devices = Microphone.devices;
        foreach (var deviceName in devices)
        {
            if(deviceName == name) {
                return true;
            }
        }
        return false;
    }

    public void EnableMicOutput(bool enable) {
        foreach (var audioSrc in audioSrcs) {
            audioSrc.volume = enable ? 1 : 0;
        }
    }

    public void StartMicrophone(System.Action OnMicReadyAct = null)
    {
        string[] devices = Microphone.devices;
        if (devices.Length <= 0)
        {
            Debug.LogError("No Microphne device");
        }
        Debug.Log(">> Microphone Devices");
        foreach (var deviceName in devices)
        {
            Debug.Log(deviceName);
        }

        if (!string.IsNullOrEmpty(microphoneDeviceName))
        {
            if (IsValidDeviceName(microphoneDeviceName))
            {
                activeDeviceName = microphoneDeviceName;
            }
            else
            {
                Debug.LogWarning(">> can't find: " + microphoneDeviceName);
                activeDeviceName = "";
            }

        }
        Debug.LogWarning(">> activeDeviceName: " + activeDeviceName);

        var sampleRate = AudioSettings.outputSampleRate;
        //AudioSettings.outputSampleRate = 8000;
        //AudioConfiguration config = AudioSettings.GetConfiguration();
        //config.sampleRate = 8000;//44100
        //AudioSettings.Reset(config);
        Debug.Log("sampleRate: " + sampleRate);

        //AudioClip micClip = Microphone.Start(activeDeviceName, true, 5, sampleRate);
        AudioClip micClip = Microphone.Start(activeDeviceName, true, 1, sampleRate);

        if (micClip != null)
        {
            foreach (var audioSrc in audioSrcs)
            {
                audioSrc.clip = micClip;
                audioSrc.loop = true;
            }

            // Wait until the microphone gets initialized.
            int delay = 0;
            while (delay <= 0)
                delay = Microphone.GetPosition(activeDeviceName);

            // Start playing.
            foreach (var audioSrc in audioSrcs)
            {
                audioSrc.Play();
            }
        }
        else
        {
            Debug.LogWarning("GenericAudioInput: Initialization failed.");
        }

        // play
        if (OnMicReadyAct != null)
        {
            OnMicReadyAct();
        }
    }

    public void StopMic()
    {
        //マイクの録音を強制的に終了
        Microphone.End(activeDeviceName);
    }
}
