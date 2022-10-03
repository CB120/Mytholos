using UnityEngine;
using Myths;

public class BeamAbility : Ability
{
    private float DurationTimer;

    [SerializeField] private BeamExtender BeamExtender;
    // TODO: Should this be derived from SO_Ability.performTime?
    
    [SerializeField] private float BeamLength;

    public override void Start()
    {
        base.Start();

        BeamExtender.SetMaxRange(BeamLength);
    }

    public override void Update()
    {
        DurationTimer += Time.deltaTime;

        if (DurationTimer > ability.performTime)
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
