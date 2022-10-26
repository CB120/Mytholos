using Myths;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAbility : Ability
{
    [Tooltip("Horizontal speed, in units/sec")]
    public float speed = 10;

    [Header("Objects")]
    [SerializeField] private MeshRenderer mesh;

    [HideInInspector]
	public Vector3 startPos;
    [HideInInspector]
    public Vector3 targetPos;
    [HideInInspector]
    public bool hasReachedPosition = false;
    

    [Header("Bomb Effects")]
	public float areaOfEffect = 1.5f;
    public float expandSpeed = 0.5f;
    [SerializeField] private ParticleSystem childParticle;

    [Header("BombInFlight")]
    [HideInInspector]
    public Vector3 nextBasePos;

    public void Awake()
    {
        mesh.material.SetColor("_Toon_Ramp_Tinting", ability.element.color);
        var main = childParticle.main;
        main.startColor = new ParticleSystem.MinMaxGradient(ability.element.color, ability.element.color * new Color(0.1f, 0.1f, 0.1f));
    }

    public override void Start()
    {
        startPos = transform.position;
        if (owningMyth.targetEnemy) 
        {
            targetPos = owningMyth.targetEnemy.gameObject.transform.position;
        } else
        {
            targetPos = Vector3.zero;
            Debug.LogWarning("owningMyth.targetEnemy is null, default-targeting the Origin.");
        }
        base.Start();
    }

    public override void Update()
    {
        // Compute the next position, with arc added in
        if (!hasReachedPosition)
        {
            nextBasePos = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            transform.position = nextBasePos;
        }
        base.Update();
    }

    public override void Trigger(Myth myth)
    {
        Attack(myth, ability.damage);
        base.Trigger(myth);
    }
}
