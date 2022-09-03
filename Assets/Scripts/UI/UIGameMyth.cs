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

    float healthMaxWidth;   // Records of UI size for slider code
    float staminaMaxWidth;

    void OnEnable()
    {
        // TODO: Create listeners // We might do this in a function instead, especially because we'll want to move our listeners to a different Myth

        // Look at referenced UI object and keep a record of their dimensions
        healthMaxWidth = healthSliderBG.rectTransform.rect.width - 2.0f;
        staminaMaxWidth = staminaSliderBG.rectTransform.rect.width - 2.0f;
        UpdateHealth(1.0f);
        UpdateStamina(0.0f);
    }

    // TODO: Update listeners when this UI starts representing a differnt party member than it used to be

    void UpdateHealth(float percent)
    {
        healthSliderFill.rectTransform.sizeDelta = new Vector2(percent * healthMaxWidth, healthSliderFill.rectTransform.rect.height);
    }

    void UpdateStamina(float percent)
    {
        staminaSliderFill.rectTransform.sizeDelta = new Vector2(percent * staminaMaxWidth, staminaSliderFill.rectTransform.rect.height);
    }
}
