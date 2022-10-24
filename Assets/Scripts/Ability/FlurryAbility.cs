using UnityEngine;
using Myths;
using Elements;

public class FlurryAbility : Ability
{
    private Color color;

    private float DurationTimer;

    private float performTimer;

    Transform Collider;

    [Header("Flurry Ability Fields")]
    [SerializeField] private ParticleSystem PS1;
    [SerializeField] private ParticleSystem PS2;

    public override void Start()
    {
        var FlurryColor = PS1.colorOverLifetime;

        Gradient grad = new Gradient();
    
        GradientColorKey StartColor = new GradientColorKey(ability.element.flurryStartColor, 0);
        GradientColorKey EndColor = new GradientColorKey(ability.element.flurryEndColor, 1);
        
        grad.SetKeys(new GradientColorKey[] { StartColor, EndColor }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });

        FlurryColor.color = grad;

        Collider = gameObject.transform.GetChild(0);

        PlayElementalSFX(); //remove this if you're adding base.Start() !
    }

    public override void Update()
    {
        performTimer += Time.deltaTime;

        if (performTimer > ability.chargeTime)
        {
            DurationTimer += Time.deltaTime;
            if (Collider) 
            Collider.gameObject.SetActive(true);
            
        }
       
        if (DurationTimer > ability.performTime - ability.chargeTime)
        {
            Destroy(gameObject);
        }
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
}
