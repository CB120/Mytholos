using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleVisuals : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnDisable()
    {
        Debug.LogWarning("PARTICLE SYSTEM DISABLED");
        Destroy(this.gameObject);
    }
}
