using UnityEngine;
using UnityEngine.UI;

public class UIPartyStat : MonoBehaviour
{
    [SerializeField] Image[] bars; // Should contain 5
    Color barColour;

    void OnEnable()
    {
        if (bars.Length > 0)
            barColour = new Color(bars[0].color.r, bars[0].color.g, bars[0].color.b, 1.0f);
    }

    public void SetUpUI(float percent)
    {
        int barsToFill = Mathf.CeilToInt(Mathf.Clamp(percent, 0, 1) * bars.Length);
        //print("Percent: " + percent + ", Bars to fill: " + barsToFill + ", Max bars: " + bars.Length);

        for (int i = 0; i < bars.Length; i++)
        {
            if (i < barsToFill)
            {
                bars[i].color = new Color(barColour.r, barColour.g, barColour.b, 0.99f);
            }
            else
            {
                Color fadeColor = Color.Lerp(barColour, Color.black, 0.5f);
                bars[i].color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0.25f);
            }
        }
    }

    public void SetUpUI(float percent, Color newColor)
    {
        barColour = newColor;
        SetUpUI(percent);
    }
}
