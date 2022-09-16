using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField] float abilitiesLazyHardcodedOffset;
    Coroutine abilitiesMenuCoroutine;

    [SerializeField] Sprite[] rings; // 0 is thin ring, 1 is thick

    //#region Test stuff to delete later
    //public bool toggleAttacks = false;
    //public int selectedMythNumber = 0;
    //private void Update()
    //{
    //    if (toggleAttacks)
    //    {
    //        toggleAttacks = false;
    //        if (abilitiesMenu.alpha == 1.0f) // Not a great way to check this
    //        {
    //            DisplayAbilities(false);
    //        }
    //        else
    //        {
    //            DisplayAbilities(true, selectedMythNumber);
    //        }
    //    }
    //}
    //#endregion

    void Start()
    {
        // Find party builder and place a listener into our team's party data so we know when there's a player participant for us to place listeners in
        PartyBuilder partyBuilder = FindObjectOfType<PartyBuilder>();
        partyBuilder.allParticipantData.partyData[partyNumber - 1].ParticipantChanged.AddListener(UpdateInputListeners);
        if (partyBuilder.allParticipantData.partyData[partyNumber - 1].participant != null)
            UpdateInputListeners(partyBuilder.allParticipantData.partyData[partyNumber - 1].participant);

    }

    private void Awake()
    {
        // Record some transform information from UI
        abilitiesMenuRectTransform = abilitiesMenu.GetComponent<RectTransform>();
        abilitiesSelectedX = abilitiesMenuRectTransform.anchoredPosition.y + abilitiesLazyHardcodedOffset;
        abilitiesUnselectedX = abilitiesSelectedX + abilitiesSelectedOffset * (partyNumber > 1 ? 1.0f : -1.0f);
    }

    void OnEnable()
    {
        // Store party information
        myths = new Myth[3];
        SetUpMythUIs();

        // Update UI for game beginning
        DisplayAbilities(false, 0, -1.0f);
    }

    void UpdateInputListeners(Participant participant)
    {
        PlayerParticipant playerParticipant = participant.GetComponent<PlayerParticipant>();

        if (playerParticipant != null)
        {
            playerParticipant.SelectMyth.AddListener(SelectMyth);
            playerParticipant.SelectAbility.AddListener(SelectAbility);
        }
    }

    void SelectMyth(int partyMemberNumber)
    {
        //if (partyMemberNumber >= 0)
        //    print("Trying to select myth " + partyMemberNumber + ", who is " + (mythUIs[partyMemberNumber].selected ? "" : "not")
        //        + " already selected. Other myth is " + (mythUIs[partyMemberNumber == 0 ? 1 : 0].selected ? "" : "not") + " already selected.");
        //else
        //    print("Trying to remove currently selected myth; myth 1 is " + (mythUIs[0].selected ? "" : "not")
        //        + " already selected, and myth 2 is " + (mythUIs[0 == 0 ? 1 : 0].selected ? "" : "not") + " already selected.");


        if (partyMemberNumber < 0 && abilitiesMenu.alpha <= 0.0f) return;                           // Don't animate menu closing if it's already closed
        if (mythUIs[partyMemberNumber == 0 ? 1 : 0].selected && partyMemberNumber >= 0) return;     // Don't animate if asking to display a myth, but other myth is already selected
        
        DisplayAbilities(partyMemberNumber >= 0, partyMemberNumber);
    }

    void SelectAbility(int abilityNumber)
    {
        if (abilityNumber >= 0 && abilityNumber < 3 && abilitiesMenu.alpha > 0.0f)
            abilities[abilityNumber].AnimateSelectedAbility();
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
            for (int i = 0; i < 2; i++)// (UIGameMyth mythUI in mythUIs)
            {
                UIGameMyth mythUI = mythUIs[i];
                mythUI.greyedOut = i != partyMemberNumber;
                mythUI.selected = i == partyMemberNumber;
                try {
                    myths[i].ring.sprite = rings[i == partyMemberNumber ? 1 : 0];
                }
                catch (Exception e) {
                    if (e != null) { } // Truly, incredible code
                }
                mythUI.UpdateOpacity();
            }

            // Populate it with relevant info
            for (int i = 0; i < 3; i++)
            {
                SO_Ability ability = i == 0 ? myths[partyMemberNumber].northAbility : i == 1 ? myths[partyMemberNumber].westAbility : myths[partyMemberNumber].southAbility;
                //print("Myth " + partyMemberNumber + ", ability " + i + " is " + ability.name + " / " + ability.ToString());
                abilities[i].UpdateUI(ability.name, ability.element, ability.damage);
            }

            // TODO: Place listeners for stamina
            // TODO: Update all visuals once (abilties with insufficient stamina are greyed out, the above listeners created above should call this same code later as well)
        }
        else
        {
            // Hide the attack menu
            //abilitiesMenu.alpha = 0.0f;

            // Make all myths ungreyed out
            for (int i = 0; i < 3; i++)
            {
                UIGameMyth mythUI = mythUIs[i];
                mythUI.greyedOut = false;
                mythUI.selected = false;
                try
                {
                    myths[i].ring.sprite = rings[0];
                }
                catch (Exception e) {
                }
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

                            // TODO: Delete this, 100% going to be problematic after sprint 2
                            if (i == 2)
                                myth.transform.position = new Vector3(1000.0f, 0.0f, -1000.0f);
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
