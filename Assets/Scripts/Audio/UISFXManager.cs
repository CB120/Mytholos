using FMODUnity;
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

[System.Serializable]
public class EnoughStaminaSet : UISFXPair
{
    [Range(1, 3)] public int availableAbilities = 1;
    [Range(0, 1)] public int party = 0;
}

public class UISFXManager : MonoBehaviour
{
    // Properties


    // Variables


    // References
    static UISFXManager Singleton;

    public UISFXPair[] uiSFXpairs;
    public EnoughStaminaSet[] enoughStaminaSets;


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

    public static void PlayEnoughStamina(int party, int abilitiesAvailable)
    {
        if (!Singleton)
        {
            Debug.LogWarning("No UISFXManager Singleton found! Did you forget to put the prefab in this scene?");
            return;
        }

        Singleton.SpawnEnoughStamina(party, abilitiesAvailable);
    }

        
        // Private
    void SpawnSound(string sound)
    {
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

    void SpawnEnoughStamina(int party, int abilitiesAvailable)
    {
        party--; //we do parties 0 and 1 'round these parts (1 and 2 is what's provided by UIGameAbility)

        foreach (EnoughStaminaSet s in enoughStaminaSets)
        {
            if (s.party == party && s.availableAbilities == abilitiesAvailable)
            {
                GameObject soundSource = Instantiate(s.uiSFXPrefab);
                Destroy(soundSource, 1f);
                return;
            }
        }

        Debug.LogWarning("No 'Enough Stamina' set found with party " + party + " and abilitiesAvailable " + abilitiesAvailable + "!");
    }
}