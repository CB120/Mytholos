using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;

public class ShotAbility : Ability
{
    private Transform ShotTransform;
    private Vector3 Direction;

    [SerializeField] private float speed;

    private float ShotTimer;
    [SerializeField] private float ShotDuration;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Direction = owningMyth.transform.forward;
        ShotTransform = this.gameObject.transform;
        transform.parent = null;
    }

    // Update is called once per frame
    public override void Update()
    {
        ShotTransform.position += Direction * speed * Time.deltaTime;

        ShotTimer += Time.deltaTime;

        if (ShotTimer > ShotDuration)
            Destroy(this.gameObject);
    }

    public override void Trigger(Myth myth)
    {
        Attack(myth, ability.damage); //Called In The Parent Ability
        Debug.LogWarning($"Shot Collided With Object: {myth.gameObject.name}");
        base.Trigger(myth);
        Destroy(this.gameObject);
    }
}
