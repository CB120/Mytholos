using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using Myths;

public class UIPartyManager : MonoBehaviour
{
    // Asset references
    [Header("Asset references")]
    [SerializeField] Texture[] rendersTeamTextures;
    [SerializeField] Texture[] rendersPortraitTextures;
    [SerializeField] SO_Myth[] mythSOs;
    [SerializeField] SO_AllParticipantData livePartyData; // We're not going to write into this until we leave the scene (use myth datas on each UIPartyTeam for each player until then)
    //[SerializeField] SO_AllParticipantData templatePartyData; // What we copy into live data, so we have a clear slate (maybe optional, if returning straight from gameplay scene)
    [SerializeField] GameObject mythSelectPrefab;
    [SerializeField] GameObject mythAbilityPrefab;

    // Scene references
    [Header("Scene references")]
    [SerializeField] UIMenuNodeGraph[] playerTeamGraphs;
    [SerializeField] UIMenuNodeGraph mythSelectionGraph;
    [SerializeField] UIMenuNodeGraph[] playerAbilityGraphs;
    [SerializeField] UIPartyMyth[] playerMythDetails; // Can be fetched from playerAbilityGraphs?
    [SerializeField] UIPartyTeam[] playerTeamDetails; // Can be fetched from playerTeamGraphs?
    [SerializeField] TextMeshProUGUI[] progressTexts;
    [SerializeField] CanvasGroup startPrompt;
    [SerializeField] Animator transitionAnimator;

    [Header("To set in editor")]
    [SerializeField] string nameOfNextScene;

    // Variables
    bool[] playersReady = new bool[2];
    bool allPlayersReady;

    void Start()
    {
        MenuMusicController.ChangeMenuState(E_MenuState.MythSelect);

        startPrompt.alpha = 0.0f;

        //SetUpPartyDataScriptableObjects(); // Might use this later down the line

        // Store references to this manager in team nodes
        foreach (UIMenuNodeGraph graph in playerTeamGraphs)
        {
            foreach (UINodePartyMember partyMember in graph.nodes)
            {
                partyMember.manager = this;
            }
        }
        // For each myth in the array
        for (int i = 0; i < mythSOs.Length; i++)
        {
            // Currently, all camera/render texture set up is done manually in the scene, but doing it here could be more flexible a system

            // Create an icon/menu node, adding it to the horizontal layout group of selectable myth icons, and set up it's adjacent nodes (and amend node to left of it)
            UINodeMyth selectNode = Instantiate(mythSelectPrefab, mythSelectionGraph.transform).GetComponent<UINodeMyth>();
            selectNode.name = "MythIcon" + i;
            mythSelectionGraph.nodes.Add(selectNode);
            selectNode.adjacent[(int)UIMenuNode.Direction.Left] = mythSelectionGraph.nodes[Mathf.Clamp(i - 1, 0, 1000)].gameObject;     // Connect node to left adjacent node (left)
            selectNode.adjacent[(int)UIMenuNode.Direction.Right] = mythSelectionGraph.nodes[0].gameObject;                              // Connect node to first node (right)
            selectNode.adjacent[(int)UIMenuNode.Direction.Up] = playerTeamGraphs[0].transform.parent.gameObject;                        // Connect node to teams split graph (up)
            selectNode.adjacent[(int)UIMenuNode.Direction.Down] = playerMythDetails[0].transform.parent.gameObject;                     // Connect node to abilities split graph (down)
            mythSelectionGraph.nodes[Mathf.Clamp(i - 1, 0, 1000)].adjacent[(int)UIMenuNode.Direction.Right] = selectNode.gameObject;    // Amend left adjacent node to point to this one (right)
            mythSelectionGraph.nodes[0].adjacent[(int)UIMenuNode.Direction.Left] = selectNode.gameObject;                               // Amend first node to point to this one (left)
            selectNode.GetComponent<Image>().sprite = mythSOs[i].icon;

            // Store a reference to this manager
            selectNode.manager = this;

            // Create a mythData config in each UIPartyTeam for this new myth
            foreach (UIPartyTeam team in playerTeamDetails)
            {
                MythData newData = new MythData();
                newData.myth = mythSOs[i];
                team.mythDatas.Add(newData);
            }
        }

        // Find player participants, set their action map to UI,  assign them their starting UI menu node, and set up UI for that player
        PlayerParticipant[] players = FindObjectsOfType<PlayerParticipant>();
        //print("Participants found: " + players.Length);
        for (int i = 0; i < players.Length; i++)
        {
            // Reference UI menu graph and set input action map
            players[i].currentMenuGraph = playerTeamGraphs[players[i].partyIndex];
            PlayerInput input = players[i].GetComponent<PlayerInput>();
            input.SwitchCurrentActionMap("UI");
        }

        // Initialise each node graph
        foreach (UIMenuNodeGraph graph in playerTeamGraphs)
            graph.InitialiseCursorsAndStartingNodes(true);

        mythSelectionGraph.InitialiseCursorsAndStartingNodes(false);

        SelectTeamMember(0, playerTeamGraphs[0].playerCurrentNode[0]);
        SelectTeamMember(1, playerTeamGraphs[1].playerCurrentNode[1]);
    }

