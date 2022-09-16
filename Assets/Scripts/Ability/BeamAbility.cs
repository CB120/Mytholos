using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;

public class BeamAbility : Ability
{
    private float ChargeTimer;
    private float DurationTimer;

    [SerializeField] private float BeamDuration;
    [SerializeField] private float BeamLength;

    BeamExtender BeamExtender;
    BeamHead BeamHead;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        BeamExtender = gameObject.GetComponentInChildren<BeamExtender>();
        BeamHead = gameObject.GetComponentInChildren<BeamHead>();

        BeamExtender.SetMaxRange(BeamLength);

    }

    // Update is called once per frame
    public override void Update()
    {
        ChargeTimer += Time.deltaTime;
        DurationTimer += Time.deltaTime;

        if (ChargeTimer > ability.chargeTime)
        {
            if (BeamHead)
            {
                BeamHead.Activate();
            }

            if (BeamExtender)
            {
                BeamExtender.Activate();
            }
        }

        if (DurationTimer > BeamDuration)
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
