using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;

namespace DetectCollision
{
    public class DetectCollision : MonoBehaviour
    {
        [SerializeField] private bool canTakeDamage = true;
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

        private void OnTriggerStay(Collider other)
        {
            Myth attackedMyth = other.gameObject.GetComponent<Myth>();
            if (attackedMyth)
            {
                if (canTakeDamage)
                {
                    StartCoroutine(WaitForSeconds());
                    ability.TriggerStay(attackedMyth);
                }
            }
        }

        IEnumerator WaitForSeconds()
        {
            canTakeDamage = false;
            yield return new WaitForSeconds(0.5f);
            canTakeDamage = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            ability.Collision();
        }

    }
}