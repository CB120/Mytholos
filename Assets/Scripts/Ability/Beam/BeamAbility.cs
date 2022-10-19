using UnityEngine;
using Myths;
using Elements;
using FMODUnity;

public class BeamAbility : Ability
{
    private float DurationTimer;

    [Header("Beam Ability Fields")]
    [SerializeField] private ParticleSystem ParticleSystem;

    [SerializeField] private BeamExtender BeamExtender;
    
    [SerializeField] private float BeamLength;

    [SerializeField] private Transform BeamHead;
    [SerializeField] private Transform BeamOrigin;

    [SerializeField] private StudioEventEmitter beamLoops;
    [SerializeField] private StudioEventEmitter beamFiredSFX;

    #region Colours
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
    #endregion

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
        
        transform.GetChild(0).gameObject.SetActive(true);
        ParticleSystem.Play();
        
        // Animation stuff
        Animator anim = owningMyth.gameObject.GetComponentInChildren<Animator>();
        if (anim)
        {
            anim.speed = 1.0f;
            anim.SetTrigger("AttackSpecial");
        }
    }

    public override void Update()
    {
        DurationTimer += Time.deltaTime;

        if (DurationTimer > ability.performTime)
        {
            Destroy(gameObject);
        }

        // SFX stuff added by Ethan
        float beamProgress = 0f;
        
        // BUG: Broken by moving charge handling to state machine.
        // if (Charged)
        // {
        //     beamProgress = (DurationTimer) / (ability.performTime - ability.chargeTime) * 100 + 100;
        //     beamFiredSFX.enabled = true;
        // } else
        // {
        //     beamProgress = ChargeTimer / ability.chargeTime * 100;
        // }
        // beamLoops.SetParameter("Beam Progress", beamProgress);
    }

    public override void TriggerStay(Myth myth)
    {
        Trigger(myth);
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
