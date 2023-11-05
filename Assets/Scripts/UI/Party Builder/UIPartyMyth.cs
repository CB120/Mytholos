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
    //[SerializeField] UIPartyStat statBrawn;
    //[SerializeField] UIPartyStat statPsyche;
    [SerializeField] UIPartyStat statAttack;
    [SerializeField] UIPartyStat statAgility;
    [SerializeField] TextMeshProUGUI mythName;
    //[SerializeField] TextMeshProUGUI mythSubtitle;
    [SerializeField] Image elementIcon;
    public UIGameAbility[] abilities = new UIGameAbility[3];
    [SerializeField] RawImage portraitRender;

    public void UpdateUI(MythData mythData, Texture portraitTexture)
    {
        Myth mythComponent = mythData.myth.prefab.GetComponent<Myth>();

        Color elementColour = mythComponent.element.color;
        foreach (Image background in backgroundsToColour)
        {
            background.color = new Color(elementColour.r, elementColour.g, elementColour.b, background.color.a);
            if (background.color.a >= 1.0f) // Hacky hacky
                background.color = Color.Lerp(background.color, Color.black, 0.5f);
        }

        statHealth.SetUpUI(mythData.myth.health, elementColour);
        statSize.SetUpUI(mythData.myth.size, elementColour);
        //statBrawn.SetUpUI(mythData.myth.brawn);
        //statPsyche.SetUpUI(mythData.myth.psyche);
        statAttack.SetUpUI(mythData.myth.attack, elementColour);
        statAgility.SetUpUI((mythData.myth.agility - 0.8f) * 5, elementColour); // Some jank

        mythName.text = mythData.myth.name;
        //mythSubtitle.text = mythComponent.element.name + " myth";
        elementIcon.sprite = mythComponent.element.icon;
        elementIcon.color = Color.Lerp(elementColour, Color.black, 0.5f);

        abilities[0].UpdateUI(mythData.northAbility);
        abilities[1].UpdateUI(mythData.westAbility);
        abilities[2].UpdateUI(mythData.southAbility);

        portraitRender.texture = portraitTexture; // Should these render textures become one of the assets included in SO_Myth?
    }
}
