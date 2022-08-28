using UnityEngine;

[CreateAssetMenu(fileName = "Untitled", menuName = "ScriptableObjects/PartyData", order = 1)]
public class SO_PartyData : ScriptableObject
{
    public Participant participant;
    public Color colour;

    public SO_MythData[] mythData;
}
