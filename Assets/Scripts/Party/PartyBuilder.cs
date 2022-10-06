using System;
using Myths;
using System.Collections.Generic;
using UnityEngine;

public class PartyBuilder : MonoBehaviour
{
    [SerializeField] private AllParticipantDataService allParticipantDataService;

    [NonSerialized] public SO_AllParticipantData allParticipantData;

    [SerializeField] private SO_SpawnPoints spawnPoints;

    // Keep these public lol
    public List<GameObject> Party1;
    public List<GameObject> Party2;

    public SO_Ability[] allAbilities;

    private int mythCounter = 0;

    Transform[] partyParents = new Transform[2];

    private void Awake()
    {
        allParticipantData = allParticipantDataService.GetAllParticipantData();

        //Ethan: these two lines were originall in Start(), moved them for BattleMusicController. If they're causing issues, move them back and tell me
        SetPartyParentReferences();
        SpawnParties();
        SetDefaultTarget();
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
                SpawnMyth(m,p);
            }

        }
    }

    void SetDefaultTarget() // Will need to set this to be the myth in play instead
    {
        if (Party1.Count == Party2.Count && Party1 != null)
        {
            for (int i = 0; i < Party1.Count; i++)
            {
                Party1[i].GetComponent<Myth>().targetEnemy = Party2[i].gameObject;
                Party2[i].GetComponent<Myth>().targetEnemy = Party1[i].gameObject;
            }
        }
    }

    void SpawnMyth(MythData mythData, int participantIndex)
    {
        
        GameObject prefab = mythData.myth.prefab;
        Vector3 spawnPosition;
        
        if (prefab == null)
        {
            Debug.LogWarning($"Could not find a matching prefab! {nameof(MythData)} may not have " +
                            $"an assigned {nameof(SO_Myth)}, or {nameof(SO_Myth)} may not have an assigned {nameof(SO_Myth.prefab)}.");
            return;
        }

        spawnPosition = spawnPoints.SpawnLocations[mythCounter];

        GameObject newMythGameObject = Instantiate(prefab, spawnPosition, Quaternion.identity, partyParents[participantIndex]);
        Myth newMyth = newMythGameObject.GetComponent<Myth>();
        allParticipantData.partyData[participantIndex].myths.Add(newMyth);
        
        
        if (participantIndex == 1)
        {
            Party1.Add(newMythGameObject);
        }
        else
        {
            Party2.Add(newMythGameObject);
        }


        // TODO: I want it on the record that I don't like this
        newMyth.northAbility = mythData.northAbility;
        newMyth.westAbility = mythData.westAbility;
        newMyth.southAbility = mythData.southAbility;
        newMyth.eastAbility = mythData.eastAbility;
        newMyth.PartyIndex = participantIndex;
        newMyth.ws = winState;
        // Eddie's awful camera code, probably delete later
        EpicEddieCam cam = FindObjectOfType<EpicEddieCam>();
        if (cam != null && mythCounter == 0 || mythCounter == 3)
        {
            cam.positions.Add(newMythGameObject.transform);
        }
        newMythGameObject.SetActive(false);
        if (mythCounter == 0 || mythCounter == 3)
        {
            //Debug.Log(mythCounter);
            newMythGameObject.SetActive(true);
        }
        mythCounter ++; // TODO: Stop fudging this where 3rd member of each party is excluded from Camera list
    }



    //Remove after playtest
    public WinState winState;  
}
