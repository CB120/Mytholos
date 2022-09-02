using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyBuilder : MonoBehaviour
{
    //Properties
    public bool debugOn;


    //Variables


    //References
    [Header("ParticipantData")]
    public SO_AllParticipantData liveAllParticipantData;
    public SO_AllParticipantData debugAllParticipantData;
    SO_AllParticipantData allParticipantData;

    public Myth[] mythPrefabs;

    public SO_Ability[] allAbilities;

    Transform[] partyParents = new Transform[2];


    //Engine-called
    private void Awake()
    {
        allParticipantData = liveAllParticipantData;
        if (debugOn) allParticipantData = debugAllParticipantData;
    }

    void Start()
    {
        SetPartyParentReferences();
        SpawnParties();

        //Destroy(this.gameObject); //not sure if we want this yet?
    }


    //Methods
    void SetPartyParentReferences()
    {
        for (int i = 0; i < partyParents.Length; i++)
        {
            partyParents[i] = GameObject.FindWithTag("Party " + (i + 1) + " Parent").transform;
        }
    }

    void SpawnParties()
    {
        for (int p = 0; p < allParticipantData.partyData.Length; p++) 
        {
            foreach (MythData m in allParticipantData.partyData[p].mythData)
            {
                SpawnMyth(m, p);
            }
        }
    }

    void SpawnMyth(MythData mythData, int participantIndex)
    {
        GameObject prefab = null;
        foreach (Myth m in mythPrefabs)
        {
            if (m.myth == mythData.myth)
            {
                prefab = m.gameObject;
                break;
            }
        }

        if (prefab == null)
        {
            Debug.LogWarning("Could not find a matching prefab! Please check AllParticipantData enums are correct, and all Myth enums are correct. Returning.");
            return;
        }

        Vector3 spawnPosition = transform.GetChild(0).position; //add logic here for when we know how spawn positions are gonna work

        GameObject newMythGameObject = Instantiate(prefab, spawnPosition, Quaternion.identity, partyParents[participantIndex]);
        Myth newMyth = newMythGameObject.GetComponent<Myth>();
        allParticipantData.partyData[participantIndex].myths.Add(newMyth);
        newMyth.northAbility = GetAbilityReference(mythData.northAbility);
        newMyth.westAbility = GetAbilityReference(mythData.westAbility);
        newMyth.southAbility = GetAbilityReference(mythData.southAbility);
        newMyth.eastAbility = GetAbilityReference(mythData.eastAbility);
    }

    SO_Ability GetAbilityReference(E_Ability ability)
    {
        foreach (SO_Ability a in allAbilities)
        {
            if (a.abilityType == ability) return a;
        }

        Debug.LogWarning("Could not find an ability that matched this enum! Please check all Ability ScriptableObjects have the correct ability enums set. Returning null.");
        return null;
    }
}
