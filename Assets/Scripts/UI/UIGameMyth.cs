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
    CanvasGroup canvasGroup;

    // Variables
    float healthMaxWidth;           // Records of UI size for slider code
    float staminaMaxWidth;
    Myth myth;                      // Might not be neccessary, but making note for time being
    public bool greyedOut = false;  // Set by UIGameParty when selecting an attack, to make selected myth more obvious
    public bool selected = false;   // Set by UIGameParty when selecting an attack, to make selected myth more obvious

    void OnEnable()
    {
        // Look at referenced UI object and keep a record of their transforms
        healthMaxWidth = healthSliderBG.rectTransform.rect.width - 2.0f;
        staminaMaxWidth = staminaSliderBG.rectTransform.rect.width - 2.0f;
        UpdateHealth(1.0f);
        UpdateStamina(1.0f); // TODO: Should set to 0.0f, but stamina doesn't currently exist/do anything

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1.0f;
    }

    void UpdateHealth(float percent) // TODO: Update this, we're assuming that max health is 100 as myths have no constant for maximum health
    {
        percent = Mathf.Clamp(percent, 0.0f, 1.0f);
        healthSliderFill.rectTransform.sizeDelta = new Vector2(percent * healthMaxWidth, healthSliderFill.rectTransform.rect.height);
        UpdateOpacity();
    }

    void UpdateStamina(float percent)
    {
        percent = Mathf.Clamp(percent, 0.0f, 1.0f);
        staminaSliderFill.rectTransform.sizeDelta = new Vector2(percent * staminaMaxWidth, staminaSliderFill.rectTransform.rect.height);
    }

    public void UpdateOpacity()
    {
        // Update opacity, based on if dead and/or selected by the UIGameParty
        if (canvasGroup != null)
        {
            if (healthSliderFill.rectTransform.rect.width <= 0.0f)
                canvasGroup.alpha = 0.2f;
            else
                canvasGroup.alpha = 1.0f * (greyedOut ? 0.6f : 1.0f);
        }
        else
            StartCoroutine(TryUpdateOpacity());

        // Spicing things up with scale
        float scaleFactor = selected ? 1.05f : greyedOut ? 0.95f : 1.0f;
        GetComponent<RectTransform>().localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

    IEnumerator TryUpdateOpacity() // Probably don't need this, but it will recall the function if it failed because the scene reference was null
    {
        yield return new WaitForSeconds(0);
        UpdateOpacity();
    }

    // TODO?: Update listeners when this UI starts representing a differnt party member than it used to be
    public void SetMyth(Myth myth)
    {
        if (myth != null)
        {
            // Remove listeners from previous referenced myth
            // TODO: Cache this for performance.
            var mythStamina = myth.GetComponent<MythStamina>();
            
            if (this.myth != null)
            {
                myth.HealthChanged.RemoveListener(UpdateHealth);
                mythStamina.staminaChanged.RemoveListener(UpdateStamina);
                // TODO: Remove a listener for stamina
            }

            // Update UI visuals and place listeners in new referenced myth
            this.myth = myth;
            myth.HealthChanged.AddListener(UpdateHealth);
            mythStamina.staminaChanged.AddListener(UpdateStamina);
            UpdateHealth(myth.Health / 100.0f);
            UpdateStamina(mythStamina.StaminaPercent);
            // TODO: Create a listener (and event) for stamina
            mythIcon.sprite = myth.myth.icon;
        }
        else
            Debug.LogWarning("UIGameMyth was passed a null reference");
    }
}
