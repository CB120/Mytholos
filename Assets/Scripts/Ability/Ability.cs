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
        owningMyth.Stamina.Value -= ability.stamina;
    }

    virtual public void Update()
    {

    }

    virtual public void Attack(Myth myth, float damage)
    {
        bool isInParty = myth.partyIndex == this.owningMyth.partyIndex;
        if (!isInParty) {
            var finalDamage = damage * DamageMultiplier;
            myth.Health.Value -= finalDamage;
        }

        if (ability.element.effectParticle != null) //Temporary to show which debuff is bein
        {
            ParticleSystem ps = Instantiate(ability.element.effectParticle, myth.transform);
            ParticleSystem.MainModule ma = ps.main;
            ma.startColor = ability.element.color;
        }

        switch (element)
        {
            case Element.Wind: ApplyWindEffect(myth, isInParty);
                break;
            case Element.Electric: ApplyElectricEffect(myth, isInParty);
                break;
            case Element.Water: ApplyWaterEffect(myth, isInParty);  
                break;
            case Element.Metal: ApplyMetalEffect(myth, isInParty);
                break;
            case Element.Fire: ApplyFireEffect(myth, isInParty);
                break;
            case Element.Earth: ApplyEarthEffect(myth, isInParty);
                break;
            case Element.Ice: ApplyIceEffect(myth, isInParty);
                break;
            case Element.Wood: ApplyWoodEffect(myth, isInParty);
                break;
        }
    }

    #region Collision
    virtual public void Trigger(Myth myth)
    {

    }

    virtual public void Collision()
    {
        Destroy(gameObject);
    }

    #endregion

    #region Effects
    virtual public void ApplyWoodEffect(Myth myth, bool isInParty)
    {
        float value = ability.damage / 2;//Life Steal
        if (isInParty)
            myth.effectController.Heal(value);

    }

    virtual public void ApplyElectricEffect(Myth myth, bool isInParty)
    {
        float value = ability.stamina; //If Myth is in the same party add half the stamina cost.
        if (!isInParty)
            value = (value / 2); //By Default Decrement the Enemy Myths Stamina

        myth.effectController.AdjustStamina(ability.stamina);

    }

    virtual public void ApplyWaterEffect(Myth myth, bool isInParty)
    {

    }

    virtual public void ApplyIceEffect(Myth myth, bool isInParty)
    {

    }

    virtual public void ApplyWindEffect(Myth myth, bool isInParty)
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

    }

    virtual public void ApplyFireEffect(Myth myth, bool isInParty)
    {

    }
    #endregion

}

