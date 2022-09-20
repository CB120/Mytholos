using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BattleMusicController : MonoBehaviour
{
    // Properties


    // Variables
    [Header("Debug readouts of current Element volumes")]
    [Range(0, 100)] public float earthVolume = 0f;
    [Range(0, 100)] public float electricVolume = 0f;
    [Range(0, 100)] public float fireVolume = 0f;
    [Range(0, 100)] public float iceVolume = 0f;
    [Range(0, 100)] public float steelVolume = 0f;
    [Range(0, 100)] public float waterVolume = 0f;
    [Range(0, 100)] public float windVolume = 0f;
    [Range(0, 100)] public float woodVolume = 0f;


    // References
    StudioEventEmitter battleMusicEmitter;

    [Space(30)]
    [Header("Asset References")]
    public SO_Element[] allElements;


    //Engine-called
    private void Awake()
    {
        battleMusicEmitter = GetComponent<StudioEventEmitter>();
    }

    void Update()
    {
        CalculateElementVolumes();
        UpdateElementVolumes();
    }


    //Listener-called
    public void OnDebrisChange() //Called by DebrisController
    {
        Debug.Log("hello!");
    }


    //Methods
        //Private
    void CalculateElementVolumes()
    {

        //FMOD seems to automatically clamp the values in the 0-100 range set for each parameter. Apply a Mathf.Clamp() if bugs occur
    }

    void UpdateElementVolumes() //yes there's a lot of repeated code, but it's way easier to read and understand in FMOD doing it this way.
                                //could be improved by a Serializable string & float struct
    {
        battleMusicEmitter.SetParameter("Earth Volume", earthVolume);
        battleMusicEmitter.SetParameter("Electric Volume", electricVolume);
        battleMusicEmitter.SetParameter("Fire Volume", fireVolume);
        battleMusicEmitter.SetParameter("Ice Volume", iceVolume);
        battleMusicEmitter.SetParameter("Steel Volume", steelVolume);
        battleMusicEmitter.SetParameter("Water Volume", waterVolume);
        battleMusicEmitter.SetParameter("Wind Volume", windVolume);
        battleMusicEmitter.SetParameter("Wood Volume", woodVolume);
    }
}
