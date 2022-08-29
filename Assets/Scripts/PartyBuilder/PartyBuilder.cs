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

    GameObject[] partyParents = new GameObject[2];


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
    }


    //Methods
    void SetPartyParentReferences()
    {
        for (int i = 0; i < partyParents.Length; i++)
        {
            partyParents[i] = GameObject.FindWithTag("Party " + (i + 1) + " Parent");
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
        //newMyth = Instantiate(<mythData.myth -> prefab>, partyParents[p], <correct spawn point>);
        //allParticipantData.partyData[p].participant.party.Add(newMyth);
        //
    }
}
