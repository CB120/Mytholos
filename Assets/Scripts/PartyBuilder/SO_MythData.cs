using UnityEngine;

[CreateAssetMenu(fileName = "Untitled", menuName = "ScriptableObjects/PartyBuilder MythData", order = 1)]
public class SO_MythData : ScriptableObject
{
    public E_Myth myth;
    public E_Ability[] abilities = new E_Ability[3];
}
