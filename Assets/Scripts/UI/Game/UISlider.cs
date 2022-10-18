using System.Collections.Generic;
using UnityEngine;

// This component is expected to be attached to a UI gameObject that has an Image (or two) as its (only) child
// This class creates an animated slider, by having a percent value passed into it, with the format function being called whenever this gameObject is resized

public class UISlider : MonoBehaviour
{
    [SerializeField] RectTransform sliderFill;      // The coloured fill of this slider, expected to be a child of this slider
    [SerializeField] RectTransform animatedFill;    // An optional underlying coloured fill that 'lags' behind the sliderFill, visualising a the difference/change in value
    [SerializeField] int sliderMargin;              // Number of pixels the slider fill is padded inline
    RectTransform rectTransform;                    // This object's rectTransform

    float prevPercent = -1;                         // A record of the previous percent, for animating the animatedFill
    float currentAnimatedPercent = -1;              // A record of the animatedFill's current fill percent
    const float timeToWait = 0.5f;                  // The time that the animatedFill should wait before animating to its next target
    const float animateSpeed = 0.5f;                // The rate (in percent per second) that the animatedFill should animate towards it next target

    float currentPercent;
    bool isAnimating;
    float currentWaitTime;

    //[SerializeField] bool maintainHeight;

    void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        FormatSliderRectTransform();
    }

    public void FormatSliderRectTransform(float percent = 1.0f)
    {
        if (!rectTransform || !sliderFill) return;

        // Format rectTransform to fit within this slider
        ReformatSliderFill(ref sliderFill);
        if (animatedFill)
            ReformatSliderFill(ref animatedFill);

        UpdateSliderPercent(percent);
    }

    public void UpdateSliderPercent(float percent)
    {
        if (percent < 0 || percent > 1) return;

        currentPercent = percent;

        // Update slider fill dimensions based on percent we're representing
        ResizeSliderFill(ref sliderFill, percent);

        // Update the animated fill, if we have one
        if (animatedFill)
        {
            // This function may be called because the slider was resized, so resize animated slider fill too
            ResizeSliderFill(ref animatedFill, currentAnimatedPercent);

            // No animation required if the actual value of this slider is the same
            if (prevPercent != percent)
            {
                // If change in percent is negative and animatedFill is not active, initiate it
                if (percent < prevPercent && !isAnimating)
                {
                    currentAnimatedPercent = prevPercent;
                    ResizeSliderFill(ref animatedFill, prevPercent);
                    currentWaitTime = timeToWait;
                    isAnimating = true;
                    animatedFill.gameObject.SetActive(true);
                }
            }

            prevPercent = percent;
        }
    }

    void ResizeSliderFill(ref RectTransform slider, float percent)
    {
        float width = Mathf.Abs(rectTransform.rect.x * 2) - sliderMargin * 2;
        float height = rectTransform.sizeDelta.y == 0 ? Mathf.Abs(rectTransform.rect.y * 2) - sliderMargin * 2 : rectTransform.sizeDelta.y - (sliderMargin * 2);
        slider.sizeDelta = new Vector2(Mathf.CeilToInt(width * percent), height);
    }

    void ReformatSliderFill(ref RectTransform slider)
    {
        slider.anchorMax = new Vector2(0, 0.5f);
        slider.anchorMin = new Vector2(0, 0.5f);
        slider.anchoredPosition = new Vector2(1, 0);
        slider.pivot = new Vector2(0, 0.5f);
    }

    void Update()
    {
        if (isAnimating)
        {
            // If the current percent is now greater than animatedFill, end the animation (animatedFill should only ever animate downwards/decrease)
            if (currentPercent > currentAnimatedPercent)
            {
                isAnimating = false;
                animatedFill.gameObject.SetActive(false);
            }

            // If delay is over
            else if (currentWaitTime <= 0.0f)
            {
                float changeInPercent = Time.deltaTime * animateSpeed;

                if (changeInPercent < Mathf.Abs(currentPercent - currentAnimatedPercent))
                {
                    // If we've yet to reach current slider percent, decrease it over time
                    currentAnimatedPercent -= changeInPercent;
                    ResizeSliderFill(ref animatedFill, currentAnimatedPercent);
                }
                else
                {
                    isAnimating = false;
                    animatedFill.gameObject.SetActive(false);
                }
            }

            // Tick down delay timer
            currentWaitTime -= Time.deltaTime;
        }
    }
}