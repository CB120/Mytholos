using UnityEngine;
using UnityEngine.UI;

// Also contains recolour/retransform information for a menu cursor
public class UIAnimator : MonoBehaviour
{
    public bool isPlaying = true;

    [Header("Sprite Animation")]
    [SerializeField] bool animateSprite = true;
    [SerializeField] Sprite[] sprites;
    [SerializeField] float frameLength;
    Image imageComponent;
    int currentFrame;
    float timer;

    [Header("Sine Animation")]
    [SerializeField] bool animateSineX;
    [SerializeField] bool animateSineY;
    [SerializeField] bool animateScale;
    [SerializeField] bool animateRotZ;
    [SerializeField] float curveHeight;
    [SerializeField] float curveFrequency;
    [SerializeField] float curveOffset;
    RectTransform rectTransform;
    Vector2 startPos;
    Vector3 startScale;
    float startRotZ;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform)
        {
            startPos = rectTransform.localPosition;
            startScale = rectTransform.localScale;
            startRotZ = rectTransform.rotation.z;
        }

        imageComponent = GetComponent<Image>();
        if (imageComponent != null && animateSprite)
        {
            imageComponent.sprite = sprites[0];
        }
    }

    private void OnEnable()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas)
            canvas.enabled = true; // Canvas seems to get disabled at some point
    }

    void Update()
    {
        if (isPlaying)
        {
            if (animateSprite)
            {
                timer += Time.deltaTime;
                if (timer >= frameLength)
                {
                    timer -= frameLength;
                    currentFrame = (currentFrame + 1) % sprites.Length;
                    imageComponent.sprite = sprites[currentFrame];
                }
                if (imageComponent.color.a < 1.0f)
                {
                    imageComponent.color = new Color(imageComponent.color.r, imageComponent.color.g, imageComponent.color.b, 1.0f);
                }
            }

            float sineValue = Mathf.Sin(Time.time * curveFrequency + (Mathf.PI * curveOffset)) * curveHeight;

            if (animateSineY && rectTransform)
                rectTransform.localPosition = new Vector2(startPos.x, startPos.y + Mathf.RoundToInt(sineValue));

            if (animateSineX && rectTransform)
                rectTransform.localPosition = new Vector2(startPos.x + Mathf.RoundToInt(sineValue), startPos.y);

            if (animateRotZ && rectTransform)
                rectTransform.localRotation = Quaternion.Euler(rectTransform.localRotation.x, rectTransform.localRotation.y, startRotZ + sineValue);

            if (animateScale && rectTransform)
                rectTransform.localScale = startScale + new Vector3(sineValue, sineValue, sineValue);
        }
        else
        {
            if (timer > 0)
            {
                timer = 0.0f;
            }
            if (currentFrame != 0)
            {
                currentFrame = 0;
                imageComponent.sprite = sprites[0];
            }
            if (imageComponent.color.a >= 1.0f)
            {
                imageComponent.color = new Color(imageComponent.color.r, imageComponent.color.g, imageComponent.color.b, 0.5f);
            }
            if (rectTransform.localPosition != (Vector3)startPos)
                rectTransform.localPosition = startPos;
        }
    }

    public void SetColour(Color newColor)
    {
        //float alpha = isPlaying ? 1.0f : 0.5f;
        //GetComponent<Image>().color = new Color(newColor.r, newColor.g, newColor.b, alpha);
        
        GetComponent<Image>().color = newColor;

        enabled = true;
    }

    public void SetTransform(RectTransform newTransform, float padding = 0)
    {
        transform.SetParent(newTransform, false);
        GetComponent<RectTransform>().anchorMin = new Vector2(0.0f, 0.0f);
        GetComponent<RectTransform>().anchorMax = new Vector2(1.0f, 1.0f);
        GetComponent<RectTransform>().sizeDelta = new Vector2(padding, padding);

        enabled = true;
    }

    public void SetChildrenHidden(bool isHidden)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(!isHidden);
        }
    }

    public void SetHidden(bool isHidden)
    {
        if (imageComponent != null)
            imageComponent.enabled = !isHidden;
        else
            GetComponent<Image>().enabled = !isHidden;

        enabled = true;
    }
}
