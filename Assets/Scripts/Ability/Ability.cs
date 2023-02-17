using UnityEngine;
using Myths;
using Elements;

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

    protected SO_Element SO_Element => ability.element;

    [Header("All-Ability SFX")] //SFX stuff, added by Ethan
    public float timeToDestroyElementSFX = 0.5f;

    public GameObject takingDamageSFXPrefab;
    public float timeToDestroyTakingDamageSFX = 0.4f;


    virtual public void Start()
    {
        SOstunTime = ability.baseStun;
        SOknockbackStrength = ability.baseKnockback;
        owningMyth.Stamina.Value -= ability.staminaCost;

        PlayElementalSFX();
        AdjustMusicLayers();
    }

    virtual public void Update()
    {

    }

    virtual public void Attack(Myth myth, float damage)
    {
        if (!myth.gameObject.activeInHierarchy) return;
        bool isInParty = myth.PartyIndex == this.owningMyth.PartyIndex;
        if (isInParty && !ability.applyBuffToParty) return; //Guard if we don't want ability to give allies buffs

        ParticleSystem particle = ability.element.debuffParticle;
        ApplyEffect(myth);

        if (!isInParty)
        {
            if (ability.element.strongAgainst.Contains(myth.element))
            {
                //Debug.Log("Attack is Strong!!");
                elementModifier = 2;
            }

            if (ability.element.weakAgainst.Contains(myth.element))
            {
                //Debug.Log("Attack is Weak!!");
                elementModifier = 0.5f;
            }

            var finalDamage = (damage * elementModifier * DamageMultiplier * owningMyth.AttackStat) / myth.DefenceStat;

            myth.Health.Value -= finalDamage;

            PlayDamageSFX();
        }
        else
        {
            if (ability.element.buffParticle != null) //Temporary to show which debuff is being used
                particle = ability.element.buffParticle; 
        }

        // TODO: Why is there an exception for ice?
        if (ability.element.element != Element.Ice)
        {
            ParticleSystem ps = Instantiate(particle, myth.transform.position, particle.transform.rotation, myth.transform);
            if (ability.element.setParticleColor)
            {
                //Debug.LogWarning("Color is Set");
                ParticleSystem.MainModule main = ps.main;
                main.startColor = ability.element.color;
            }
        }

        if (SOknockbackStrength > 0 && ability.hasKnockback)
        {
            //Debug.Log(SOstunTime);
            myth.Knockback(SOknockbackStrength, owningMyth.gameObject, SOstunTime);
        }

        //if (SOstunTime > 0 && SOknockbackStrength == 0)
        //{
        //    myth.Stun(SOstunTime);
        //}

        // Animate the myth that was hit
        Animator anim = myth.gameObject.GetComponentInChildren<Animator>();
        if (anim && myth.Health.Value > 0) // Only set trigger if still alive, otherwise will interupt death animation
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

    virtual public void ApplyEffect(Myth myth)
    {
        bool isInParty = myth.PartyIndex == owningMyth.PartyIndex;
        
        myth.effectController.ApplyEffectForDefaultDuration(SO_Element, !isInParty);
    }

    #region Music & SFX
    public void PlayElementalSFX()
    {
        GameObject sfx = Instantiate(SO_Element.sfxPrefab, transform);
        Destroy(sfx, timeToDestroyElementSFX);
    }

    protected void PlayDamageSFX()
    {
        GameObject sfxGameObject = Instantiate(takingDamageSFXPrefab, transform.position, Quaternion.identity);
        Destroy(sfxGameObject, timeToDestroyTakingDamageSFX);
    }

    void AdjustMusicLayers()
    {
        switch (SO_Element.element) 
        {
            case Element.Electric:
                BattleMusicController.OnElectricAbility();
                return;
            case Element.Wind:
                BattleMusicController.OnWindAbility();
                return;
            default:
                return;
        }
    }
    #endregion
}

