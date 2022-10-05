using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;
using Elements;

public class Ability : MonoBehaviour //Parent Class to All Abilities
{
    public SO_Ability ability;
    public Myth owningMyth;
    public ParticleSystem abilityPS;

    public float DamageMultiplier { get; set; } = 1;

    private Element element { get => ability.element.element;}


    virtual public void Start()
    {
        owningMyth.Stamina.Value -= ability.staminaCost;
    }

    virtual public void Update()
    {

    }

    virtual public void Attack(Myth myth, float damage)
    {
        bool isInParty = myth.partyIndex == this.owningMyth.partyIndex;
        ParticleSystem particle = ability.element.debuffParticle;
        if (!isInParty)
        {
            var finalDamage = damage * DamageMultiplier * myth.AttackStat / myth.DefenceStat;
            myth.Health.Value -= finalDamage; ;
        }
        else
        {
            if (ability.element.buffParticle != null) //Temporary to show which debuff is being used
                particle = ability.element.buffParticle; 
        }

        ParticleSystem ps = Instantiate(particle, myth.transform);
        ParticleSystem.MainModule ma = ps.main;
        ma.startColor = ability.element.color;
        ApplyEffect(myth);
    }


    #region Collision
    virtual public void Trigger(Myth myth)
    {

    }

    virtual public void Collision()
    {
        Destroy(gameObject);
    }

    virtual public void ClearDebris(GameObject obj)//@Jack you can call this function to clear debris
    {

    }

    #endregion

    #region Effects
    virtual public void ApplyEffect(Myth myth)
    {
        bool isInParty = myth.partyIndex == this.owningMyth.partyIndex;

        if(isInParty) myth.effectController.DeactivateBuff(ability.element.element, isInParty);
        switch (element)
        {
            case Element.Wind:
                ApplyWindEffect(myth, isInParty);
                myth.effectController.ActivateBuff(Element.Wind, !isInParty, ability.isInstantEffect);
                break;
            case Element.Electric:
                ApplyElectricEffect(myth, isInParty);
                myth.effectController.ActivateBuff(Element.Electric, !isInParty, ability.isInstantEffect);
                break;
            case Element.Water:
                ApplyWaterEffect(myth, isInParty);
                myth.effectController.ActivateBuff(Element.Water, !isInParty, ability.isInstantEffect);
                break;
            case Element.Metal:
                ApplyMetalEffect(myth, isInParty);
                myth.effectController.ActivateBuff(Element.Metal, !isInParty, ability.isInstantEffect);
                break;
            case Element.Fire:
                ApplyFireEffect(myth, isInParty);
                myth.effectController.ActivateBuff(Element.Fire, !isInParty, ability.isInstantEffect);
                break;
            case Element.Earth:
                ApplyEarthEffect(myth, isInParty);
                myth.effectController.ActivateBuff(Element.Earth, !isInParty, ability.isInstantEffect);
                break;
            case Element.Ice:
                ApplyIceEffect(myth, isInParty);
                myth.effectController.ActivateBuff(Element.Ice, !isInParty, ability.isInstantEffect);
                break;
            case Element.Wood:
                ApplyWoodEffect(myth, isInParty);
                myth.effectController.ActivateBuff(Element.Wood, !isInParty, ability.isInstantEffect);
                break;
        }
    }

    virtual public void ApplyWoodEffect(Myth myth, bool isInParty)//Heal Allies, Damage Enemies
    {
        if (isInParty) return;
        float value = ability.damage;//Life Steal
        myth.Health.Value -= ability.damage;
        owningMyth.effectController.Heal(ability.damage / 2);
    }

    virtual public void ApplyElectricEffect(Myth myth, bool isInParty)//Stamina Buff, Stamina Debuff
    {
        if (isInParty) return;
        float value = ability.staminaCost; //If Myth is in the same party add half the stamina cost.
        value = (value / 2); //By Default Decrement the Enemy Myths Stamina
        myth.effectController.AdjustStamina(ability.staminaCost);

        //myth.effectController.ApplyStaminaBuff(ability.element.buffLength, ability.regenSpeed); //Will Apply a Buff Rather than Direct Stamina Boost
    }

    virtual public void ApplyWaterEffect(Myth myth, bool isInParty)
    {
        if (isInParty) return;
        myth.effectController.BuffCleanse();
    }

    virtual public void ApplyIceEffect(Myth myth, bool isInParty)
    {

    }

    virtual public void ApplyWindEffect(Myth myth, bool isInParty)//Agility Buff, Agility Debuff
    {
        if (isInParty) return;
        myth.effectController.AdjustAgility(ability.element.buffLength, ability.statIncrease);
    }

    virtual public void ApplyMetalEffect(Myth myth, bool isInParty)
    {
        if (isInParty) return;
        myth.effectController.DefenceDebuff(3);
    }

    virtual public void ApplyEarthEffect(Myth myth, bool isInParty)
    {
        if (isInParty) return;
        myth.effectController.AgilityDebuff(3);
    }

    virtual public void ApplyFireEffect(Myth myth, bool isInParty)
    {
        if (isInParty) return;
        myth.effectController.Burn(1, 5);
    }
    #endregion

}

