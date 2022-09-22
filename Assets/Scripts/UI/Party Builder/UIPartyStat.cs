using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Myths;

public class UIPartyStat : MonoBehaviour
{
    [SerializeField] Image[] bars; // Should contain 5

    public void SetUpUI(float percent)
    {
        int barsToFill = Mathf.RoundToInt(Mathf.Clamp(percent, 0, 1) * bars.Length);

        for (int i = 0; i < bars.Length; i++)
        {
            if (i <= barsToFill)
                bars[i].color = new Color(bars[i].color.r, bars[i].color.g, bars[i].color.b, 1.0f);
            else
                bars[i].color = new Color(bars[i].color.r, bars[i].color.g, bars[i].color.b, 0.2f);
        }
    }
}
