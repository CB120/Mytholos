using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameAbility : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI abilityNameTMP;
    [SerializeField] private Image elementIcon;
    [SerializeField] private TextMeshProUGUI staminaCostTMP;
    [SerializeField] private Color[] elementColours;

    public void UpdateUI(string abilityName, E_Element element, float staminaCost)
    {
        abilityNameTMP.text = abilityName;
        elementIcon.color = elementColours[(int)element]; // TODO: Create a scriptiable object for commonly used asset resources (in this case, symbols/colours for each element)
        //staminaCostTMP.text = Mathf.RoundToInt(staminaCost * 100.0f) + "%"; // Assumes that stamina costs are passed in as a float ranging between 0 and 1
        staminaCostTMP.text = Mathf.RoundToInt(staminaCost) + ""; // For sprint 2, we'll just show the damage
    }

    // TODO: Greying out ability if insufficient stamina, and placing a listener somewhere to call a function here that (animates and) un-greys out this ability when ready
    // Also, a lister to grey out the ability if stamina drops below required?
}