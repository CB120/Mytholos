using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;

namespace DetectCollision
{
    public class DetectCollision : MonoBehaviour
    {
        [SerializeField] private Ability ability;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Terrain")
                ability.TerrainInteraction();

            Myth attackedMyth = other.gameObject.GetComponent<Myth>();
            if (attackedMyth)
            {
                ability.Trigger(attackedMyth);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            ability.Collision();
        }

    }
}