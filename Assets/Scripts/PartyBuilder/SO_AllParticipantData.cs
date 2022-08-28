using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MythData //used sort of like a struct, bundling together a Participant's Myth selection and their ability choices for PartyBuilder
{
    public E_Myth myth;
    public E_Ability[] abilities = new E_Ability[3];
}


[System.Serializable]
public class PartyData
{
    [HideInInspector]
    public Participant participant; //reference to this party's Participant
    public Color colour;

    public MythData[] mythData = new MythData[3];
}

[CreateAssetMenu(fileName = "Untitled", menuName = "ScriptableObjects/AllParticipantData", order = 1)]
public class SO_AllParticipantData : ScriptableObject //accessed by BOTH Participants, their Myths and PartyBuilder to keep all the intra-Scene data centralised
{
    public PartyData[] partyData = new PartyData[2];
}
