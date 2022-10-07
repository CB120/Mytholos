using UnityEngine;
using Myths;
using Elements;

public class BeamAbility : Ability
{
    private float DurationTimer;

    [SerializeField] private ParticleSystem ParticleSystem;

    [SerializeField] private BeamExtender BeamExtender;
    
    [SerializeField] private float BeamLength;

    [SerializeField] private Transform BeamHead;
    [SerializeField] private Transform BeamOrigin;


    private float ChargeTimer;
    private bool Charged;

    private GradientColorKey FireStart = new GradientColorKey(new Color(0.68f, 0.06f, 0.0f), 0);
    private GradientColorKey FireEnd = new GradientColorKey(new Color(1.0f, 0.25f, 0.0f), 1);
    private GradientColorKey ElectricStart = new GradientColorKey(new Color(1.0f, 0.97f, 0.0f), 0);
    private GradientColorKey ElectricEnd = new GradientColorKey(new Color(0.69f, 1.0f, 0.47f), 1);
    private GradientColorKey WaterStart = new GradientColorKey(new Color(0.0f, 0.06f, 1.0f), 0);
    private GradientColorKey WaterEnd = new GradientColorKey(new Color(0.0f, 0.97f, 1.0f), 1);
    private GradientColorKey IceStart = new GradientColorKey(new Color(0.0f, 0.93f, 1.0f), 0);
    private GradientColorKey IceEnd = new GradientColorKey(new Color(1.0f, 1.0f, 1.0f), 1);
    private GradientColorKey WindStart = new GradientColorKey(new Color(0.65f, 0.65f, 0.65f), 0);
    private GradientColorKey WindEnd = new GradientColorKey(new Color(1.0f, 1.0f, 1.0f), 1);
    private GradientColorKey EarthStart = new GradientColorKey(new Color(0.53f, 0.22f, 0.13f), 0);
    private GradientColorKey EarthEnd = new GradientColorKey(new Color(1.0f, 0.29f, 0.01f), 1);

    private Element element { get => ability.element.element; }

    private GradientColorKey StartColor;
    private GradientColorKey EndColor;


    public override void Start()
    {
        base.Start();

        BeamExtender.SetMaxRange(BeamLength);

        var BeamColour = ParticleSystem.colorOverLifetime;

        Gradient grad = new Gradient();
        SetColor();
        grad.SetKeys(new GradientColorKey[] { StartColor, EndColor}, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });

        BeamColour.color = grad;
    }

    public override void Update()
    {
        ChargeTimer += Time.deltaTime;

        if (ChargeTimer > ability.chargeTime)
        {
            Charged = true;
            transform.GetChild(0).gameObject.SetActive(true);
            ParticleSystem.Play();
        }
            
        if (Charged)
        DurationTimer += Time.deltaTime;

        if (DurationTimer > ability.performTime - ability.chargeTime)
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

    public override void TerrainInteraction()
    {
        BeamExtender.AtMaxRange = true;
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
        }
    }
}
