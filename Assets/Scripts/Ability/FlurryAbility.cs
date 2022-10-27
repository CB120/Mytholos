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
    [SerializeField] private ParticleSystem PS1; //Obsolete
    [SerializeField] private ParticleSystem PS2; //Obsolete
    [SerializeField] ParticleSystemRenderer pr;

    public override void Start()
    {
        base.Start();
        pr.material.SetColor("_Toon_Ramp_Tinting", ability.element.color);
        Collider = gameObject.transform.GetChild(0);
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
