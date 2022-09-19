using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamHead : MonoBehaviour
{
    public SphereCollider Collider;
    public MeshRenderer Renderer;

    public bool Activated;

    public Transform BeamHeadPosition;
    private Vector3 BeamHeadPositionVector;

    private void Start()
    {
        Collider = this.GetComponent<SphereCollider>();
        Renderer = this.GetComponent<MeshRenderer>();
    }
    void Update()
    {
        if (Activated) 
        {
            BeamHeadPositionVector = BeamHeadPosition.position;
            gameObject.transform.position = BeamHeadPositionVector;
        }
    }

    public void Activate()
    {
        if (Collider)
            Collider.enabled = true;
        if (Renderer)
            Renderer.enabled = true;
        Activated = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.name == "BeamHead")
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
