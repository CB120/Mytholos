using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;

public class BeamAbility : Ability
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Trigger(Myth myth)
    {
        Attack(myth, ability.damage); //Called In The Parent Ability
        Debug.LogWarning($"Swipe Collided With Object: {myth.gameObject.name}");
        base.Trigger(myth);
    }
}
