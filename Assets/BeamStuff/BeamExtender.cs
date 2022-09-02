using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamExtender : MonoBehaviour
{
    public BeamSettingsConduit Beam;

    private Transform BeamTransform;

    void Start()
    {
        Beam = transform.parent.GetComponent<BeamSettingsConduit>();
        BeamTransform = this.gameObject.transform;
    }

    void Update()
    {
 
            //Debug.Log("Hi");
            BeamTransform.localScale += (new Vector3(3.0f, 0, 0)) * Time.deltaTime;
       
    }

    
}
