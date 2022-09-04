using System.Collections.Generic;
using Myths;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PartyData
{
    public UnityEvent<Participant> ParticipantChanged = new();

    [HideInInspector]
    public Participant participant; //reference to this party's Participant
    public Participant Participant
    {
        get => participant;
        set
        {
            participant = value;
            ParticipantChanged.Invoke(value);
        }
    }
    [HideInInspector]
    public List<Myth> myths = new List<Myth>(); //3 references to the Myth scripts on the Myth GameObjects in this Participant's party. Auto-populated in Arena scene by PartyBuilder

    public Color colour;

    public MythData[] mythData = new MythData[3];
}

[CreateAssetMenu(fileName = "Untitled", menuName = "ScriptableObjects/AllParticipantData", order = 1)]
public class SO_AllParticipantData : ScriptableObject //accessed by BOTH Participants, their Myths and PartyBuilder to keep all the intra-Scene data centralised
{
    public PartyData[] partyData = new PartyData[2];
}
