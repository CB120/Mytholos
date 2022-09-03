using System.Collections;
using System.Collections.Generic;
using Myths;
using UnityEngine;

public class Ability : MonoBehaviour //Parent Class to All Abilities
{
    public SO_Ability ability;
    public Myth owningMyth;

    virtual public void Update()
    {
        
    }
    virtual public void Attack(Myth myth, float damage)
    {
        if (myth.partyIndex != this.owningMyth.partyIndex)//Ensures Myths cannot harm others in their party 
        {                                                 
            Debug.LogWarning($"{myth.gameObject.name} was Attacked by {owningMyth.gameObject.name}");
            myth.TakeDamage(damage);
        }
        else
        {
            //Debug.LogWarning($"{myth.gameObject.name} was Attacked by it's own team");
            myth.TakeDamage(0);
        }
    }

    virtual public void Trigger(Myth myth)
    {

    }
}
