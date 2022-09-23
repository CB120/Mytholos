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
    [SerializeField] GameObject[] mythCameraSetUp;
    [SerializeField] GameObject[] myths;
    [SerializeField] RenderTexture[] renders;

    // Scene references
    [SerializeField] HorizontalLayoutGroup mythIconLayout;

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
    }

    void Update()
    {
        
    }
}
