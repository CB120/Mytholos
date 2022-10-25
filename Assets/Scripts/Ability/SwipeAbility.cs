using System.Collections;
using System.Collections.Generic;
using Myths;
using UnityEngine;

public class SwipeAbility : Ability
{
    [Header("Shot Ability Fields")]
    [SerializeField] private TrailRenderer trail;

    private void Awake()
    {
        trail.startColor = ability.element.color * new Color(1, 1, 1, 1f);
        trail.endColor = ability.element.color * new Color(1, 1, 1, 0.1f);
    }

    public override void Start()
    {
        base.Start();
        Destroy(this.gameObject, 0.5f);
    }

    public override void Update()
    {
      base.Update();
    }

    public override void Trigger(Myth myth)
    {
        Attack(myth, ability.damage);
        //Debug.LogWarning($"Swipe Collided With Object: {myth.gameObject.name}");
        base.Trigger(myth);
    }
}
