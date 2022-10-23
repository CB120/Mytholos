using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;
using Elements;

public class ShotAbility : Ability
{
    private Transform ShotTransform;
    private Vector3 Direction;

    [Header("Shot Ability Fields")]
    [SerializeField] private float speed;

    private float ShotTimer;
    [SerializeField] private float ShotDuration;

    [SerializeField] private ParticleSystem Light;
    [SerializeField] private ParticleSystem Projectile;
    [SerializeField] private TrailRenderer Trail;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Direction = owningMyth.transform.forward;
        ShotTransform = this.gameObject.transform;
        transform.parent = null;

        var LightColour = Light.colorOverLifetime;
        var ProjectileColour = Projectile.colorOverLifetime;

        Gradient grad = new Gradient();
        Gradient Trailgrad = new Gradient();
    
        GradientColorKey StartColor = new GradientColorKey(ability.element.shotStartColor, 0);
        GradientColorKey EndColor = new GradientColorKey(ability.element.shotEndColor, 1);
        
        grad.SetKeys(new GradientColorKey[] { StartColor, EndColor }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 0.5f), new GradientAlphaKey(1.0f, 1.0f) });
        Trailgrad.SetKeys(new GradientColorKey[] { StartColor}, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) });

        LightColour.color = grad;
        ProjectileColour.color = grad;
        Trail.colorGradient = Trailgrad;
    }

    // Update is called once per frame
    public override void Update()
    {
        ShotTransform.position += Direction * speed * Time.deltaTime;

        ShotTimer += Time.deltaTime;

        if (ShotTimer > ShotDuration)
            Destroy(this.gameObject);
    }

    public override void Trigger(Myth myth)
    {
        Attack(myth, ability.damage); //Called In The Parent Ability
        Debug.LogWarning($"Shot Collided With Object: {myth.gameObject.name}");
        base.Trigger(myth);
        if (myth != owningMyth)
        {
            Destroy(this.gameObject);
        }
        
    }

    public override void TerrainInteraction()
    {
        Destroy(this.gameObject);
    }
}
