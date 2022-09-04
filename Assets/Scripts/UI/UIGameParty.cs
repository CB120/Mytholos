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
    Myth[] myths;
    [SerializeField] UIGameMyth[] mythUIs;
    [SerializeField] CanvasGroup abilitiesMenu;
    RectTransform abilitiesMenuRectTransform;
    [SerializeField] UIGameAbility[] abilities;

    float abilitiesSelectedX;       // Records of target UI positions for the abilities menu
    float abilitiesUnselectedX;
    float abilitiesSelectedOffset = 10.0f;
    Coroutine abilitiesMenuCoroutine;

    // TO DELETE LATER!!!
    #region Test stuff to delete later
    public bool toggleAttacks = false;
    public int selectedMythNumber = 0;
    private void Update()
    {
        if (toggleAttacks)
        {
            toggleAttacks = false;
            if (abilitiesMenu.alpha == 1.0f) // Not a great way to check this
            {
                DisplayAbilities(false);
            }
            else
            {
                DisplayAbilities(true, selectedMythNumber);
            }
        }
    }
    #endregion

    void OnEnable()
    {
        // Record some transform information from UI
        abilitiesMenuRectTransform = abilitiesMenu.GetComponent<RectTransform>();
        abilitiesSelectedX = abilitiesMenuRectTransform.anchoredPosition.y;
        abilitiesUnselectedX = abilitiesSelectedX + abilitiesSelectedOffset * (partyNumber > 1 ? 1.0f : -1.0f);

        // TODO: Create listeners?
        // ...

        // Store party information
        myths = new Myth[3];
        SetUpMythUIs();

        // Update UI for game beginning
        DisplayAbilities(false, 0, -1.0f);
    }

    void DisplayAbilities(bool showUI, int partyMemberNumber = -1, float animationSpeed = 25.0f)
    {
        if (abilitiesMenuCoroutine != null)
        {
            StopCoroutine(abilitiesMenuCoroutine);
            SetAbilitiesMenuTargetVisuals(!showUI);
        }
        abilitiesMenuCoroutine = StartCoroutine(AnimateAbilitiesMenu(showUI, animationSpeed));

        if (showUI)
        {
            // Show the attack menu
            //abilitiesMenu.alpha = 1.0f;

            // Make all unselected myths greyed out, and vice versa
            int counter = 0;
            foreach (UIGameMyth mythUI in mythUIs)
            {
                mythUI.greyedOut = counter != partyMemberNumber;
                mythUI.selected = counter == partyMemberNumber;
                counter++;
                mythUI.UpdateOpacity();
            }

            // Populate it with relevant info
            for (int i = 0; i < 3; i++)
            {
                SO_Ability ability = i == 0 ? myths[partyMemberNumber].northAbility : i == 1 ? myths[partyMemberNumber].eastAbility : myths[partyMemberNumber].southAbility;
                abilities[i].UpdateUI(ability.name, E_Element.Wind, ability.damage);
            }

            // TODO: Place listeners for stamina
            // TODO: Update all visuals once (abilties with insufficient stamina are greyed out, the above listeners created above should call this same code later as well)
        }
        else
        {
            // Hide the attack menu
            //abilitiesMenu.alpha = 0.0f;

            // Make all myths ungreyed out
            foreach (UIGameMyth mythUI in mythUIs)
            {
                mythUI.greyedOut = false;
                mythUI.selected = false;
                mythUI.UpdateOpacity();
            }
        }
    }

    IEnumerator AnimateAbilitiesMenu(bool showUI, float animationSpeed) // Not awesome workaround; if animationSpeed is negative, we skip the while loop entirely
    {
        // Animate menu positon and alpha until it's reached it's target
        while (Mathf.Abs(abilitiesMenuRectTransform.anchoredPosition.x - (showUI ? abilitiesSelectedX : abilitiesUnselectedX)) > 0.5f && animationSpeed > 0.0f)
        {
            // Position
            float difference = (showUI ? abilitiesSelectedX : abilitiesUnselectedX) - abilitiesMenuRectTransform.anchoredPosition.x;
            abilitiesMenuRectTransform.anchoredPosition += Vector2.right * difference * animationSpeed * Time.deltaTime; // Janky ease in/out
            // Alpha
            abilitiesMenu.alpha = (showUI ? 1.0f : 0.0f) + (Mathf.Abs(difference / abilitiesSelectedOffset)) * ((showUI ? -1.0f : 1.0f));
            yield return new WaitForSeconds(0);
        }

        // Set alpha and position correctly and finish this coroutine
        SetAbilitiesMenuTargetVisuals(showUI);
    }

    void SetAbilitiesMenuTargetVisuals(bool showUI)
    {
        abilitiesMenuRectTransform.anchoredPosition = new Vector2((showUI ? abilitiesSelectedX : abilitiesUnselectedX), abilitiesMenuRectTransform.anchoredPosition.y);
        abilitiesMenu.alpha = showUI ? 1.0f : 0.0f;
    }

    // TODO: Party member number must be 0 or 1, and swaps them out with member at index 2 // Would it be better to pass in Myth/Participant instead?
    void SubPartyMember(int partyMemberNumber, float animationDuration = 0.25f)
    {

    }

    void SetUpMythUIs()
    {
        int counter = 0;

        if (partyNumber == 1 || partyNumber == 2)
        {
            GameObject partyParent = GameObject.FindGameObjectWithTag("Party " + partyNumber + " Parent"); // Is this a good way of doing this?
            if (partyParent != null)
            {
                for (int i = 0; i < partyParent.transform.childCount; i++)
                {
                    Myth myth = partyParent.transform.GetChild(i).GetComponent<Myth>();
                    if (myth != null)
                    {
                        if (i/*myth.partyIndex*/ < 3) // TODO: Hook up UI to myths based on their party order
                        {
                            myths[i/*myth.partyIndex*/] = myth;
                            counter++;
                            //Debug.Log("Player " + partyNumber + " adding myth " + myth.myth.name + " into party in slot " + i);
                        }
                    }
                }

                //Debug.Log("Player " + partyNumber + " has a team of " + counter + " myths");
                //if (counter < 3)
                //    Debug.LogWarning("Trying to set up UIGameParty, but player " + partyNumber + " only found " + counter + " myths - we'll look again...");

                // Place references within UIGameMyth children
                for (int i = 0; i < 3; i++)
                {
                    //Debug.Log("Player " + partyNumber + " adding myth " + i + "... Myth is valid? " + (myths[i] != null) + " Myth UI is valid? " + (mythUIs[i] != null));
                    if (myths[i] != null && mythUIs[i] != null)
                    {
                        mythUIs[i].SetMyth(myths[i]);
                        //Debug.Log("Player " + partyNumber + " adding myth " + myths[i].myth.name + " into party in slot " + i);
                    }
                }
            }
            //else
            //    Debug.LogWarning("Party " + partyNumber + "Parent doesn't exist in the scene - we'll look again...");

            if (counter < 1)
                StartCoroutine(TrySetUpMythUIs());
        }
        else
            Debug.LogWarning("UIGameParty has an invalid player number");
    }

    IEnumerator TrySetUpMythUIs() // UIGameParty will continually look for its myths in the scene until it finds them
    {
        yield return new WaitForSeconds(0);
        SetUpMythUIs();
    }
}