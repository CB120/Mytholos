using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;
using FMODUnity;

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
    [SerializeField] private ParticleSystem childParticle;
    [SerializeField] private int maxStrength;

    [Header("SFX")]  //SFX stuff, added by Ethan
    public StudioEventEmitter explosionSFX;
    public GameObject groundHitSFXPrefab;
    [Tooltip("s seconds | How long before the 'Lob Ground Hit' SFX Prefab gets destroyed after instantiation. No effect on the heard sound")]
    public float timeToDestroyGroundHit = 1.5f;
    [Tooltip("m/s meters per second | Minimum velocity for the ground hit sound to be heard")]
    public float groundHitSFXVelocityThreshold;

    public void Awake()
    {
        rend.materials[1].SetColor("_Toon_Ramp_Tinting", ability.element.color);
    }

    public override void Start()
    {
        Vector3 force = (Vector3.zero - transform.position) + new Vector3(0, 2, 0);
        if (owningMyth.targetEnemy)
        {
            force = (owningMyth.targetEnemy.gameObject.transform.position - transform.position) + new Vector3(0, 2, 0);
        } 
        else
        {
            Debug.LogWarning("owningMyth.targetEnemy is null, swapping its position for Vector3.zero");
        }

        rigidBody.AddForce(Vector3.ClampMagnitude((force * strength), maxStrength), ForceMode.Impulse);

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
        ParticleSystem ps = Instantiate(childParticle, this.transform.position, Quaternion.identity);
        var main = ps.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().main;
        main.startColor = new ParticleSystem.MinMaxGradient(ability.element.color, ability.element.color * new Color(0.1f, 0.1f, 0.1f));

        hasExploded = true;
        explosionSFX.enabled = true; //SFX, added by Ethan
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= groundHitSFXVelocityThreshold)
        {
            GameObject sound = Instantiate(groundHitSFXPrefab, transform.position, Quaternion.identity);
            Destroy(sound, timeToDestroyGroundHit);
        }
    }
}