    void Update()
    {
        if (allPlayersReady)
            startPrompt.alpha = Mathf.Pow(0.5f + 0.5f * Mathf.Sin(Time.time * 5.0f), 0.35f);
        else
            startPrompt.alpha = 0.0f;
    }

    void SetUpPartyDataScriptableObjects() // Not quite an accurate name anymore // TODO: Create this functionality, later, if we really want it
    {
        // Reuse the old team if we've been flagged to do so (e.g. hitting 'return to party builder' from gameplay scene)
        if (PlayerPrefs.HasKey("PartyBuilderKeepTeams"))
        {
            if (PlayerPrefs.GetInt("PartyBuilderKeepTeams") > 0)
            {
                // Remove the flag
                PlayerPrefs.SetInt("PartyBuilderKeepTeams", 0);

                // Copy existing myth config data into each team

                // Set up each player's team to have those myths selected by default

                return;
            }
        }

        // Clear the live party data
        // livePartyData = templatePartyData; // Does... this work?
    }

    public void SelectTeamMember(int teamIndex, UIMenuNode selectedNode)
    {
        // Need to use selected node to figure out what member index the given player selected
        int selectedIndex = playerTeamGraphs[teamIndex].nodes.IndexOf(selectedNode);                // Which no. member in the team?
        int mythIndex = playerTeamDetails[teamIndex].selectedMythIndices[selectedIndex];            // Which no. myth in the myth select graph?

        // Need to update cursor in myth select graph, and update myth details to match newly selected myth...

        // If this team member has been assigned a myth, update the position of our cursor in mythSelection, and update mythDetails to match
        if (mythIndex >= 0)
        {
            // Navigate that player's cursor to the node that matches the myth assigned to that team member
            UIMenuNode nodeInMythSelect = mythSelectionGraph.nodes[mythIndex];
            mythSelectionGraph.Navigate(nodeInMythSelect, teamIndex, UIMenuNode.Direction.Right); // This should inherently also call SelectMyth();
        }
        // If no myth has been assigned to this, find the next available myth in myth selection, select it, update team member and myth details accordingly
        else
        {
            UIMenuNode nodeInMythSelect = mythSelectionGraph.nodes[GetFirstAvailableIndexInMythSelect()];
            mythSelectionGraph.Navigate(nodeInMythSelect, teamIndex, UIMenuNode.Direction.Right); // This should inherently also call SelectMyth();
        }
    }

    public void SelectMyth(int teamIndex, UIMenuNode selectedNode)
    {
        // Need to use selected node to figure out which myth the given player has selected
        int selectedIndex = playerTeamGraphs[teamIndex].nodes.IndexOf(playerTeamGraphs[teamIndex].playerCurrentNode[teamIndex]);    // Which no. member in the team?
        int mythIndex = mythSelectionGraph.nodes.IndexOf(selectedNode);                                                             // Which no. myth in the myth select graph?

        // Using the current selected team member index, update that team member's myth, and update myth details to match newly selected myth...
        //print("Updating team UI for player " + teamIndex + ": Member no.: " + selectedIndex + ", Myth no.: " + mythIndex);
        playerTeamDetails[teamIndex].UpdateUI(selectedIndex, mythIndex, rendersTeamTextures[mythIndex]);
        playerMythDetails[teamIndex].UpdateUI(playerTeamDetails[teamIndex].mythDatas[mythIndex], rendersPortraitTextures[mythIndex]);

        // Regenerate the abilies list UI to match this new myth selection
        GenerateAbilitiesList(teamIndex, mythIndex);

        UpdateProgressText(teamIndex);
        UpdateMythSelectIcons();
    }

