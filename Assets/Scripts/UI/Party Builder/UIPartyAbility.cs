using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Myths;

public class UIPartyAbility : MonoBehaviour
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

    SO_Ability ability;

    public void SetUpUI(SO_Ability newAbility)
    {
        ability = newAbility;
        statDamage.SetUpUI(ability.damage / maxDamage);
        statStamina.SetUpUI(ability.stamina / maxStamina);
        statImpact.SetUpUI(0 / maxImpact);
        abilityName.text = ability.name;
        styleEffectName.text = ability.abilityPrefab.name.Replace("Ability", "").Replace("Prefab", ""); // TODO: Include ability effect description in this
        damageTypeName.text = "-"; // TODO: Make this say "Brawn" or "Psyche", based on the damage type of the ability (or "-" if does no damage)

        if (ability.element != null)
        {
            Color color = ability.element.color;
            // elementIcon.sprite = ability.element.icon; // TODO: Create element icons
            elementBand.color = new Color(color.r, color.g, color.b, 1.0f);
            statBG.color = new Color(color.r, color.g, color.b, 0.2f);
        }
    }
}
