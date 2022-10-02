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

    [Header("Effects")]
    public float effectValue; //Use to determine the value of the given effect
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

        switch (element)
        {
            case Element.Wind:
                ApplyWindEffect(myth, isInParty);
                break;
            case Element.Electric:
                ApplyElectricEffect(myth, isInParty);
                break;
            case Element.Water:
                ApplyWaterEffect(myth, isInParty);
                break;
            case Element.Metal:
                ApplyMetalEffect(myth, isInParty);
                break;
            case Element.Fire:
                ApplyFireEffect(myth, isInParty);
                break;
            case Element.Earth:
                ApplyEarthEffect(myth, isInParty);
                break;
            case Element.Ice:
                ApplyIceEffect(myth, isInParty);
                break;
            case Element.Wood:
                ApplyWoodEffect(myth, isInParty);
                break;
        }
    }

    virtual public void ApplyWoodEffect(Myth myth, bool isInParty)//Heal Allies, Damage Enemies
    {
        float value = ability.damage;//Life Steal

        if (isInParty)
            myth.effectController.Heal(ability.statIncrease);
        else { //Life Steal
            myth.Health.Value -= ability.damage;
            owningMyth.effectController.Heal(ability.damage / 2);
        }

    }

    virtual public void ApplyElectricEffect(Myth myth, bool isInParty)//Stamina Buff, Stamina Debuff
    {
        float value = ability.staminaCost; //If Myth is in the same party add half the stamina cost.
        if (!isInParty)
            value = (value / 2); //By Default Decrement the Enemy Myths Stamina

        myth.effectController.AdjustStamina(ability.staminaCost);

        //myth.effectController.ApplyStaminaBuff(ability.element.buffLength, ability.regenSpeed); //Will Apply a Buff Rather than Direct Stamina Boost
    }

    virtual public void ApplyWaterEffect(Myth myth, bool isInParty)
    {
        if (!isInParty)
            myth.effectController.BuffCleanse();
    }

    virtual public void ApplyIceEffect(Myth myth, bool isInParty)
    {

    }

    virtual public void ApplyWindEffect(Myth myth, bool isInParty)//Agility Buff, Agility Debuff
    {
        if (isInParty)
            myth.effectController.AdjustAgility(2);
        else
            myth.effectController.Displace(2);
    }

    virtual public void ApplyMetalEffect(Myth myth, bool isInParty)
    {

    }

    virtual public void ApplyEarthEffect(Myth myth, bool isInParty)
    {
        if (!isInParty)
            myth.effectController.AgilityDebuff(3);
    }

    virtual public void ApplyFireEffect(Myth myth, bool isInParty)
    {
        if (!isInParty)
            myth.effectController.Burn(1, 5);
    }
    #endregion

}