    public void AssignAbility(int teamIndex, int assignedIndex, SO_Ability ability)
    {
        // Need some nifty numbers
        int selectedIndex = playerTeamGraphs[teamIndex].nodes.IndexOf(playerTeamGraphs[teamIndex].playerCurrentNode[teamIndex]);    // Which no. member in the team?
        int mythIndex = playerTeamDetails[teamIndex].selectedMythIndices[selectedIndex];                                            // Which no. myth in the myth select graph?

        SO_Ability oldAbility = null;

        // Update that ability slot in that team's mythData
        switch (assignedIndex)
        {
            case 0:
                oldAbility = playerTeamDetails[teamIndex].mythDatas[mythIndex].northAbility;
                playerTeamDetails[teamIndex].mythDatas[mythIndex].northAbility = ability;
                break;
            case 1:
                oldAbility = playerTeamDetails[teamIndex].mythDatas[mythIndex].westAbility;
                playerTeamDetails[teamIndex].mythDatas[mythIndex].westAbility = ability;
                break;
            case 2:
                oldAbility = playerTeamDetails[teamIndex].mythDatas[mythIndex].southAbility;
                playerTeamDetails[teamIndex].mythDatas[mythIndex].southAbility = ability;
                break;
            default:
                break;
        }

        // Update the myth details UI to reflect this change
        playerMythDetails[teamIndex].UpdateUI(playerTeamDetails[teamIndex].mythDatas[mythIndex], rendersPortraitTextures[mythIndex]);
        playerMythDetails[teamIndex].abilities[assignedIndex].AnimateSelectedAbility();

        // Greying out / ungreying out abilities in the abilities list
        foreach (UIMenuNode node in playerAbilityGraphs[teamIndex].nodes) // Gonna trigger someone's GetComponent nerve
        {
            UIPartyAbility abilityUI = node.GetComponent<UIPartyAbility>();
            if (abilityUI.ability == ability)
            {
                abilityUI.GreyOut(true);
                break;
            }
        }
        if (oldAbility != null)
        {
            foreach (UIMenuNode node in playerAbilityGraphs[teamIndex].nodes) // Gonna trigger someone's GetComponent nerve
            {
                UIPartyAbility abilityUI = node.GetComponent<UIPartyAbility>();
                if (abilityUI.ability == oldAbility)
                {
                    abilityUI.GreyOut(false);
                    break;
                }
            }
        }

        UpdateProgressText(teamIndex);
    }

    public void GenerateAbilitiesList(int teamIndex, int mythIndex)
    {
        SO_Myth myth = mythSOs[mythIndex];

        //// Save the cursors from an untimely death
        //foreach (UIAnimator cursor in playerAbilityGraphs[teamIndex].playerCursors)
        //{
        //    cursor.SetTransform(playerAbilityGraphs[teamIndex].GetComponent<RectTransform>());
        //}

        // Destroy all existing ability UIs and clear the ability graph's list of nodes
        for (int i = playerAbilityGraphs[teamIndex].nodes.Count - 1; i >= 0; i--)
            Destroy(playerAbilityGraphs[teamIndex].nodes[i].gameObject);

        playerAbilityGraphs[teamIndex].nodes = new List<UIMenuNode>();

        // For each ability in the given myth's pool of abilities, create a new ability UI, and grey it out if already selected by the myth data in question (and format nodes)
        for (int i = 0; i < myth.abilities.Length; i++)
        {
            SO_Ability ability = myth.abilities[i];

            // Create a node, adding it to the group of abilities, and set up it's adjacent nodes (and amend node above of it)
            UINodeAbility selectNode = Instantiate(mythAbilityPrefab, playerAbilityGraphs[teamIndex].transform).GetComponent<UINodeAbility>();
            playerAbilityGraphs[teamIndex].nodes.Add(selectNode);

            if (i == 0) // First node should point to myth select graph
            {
                selectNode.adjacent[(int)UIMenuNode.Direction.Up] = mythSelectionGraph.gameObject;                              // Connect node to myth select graph (up)
            }
            else // Subsequent nodes should point to previous node, and make previous node point to it
            {
                selectNode.adjacent[(int)UIMenuNode.Direction.Up] = playerAbilityGraphs[teamIndex].nodes[i - 1].gameObject;     // Connect node to up adjacent node (up)
                playerAbilityGraphs[teamIndex].nodes[i - 1].adjacent[(int)UIMenuNode.Direction.Down] = selectNode.gameObject;   // Amend up adjacent node to point to this one (down)
            }

            selectNode.GetComponent<UIPartyAbility>().SetUpUI(ability);

            // Store a reference to this manager
            selectNode.manager = this;

            // Grey out if neccessary
            if (playerTeamDetails[teamIndex].mythDatas[mythIndex].northAbility == ability) selectNode.GetComponent<UIPartyAbility>().GreyOut(true);
            if (playerTeamDetails[teamIndex].mythDatas[mythIndex].westAbility == ability) selectNode.GetComponent<UIPartyAbility>().GreyOut(true);
            if (playerTeamDetails[teamIndex].mythDatas[mythIndex].southAbility == ability) selectNode.GetComponent<UIPartyAbility>().GreyOut(true);
        }

        // Initialise start node / cursor
        playerAbilityGraphs[teamIndex].InitialiseCursorsAndStartingNodes(false);
    }

