using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamHead : MonoBehaviour
{
    public Transform BeamHeadPosition;
    private Vector3 BeamHeadPositionVector;


    void Update()
    {
        BeamHeadPositionVector = BeamHeadPosition.position;
        gameObject.transform.position = BeamHeadPositionVector;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.name == "BeamHead")
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
