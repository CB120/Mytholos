using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Participant : MonoBehaviour
{
    //Properties
    [SerializeField]
    public static bool debugOn; //Ethan: Something I like to add to my scripts, allows me to 'mute' all my optional Debug messages


    //Variables
        //Static
    static int numberOfParticipants = 0; //used to track which party index each participant should use

    int partyIndex = -1; //0 = Player 1, 1 = Player 2. Used to fetch the correct element from the SO_AllParticipantData array


    //References
    [HideInInspector] 
    public Myth[] party = { null, null, null }; //3 references to the Myth scripts on the Myth GameObjects in this Participant's party. Auto-populated in Arena scene by PartyBuilder


    //Engine-called
    protected void Awake()
    {
        DontDestroyOnLoad(gameObject);

        partyIndex = numberOfParticipants;
        numberOfParticipants++;
        if (debugOn) Debug.Log(gameObject.name + "'s partyIndex is " + partyIndex);
    }
}
