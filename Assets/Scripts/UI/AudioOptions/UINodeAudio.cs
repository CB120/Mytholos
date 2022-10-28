using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINodeAudio : UIMenuNode
{
    //Properties
    [SerializeField] Slider volume;
    [SerializeField] float increments;
    [SerializeField] string parameterName;

    string playerPrefsKeyPrefix = "BusVolume_";


    //Engine-called
    private void Start()
    {
        volume.value = PlayerPrefs.GetFloat(playerPrefsKeyPrefix + parameterName, 90f);
        RuntimeManager.StudioSystem.setParameterByName(parameterName, volume.value, false);
    }


    //Called by UI Systems
    public override void OnAudioFuckYa(Direction direction, int playerNumber) //the name was Christian
    {
        base.OnAudioFuckYa(direction, playerNumber);
        switch (direction)
        {
            case Direction.Left:
                volume.value -= increments;
                break;
            case Direction.Right:
                volume.value += increments;
                break;
            default:
                break;
        }

        RuntimeManager.StudioSystem.setParameterByName(parameterName, volume.value, false);
        
        PlayerPrefs.SetFloat(playerPrefsKeyPrefix + parameterName, volume.value);
    }
}
