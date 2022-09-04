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
    [SerializeField] UIGameMyth[] mythUIs;
    [SerializeField] CanvasGroup abilitiesMenu;
    [SerializeField] UIGameAbility[] abilities;

    void OnEnable()
    {
        // TODO: Create listeners
        // ...

        // Store party information
        SetUpMythUIs();

        // Update UI for game beginning
        DisplayAbilities(false, 0, 0);
    }

    void DisplayAbilities(bool showUI, int partyMemberNumber = -1, float animationDuration = 0.25f)
    {

    }

    // Party member number must be 0 or 1, and swaps them out with member at index 2 // Would it be better to pass in Myth/Participant instead?
    void SubPartyMember(int partyMemberNumber, float animationDuration = 0.25f)
    {

    }

    void SetUpMythUIs()
    {
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
                        if (i/*myth.partyIndex*/ < 3) // TODO: Hook up UI to myths based on their party index
                        {
                            myths[i/*myth.partyIndex*/] = myth;
                            counter++;
                        }
                    }
                }

                if (counter < 3)
                    Debug.LogWarning("Trying to set up UIGameParty, but player " + partyNumber + " only found " + counter + " myths");

                // Place references within UIGameMyth children
                for (int i = 0; i < 3; i++)
                    mythUIs[i].SetMyth(myths[i]);
            }
            else
                Debug.LogWarning("Party " + partyNumber + "Parent doesn't exist in the scene");
        }
        else
            Debug.LogWarning("UIGameParty has an invalid player number");
    }
}
