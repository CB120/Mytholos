using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Debris;

public class DebrisSFX : MonoBehaviour
{
    // Properties


    // Variables
    [HideInInspector] public int regionWidth = 17;
    [HideInInspector] public float debrisVolumeScalar = 1f;

    float volumeScalar;


    // References
    [Header("Scene References")]
    public DebrisRegion region;

    [Header("FMOD Emitters")]
    public StudioEventEmitter fireLoop;
    public StudioEventEmitter electricityLoop;
    public StudioEventEmitter iceLoop;

    [HideInInspector] public SO_Element fireElement;
    [HideInInspector] public SO_Element electricityElement;
    [HideInInspector] public SO_Element iceElement;


    // Engine-called
    void Start()
    {
        fireLoop.SetParameter("Debris Volume", 0f);
        electricityLoop.SetParameter("Debris Volume", 0f);
        iceLoop.SetParameter("Debris Volume", 0f);

        volumeScalar = 1 / Mathf.Pow(regionWidth, 2) * debrisVolumeScalar;
    }


    // Called by other systems
    public void OnRegionDebrisChange() //called by DebrisRegion.numberOfTilesWithElementChanged
    {
        UpdateLoopVolumes();
    }   
    

    // Methods
        // Private
    void UpdateLoopVolumes()
    {
        float fireValue = region.NumberOfTilesWithElement(fireElement) * volumeScalar;
        Debug.Log(fireValue);
        fireLoop.SetParameter("Debris Volume", fireValue);
        electricityLoop.SetParameter("Debris Volume", region.NumberOfTilesWithElement(electricityElement) * volumeScalar);
        iceLoop.SetParameter("Debris Volume", region.NumberOfTilesWithElement(iceElement) * volumeScalar);
    }
}
