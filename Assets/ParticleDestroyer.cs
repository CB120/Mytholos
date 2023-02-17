using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Move somwhere
public class ParticleDestroyer : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Destroy(other);
    }
}
