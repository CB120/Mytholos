using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour //Parent Class to All Abilities
{
    public SO_Ability ability;

    virtual public void Update()
    {
        
    }
    virtual public void Attack(Myth myth, float damage)
    {
        myth.health -= damage;
    }


    //Just Instantiate the Collider until it hits something and Then Instantiate the Ability Object - Look at Jack's Notes
    //Will most likely just be used for melee attacks to ensure player is close enough.
    virtual public void FakeAttack() 
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        
    }
}
