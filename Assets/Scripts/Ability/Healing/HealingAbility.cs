using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;

public class HealingAbility : Ability
{
    [Header("Heal Ability Fields")]
    public float areaOfEffect = 2;
    public float expandSpeed = 2f;

    private float BuffLimiter = 0.1f;
    public float timeToDestroy { get => ability.timeToDestroy; }
    [SerializeField] private TrailRenderer[] trails;
    [SerializeField]
    HashSet<Myth> overlappedMyths = new HashSet<Myth>();
    [SerializeField] private Collider trigger;

    public override void Start()
    {
        Invoke("ResetScale", (timeToDestroy * 0.75f));
        Invoke("DeactivateBuff", timeToDestroy * 0.9f);
        Destroy(gameObject, timeToDestroy);

        foreach(TrailRenderer trail in trails)
        {
            trail.startColor = ability.element.color;
            trail.endColor = ability.element.color * new Color(1, 1, 1, 0.25f);
        }
        base.Start();
    }

    public override void Update()
    {
        transform.localScale = Vector3.Lerp(
                transform.localScale,
                new Vector3(areaOfEffect, transform.localScale.y, areaOfEffect),
                expandSpeed * Time.deltaTime
        );
        base.Update();
    }

    private void ResetScale()
    {
        areaOfEffect = 0;
    }

    private void DeactivateBuff()
    {
        foreach (Myth myth in overlappedMyths)
        {
            trigger.enabled = false;
            if (!myth.effectController.appliedBuffs.Contains(Elements.Element.Ice))
            myth.effectController.DeactivateBuff(ability.element.element, myth.PartyIndex != owningMyth.PartyIndex);
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Myth myth = other.gameObject.GetComponent<Myth>();
        if (myth == null || myth.partyIndex != owningMyth.partyIndex) return;
        if (EffectWillRemain())
        {
            ApplyEffect(myth);
        }

        overlappedMyths.Add(myth);
        myth.effectController.ActivateBuff(ability.element.element, myth.PartyIndex != owningMyth.PartyIndex);
        InvokeRepeating("SpawnEffects", 0, 1f);
    }

    //Effect Application
    private void OnTriggerStay(Collider other)//Would have preferred this to be onTrigger Enter, however if there are overlapping pools, an effect may be removed
    {
        Myth myth = other.gameObject.GetComponent<Myth>();
        if (myth == null || EffectWillRemain() || myth.partyIndex != owningMyth.partyIndex) return;
            ApplyEffect(myth);
    }

    private void OnTriggerExit(Collider other)
    {
        Myth myth = other.gameObject.GetComponent<Myth>();
        if (!myth) return;
        myth.Health.RegenSpeed = myth.Stamina.defaultRegenSpeed;
        myth.Stamina.RegenSpeed = myth.Stamina.defaultRegenSpeed;

        if (!EffectWillRemain())//If we don't want healing pool to wipe effect on exit
        {
            myth.effectController.DeactivateBuff(ability.element.element, myth.PartyIndex != owningMyth.PartyIndex);
        }

        if (ability.element.element == Elements.Element.Wood)
            myth.Health.RegenSpeed = 0;

        if (overlappedMyths.Contains(myth)) overlappedMyths.Remove(myth);
    }

    private bool EffectWillRemain()
    {
        switch (ability.element.element)
        {
            case Elements.Element.Ice: return true;
            case Elements.Element.Wind: return true;
            default: return false;
        }
    }

    private void SpawnEffects()
    {
        foreach (Myth myth in overlappedMyths)
        {
            if (myth.PartyIndex == this.owningMyth.PartyIndex)
            {
                if (!myth.gameObject.activeInHierarchy) { 
                    overlappedMyths.Remove(myth);
                    return;
                }

                ParticleSystem ps = Instantiate(ability.element.buffParticle, myth.transform);
                if (ability.element.setParticleColor)
                {
                    ParticleSystem.MainModule ma = ps.main;
                    ma.startColor = ability.element.color;
                }
            }
        }
    }

    public override void ApplyEarthEffect(Myth myth, bool isInParty)
    {
        if (!isInParty) return; 
        myth.effectController.DefenceBuff(BuffLimiter);
    }

    public override void ApplyElectricEffect(Myth myth, bool isInParty)
    {
        if (isInParty) myth.Stamina.RegenSpeed = ability.regenSpeed;
    }

    public override void ApplyFireEffect(Myth myth, bool isInParty)
    {
        if (!isInParty) return;
        myth.effectController.AttackBuff(BuffLimiter);
    }

    public override void ApplyIceEffect(Myth myth, bool isInParty)
    {
        if(isInParty)
        myth.effectController.IceOvershield();
    }

    public override void ApplyMetalEffect(Myth myth, bool isInParty)
    {
        if(isInParty)
        myth.effectController.SizeBuff(BuffLimiter);
    }

    public override void ApplyWaterEffect(Myth myth, bool isInParty)
    {
        if (!isInParty) return;
        myth.effectController.DebuffCleanse();
    }

    public override void ApplyWindEffect(Myth myth, bool isInParty)
    {
        if(isInParty)
        myth.effectController.AgilityBuff(ability.element.buffLength);

    }

    public override void ApplyWoodEffect(Myth myth, bool isInParty)
    {
        if(isInParty)
        myth.Health.RegenSpeed = ability.regenSpeed;
    }
}
