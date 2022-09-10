using System.Collections;
using System.Collections.Generic;
using Myths;
using UnityEngine;

public class SwipeAbility : Ability
{

    private void Start()
    {
        Destroy(this.gameObject, 0.5f);
        owningMyth.Stamina -= ability.stamina;
    }

    public override void Update()
    {
;       base.Update();
    }

    public override void Trigger(Myth myth)
    {
        Attack(myth, ability.damage);
        //Debug.LogWarning($"Swipe Collided With Object: {myth.gameObject.name}");
        base.Trigger(myth);
    }
}
