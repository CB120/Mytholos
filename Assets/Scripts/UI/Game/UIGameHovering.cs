using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;
using Elements;

public class UIGameHovering : MonoBehaviour
{
    [SerializeField] Camera gameCamera;
    [SerializeField] UISlider healthSlider;
    [SerializeField] UISlider staminaSlider;
    [SerializeField] MythUI buffIcons;
    RectTransform healthTransform;
    RectTransform staminaTransform;
    Myth myth;
    RectTransform rectTransform;

    void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        healthTransform = healthSlider.GetComponent<RectTransform>();
        staminaTransform = staminaSlider.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myth != null && rectTransform != null && gameCamera != null)
        {
            rectTransform.localPosition = gameCamera.WorldToScreenPoint(myth.transform.position) - new Vector3(640, 360, 0);
            rectTransform.localPosition = Vector3Int.RoundToInt( new Vector2(rectTransform.localPosition.x, rectTransform.localPosition.y) + new Vector2(320, 180 - 24) );
        }
    }

    public void SetMyth(Myth myth)
    {
        if (myth != null)
        {
            // Remove listeners from previous referenced myth
            if (this.myth != null)
            {
                myth.Health.valueChanged.RemoveListener(UpdateHealth);
                myth.Stamina.valueChanged.RemoveListener(UpdateStamina);
            }

            // Update UI visuals and place listeners in new referenced myth
            this.myth = myth;
            myth.Health.valueChanged.AddListener(UpdateHealth);
            myth.Stamina.valueChanged.AddListener(UpdateStamina);
            UpdateHealth(myth.Health.ValuePercent);
            UpdateStamina(myth.Stamina.ValuePercent);
            UpdateUI();

            // Update buff icons
            buffIcons.UpdateListeners(myth.GetComponent<Effects>());
        }
        else
            Debug.LogWarning("UIGameMyth was passed a null reference");
    }

    void UpdateHealth(float percent)
    {
        healthSlider.UpdateSliderPercent(percent);
        if (percent <= 0)
            UpdateUI();
    }

    void UpdateStamina(float percent)
    {
        staminaSlider.UpdateSliderPercent(percent);
    }

    public void UpdateUI()
    {
        // Update sliders, zeroing them if defeated
        healthSlider.FormatSliderRectTransform(myth.Health.ValuePercent);
        staminaSlider.FormatSliderRectTransform(myth.Stamina.ValuePercent);
    }
}
