using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffUI : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private float growSpeed;
    public bool isEnabled;
    [SerializeField] Vector2 normalSize;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isEnabled && rect.sizeDelta != normalSize)
            rect.sizeDelta = Vector2.Lerp(rect.sizeDelta, normalSize, growSpeed * Time.deltaTime);
        else
            rect.sizeDelta = Vector2.Lerp(rect.sizeDelta, new Vector2(0, 0), growSpeed * 1.25f * Time.deltaTime);
    }
}
