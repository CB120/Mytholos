using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Participant : MonoBehaviour
{
    //Properties
    [Tooltip("If ticked, allows all non-critical Debug messages to be shown")]
    public bool debugOn;


    //Variables
        //Static
    static int numberOfParticipants = 0; //used to track which party index each participant should use

    protected int partyIndex = -1; //0 = Player 1, 1 = Player 2. Used to fetch the correct element from the SO_AllParticipantData array


    //References
    public SO_AllParticipantData liveParticipantData;
    public SO_AllParticipantData debugParticipantData;


    //Engine-called
    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);

        partyIndex = numberOfParticipants;
        numberOfParticipants++;
        if (debugOn) Debug.Log(gameObject.name + "'s partyIndex is " + partyIndex);

        liveParticipantData.partyData[partyIndex].Participant = this;
        debugParticipantData.partyData[partyIndex].Participant = this;
    }
}
