using System;
using Myths;
using System.Collections.Generic;
using UnityEngine;

public class PartyBuilder : MonoBehaviour
{
    [SerializeField] private AllParticipantDataService allParticipantDataService;
    [SerializeField] private PlayerParticipantRuntimeSet playerParticipantRuntimeSet;

    [NonSerialized] public SO_AllParticipantData allParticipantData;

    [SerializeField] private SO_SpawnPoints spawnPoints;

    // Keep these public lol
    public List<GameObject> Party1;
    public List<GameObject> Party2;
    
    private int mythCounter = 0;

    Transform[] partyParents = new Transform[2];

    private void Awake()
    {
        allParticipantData = allParticipantDataService.GetAllParticipantData();
        //Ethan: these two lines were originall in Start(), moved them for BattleMusicController. If they're causing issues, move them back and tell me
        SetPartyParentReferences();
        SpawnParties();
        // SetDefaultTarget();
    }

    private void OnEnable()
    {
        playerParticipantRuntimeSet.itemAdded.AddListener(InitialisePlayerParticipant);
    }
    
    private void OnDisable()
    {
        playerParticipantRuntimeSet.itemAdded.RemoveListener(InitialisePlayerParticipant);
    }

    private void InitialisePlayerParticipant(PlayerParticipant playerParticipant)
    {
        playerParticipant.Initialise();
        
        playerParticipant.mythInPlayChanged.AddListener(OnMythInPlayChanged);
        
        OnMythInPlayChanged(playerParticipant);
    }

    private void OnMythInPlayChanged(PlayerParticipant playerParticipant)
    {
        allParticipantData.partyData[1 - playerParticipant.partyIndex].myths.ForEach(myth => myth.targetEnemy = playerParticipant.MythInPlay.gameObject);
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
            var partyData = allParticipantData.partyData[p];
            partyData.myths.Clear();
            
            foreach (MythData m in partyData.mythData)
            {
                SpawnMyth(m,p);
            }
        }

        foreach (var playerParticipant in playerParticipantRuntimeSet.items)
        {
            InitialisePlayerParticipant(playerParticipant);
        }
    }

    public void mythDeathSwap(int PartyIndex)
    {
        foreach (PlayerParticipant participant in playerParticipantRuntimeSet.items)
        {
            if (participant.partyIndex == PartyIndex)
            {
                participant.SwapInDirection(1);
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
        
        
        if (participantIndex == 0)
        {
            Party1.Add(newMythGameObject);
        }
        else
        {
            Party2.Add(newMythGameObject);
        }
        
        newMyth.Initialise(mythData);

        // TODO: Is this needed
        newMyth.PartyIndex = participantIndex;
        newMyth.pb = this;
        
        newMythGameObject.SetActive(false);
        
        mythCounter ++;
    }
}
