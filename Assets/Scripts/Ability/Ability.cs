using System.Collections;
using System.Collections.Generic;
using Myths;
using UnityEngine;

public class Ability : MonoBehaviour //Parent Class to All Abilities
{
    public SO_Ability ability;
    public float baseDamage;
    public Myth owningMyth;

    virtual public void Update()
    {
        
    }
    virtual public void Attack(Myth myth, float damage)
    {
        if (myth.partyIndex != this.owningMyth.partyIndex)
        {
            myth.TakeDamage(damage);
        }
    }
}
