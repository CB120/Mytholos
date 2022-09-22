using System;
using Myths;
using UnityEngine;

public class PartyBuilder : MonoBehaviour
{
    [SerializeField] private AllParticipantDataService allParticipantDataService;

    [NonSerialized] public SO_AllParticipantData allParticipantData;

    public SO_Ability[] allAbilities;

    Transform[] partyParents = new Transform[2];
    private Vector3 currentSpawnPosition = new Vector3(0, 2, 0);
    private Vector3 spawnOffset = new Vector3(3, 0, 0);

    private void Awake()
    {
        allParticipantData = allParticipantDataService.GetAllParticipantData();

        //Ethan: these two lines were originall in Start(), moved them for BattleMusicController. If they're causing issues, move them back and tell me
        SetPartyParentReferences();
        SpawnParties();
    }

    void Start()
    {
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

    // TODO: Delete this I'm so sorry --Eddie
    int partyCounter = 0;

    void SpawnMyth(MythData mythData, int participantIndex)
    {
        GameObject prefab = mythData.myth.prefab;

        if (prefab == null)
        {
            Debug.LogWarning($"Could not find a matching prefab! {nameof(MythData)} may not have " +
                            $"an assigned {nameof(SO_Myth)}, or {nameof(SO_Myth)} may not have an assigned {nameof(SO_Myth.prefab)}.");
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
        newMyth.PartyIndex = participantIndex;
        newMyth.ws = winState;

        // Eddie's awful camera code, probably delete later
        EpicEddieCam cam = FindObjectOfType<EpicEddieCam>();
        if (cam != null && partyCounter % 3 != 2)
        {
            cam.positions.Add(newMythGameObject.transform);
        }
        partyCounter++; // TODO: Stop fudging this where 3rd member of each party is excluded from Camera list
    }

    //Remove after playtest
    public WinState winState;

  
}
