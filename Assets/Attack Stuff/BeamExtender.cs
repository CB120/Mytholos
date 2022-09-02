using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamExtender : MonoBehaviour
{
    private Transform BeamTransform;

    // Start is called before the first frame update
    void Start()
    {
        BeamTransform = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        BeamTransform.localScale += (new Vector3(3, 0, 0)) * Time.deltaTime;
    }

    
}
