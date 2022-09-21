using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[System.Serializable]
public class MusicLayer
{
    [Tooltip("Element Name, '<Name> Volume' in FMOD")]
    public string name;
    [Tooltip("Current Layer volume")]
    [Range(0, 100)] public float volume = 0f;
    [Tooltip("Fade-in/out target volume")]
    [Range(0, 100)] public float targetVolume = 0f;
    [Tooltip("If enabled, Volume slider allows direct control of FMOD parameter")]
    public bool manualVolumeOverride = false;
}

public class BattleMusicController : MonoBehaviour
{
    // Properties
    [Tooltip("Reorder this list for layer priority")]
    public MusicLayer[] musicLayers;

    [Tooltip("% volume per second | Larger values = faster crossfades")]
    public float volumeLerpRate = 20f;


    // Variables


    // References
    StudioEventEmitter battleMusicEmitter;

    [Space(30)]
    [Header("Asset References")]
    public AllParticipantDataService allParticipantDataService;
    public SO_Element[] allElements;


    //Engine-called
    private void Awake()
    {
        battleMusicEmitter = GetComponent<StudioEventEmitter>();
    }

    private void Start()
    {
        CalculateInitialVolumes();
        UpdateVolumesImmediate();
    }

    void Update()
    {
        UpdateFades();
        UpdateElementVolumes();
    }


    //Listener-called
    public void OnDebrisChange() //Called by DebrisController
    {
        CalculateElementVolumes();
    }


    //Methods
        //Private
    void CalculateElementVolumes()
    {

        //Apply a Mathf.Clamp() if bugs occur
    }

    void CalculateInitialVolumes()
    {
        SO_AllParticipantData ParticipantData = allParticipantDataService.GetAllParticipantData();


    }

    void UpdateFades() //applies the lerp between the current volume and target volume
    {
        foreach (MusicLayer m in musicLayers)
        {
            if (m.manualVolumeOverride) continue;

            //Adding/subtracting a constant per second = linear transition.
            //Adjust the volume curve in FMOD, don't try and change the lerp curve
            if (m.volume > m.targetVolume) m.volume -= volumeLerpRate * Time.deltaTime; 
            if (m.volume < m.targetVolume) m.volume += volumeLerpRate * Time.deltaTime;

            //If we end up with targetVolumes that aren't 0 or 100, add a 'if volume delta <= 0.1, volume = target' here

            m.volume = Mathf.Clamp(m.volume, 0f, 100f);
        }
    }

    void UpdateElementVolumes() //applies the current volumes to FMOD
    {
        foreach (MusicLayer m in musicLayers)
        {
            battleMusicEmitter.SetParameter(m.name + " Volume", m.volume); //Syntax in FMOD is currently '<Element> Volume', e.g 'Earth Volume'
        }
    }

    void UpdateVolumesImmediate() //Instantly applies a fade, should only be used at start of track
    {
        foreach (MusicLayer m in musicLayers)
        {
            m.targetVolume = Mathf.Clamp(m.targetVolume, 0f, 100f);
            m.volume = m.targetVolume;
        }
        UpdateElementVolumes();
    }
}
