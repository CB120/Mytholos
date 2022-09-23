using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Myths;

// A stat sheet displaying all the information relevant to the selected myth within this party
public class UIPartyMyth : MonoBehaviour
{
    // Scene references
    [SerializeField] Image[] backgroundsToColour;
    [SerializeField] UIPartyStat statHealth;
    [SerializeField] UIPartyStat statSize;
    [SerializeField] UIPartyStat statBrawn;
    [SerializeField] UIPartyStat statPsyche;
    [SerializeField] UIPartyStat statAgility;
    [SerializeField] TextMeshProUGUI mythName;
    [SerializeField] TextMeshProUGUI mythSubtitle;
    [SerializeField] UIGameAbility[] abilities = new UIGameAbility[3];
    [SerializeField] RawImage portraitRender;

    public void UpdateUI()
    {

    }
}
