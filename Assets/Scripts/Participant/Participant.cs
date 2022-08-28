using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Participant : MonoBehaviour
{
    //Properties


    //Variables


    //References
    [Tooltip("Reference to this Participant's partyData generated in the Main Menu")]
    public SO_PartyData partyData;

    [HideInInspector] 
    public Myth[] party = { null, null, null }; //3 references to the Myth scripts on the Myth GameObjects in this Participant's party. Auto-populated in Arena scene by PartyBuilder
}