    void UpdateProgressText(int playerNumber)
    {
        playersReady[playerNumber] = false;

        if (!PlayerHasAFullParty(playerNumber))
        {
            progressTexts[playerNumber].text = "Need to select 3 myths for your team...";
        }
        else if (!PlayerPartyMembersAllHaveFullAbilities(playerNumber))
        {
            progressTexts[playerNumber].text = "Need to assign 3 abilities to each myth...";
        }
        else
        {
            // Set ready to true
            playersReady[playerNumber] = true;
            progressTexts[playerNumber].text = "Your team is ready!";
        }

        // If both players ready, note this
        allPlayersReady = true;
        foreach (bool playerReady in playersReady)
        {
            if (!playerReady)
            {
                allPlayersReady = false;
                break;
            }
        }
    }

    bool PlayerHasAFullParty(int playerNumber)
    {
        foreach (int mythIndex in playerTeamDetails[playerNumber].selectedMythIndices)
        {
            if (mythIndex < 0)
                return false;
        }

        return true;
    }
    bool PlayerPartyMembersAllHaveFullAbilities(int playerNumber)
    {
        foreach (int mythIndex in playerTeamDetails[playerNumber].selectedMythIndices)
        {
            MythData mythData = playerTeamDetails[playerNumber].mythDatas[mythIndex];

            if (mythData.northAbility == null) return false;
            if (mythData.westAbility == null) return false;
            if (mythData.southAbility == null) return false;
        }

        return true;
    }

    int GetFirstAvailableIndexInMythSelect()
    {
        List<int> occupiedIndices = new List<int>();

        foreach (UIPartyTeam team in playerTeamDetails)
        {
            foreach (int index in team.selectedMythIndices)
            {
                occupiedIndices.Add(index);
            }
        }

        for (int i = 0; i < mythSelectionGraph.nodes.Count; i++)
        {
            if (!occupiedIndices.Contains(i))
                return i;
        }

        Debug.LogWarning("No available myths to select in the myth select graph!");
        return -1;
    }

    // Should really stop copy/pasting this, but regardless...
    public void TryStartGame()
    {
        if (!allPlayersReady) return;

        PlayerParticipant[] playerParticipants = FindObjectsOfType<PlayerParticipant>();

        // Look at our team data and write into party data
        for (int i = 0; i < livePartyData.partyData.Length; i++)
        {
            MythData[] mythData = new MythData[playerTeamDetails[i].selectedMythIndices.Length];

            for (int k = 0; k < playerTeamDetails[i].selectedMythIndices.Length; k++)
            {
                int mythIndex = playerTeamDetails[i].selectedMythIndices[k];
                mythData[k] = playerTeamDetails[i].mythDatas[mythIndex];
            }

            livePartyData.partyData[i].mythData = mythData;
            
            foreach (PlayerParticipant playerParticipant in playerParticipants)
            {
                if (playerParticipant.partyIndex == i)
                {
                    livePartyData.partyData[i].Participant = playerParticipant;
                    break;
                }
            }
        }

        // Initiate loading next scene via the transition UI element
        if (transitionAnimator)
        {
            foreach (PlayerParticipant participant in FindObjectsOfType<PlayerParticipant>())
            {
                participant.DisablePlayerInput(0.5f);
                participant.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            }

            transitionAnimator.SetInteger("Direction", 1);
            transitionAnimator.SetTrigger("Fade");
            StartCoroutine(LoadScene(0.35f));
        }
        else
        {
            SceneManager.LoadScene(nameOfNextScene);
        }
    }

