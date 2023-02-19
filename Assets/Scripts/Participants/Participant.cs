using UnityEngine;

namespace Participants
{
    public class Participant : MonoBehaviour
    {
        [SerializeField] private AllParticipantDataService allParticipantDataService;
    
        //Properties
        [Tooltip("If ticked, allows all non-critical Debug messages to be shown")]
        public bool debugOn;


        //Variables
        //Static
        static int numberOfParticipants = 0; //used to track which party index each participant should use

        /*protected*/ public int partyIndex = -1; //0 = Player 1, 1 = Player 2. Used to fetch the correct element from the SO_AllParticipantData array

        protected SO_AllParticipantData ParticipantData { get; private set; }

        //Engine-called
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

            partyIndex = numberOfParticipants;
            numberOfParticipants++;
        
            if (debugOn) Debug.Log(gameObject.name + "'s partyIndex is " + partyIndex);

            // TODO: This also should not be handled here maybe possibly
            UpdateParticipantData();
        }

        public void UpdateParticipantData()
        {
            ParticipantData = allParticipantDataService.GetAllParticipantData();
            ParticipantData.partyData[partyIndex].Participant = this;
        }

        public void DestroyParticipant()
        {
            numberOfParticipants--; // May cause issues if this isn't being called on every participant to destroy all participants
            Destroy(gameObject);
        }
    }
}
