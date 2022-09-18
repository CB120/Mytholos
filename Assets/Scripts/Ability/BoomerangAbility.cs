using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;

public class BoomerangAbility : Ability
{
    private Transform BoomerangTransform;
    private Vector3 Direction;

    private bool Returning;

    [SerializeField] private float speed;
    [SerializeField] private float acceleration;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Direction = owningMyth.transform.forward;
        BoomerangTransform = this.gameObject.transform;
        transform.parent = null;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (speed <= 0)
            Returning = true;

        if (!Returning)
        {
            speed -= acceleration * Time.deltaTime;
            BoomerangTransform.position += Direction * speed * Time.deltaTime;
        }
        
        if (Returning)
        {
            speed += acceleration * Time.deltaTime;
            Direction = ( owningMyth.transform.position - BoomerangTransform.position).normalized;
            BoomerangTransform.position += Direction * speed * Time.deltaTime;
            if (Vector3.Distance(BoomerangTransform.position, owningMyth.transform.position) < .001f)
                Destroy(this.gameObject);
        }
            
    }

    public override void Trigger(Myth myth)
    {
        Attack(myth, ability.damage); //Called In The Parent Ability
        Debug.LogWarning($"Shot Collided With Object: {myth.gameObject.name}");
        base.Trigger(myth);
        Destroy(this.gameObject);
    }
}

