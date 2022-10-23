using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;
using Elements;

[System.Serializable]
public class ElementSFXPairs
{
    public Element element;
    public GameObject sfx;
}

public class Ability : MonoBehaviour //Parent Class to All Abilities
{
    [Header("All-Ability Fields")]
    public SO_Ability ability;
    public Myth owningMyth;
    public ParticleSystem abilityPS;
    private float SOknockbackStrength;
    private float SOstunTime;
    private float elementModifier = 1;

    public float DamageMultiplier { get; set; } = 1;

    private Element element { get => ability.element.element;}

    [Header("All-Ability SFX")] //SFX stuff, added by Ethan
    public ElementSFXPairs[] elementSFXPairs;
    public float timeToDestroyElementSFX = 0.5f;

    public GameObject takingDamageSFXPrefab;
    public float timeToDestroyTakingDamageSFX = 0.4f;


    virtual public void Start()
    {
        SOstunTime = ability.baseStun;
        SOknockbackStrength = ability.baseKnockback;
        owningMyth.Stamina.Value -= ability.staminaCost;

        foreach (ElementSFXPairs p in elementSFXPairs)
        {
            if (p.element == element)
            {
                //Currently commented out so it doesn't cause errors
                GameObject sfx = Instantiate(p.sfx, transform);
                Destroy(sfx, timeToDestroyElementSFX);
            }
        }
    }

    virtual public void Update()
    {

    }

    virtual public void Attack(Myth myth, float damage)
    {
        bool isInParty = myth.partyIndex == this.owningMyth.partyIndex;
        if (isInParty && !ability.applyBuffToParty) return; //Guard if we don't want ability to give allies buffs

        ParticleSystem particle = ability.element.debuffParticle;
        ApplyEffect(myth);

        if (!isInParty)
        {
            if (ability.element.strongAgainst.Contains(myth.element))
            {
                Debug.Log("Attack is Strong!!");
                elementModifier = 2;
            }
            var finalDamage = damage * elementModifier * DamageMultiplier * owningMyth.AttackStat / myth.DefenceStat;
            //Debug.Log("attack stat is " + owningMyth.AttackStat + "Final Damage is " + finalDamage);

            if (myth.effectController.appliedDebuffs.Contains(Element.Wood))//Health Steal if wood buff is applied
            {
                owningMyth.Health.Value += (finalDamage / 2f);
            }

            if (!myth.effectController.appliedBuffs.Contains(Element.Ice)) //If The Myth Doesn't Currently Have An Ice Buff
            {
                myth.Health.Value -= finalDamage;
            }
            else
            {
                myth.effectController.RemoveIceOvershield();
            }

            GameObject sfxGameObject = Instantiate(takingDamageSFXPrefab, transform.position, Quaternion.identity);
            Destroy(sfxGameObject, timeToDestroyTakingDamageSFX);
        }
        else
        {
            if (ability.element.buffParticle != null) //Temporary to show which debuff is being used
                particle = ability.element.buffParticle; 
        }

        if (ability.element.element != Element.Ice)
        {
            ParticleSystem ps = Instantiate(particle, myth.transform);
            if (ability.element.setParticleColor)
            {
                Debug.LogWarning("Color is Set");
                ParticleSystem.MainModule main = ps.main;
                main.startColor = ability.element.color;
            }
        }

        if (SOknockbackStrength > 0)
        {
            //Debug.Log(SOstunTime);
            myth.Knockback(SOknockbackStrength, owningMyth.gameObject, SOstunTime);
        }

        if(SOstunTime > 0 && SOknockbackStrength == 0)
        {
            myth.Stun(SOstunTime);
        }

        // Animate the myth that was hit
        Animator anim = myth.gameObject.GetComponentInChildren<Animator>();
        if (anim)
        {
            anim.speed = 1.0f;
            anim.SetTrigger("Hurt");
        }
    }

    #region Collision
    virtual public void Trigger(Myth myth)
    {

    }

    virtual public void TriggerStay(Myth myth)
    {

    }

    virtual public void Collision()
    {
        Destroy(gameObject);
    }

