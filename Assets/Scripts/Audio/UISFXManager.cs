using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UISFXPair
{
    [Tooltip("No relation to FMOD, just whatever other scripts are 'calling'. For consistency's sake please use the name set in FMOD. <3")]
    public string soundName;
    [Tooltip("Reference to the UI SFX Prefab")]
    public GameObject uiSFXPrefab;
}

public class UISFXManager : MonoBehaviour
{
    // Properties


    // Variables


    // References
    static UISFXManager Singleton;

    //[Space(30)]
    public UISFXPair[] uiSFXpairs;


    // Engine-called
    private void Awake()
    {
        if (Singleton != null && Singleton != this)
        {
            Destroy(gameObject);
            return;
        }

        Singleton = this;
    }


    // Methods
        // Public
            // Static
    public static void PlaySound(string sound)
    {
        if (Singleton)
        {
            Singleton.SpawnSound(sound);
        } else
        {
            Debug.LogWarning("No UISFXManager Singleton found! Did you forget to put the prefab in this scene?");
        }
    }

        
        // Private
    void SpawnSound(string sound)
    {
        // Add check for 'skipped' Nodes here

        foreach (UISFXPair p in uiSFXpairs)
        {
            if (p.soundName == sound)
            {
                GameObject soundSource = Instantiate(p.uiSFXPrefab);
                Destroy(soundSource, 1f);
                return;
            }
        }

        Debug.LogWarning("No sound pair found with string '" + sound + "'! Please check they are typed correctly.");
    }
}