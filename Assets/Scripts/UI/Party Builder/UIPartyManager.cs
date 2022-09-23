using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Myths;

public class UIPartyManager : MonoBehaviour
{
    // Asset references
    [SerializeField] RenderTexture[] rendersTeam;
    [SerializeField] RenderTexture[] rendersPortrait;
    [SerializeField] GameObject[] mythPrefabs;
    [SerializeField] SO_Myth[] mythSOs;

    // Scene references
    [SerializeField] HorizontalLayoutGroup mythIconLayout;
    [SerializeField] GameObject[] mythCameraSetUps;
    [SerializeField] GameObject[] player1StartGraph;
    [SerializeField] GameObject[] player2StartGraph;
    [SerializeField] UIPartyMyth[] playerMythDetails;
    [SerializeField] UIPartyTeam[] playerTeamDetails;

    // Variables

    void Start()
    {
        // For each myth in the array
        // Instantiate a new camera set up at the correct transforms, and hook up camera to next render texture
        // Instantiate the current myth, parenting it to the camera set up, and strip it of any unneccesary components
        // Set animator properties on that myth
        // Create an icon/menu node, adding it to the horizontal layout group of selectable myth icons, and set up it's adjacent nodes (and amend node to left of it)
        // ...

        // Find player participants, set their action map to UI, and assign them their starting UI menu node
        PlayerParticipant[] players = FindObjectsOfType<PlayerParticipant>();
        foreach (PlayerParticipant player in players)
        {

        }
    }

    void Update()
    {
        
    }
}
