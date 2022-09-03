using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameParty : MonoBehaviour
{
    // TODO: Store a reference to relevant player's party and/or party members
    [SerializeField] UIGameMyth[] partyMembers;
    [SerializeField] CanvasGroup abilitiesMenu;
    [SerializeField] UIGameAbility[] abilities;

    void OnEnable()
    {
        // TODO: Create listeners

        // ...
    }

    void DisplayAbilities(int partyMemberNumber, bool showUI = true)
    {

    }

    void SubPartyMember(int partyMemberNumber) // Party member number must be 0 or 1, and swaps them out with member at index 2 // Would it be better to pass in Myth/Participant instead?
    {

    }
}
