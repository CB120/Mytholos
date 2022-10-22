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

    [SerializeField] private StudioEventEmitter beamFiredLoopSFX;
    [SerializeField] private StudioEventEmitter beamFiredSFX;

    public override void Start()
    {
        base.Start();

        BeamExtender.SetMaxRange(BeamLength);

        var BeamColour = ParticleSystem.colorOverLifetime;

        Gradient grad = new Gradient();
        
        GradientColorKey StartColor = new GradientColorKey(ability.element.beamStartColor, 0);
        GradientColorKey EndColor = new GradientColorKey(ability.element.beamEndColor, 1);

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

        beamFiredLoopSFX.SetParameter("Beam Progress", DurationTimer / ability.performTime * 100);
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
}
