using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Myths;

public class UIPartyAbility : MonoBehaviour // Not to be confused with UINodeAbility, which handles node/button related behaviour for when this ability is assigned
{
    [SerializeField] UIPartyStat statDamage;
    [SerializeField] UIPartyStat statStamina;
    [SerializeField] UIPartyStat statImpact;
    [SerializeField] TextMeshProUGUI abilityName;
    [SerializeField] TextMeshProUGUI styleEffectName;
    [SerializeField] TextMeshProUGUI damageTypeName;
    [SerializeField] Image elementIcon;
    [SerializeField] Image elementBand;
    [SerializeField] Image statBG;

    const float maxDamage = 50; // TODO: Amend these values
    const float maxStamina = 100;
    const float maxImpact = 100;

    public SO_Ability ability;
    public bool isGreyedOut;

    public void SetUpUI(SO_Ability newAbility)
    {
        if (newAbility == null)
        {
            Debug.LogWarning("Myth has an invalid ability");
            return;
        }

        ability = newAbility;
        statDamage.SetUpUI(ability.damage / maxDamage);
        statStamina.SetUpUI(ability.staminaCost / maxStamina);
        statImpact.SetUpUI(0 / maxImpact);
        abilityName.text = ability.name;

        //styleEffectName.text = ability.abilityPrefab.name.Replace("Ability", "").Replace("Prefab", ""); // TODO: Include ability effect description in this
        string styleName = ability.description;
        string effectName = "";
        if (ability.element != null)
            effectName = ability.element.debuffDescription;

        if (styleName == "Pool")
        {
            effectName = ability.element.buffDescription;
        }

        if (effectName.Length > 0)
            styleName += ", " + effectName;
        // 27 max
        if (styleName.Length > 25)
            styleName = styleName.Substring(0, 23) + "...";

        styleEffectName.text = styleName;

        damageTypeName.text = "-"; // TODO: Make this say "Brawn" or "Psyche", based on the damage type of the ability (or "-" if does no damage)

        if (ability.element != null)
        {
            Color color = ability.element.color;
            elementIcon.sprite = ability.element.icon;
            elementIcon.color = new Color(color.r, color.g, color.b, 1.0f);
            elementBand.color = new Color(color.r, color.g, color.b, 1.0f);
            statBG.color = new Color(color.r, color.g, color.b, 0.2f);
        }
    }

    public void GreyOut(bool isNowGreyedOut)
    {
        isGreyedOut = isNowGreyedOut;

        // Visually do something
        GetComponent<CanvasGroup>().alpha = isGreyedOut ? 0.5f : 1.0f;
    }
}
