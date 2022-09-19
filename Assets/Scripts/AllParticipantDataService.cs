using UnityEngine;

[CreateAssetMenu]
public class AllParticipantDataService : ScriptableObject
{
    [SerializeField] private bool useDebugParticipantData;
    [SerializeField] private SO_AllParticipantData liveAllParticipantData;
    [SerializeField] private SO_AllParticipantData debugAllParticipantData;

    public SO_AllParticipantData GetAllParticipantData()
    {
        return useDebugParticipantData ? debugAllParticipantData : liveAllParticipantData;
    }
}