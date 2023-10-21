using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    TextMeshProUGUI text;
    Camera gameCamera;
    Vector3 startPosition;
    Vector3 endPosition;
    float timer;
    float timeToBeginFadeOut = 1.25f;
    float timeToDie = 1.5f;
    RectTransform rectTransform;
    //[SerializeField] Color[] damageColors = new Color[3]; // low, normal, high

    public static Color defaultColor = new Color(1f, 1f, 0f, 1f);
    public static Color ineffectiveColor = new Color(.6f, .6f, .6f, 1f);
    public static Color effectiveColor = new Color(1f, .33f, 0f, 1f);

    public void SetUp(float value, Vector3 worldPosition, Camera camera, int effectiveness = 0)
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = value.ToString();
        //text.color = damageColors[value < 5 ? 0 : value < 20 ? 1 : 2];
        if (value > 1) {
            text.color = effectiveness switch {
                -1 => ineffectiveColor,
                1 => effectiveColor,
                _ => defaultColor
            };
        } else {
            text.color = ineffectiveColor;
        }

        rectTransform = GetComponent<RectTransform>();
        float scaleFactor = value < 5 ? 0.8f : value < 20 ? 1.0f : 1.5f;
        rectTransform.localScale = new Vector2(scaleFactor, scaleFactor);
        gameCamera = camera;

        startPosition = worldPosition;
        endPosition = startPosition + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)); //Random.Range(1, 2), Random.Range(1, 2));

        SetPosition(startPosition);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (rectTransform)
        {
            Vector3 position = Vector3.Lerp(startPosition, endPosition, Mathf.Pow((timer / timeToDie), 0.25f));
            SetPosition(position);
        }

        if (timer > timeToBeginFadeOut)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1 - ((timer - timeToBeginFadeOut) / (timeToDie - timeToBeginFadeOut)));
        }

        if (timer > timeToDie)
        {
            Destroy(gameObject);
        }
    }

    void SetPosition(Vector3 position)
    {
        if (rectTransform != null && gameCamera != null)
        {
            rectTransform.localPosition = gameCamera.WorldToScreenPoint(position) - new Vector3(640, 360, 0);
            rectTransform.localPosition = Vector3Int.RoundToInt(new Vector2(rectTransform.localPosition.x, rectTransform.localPosition.y) + new Vector2(320, 180 + 16));
        }
    }
}
