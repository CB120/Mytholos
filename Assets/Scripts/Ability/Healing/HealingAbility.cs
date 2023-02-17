using System.Collections.Generic;
using UnityEngine;
using Myths;

// TODO: Rename to Pool Ability
public class HealingAbility : Ability
{
    [Header("Heal Ability Fields")]
    public float areaOfEffect = 2;
    public float expandSpeed = 2f;

    public float timeToDestroy => ability.timeToDestroy;
    [SerializeField] private TrailRenderer[] trails;
    HashSet<Myth> overlappedMyths = new HashSet<Myth>();

    public override void Start()
    {
        Invoke("ResetScale", (timeToDestroy * 0.75f));
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

    private void OnTriggerEnter(Collider other)
    {
        Myth myth = other.gameObject.GetComponent<Myth>();
        if (myth == null || myth.PartyIndex != owningMyth.PartyIndex) return;
        ApplyEffect(myth);

        overlappedMyths.Add(myth);
        InvokeRepeating("SpawnEffects", 0, 1f);
    }

    private void OnTriggerExit(Collider other)
    {
        Myth myth = other.gameObject.GetComponent<Myth>();
        if (!myth) return;
        myth.Health.RegenSpeed = myth.Health.defaultRegenSpeed;

        if (!EffectWillRemain())//If we don't want healing pool to wipe effect on exit
        {
            // TODO: Call RemoveEffect
        }

        if (ability.element.element == Elements.Element.Wood)
            myth.Health.RegenSpeed = 0;

        if (overlappedMyths.Contains(myth)) overlappedMyths.Remove(myth);
    }

    private bool EffectWillRemain()
    {
        switch (ability.element.element)
        {
            case Elements.Element.Ice: return true; // Overshield
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

    // TODO: Effects should remain for as long as the myth is in the pool
    public override void ApplyEffect(Myth myth)
    {
        bool isInParty = myth.PartyIndex == owningMyth.PartyIndex;
        
        myth.effectController.ApplyEffect(SO_Element, !isInParty);
    }
}
