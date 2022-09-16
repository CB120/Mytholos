using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamExtender : MonoBehaviour
{

    private Transform BeamTransform;

    private BoxCollider Collider;
    private MeshRenderer Renderer;

    private bool Activated;
    private bool AtMaxRange;

    private float MaxRange;

    void Start()
    {
        BeamTransform = this.gameObject.transform;
        Collider = gameObject.GetComponentInChildren<BoxCollider>();
        Renderer = gameObject.GetComponentInChildren<MeshRenderer>();
    }

    void Update()
    {    
        if (Activated && !AtMaxRange)
            BeamTransform.localScale += (new Vector3(0, 0, 30.0f)) * Time.deltaTime;

        if (BeamTransform.localScale.z > MaxRange)
            AtMaxRange = true;
    }

    public void SetMaxRange(float Length)
    {
        MaxRange = Length;
    }

    public void Activate()
    {
        if (Collider)
        Collider.enabled = true;
        if(Renderer)
        Renderer.enabled = true;
        Activated = true;

    }

}
