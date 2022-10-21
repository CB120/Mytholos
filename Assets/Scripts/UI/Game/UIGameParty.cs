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
    public int partyNumber; // 1 or 2, used to find party by tag name
    Myth[] myths;
    [SerializeField] UIGameMyth[] mythUIs;
    [SerializeField] UIGameHovering hoveringUI;
    [SerializeField] UIAnimator cursor;
    [SerializeField] CanvasGroup abilitiesMenu;
    RectTransform abilitiesMenuRectTransform;
    [SerializeField] UIGameAbility[] abilities;
    Dictionary<SO_Ability, UIGameAbility> abilityUIByAbilitySO = new();
    PartyBuilder partyBuilder;

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

    private void Awake()
    {
        // Record some transform information from UI
        abilitiesMenuRectTransform = abilitiesMenu.GetComponent<RectTransform>();
        abilitiesSelectedX = abilitiesMenuRectTransform.anchoredPosition.y + abilitiesLazyHardcodedOffset;
        abilitiesUnselectedX = abilitiesSelectedX + abilitiesSelectedOffset * (partyNumber > 1 ? 1.0f : -1.0f);

        foreach (UIGameAbility a in abilities)
        {
            a.thisGameParty = this;
        }
    }

    void OnEnable()
    {
        // Store party information
        myths = new Myth[3];
        SetUpMythUIs();
    }

    void Start()
    {
        // Find party builder and place a listener into our team's party data so we know when there's a player participant for us to place listeners in
        partyBuilder = FindObjectOfType<PartyBuilder>();
        partyBuilder.allParticipantData.partyData[partyNumber - 1].ParticipantChanged.AddListener(UpdateInputListeners);
        if (partyBuilder.allParticipantData.partyData[partyNumber - 1].participant != null)
            UpdateInputListeners(partyBuilder.allParticipantData.partyData[partyNumber - 1].participant);
        else
            Debug.LogWarning("UI Party failed to locate their player participant");

        //if (partyBuilder)
        //    SelectMyth(partyBuilder.allParticipantData.partyData[partyNumber - 1].participant as PlayerParticipant);
    }

    void UpdateInputListeners(Participant participant)
    {
        PlayerParticipant playerParticipant = participant.GetComponent<PlayerParticipant>();

        if (playerParticipant != null)
        {
            //playerParticipant.SelectMyth.AddListener(SelectMyth);
            playerParticipant.mythInPlayChanged.AddListener(SelectMyth);
            // TODO: Unlisten?
            playerParticipant.SelectAbility.AddListener(SelectAbility);

            // Awful awful awful
            StartCoroutine(SelectMythDelayed(playerParticipant));
        }
    }

    // Awful awful awful
    IEnumerator SelectMythDelayed(PlayerParticipant playerParticipant)
    {
        yield return new WaitForEndOfFrame();
        SelectMyth(playerParticipant);
    }

    void SelectMyth(PlayerParticipant participant)
    {
        // Find index of the newly selected myth for later use
        int partyMemberNumber = -1;
        for (int i = 0; i < myths.Length; i++)
        {
            if (myths[i] == participant.MythInPlay)
                partyMemberNumber = i;
        }

        // Update team UI to highlight selected myth, and unhighlight any unselected myths
        for (int i = 0; i < 3; i++)
            mythUIs[i].UpdateUI(partyMemberNumber);

        // Update hovering UI
        hoveringUI.SetMyth(myths[partyMemberNumber]);

        // Update team cursor
        if (cursor)
            cursor.SetTransform(mythUIs[partyMemberNumber].GetComponent<RectTransform>());

        // Update the displayed abilities UI
        DisplayAbilities(participant, partyMemberNumber);
    }

    void SelectAbility(SO_Ability ability)
    {
        // TODO: Do not animate the ability UI if the myth is currently engaged and will not actually use the attack (this doesn't get called if stamina is insufficient)

        if (abilityUIByAbilitySO.ContainsKey(ability) && abilitiesMenu.alpha > 0.0f)
            abilityUIByAbilitySO[ability].AnimateSelectedAbility();
    }

    void DisplayAbilities(PlayerParticipant participant, int partyMemberNumber = -1, float animationSpeed = 25.0f)
    {
        //if (abilitiesMenuCoroutine != null)
        //{
        //    StopCoroutine(abilitiesMenuCoroutine);
        //    SetAbilitiesMenuTargetVisuals(!showUI);
        //}
        //abilitiesMenuCoroutine = StartCoroutine(AnimateAbilitiesMenu(showUI, animationSpeed));

        // Show the attack menu
        SetAbilitiesMenuTargetVisuals(true);

        // Populate it with relevant info
        for (int i = 0; i < 3; i++)
        {
            SO_Ability ability = i == 0 ? myths[partyMemberNumber].northAbility : i == 1 ? myths[partyMemberNumber].westAbility : myths[partyMemberNumber].southAbility;
            abilities[i].UpdateUI(ability, myths[partyMemberNumber]);
            abilityUIByAbilitySO[ability] = abilities[i];
        }

        // TODO: Place listeners for stamina
        // TODO: Update all visuals once (abilties with insufficient stamina are greyed out, the above listeners created above should call this same code later as well)
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
        bool setUpSucceeded = false;

        if (partyNumber == 1 || partyNumber == 2)
        {
            GameObject partyParent = GameObject.FindGameObjectWithTag("Party " + partyNumber + " Parent");
            if (partyParent != null)
            {
                for (int i = 0; i < partyParent.transform.childCount; i++)
                {
                    Myth myth = partyParent.transform.GetChild(i).GetComponent<Myth>();
                    if (myth != null)
                    {
                        if (i < 3)
                        {
                            myths[i] = myth;
                            setUpSucceeded = true;
                        }
                    }
                }

                // Place references within UIGameMyth children
                for (int i = 0; i < 3; i++)
                {
                    if (myths[i] != null && mythUIs[i] != null)
                        mythUIs[i].SetMyth(myths[i], i);
                }
            }

            if (!setUpSucceeded)
                StartCoroutine(TrySetUpMythUIs());
            else if (partyBuilder)
                SelectMyth(partyBuilder.allParticipantData.partyData[partyNumber - 1].participant as PlayerParticipant);

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
