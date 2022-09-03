using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameMyth : MonoBehaviour
{
    [SerializeField] Image healthSliderBG;
    [SerializeField] Image healthSliderFill;
    [SerializeField] Image staminaSliderBG;
    [SerializeField] Image staminaSliderFill;

    float healthMaxWidth;
    float staminaMaxWidth;

    void OnEnable()
    {
        // TODO: Create listeners

        // Look at referenced UI object and keep a record of their dimensions
        healthMaxWidth = healthSliderBG.rectTransform.sizeDelta.x - 2.0f;
        staminaMaxWidth = staminaSliderBG.rectTransform.sizeDelta.x - 2.0f;
        UpdateHealth(1.0f);
        UpdateStamina(0.0f);
    }

    // TODO: Update listeners when this UI starts representing a differnt party member than it used to be

    void UpdateHealth(float percent)
    {
        healthSliderFill.rectTransform.sizeDelta = new Vector2(percent * healthMaxWidth, healthSliderFill.rectTransform.sizeDelta.y);
    }

    void UpdateStamina(float percent)
    {
        staminaSliderFill.rectTransform.sizeDelta = new Vector2(percent * staminaMaxWidth, staminaSliderFill.rectTransform.sizeDelta.y);
    }
}
