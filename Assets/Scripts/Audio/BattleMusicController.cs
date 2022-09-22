using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Myths;

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

    [HideInInspector]
    public int score = 0;
}

public class BattleMusicController : MonoBehaviour
{
    // Properties
    [Tooltip("Reorder this list for layer priority")]
    public MusicLayer[] musicLayers;

    [Tooltip("% volume per second | Larger values = faster crossfades")]
    public float volumeLerpRate = 20f;
    [Tooltip("Desired number of Layers - Algorithm will target this number of Layers playing at once")]
    [Range(0, 8)] public int desiredLayers = 3;

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
        CalculateInitialScores();
        UpdateTargetVolumes();
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
        CalculateElementScores();
        UpdateTargetVolumes();
    }


    // Methods
        // Private
    void CalculateElementScores()
    {

        //Apply a Mathf.Clamp() if bugs occur
    }

    void CalculateInitialScores()
    {
        //Set the scores based on myth choices
        SO_AllParticipantData participantData = allParticipantDataService.GetAllParticipantData();

        foreach (PartyData p in participantData.partyData)
        {
            foreach (Myth m in p.myths)
            {
                musicLayers[GetIndexOfLayer(m.element.name)].score++;
            }
        }
    }

    void UpdateTargetVolumes() { 
        //Find the top <desiredLayers> elements (if there's a tie, it's broken by the order of MusicLayers)
        List<int> maxLayerIndexes = new List<int>(); //'applied' indexes get added here

        for (int a = 0; a < desiredLayers; a++) {
            //Max search
            int maxScore = int.MinValue;
            int maxIndex = -1;
            for (int i = 0; i < musicLayers.Length; i++)
            {
                if (musicLayers[i].score > maxScore && !maxLayerIndexes.Contains(i))
                {
                    maxScore = musicLayers[i].score;
                    maxIndex = i;
                }
            }

            maxLayerIndexes.Add(maxIndex);
        }

        //Apply the target volumes, based on whether a layer was in maxLayerIndexes or not
        foreach (int i in maxLayerIndexes)
        {
            musicLayers[i].targetVolume = 100f;
        }

        for (int i = 0; i < musicLayers.Length; i++)
        {
            if (!maxLayerIndexes.Contains(i)) musicLayers[i].targetVolume = 0f;
        }
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


    // Functions
    int GetIndexOfLayer(string elementName)
    {
        for (int i = 0; i < musicLayers.Length; i++)
        {
            if (musicLayers[i].name == elementName) return i;
        }

        Debug.LogWarning("Could not find Music Layer with name " + elementName + ", return -1, expect index errors.");
        return -1;
    }
}
