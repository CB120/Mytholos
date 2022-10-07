using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ControllerSetupSFX : MonoBehaviour
{
    // Properties


    // Variables
    bool p1SoundPlayed = false;


    // References
    public StudioEventEmitter p1Sound;
    public StudioEventEmitter p2Sound;


    // Called by InputManager
    public void PlayJoinSound()
    {
        if (p1SoundPlayed)
        {
            p2Sound.enabled = true;
        } else
        {
            p1Sound.enabled = true;
            p1SoundPlayed = true;
        }
    }
}
