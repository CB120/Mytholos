using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimation : MonoBehaviour
{
    Vector3 defaultPosition;
    Vector3 finalPosition;
    [SerializeField] int difference;

    private void Start()
    {
        defaultPosition = new Vector3(transform.localPosition.x - difference, transform.localPosition.y, transform.localPosition.z);
        finalPosition = new Vector3(transform.localPosition.x + difference, transform.localPosition.y, transform.localPosition.z);
    }
    void Update()
    {
        this.transform.localPosition = Vector3.Lerp(defaultPosition, finalPosition, (Mathf.Sin(10f * Time.time)));
           
    }
}
