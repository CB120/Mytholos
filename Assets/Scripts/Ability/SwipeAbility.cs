using System.Collections;
using System.Collections.Generic;
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
}
