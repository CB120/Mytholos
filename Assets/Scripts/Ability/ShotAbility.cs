using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;

public class ShotAbility : Ability
{
    private Transform ShotTransform;
    private float speed;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        ShotTransform = this.gameObject.transform;
        speed = 5;
    }

    // Update is called once per frame
    public override void Update()
    {
        ShotTransform.position += owningMyth.transform.forward * speed * Time.deltaTime;
    }

    public override void Trigger(Myth myth)
    {
        Attack(myth, ability.damage); //Called In The Parent Ability
        Debug.LogWarning($"Shot Collided With Object: {myth.gameObject.name}");
        base.Trigger(myth);
    }
}
