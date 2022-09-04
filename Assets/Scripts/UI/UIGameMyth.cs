using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Myths;

public class UIGameMyth : MonoBehaviour
{
    // References
    [SerializeField] Image mythIcon;
    [SerializeField] Image healthSliderBG;
    [SerializeField] Image healthSliderFill;
    [SerializeField] Image staminaSliderBG;
    [SerializeField] Image staminaSliderFill;

    // Variables
    float healthMaxWidth;   // Records of UI size for slider code
    float staminaMaxWidth;
    Myth myth;              // Might not be neccessary, but making note for time being

    void OnEnable()
    {
        // Look at referenced UI object and keep a record of their dimensions
        healthMaxWidth = healthSliderBG.rectTransform.rect.width - 2.0f;
        staminaMaxWidth = staminaSliderBG.rectTransform.rect.width - 2.0f;
        UpdateHealth(1.0f);
        UpdateStamina(1.0f); // TODO: Should set to 0.0f, but stamina doesn't currently exist/do anything
    }

    void UpdateHealth(float percent) // TODO: Update this, we're assuming that max health is 100 as myths have no constant for maximum health
    {
        percent = Mathf.Clamp(percent, 0.0f, 1.0f);
        healthSliderFill.rectTransform.sizeDelta = new Vector2(percent * healthMaxWidth, healthSliderFill.rectTransform.rect.height);
    }

    void UpdateStamina(float percent)
    {
        percent = Mathf.Clamp(percent, 0.0f, 1.0f);
        staminaSliderFill.rectTransform.sizeDelta = new Vector2(percent * staminaMaxWidth, staminaSliderFill.rectTransform.rect.height);
    }

    // TODO?: Update listeners when this UI starts representing a differnt party member than it used to be
    public void SetMyth(Myth myth)
    {
        this.myth = myth;
        myth.HealthChanged.AddListener(UpdateHealth);
        // TODO: Create a listener (and event) for stamina
        mythIcon.sprite = myth.myth.icon;
    }
}
