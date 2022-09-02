using System.Collections;
using System.Collections.Generic;
using Myths;
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

    private Vector3 currentSpawnPosition = new Vector3(0, 2, 0);
    private Vector3 spawnOffset = new Vector3(3, 0, 0);


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
            // Clear the myth lists since they will persist from previous sessions
            allParticipantData.partyData[p].myths.Clear();
            
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

        Vector3 spawnPosition = currentSpawnPosition; // transform.GetChild(0).position; //add logic here for when we know how spawn positions are gonna work

        currentSpawnPosition += spawnOffset;

        GameObject newMythGameObject = Instantiate(prefab, spawnPosition, Quaternion.identity, partyParents[participantIndex]);
        Myth newMyth = newMythGameObject.GetComponent<Myth>();
        allParticipantData.partyData[participantIndex].myths.Add(newMyth);
        newMyth.northAbility = mythData.northAbility;
        newMyth.westAbility = mythData.westAbility;
        newMyth.southAbility = mythData.southAbility;
        newMyth.eastAbility = mythData.eastAbility;
    }
}
