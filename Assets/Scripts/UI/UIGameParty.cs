using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Myths;

public class UIGameParty : MonoBehaviour
{
    // TODO: Store a reference to relevant player's party and/or party members
    [SerializeField] int partyNumber; // 1 or 2, used to find party by tag name
    Myth[] myths = new Myth[3];
    [SerializeField] UIGameMyth[] partyMembers;
    [SerializeField] CanvasGroup abilitiesMenu;
    [SerializeField] UIGameAbility[] abilities;

    void OnEnable()
    {
        // TODO: Create listeners
        // ...

        // Store party information
        if (partyNumber == 1 || partyNumber == 2)
        {
            GameObject partyParent = GameObject.FindGameObjectWithTag("Party " + partyNumber + " Parent");
            int counter = 0;
            if (partyParent != null)
            {
                for (int i = 0; i < partyParent.transform.childCount; i++)
                {
                    Myth myth = partyParent.transform.GetChild(i).GetComponent<Myth>();
                    if (myth != null)
                    {
                        if (myth.partyIndex < 3)
                        {
                            myths[myth.partyIndex] = myth;
                            counter++;
                        }
                    }
                }
            }
            if (counter < 3)
                Debug.LogWarning("Trying to set up UIGameParty, but player " + partyNumber + " only found " + counter + " myths");
        }
        else
            Debug.LogWarning("UIGameParty has an invalid player number");
    }

    void DisplayAbilities(int partyMemberNumber, bool showUI = true)
    {

    }

    void SubPartyMember(int partyMemberNumber) // Party member number must be 0 or 1, and swaps them out with member at index 2 // Would it be better to pass in Myth/Participant instead?
    {

    }
}
