using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;
public class LobAbility : Ability
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] float strength;
    [SerializeField] private MeshRenderer rend;

    [Header("Explosion")]
    public float areaOfEffect;
    public float timeToExplode;
    private bool hasExploded;
    [SerializeField] private float expandSpeed;
    [SerializeField] private float timeToDestroy;
    public CapsuleCollider triggerCollider;

    void Start()
    {
        Vector3 force = (owningMyth.targetEnemy.gameObject.transform.position - transform.position) + new Vector3(0, 2, 0);
        rigidBody.AddForce((force * strength), ForceMode.Impulse);

        Invoke("Explode", timeToExplode);
        base.Start();
    }

    private void FixedUpdate()
    {
        if (hasExploded)
        {
            triggerCollider.radius = Mathf.Lerp(triggerCollider.radius, areaOfEffect, expandSpeed * Time.deltaTime);
        }
    }

    void Explode()
    {
        rend.enabled = false;
        Instantiate(abilityPS, this.transform);
        hasExploded = true;
        Destroy(this.gameObject, timeToDestroy);
    }

    private void OnTriggerEnter(Collider other)
    {
        Myth attackedMyth = other.gameObject.GetComponent<Myth>();
        if (attackedMyth)
        {
            Attack(attackedMyth, ability.damage);
        }
    }
}
