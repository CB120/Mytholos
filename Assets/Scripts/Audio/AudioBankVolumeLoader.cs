using FMODUnity;
using UnityEngine;

public class AudioBankVolumeLoader : MonoBehaviour
{
    //Properties
    public string[] parameterNames;

    string playerPrefsKeyPrefix = "BusVolume_";


    // Engine-called
    void Start()
    {
        LoadVolumes();
        Destroy(gameObject);
    }


    // Methods
    void LoadVolumes()
    {
        foreach (string p in parameterNames)
        {
            RuntimeManager.StudioSystem.setParameterByName(p, PlayerPrefs.GetFloat(playerPrefsKeyPrefix + p, 90f), false);
        }
    }
}
