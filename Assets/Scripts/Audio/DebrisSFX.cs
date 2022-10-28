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
    float electricityTimer = 2f;


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

    private void Update()
    {
        electricityTimer -= Time.deltaTime;
        if (electricityTimer <= 0f) electricityLoop.SetParameter("Debris Volume Instant", 0);
    }


    // Called by other systems
    public void OnRegionDebrisChange() //called by DebrisRegion.numberOfTilesWithElementChanged
    {
        UpdateLoopVolumes();
    }   

    public void OnDebrisElectrificationChange() //called by DebrisRegion.numberOfElectrifiedTilesChanged
    {
        electricityLoop.SetParameter("Debris Volume Instant", region.NumberOfElectrifiedTiles * volumeScalar);
        electricityTimer = 1f;
    }
    

    // Methods
        // Private
    void UpdateLoopVolumes()
    {
        fireLoop.SetParameter("Debris Volume", region.NumberOfTilesWithElement(fireElement) * volumeScalar);
        iceLoop.SetParameter("Debris Volume", region.NumberOfTilesWithElement(iceElement) * volumeScalar);
    }
}
