using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookAnimator : MonoBehaviour
{
    [SerializeField] UINodeBook node;
    [SerializeField] GameObject book;
    Vector3 rotation;
    float rotateSpeed;
    // Sine wave animation
    float curveHeight = 10;
    float curveFrequency = 5;
    float curveOffset = 0;
    Vector3 startScale;

    private void Awake()
    {
        rotation = new Vector3(-60f, 150f, 30f);
        rotateSpeed = Random.Range(35, 39);
        startScale = book.transform.localScale;
        node = GetComponent<UINodeBook>();
    }
    void Update()
    {
        if (!node.isSelected)
        {
            book.transform.localScale = startScale / 2f;
            book.transform.Rotate(rotation, (rotateSpeed * Time.deltaTime));
        }
        else
        {
            book.transform.localRotation = Quaternion.Euler(new Vector3(-20, 211, 15));
            float sineValue = Mathf.Sin(Time.time * curveFrequency + (Mathf.PI * curveOffset)) * curveHeight;

            book.transform.localScale = startScale + new Vector3(sineValue, sineValue, sineValue);
        }
    }
}
