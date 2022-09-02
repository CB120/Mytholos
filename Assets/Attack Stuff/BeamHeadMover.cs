using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamHeadMover : MonoBehaviour
{
    public Transform BeamHeadPosition;
    private Vector3 BeamHeadPositionVector;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BeamHeadPositionVector = BeamHeadPosition.position;
        gameObject.transform.position = BeamHeadPositionVector;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.name == "BeamHead")
        {
            Debug.Log("HeadTime");
        }
    }
}
