using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Myths;
using Debris;
using System.Linq;

[System.Serializable]
public class MusicLayer
{
    [Tooltip("Element Name, '<Name> Volume' in FMOD")]
    public string name;
    [Tooltip("Current Layer volume")]
    [Range(0, 100)] public float volume = 0f;
    //[Tooltip("Fade-in/out target volume")]
    [HideInInspector][Range(0, 100)] public float targetVolume = 0f; 
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
    public float fadeInRate = 40f;
    [Tooltip("% volume per second | Larger values = faster crossfades")]
    public float fadeOutRate = 20f;
    [Tooltip("s seconds | The time before a non-Debris Ability's layer fade-out begins, if not overriden by Debris first")]
    public float nonDebrisFadeOutDelay = 5f;
    [Tooltip("Desired number of Layers - Algorithm will target this number of Layers playing at once")]
    [Range(0, 8)] public int desiredLayers = 3;

    // Variables


    // References
    static BattleMusicController Singleton;

    StudioEventEmitter battleMusicEmitter;
    DebrisRegion debrisRegion;

    [Space(30)]
    [Header("Asset References")]
    public AllParticipantDataService allParticipantDataService;
    public SO_Element[] allElements;
    public SO_Element[] nonDebrisElements;


    // Engine-called
    private void Awake()
    {
        Singleton = this;

        battleMusicEmitter = GetComponent<StudioEventEmitter>();
        debrisRegion = GameObject.FindWithTag("Grid").GetComponent<DebrisRegion>();

        ReorderAllElements();

        if (MenuMusicController.Singleton) Destroy(MenuMusicController.Singleton.gameObject);
    }

    private void Start()
    {
        CalculateInitialScores();
        UpdateTargetVolumes();
        UpdateVolumesImmediate();
        ResetAllScores();
    }

    void Update()
    {
        UpdateFades();
        UpdateLayerVolumes();
    }


    // Listener-called
    public void OnDebrisChange() // Called by DebrisController
    {
        CalculateElementScores();
        UpdateTargetVolumes();
    }


    // Called by other systems
    public static void OnElectricAbility()
    {
        Singleton.S_OnElectricAbility();
    }

    public static void OnWindAbility()
    {
        Singleton.S_OnWindAbility();
    }


    // Methods
        // Private
    void CalculateElementScores()
    {
        for (int i = 0; i < allElements.Length; i++)
        {
            if (!nonDebrisElements.Contains(allElements[i])) musicLayers[i].score = debrisRegion.NumberOfTilesWithElement(allElements[i]);
        }
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

                if (m.element.name == "Wind") Invoke("ZeroWindTargetVolume", nonDebrisFadeOutDelay);
                if (m.element.name == "Electric") Invoke("ZeroElectricTargetVolume", nonDebrisFadeOutDelay);
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
                if (musicLayers[i].score > maxScore && !maxLayerIndexes.Contains(i) && musicLayers[i].score > 0)
                {
                    maxScore = musicLayers[i].score;
                    maxIndex = i;
                }
            }

            if (maxIndex >= 0 && maxIndex < musicLayers.Length) maxLayerIndexes.Add(maxIndex);
        }

        //Apply the target volumes, based on whether a layer was in maxLayerIndexes or not
        foreach (int i in maxLayerIndexes)
        {
            musicLayers[i].targetVolume = 100f;
        }

        for (int i = 0; i < musicLayers.Length; i++)
        {
            if (!maxLayerIndexes.Contains(i) && !nonDebrisElements.Contains(allElements[i])) musicLayers[i].targetVolume = 0f;
        }
    }

    void UpdateFades() //applies the lerp between the current volume and target volume
    {
        foreach (MusicLayer m in musicLayers)
        {
            if (m.manualVolumeOverride) continue; //GUARD to allow manualVolumeOverride to disable the automated fading

            //Adding/subtracting a constant per second = linear transition.
            //Adjust the volume curve in FMOD, don't try and change the lerp curve
            if (m.volume > m.targetVolume) m.volume -= fadeOutRate * Time.deltaTime; 
            if (m.volume < m.targetVolume) m.volume += fadeInRate * Time.deltaTime;
            if (Mathf.Abs(m.volume - m.targetVolume) <= 0.1f) m.volume = m.targetVolume;

            //If we end up with targetVolumes that aren't 0 or 100, add an 'if volume delta <= 0.1, volume = target' here

            m.volume = Mathf.Clamp(m.volume, 0f, 100f);
        }
    }

    void UpdateLayerVolumes() //applies the current volumes to FMOD
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
        UpdateLayerVolumes();
    }

    void ResetAllScores() //clears the score values so they can be calculated afresh
    {
        foreach (MusicLayer m in musicLayers)
        {
            m.score = 0;
        }
    }

    void ReorderAllElements()
    {
        SO_Element[] temp = new SO_Element[allElements.Length];

        foreach (SO_Element e in allElements)
        {
            temp[GetIndexOfLayer(e.name)] = e;
        }
        allElements = temp;
    }


    // Helpers
    void S_OnElectricAbility()
    {
        musicLayers[GetIndexOfElement("Electric")].targetVolume = 100f;
        Invoke("ZeroElectricTargetVolume", nonDebrisFadeOutDelay);
    }

    void S_OnWindAbility()
    {
        musicLayers[GetIndexOfElement("Wind")].targetVolume = 100f;
        Invoke("ZeroWindTargetVolume", nonDebrisFadeOutDelay);
    }

    void ZeroElectricTargetVolume()
    {
        musicLayers[GetIndexOfLayer("Electric")].targetVolume = 0f;
    }

    void ZeroWindTargetVolume()
    {
        musicLayers[GetIndexOfLayer("Wind")].targetVolume = 0f;
    }


    // Functions
    int GetIndexOfLayer(string elementName)
    {
        for (int i = 0; i < musicLayers.Length; i++)
        {
            if (musicLayers[i].name == elementName) return i;
        }

        Debug.LogWarning("Could not find Music Layer with name " + elementName + ", returning -1, expect index errors.");
        return -1;
    }

    int GetIndexOfElement(string elementName) //this may be redundant as allElements should be getting re-ordered to match MusicLayers on Start()
    {
        for (int i = 0; i < allElements.Length; i++)
        {
            if (allElements[i].name == elementName) return i;
        }

        Debug.LogWarning("Could not find Element SO with name " + elementName + ", returning -1, expect index errors.");
        return -1;
    }
}
