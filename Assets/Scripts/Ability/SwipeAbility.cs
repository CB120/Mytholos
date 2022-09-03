using System.Collections;
using System.Collections.Generic;
using Myths;
using UnityEngine;

public class SwipeAbility : Ability
{

    private void Start()
    {
        Destroy(this.gameObject, 0.5f);
    }

    public override void Update()
    {
;       base.Update();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Myth myth = collision.gameObject.GetComponent<Myth>();
        myth.TakeDamage(this.baseDamage);
    }
}
