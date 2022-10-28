using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[System.Serializable]
public class AudioBus
{
    [Tooltip("Name of Parameter in FMOD")]
    public string name;
    [Tooltip("Value of Parameter. This corresponds with Automation curves set on the volume in the Mixer window.")]
    [Range(0, 100)] public float volume = 90f;
}

public class BusVolumeController : MonoBehaviour
{
    // Properties
    //[Header("Properties")]
    [Tooltip("Name-Volume pairs for Parameters in FMOD. Intended for Global Parameters automating Mixer Bus volumes.")]
    public AudioBus[] audioBuses;

    string playerPrefsKeyPrefix = "BusVolume_";


    // Variables


    // References


    
    // Engine - called
    void Start()
    {
        LoadSavedVolumes();
    }

    void Update()
    {
        UpdateParameterValues();
    }


    // Methods
    void LoadSavedVolumes()
    {
        foreach (AudioBus b in audioBuses)
        {
            b.volume = PlayerPrefs.GetFloat(playerPrefsKeyPrefix + name, 90f);
        }
    }

    void UpdateParameterValues()
    {
        foreach (AudioBus b in audioBuses)
        {
            RuntimeManager.StudioSystem.setParameterByName(b.name, b.volume, false);
        }
    }
}
