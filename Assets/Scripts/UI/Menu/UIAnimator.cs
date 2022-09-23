using UnityEngine;
using UnityEngine.UI;

// Also contains recolour/retransform information for a menu cursor
public class UIAnimator : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] float frameLength;
    int currentFrame;
    float timer;
    public bool isPlaying = true;
    Image imageComponent;
    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        imageComponent = GetComponent<Image>();
        if (imageComponent != null)
        {
            imageComponent.sprite = sprites[0];
        }
    }

    void Update()
    {
        if (isPlaying)
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
        }
    }

    public void SetColour(Color newColor)
    {
        float alpha = isPlaying ? 1.0f : 0.5f;
        imageComponent.color = new Color(newColor.r, newColor.g, newColor.b, alpha);
    }

    public void SetTransform(RectTransform newTransform)
    {
        transform.parent = newTransform;
        rectTransform.anchorMin.Set(0.0f, 0.0f);
        rectTransform.anchorMax.Set(1.0f, 1.0f);
    }
}
