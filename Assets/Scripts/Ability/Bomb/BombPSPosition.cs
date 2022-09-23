using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPSPosition : MonoBehaviour
{
    public GameObject parent;
    public GameObject child;
    public GameObject particle;

    void FixedUpdate()
    {
        particle.transform.position = new Vector3(parent.transform.position.x, child.transform.position.y, parent.transform.position.z);
    }
}
