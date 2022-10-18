using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Myths;

public class UIGameMyth : MonoBehaviour
{
    [Header("Scene references")]
    [SerializeField] Image mythIcon;
    [SerializeField] UISlider healthSlider;
    [SerializeField] RectTransform healthTransform;
    [SerializeField] UISlider staminaSlider;
    [SerializeField] RectTransform staminaTransform;
    CanvasGroup canvasGroup;
    RectTransform rectTransform;
    Myth myth; // Record of associated myth

    [Header("Asset references")]
    Sprite[] mythIcons; // Selected, not selected, defeated

    [Header("Variables")]
    public bool selected = false;   // Record of if this myth is the active myth
    int memberNumber; // Which no. member is this myth in the team?
    int selectedMember; // Which no. member is the selected member?
    [SerializeField] float[] rootHeight;
    [SerializeField] float[] rootWidth;
    [SerializeField] float[] healthHeight;
    [SerializeField] float[] staminaHeight;

    void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        healthTransform = healthSlider.GetComponent<RectTransform>();
        staminaTransform = staminaSlider.GetComponent<RectTransform>();
    }

    void UpdateHealth(float percent)
    {
        healthSlider.UpdateSliderPercent(percent);
        if (percent <= 0)
            UpdateUI(selectedMember);
    }

    void UpdateStamina(float percent)
    {
        staminaSlider.UpdateSliderPercent(percent);
    }

    public void UpdateUI(int selectedIndex)
    {
        selectedMember = selectedIndex;
        selected = selectedIndex == memberNumber;
        int index = selected ? 0 : 1;
        bool isAlive = myth.Health.Value > 0.0f;

        // Set UI transforms based on if selected or not
        rectTransform.sizeDelta = new Vector2(rootWidth[index], rootHeight[index]);
        rectTransform.anchoredPosition = new Vector2((memberNumber * rootWidth[1]) + (selectedIndex < memberNumber ? rootWidth[0] - rootWidth[1] : 0), 0);
        healthTransform.sizeDelta = new Vector2(healthTransform.sizeDelta.x, healthHeight[index]);
        staminaTransform.sizeDelta = new Vector2(staminaTransform.sizeDelta.x, staminaHeight[index]);

        // Set myth icon sprite based on if selected or not
        mythIcon.sprite = isAlive ? mythIcons[index] : mythIcons[2];

        // Set group alpha based on if selected or not
        canvasGroup.alpha = isAlive ? index == 0 ? 1.0f : 0.6f : 0.25f;

        // Update sliders, zeroing them if defeated
        healthSlider.FormatSliderRectTransform(isAlive ? myth.Health.ValuePercent : 0);
        staminaSlider.FormatSliderRectTransform(isAlive ? myth.Stamina.ValuePercent : 0);
    }

    public void SetMyth(Myth myth, int memberIndex)
    {
        if (myth != null)
        {
            memberNumber = memberIndex;

            // Remove listeners from previous referenced myth
            if (this.myth != null)
            {
                myth.Health.valueChanged.RemoveListener(UpdateHealth);
                myth.Stamina.valueChanged.RemoveListener(UpdateStamina);
            }

            // Update UI visuals and place listeners in new referenced myth
            this.myth = myth;
            mythIcons = new Sprite[] { myth.myth.icon, myth.myth.iconOff, myth.myth.iconDead };
            myth.Health.valueChanged.AddListener(UpdateHealth);
            myth.Stamina.valueChanged.AddListener(UpdateStamina);
            UpdateHealth(myth.Health.ValuePercent);
            UpdateStamina(myth.Stamina.ValuePercent);
            UpdateUI(0);
        }
        else
            Debug.LogWarning("UIGameMyth was passed a null reference");
    }
}