    IEnumerator LoadScene(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene(nameOfNextScene);
    }

    public bool IsMythAlreadySelectedInATeamAndMoveAgainIfSo(UIMenuNode node, int teamIndex, UIMenuNode.Direction direction)
    {
        // Need some nifty numbers
        int selectedIndex = playerTeamGraphs[teamIndex].nodes.IndexOf(playerTeamGraphs[teamIndex].playerCurrentNode[teamIndex]);    // Member no. to overlook
        int mythIndex = mythSelectionGraph.nodes.IndexOf(node);

        bool isMythAlreadySelectedInATeam = false;

        if (mythIndex >= 0)
        {
            for (int i = 0; i < playerTeamDetails.Length; i++)
            {
                for (int k = 0; k < playerTeamDetails[i].selectedMythIndices.Length; k++)
                {
                    if (!(i == teamIndex && k == selectedIndex)) // We don't check the current team member on the team in question
                    {
                        if (playerTeamDetails[i].selectedMythIndices[k] == mythIndex)
                        {
                            isMythAlreadySelectedInATeam = true;
                            break;
                        }
                    }
                }
            }
        }

        if (isMythAlreadySelectedInATeam)
            mythSelectionGraph.ParseNavigation(direction, teamIndex);

        return isMythAlreadySelectedInATeam;
    }

    public void UpdateMythSelectIcons() // For each team member on each team that is not 'active', grey their selected myth out, ungrey any other myth
    {
        List<int> selectedMythIndices = new List<int>();

        // Need some nifty numbers
        int[] selectedIndices = new int[playerTeamGraphs.Length];
        int[] mythIndices = new int[playerTeamGraphs.Length];

        for (int i = 0; i < playerTeamGraphs.Length; i++)
        {
            selectedIndices[i] = playerTeamGraphs[i].nodes.IndexOf(playerTeamGraphs[i].playerCurrentNode[i]);    // Which no. member in the team?
            mythIndices[i] = playerTeamDetails[i].selectedMythIndices[selectedIndices[i]];                       // Which no. myth in the myth select graph?
        }

        for (int i = 0; i < playerTeamDetails.Length; i++)                          // For each team...
        {
            for (int k = 0; k < playerTeamDetails[i].selectedMythIndices.Length; k++)   // For each member on that team...
            {
                if (k != selectedIndices[i])                                                // We don't check the current member on the team in question...
                {
                    selectedMythIndices.Add(playerTeamDetails[i].selectedMythIndices[k]);       // Add that selected myth into the list
                }
            }
        }

        // Grey out or ungrey if that myth's index appears in the list we created (of myths currently selected)
        for (int i = 0; i < mythSelectionGraph.nodes.Count; i++)
        {
            if (selectedMythIndices.Contains(i))
                mythSelectionGraph.nodes[i].GetComponent<Image>().color = new Color(1, 1, 1, 0.35f);
            else
                mythSelectionGraph.nodes[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }

    public void RemovePartyMember(int teamIndex)
    {
        // Return out of this if the provided player only has one member left in their team
        int counter = 0;
        foreach (int mythIndex in playerTeamDetails[teamIndex].selectedMythIndices)
        {
            if (mythIndex >= 0)
                counter++;
        }
        if (counter <= 1) return;

        // Find which number team member that player has active
        int selectedIndex = playerTeamGraphs[teamIndex].nodes.IndexOf(playerTeamGraphs[teamIndex].playerCurrentNode[teamIndex]);

        // Set the selected index in team details to -1
        playerTeamDetails[teamIndex].selectedMythIndices[selectedIndex] = -1;

        // Navigate cursor in the team graph to the left
        playerTeamGraphs[teamIndex].ParseNavigation(UIMenuNode.Direction.Left, teamIndex);

        // Update visuals
        playerTeamDetails[teamIndex].mythTeamRenderImages[selectedIndex].enabled = false;
    }
}
