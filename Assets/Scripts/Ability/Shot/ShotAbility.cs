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

    #region Colours
    private GradientColorKey FireStart = new GradientColorKey(new Color(0.68f, 0.06f, 0.0f), 0);
    private GradientColorKey FireEnd = new GradientColorKey(new Color(0.68f, 0.06f, 0.0f), 1);
    private GradientColorKey ElectricStart = new GradientColorKey(new Color(1.0f, 0.97f, 0.0f), 0);
    private GradientColorKey ElectricEnd = new GradientColorKey(new Color(1.0f, 0.97f, 0.0f), 1);
    private GradientColorKey WaterStart = new GradientColorKey(new Color(0.0f, 0.06f, 1.0f), 0);
    private GradientColorKey WaterEnd = new GradientColorKey(new Color(0.0f, 0.06f, 1.0f), 1);
    private GradientColorKey IceStart = new GradientColorKey(new Color(0.0f, 0.93f, 1.0f), 0);
    private GradientColorKey IceEnd = new GradientColorKey(new Color(0.0f, 0.93f, 1.0f), 1);
    private GradientColorKey WindStart = new GradientColorKey(new Color(0.65f, 0.65f, 0.65f), 0);
    private GradientColorKey WindEnd = new GradientColorKey(new Color(0.65f, 0.65f, 0.65f), 1);
    private GradientColorKey EarthStart = new GradientColorKey(new Color(0.53f, 0.22f, 0.13f), 0);
    private GradientColorKey EarthEnd = new GradientColorKey(new Color(0.53f, 0.22f, 0.13f), 1);
    private GradientColorKey WoodStart = new GradientColorKey(new Color(0f, 0.8f, 0f), 0);
    private GradientColorKey WoodEnd = new GradientColorKey(new Color(0f, 0.8f, 0f), 1);
    #endregion

    private Element element { get => ability.element.element; }

    private GradientColorKey StartColor;

    private GradientColorKey EndColor;
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
        SetColor();
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
        Destroy(this.gameObject);
    }

    public override void TerrainInteraction()
    {
        Destroy(this.gameObject);
    }

    private void SetColor()
    {
        switch (element)
        {
            case Element.Wind:
                StartColor = WindStart;
                EndColor = WindEnd;
                break;
            case Element.Electric:
                StartColor = ElectricStart;
                EndColor = ElectricEnd;
                break;
            case Element.Water:
                StartColor = WaterStart;
                EndColor = WaterEnd;
                break;
            case Element.Fire:
                StartColor = FireStart;
                EndColor = FireEnd;
                break;
            case Element.Earth:
                StartColor = EarthStart;
                EndColor = EarthEnd;
                break;
            case Element.Ice:
                StartColor = IceStart;
                EndColor = IceEnd;
                break;
            case Element.Wood:
                StartColor = WoodStart;
                EndColor = WoodEnd;
                break;
        }
    }
}
