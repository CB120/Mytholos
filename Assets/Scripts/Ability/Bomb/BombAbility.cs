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

    [Header("BombInFlight")]
    [HideInInspector]
    public Vector3 nextBasePos;


    public override void Start()
    {
        startPos = transform.position;
        //targetPos = owningMyth != null ? owningMyth.targetEnemy.gameObject.transform.position : new Vector3(10, 0, 10);
        targetPos = owningMyth.targetEnemy.gameObject.transform.position;
        base.Start();
        Debug.Log($"StartPos: {startPos}, EndPos: {targetPos}");
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
