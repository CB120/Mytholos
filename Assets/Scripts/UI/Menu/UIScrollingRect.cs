using UnityEngine;
using UnityEngine.UI;

public class UIScrollingRect : MonoBehaviour
{
    [SerializeField] RawImage image;
    [SerializeField] float xSpeed, ySpeed;

    void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(xSpeed, ySpeed) * Time.deltaTime, image.uvRect.size);
    }
}
