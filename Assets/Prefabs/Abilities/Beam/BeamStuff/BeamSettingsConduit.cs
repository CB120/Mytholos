using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamSettingsConduit : MonoBehaviour
{

    public BeamAttack Beam;
    // Start is called before the first frame update
    void Start()
    {
        Beam = transform.parent.GetComponent<BeamAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
