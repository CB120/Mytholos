using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamHeadMover : MonoBehaviour
{
    public Transform BeamHeadPosition;
    private Vector3 BeamHeadPositionVector;

   // private BeamAttack Beam;

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
            /*
            Beam = transform.parent.gameObject.GetComponent<BeamAttack>();
            Beam.DestroyBeam();
            */
        }
    }
}
