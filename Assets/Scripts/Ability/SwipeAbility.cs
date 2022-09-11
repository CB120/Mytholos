using System.Collections;
using System.Collections.Generic;
using Myths;
using UnityEngine;

public class SwipeAbility : Ability
{

    public override void Start()
    {
        base.Start();
        Destroy(this.gameObject, 0.5f);
    }

    public override void Update()
    {
      base.Update();
    }

    public override void Trigger(Myth myth)
    {
        Attack(myth, ability.damage);
        //Debug.LogWarning($"Swipe Collided With Object: {myth.gameObject.name}");
        base.Trigger(myth);
    }
}
