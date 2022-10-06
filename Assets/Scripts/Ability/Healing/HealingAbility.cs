using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;

public class HealingAbility : Ability
{
    public float areaOfEffect = 2;
    public float expandSpeed = 2f;
    public float timeToDestroy = 5f;
    [SerializeField] private TrailRenderer[] trails;
    [SerializeField]
    HashSet<Myth> overlappedMyths = new HashSet<Myth>();

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
            if(!myth.effectController.appliedBuffs.Contains(Elements.Element.Ice))
            myth.effectController.DeactivateBuff(ability.element.element, myth.partyIndex != owningMyth.partyIndex);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Myth myth = other.gameObject.GetComponent<Myth>();
        if (myth == null) return;
        if (ability.element.element == Elements.Element.Ice) ApplyEffect(myth);

        overlappedMyths.Add(myth);
        myth.effectController.ActivateBuff(ability.element.element, myth.partyIndex != owningMyth.partyIndex);
        InvokeRepeating("SpawnEffects", 0, 0.5f);
    }

    //Effect Application
    private void OnTriggerStay(Collider other)//Would have preferred this to be onTrigger Enter, however if there are overlapping pools, an effect may be removed
    {
        Myth myth = other.gameObject.GetComponent<Myth>();
        if (myth == null || ability.element.element == Elements.Element.Ice) return;
            ApplyEffect(myth);
    }

    private void OnTriggerExit(Collider other)
    {
        Myth myth = other.gameObject.GetComponent<Myth>();
        if (!myth) return;
        myth.Health.regenSpeed = myth.Stamina.defaultRegenSpeed;
        myth.Stamina.regenSpeed = myth.Stamina.defaultRegenSpeed;
        if (!myth.effectController.appliedBuffs.Contains(Elements.Element.Ice))
            myth.effectController.DeactivateBuff(ability.element.element, myth.partyIndex != owningMyth.partyIndex);

        if (overlappedMyths.Contains(myth)) overlappedMyths.Remove(myth);
    }

    private void SpawnEffects()
    {
        foreach (Myth myth in overlappedMyths)
        {
            if (myth.partyIndex == this.owningMyth.partyIndex)
            {
                ParticleSystem ps = Instantiate(ability.element.buffParticle, myth.transform);
                ParticleSystem.MainModule ma = ps.main;
                ma.startColor = ability.element.color;
            }
        }
    }

    public override void ApplyEarthEffect(Myth myth, bool isInParty)
    {
        if (isInParty) myth.effectController.DefenceBuff(3);
        if (!isInParty) myth.effectController.AgilityDebuff(3);
    }

    public override void ApplyElectricEffect(Myth myth, bool isInParty)
    {
        if (isInParty) myth.Stamina.regenSpeed = ability.regenSpeed;
    }

    public override void ApplyFireEffect(Myth myth, bool isInParty)
    {
        if (isInParty) myth.effectController.AttackBuff(3);
        if (!isInParty) myth.effectController.Burn(1, 5);
    }

    public override void ApplyIceEffect(Myth myth, bool isInParty)
    {
        if(isInParty)
        myth.effectController.IceOvershield();
    }

    public override void ApplyMetalEffect(Myth myth, bool isInParty)
    {
        if (isInParty) myth.effectController.MetalDefence(0.1f);
        if (!isInParty) myth.effectController.DefenceDebuff(3);
    }

    public override void ApplyWaterEffect(Myth myth, bool isInParty)
    {
        if (isInParty) myth.effectController.DebuffCleanse();
        if (!isInParty) myth.effectController.BuffCleanse();
    }

    public override void ApplyWindEffect(Myth myth, bool isInParty)
    {
        //myth.effectController.AdjustAgility(ability.element.buffLength, ability.statIncrease);
    }

    public override void ApplyWoodEffect(Myth myth, bool isInParty)
    {
        myth.Health.regenSpeed = ability.regenSpeed;
    }
}
