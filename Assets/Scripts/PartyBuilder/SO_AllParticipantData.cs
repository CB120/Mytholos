using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MythData //used sort of like a struct, bundling together a Participant's Myth selection and their ability choices for PartyBuilder
{
    public E_Myth myth;
    public E_Ability northAbility;
    public E_Ability westAbility;
    public E_Ability southAbility;
    public E_Ability eastAbility = E_Ability.Dodge;
}


[System.Serializable]
public class PartyData
{
    [HideInInspector]
    public Participant participant; //reference to this party's Participant
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
