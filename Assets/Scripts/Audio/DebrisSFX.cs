using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class DebrisSFX : MonoBehaviour
{
    // Properties


    // Variables


    // References
    [Header("FMOD Emitters")]
    public StudioEventEmitter fireLoop;
    public StudioEventEmitter electricityLoop;
    public StudioEventEmitter iceLoop;


    // Engine-called
    void Start()
    {
        fireLoop.SetParameter("Debris Volume", 0f);
        electricityLoop.SetParameter("Debris Volume", 0f);
        iceLoop.SetParameter("Debris Volume", 0f);
    }


    // Methods
        // Public
    
}
