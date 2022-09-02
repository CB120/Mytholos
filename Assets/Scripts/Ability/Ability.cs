using System.Collections;
using System.Collections.Generic;
using Myths;
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

    public void OnCollisionEnter(Collision collision)
    {
        
    }
}
