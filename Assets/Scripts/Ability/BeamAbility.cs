using UnityEngine;
using Myths;

public class BeamAbility : Ability
{
    private float DurationTimer;

    [SerializeField] private float BeamDuration;
    [SerializeField] private float BeamLength;

    BeamExtender BeamExtender;
    BeamHead BeamHead;

    public override void Start()
    {
        base.Start();
        BeamExtender = gameObject.GetComponentInChildren<BeamExtender>();
        BeamHead = gameObject.GetComponentInChildren<BeamHead>();

        BeamExtender.SetMaxRange(BeamLength);

        if (BeamHead)
        {
            BeamHead.Activate();
        }

        if (BeamExtender)
        {
            BeamExtender.Activate();
        }
    }

    public override void Update()
    {
        DurationTimer += Time.deltaTime;

        if (DurationTimer > BeamDuration)
        {
            Destroy(gameObject);
        }
    }

    public override void Trigger(Myth myth)
    {
        Attack(myth, ability.damage); //Called In The Parent Ability
        Debug.LogWarning($"Beam Collided With Object: {myth.gameObject.name}");
        base.Trigger(myth);
    }
}
