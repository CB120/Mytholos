using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;

public class DetectCollision : MonoBehaviour
{
    [SerializeField] private Ability ability;
    private void OnTriggerEnter(Collider other)
    {
        Myth attackedMyth = other.gameObject.GetComponent<Myth>();
        if (attackedMyth)
        {
            ability.Trigger(attackedMyth);
        }
    }
}