    virtual public void ClearDebris(GameObject obj)//@Jack you can call this function to clear debris
    {

    }

    virtual public void TerrainInteraction()
    {

    }

    #endregion

    #region Effects
    virtual public void ApplyEffect(Myth myth)
    {
        bool isInParty = myth.partyIndex == this.owningMyth.partyIndex;
        switch (element)
        {
            case Element.Wind:
                ApplyWindEffect(myth, isInParty);
                myth.effectController.ActivateBuff(Element.Wind, !isInParty);
                break;
            case Element.Electric:
                ApplyElectricEffect(myth, isInParty);
                break;
            case Element.Water:
                ApplyWaterEffect(myth, isInParty);
                myth.effectController.ActivateBuff(Element.Water, !isInParty);
                break;
            case Element.Metal:
                ApplyMetalEffect(myth, isInParty);
                myth.effectController.ActivateBuff(Element.Metal, !isInParty);
                break;
            case Element.Fire:
                ApplyFireEffect(myth, isInParty);
                myth.effectController.ActivateBuff(Element.Fire, !isInParty);
                break;
            case Element.Earth:
                ApplyEarthEffect(myth, isInParty);
                myth.effectController.ActivateBuff(Element.Earth, !isInParty);
                break;
            case Element.Ice:
                ApplyIceEffect(myth, isInParty);
                myth.effectController.ActivateBuff(Element.Ice, !isInParty);
                break;
            case Element.Wood:
                ApplyWoodEffect(myth, isInParty);
                break;
        }
    }

    virtual public void ApplyWoodEffect(Myth myth, bool isInParty)//While Debuff is Active, all damage to enemies dealt will heal the attacking player slightly
    {
        if (isInParty) return;
        myth.effectController.ActivateBuff(Element.Wood, !isInParty);
        myth.effectController.ApplyLifeStealDebuff(!isInParty, ability.element.buffLength);
    }

    virtual public void ApplyElectricEffect(Myth myth, bool isInParty)//Stamina Buff, Stamina Debuff
    {
        if (isInParty) return;
        myth.effectController.ActivateBuff(Element.Electric, !isInParty);
        myth.effectController.ApplyStaminaEffect(!isInParty, ability.element.buffLength);
    }

    virtual public void ApplyIceEffect(Myth myth, bool isInParty)//Freezes Enemy, Grants Overshield
    {
        myth.effectController.ActivateBuff(Element.Ice, !isInParty);
        if (isInParty && ability.applyBuffToParty)
            myth.effectController.IceOvershield();
        else
            myth.effectController.FreezeDebuff(ability.element.buffLength);
    }

    virtual public void ApplyWindEffect(Myth myth, bool isInParty)//Agility Buff, Agility Debuff
    {
        if(isInParty && ability.applyBuffToParty)
        {
            myth.effectController.AgilityBuff(ability.element.buffLength);
            myth.effectController.ActivateBuff(Element.Ice, !isInParty);
        }
        else if(!isInParty)
        {
            myth.effectController.Disorient(ability.statIncrease, owningMyth, ability.element.buffLength / 2);
        }
    }

    virtual public void ApplyWaterEffect(Myth myth, bool isInParty)
    {
        if (isInParty) return;
        myth.effectController.BuffCleanse();
    }

    virtual public void ApplyMetalEffect(Myth myth, bool isInParty)
    {
        if (isInParty) return;
        myth.effectController.DefenceDebuff(ability.element.buffLength);
        myth.effectController.ActivateBuff(Element.Metal, !isInParty);
    }

    virtual public void ApplyEarthEffect(Myth myth, bool isInParty)
    {
        if (isInParty) return;
        myth.effectController.AgilityDebuff(ability.element.buffLength);
        myth.effectController.ActivateBuff(Element.Earth, !isInParty);
    }

    virtual public void ApplyFireEffect(Myth myth, bool isInParty)
    {
        if (isInParty) return;
        myth.effectController.Burn(5, ability.element.buffLength);
        myth.effectController.ActivateBuff(Element.Fire, !isInParty);
    }
    #endregion

}

