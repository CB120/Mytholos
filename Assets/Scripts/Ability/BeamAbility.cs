using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;

public class BeamAbility : Ability
{
    private float BeamTimer;
    private float BeamDuration;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        BeamDuration = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        BeamTimer += Time.deltaTime;
        if (BeamTimer > BeamDuration)
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
