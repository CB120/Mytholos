using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;

public class FlurryAbility : Ability
{
    private float ChargeTimer;
    private float DurationTimer;

    Transform Collider;

    [SerializeField] private float FlurryDuration;

    public override void Start()
    {
        base.Start();
        Collider = this.gameObject.transform.GetChild(0);
    }

    public override void Update()
    {
        ChargeTimer += Time.deltaTime;
        DurationTimer += Time.deltaTime;

        if (ChargeTimer > ability.chargeTime)
        {
            if (Collider)
            {
                Collider.gameObject.SetActive(true);
            }
        }

        if (DurationTimer > FlurryDuration)
        {
            Destroy(this.gameObject);
        }
    }

    public override void Trigger(Myth myth)
    {
        Attack(myth, ability.damage); //Called In The Parent Ability
        Debug.LogWarning($"Beam Collided With Object: {myth.gameObject.name}");
        base.Trigger(myth);
    }
}
