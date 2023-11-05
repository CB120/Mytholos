using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Myths;
using Elements;
public class Effects : MonoBehaviour
{
    public Myth myth;
    private float defaultWalkSpeed = 0;
    private float defaultAttackStat = 0;
    private float defaultDefenceStat = 0;
    private float defaultSizeStat = 0;
    //[SerializeField] private MythUI mythUI;
    public HashSet<Element> appliedBuffs = new();
    public HashSet<Element> appliedDebuffs = new();
    [SerializeField] private AlternateEffects alternateIce;

    [Header("Effect Manipulation")]
    [SerializeField] private float rotateSpeed = 500;
    private bool isDisoriented;

    public UnityEvent<Element, bool> ActivateBuffEvent = new();
    public UnityEvent<Element, bool, bool, bool> DeactivateBuffEvent = new();

    private void Awake()
    {
        defaultWalkSpeed = myth.walkSpeed;
        defaultAttackStat = myth.AttackStat;
        defaultDefenceStat = myth.DefenceStat;
        defaultSizeStat = myth.SizeStat;
    }

    private void OnEnable()
    {
        CleanseAllBuffs();
        // TODO: Race condition with MythStat.Awake. Might permanently set stamina regen to zero.
        // CleanseAllDebuffs();
    }

    #region Effect Application

    #region Wood - Baxter
    public void ApplyLifeStealDebuff(bool isDebuff, float duration)//Wood
    {
        CancelInvoke("RemoveLifeStealDebuff");
        Invoke("RemoveLifeStealDebuff", duration);
    }

    private void RemoveLifeStealDebuff()
    {
        DeactivateBuff(Element.Wood, true);
    }

    public void CleanseHealthBuff()
    {
        myth.Health.RegenSpeed = 0;
    }
    #endregion

    #region Electric - Baxter
    public void ApplyStaminaEffect(bool isDebuff, float duration)//Electric
    {
        CancelInvoke("RemoveStaminaDebuff");
        float value = this.myth.Stamina.RegenSpeed;
        if (isDebuff) value /= 2;
        else value *= 1.25f;
        this.myth.Stamina.RegenSpeed = Mathf.Clamp(value, 0.25f, 15);
        Invoke("RemoveStaminaDebuff", duration);
    }

    public void IncreaseStamina(float increase)
    {
        this.myth.Stamina.Value += increase;
        Invoke("RemoveStaminaBuff", .5f);
    }

    private void RemoveStaminaBuff()
    {
        DeactivateBuff(Element.Electric, false);
    }

    private void RemoveStaminaDebuff()
    {
        this.myth.Stamina.RegenSpeed = this.myth.Stamina.defaultRegenSpeed;
        DeactivateBuff(Element.Electric, true);
    }
    #endregion

    #region Ice - Baxter
    public void FreezeDebuff(float duration, float effectChance)//Ice Debuff 
    {
        CancelInvoke("RemoveFreezeDebuff");

        float rollChance = Random.Range(0f, 1f);
        Debug.LogError(rollChance + " " + effectChance);
        if (rollChance <= effectChance)
        {
            ActivateBuff(Element.Ice, true);
            Debug.Log($"Freeze Time: {duration}");
            myth.Stun(duration);
            alternateIce.effectObject.SetActive(true);
            Invoke("RemoveFreezeDebuff", duration);
        }
    }

    private void RemoveFreezeDebuff() //Ice Debuff
    {
        //Unfreeze
        Debug.Log("Removed Debuff");

        if (appliedDebuffs.Contains(Element.Ice))
        {
            alternateIce.effectObject.SetActive(false);
            //Instantiate(particle, myth.transform.position + ability.element.buffOffsetPos, Quaternion.identity, myth.transform);
            ParticleSystem ps = Instantiate(alternateIce.element.debuffParticle, myth.transform);
            DeactivateBuff(Element.Ice, true);
        }

    }

    public void IceOvershield() //Ice Buff
    {
        ActivateBuff(Element.Ice, false);
    }

    public void RemoveIceOvershield()//Ice Buff
    {
        DeactivateBuff(Element.Ice, false);
    }
    #endregion

    #region Wind - Baxter 
    public void AgilityBuff(float duration)//Ice Debuff 
    {
        CancelInvoke("RemoveAgilityBuff");
        myth.walkSpeed = Mathf.Clamp(myth.walkSpeed * 2, 0.5f, defaultWalkSpeed * 2);
        Invoke("RemoveAgilityBuff", duration);
    }

    private void RemoveAgilityBuff() //Ice Debuff
    {
        myth.walkSpeed = defaultWalkSpeed;
        DeactivateBuff(Element.Wind, false);
    }

    public void Disorient(float knockback, Myth sendingMyth, float duration, float effectChance)
    {
        float rollChance = Random.Range(0f, 1f);
        Debug.LogError(rollChance + " " + effectChance);
        if (rollChance <= effectChance)
        {
            CancelInvoke("EndDisorient");
            isDisoriented = true;
            myth.Knockback(knockback, sendingMyth.gameObject, duration * 0.75f);
            Invoke("EndDisorient", duration);

        }
    }

    private void EndDisorient()
    {
        isDisoriented = false;
        DeactivateBuff(Element.Wind, true);
    }
    #endregion 

    #region Fire - Will
    private float burnDamage;
    private bool burning;
    public void Burn(float damagevalue, float duration = 0)//Fire
    {
        CancelInvoke("EndBurn");
        burnDamage = damagevalue;
        burning = true;
        if (duration > 0)
            Invoke("EndBurn", duration);
    }

    public void EndBurn()
    {
        burning = false;
        DeactivateBuff(Element.Fire, true);
    }

    public void AttackBuff(float duration)
    {
        CancelInvoke("RemoveAttackBuff");
        myth.AttackStat = Mathf.Clamp(myth.AttackStat * 2, defaultAttackStat / 2, defaultAttackStat * 2);
        ActivateBuff(Element.Fire, false);
        Invoke("RemoveAttackBuff", duration);
    }

    private void RemoveAttackBuff()
    {
        myth.AttackStat = defaultAttackStat;
        DeactivateBuff(Element.Fire, false);
    }
    #endregion

    #region Earth - Will
    public void DefenceBuff(float duration)
    {
        CancelInvoke("RemoveDefenceBuff");
        myth.DefenceStat = Mathf.Clamp(myth.DefenceStat * 2, defaultDefenceStat / 2, defaultDefenceStat * 2);
        ActivateBuff(Element.Earth, false);
        Invoke("RemoveDefenceBuff", duration);
    }

    private void RemoveDefenceBuff()
    {
        myth.DefenceStat = defaultDefenceStat;
        DeactivateBuff(Element.Earth, false);
    }

    public void AgilityDebuff(float duration = 0)
    {
        CancelInvoke("RemoveAgilityDebuff");
        myth.walkSpeed = Mathf.Clamp(myth.walkSpeed / 2, defaultWalkSpeed / 2, defaultWalkSpeed * 2);
        if (duration > 0)
            Invoke("RemoveAgilityDebuff", duration);
    }

    public void RemoveAgilityDebuff()
    {
        myth.walkSpeed = defaultWalkSpeed;
        DeactivateBuff(Element.Earth, true);
    }

    #endregion

    #region Water - Will

    public void BuffCleanse()
    {
        CleanseAllBuffs();
        ActivateBuff(Element.Water, true);
        Invoke("RemoveWaterIcon", 1);
    }

    public void RemoveWaterIcon()
    {
        DeactivateBuff(Element.Water, false);
        DeactivateBuff(Element.Water, true);
    }


    public void DebuffCleanse()
    {
        CleanseAllDebuffs();
        ActivateBuff(Element.Water, false);
        Invoke("RemoveWaterIcon", 1);
    }

    #endregion

    #region Metal - Will
    public void AttackDebuff(float duration)
    {
        CancelInvoke("RemoveAttackDebuff");
        myth.AttackStat = Mathf.Clamp(myth.AttackStat / 2, defaultAttackStat / 2, defaultAttackStat * 2);
        ActivateBuff(Element.Metal, true);
        Invoke("RemoveAttackDebuff", duration);
    }

    private void RemoveAttackDebuff()
    {
        myth.AttackStat = defaultAttackStat;
        DeactivateBuff(Element.Metal, true);
    }

    public void SizeBuff(float duration)
    {
        CancelInvoke("RemoveSizeBuff");
        myth.SizeStat = Mathf.Clamp(myth.SizeStat * 2, defaultSizeStat / 2, defaultSizeStat * 2);
        ActivateBuff(Element.Metal, false);
        Invoke("RemoveSizeBuff", duration);
    }

    private void RemoveSizeBuff()
    {
        myth.SizeStat = defaultSizeStat;
        DeactivateBuff(Element.Metal, false);
    }

    #endregion
    #endregion

    #region Effect Interface
    public void ActivateBuff(Element element, bool isDebuff) //Baxter
    {
        //mythUI.RefreshLayout();
        ActivateBuffEvent.Invoke(element, isDebuff);

        if (isDebuff)
        {
            appliedDebuffs.Add(element);
            //mythUI.effectUIData[element].negativeBuff.gameObject.SetActive(true);
            //mythUI.effectUIData[element].negativeBuff.isEnabled = true;
        }
        else
        {
            appliedBuffs.Add(element);
            //mythUI.effectUIData[element].positiveBuff.gameObject.SetActive(true);
            //mythUI.effectUIData[element].positiveBuff.isEnabled = true;
        }
    }

    public void DeactivateBuff(Element element, bool isDebuff)
    {
        //mythUI.RefreshLayout();
        DeactivateBuffEvent.Invoke(element, isDebuff, appliedDebuffs.Contains(element), appliedBuffs.Contains(element));

        if (isDebuff && appliedDebuffs.Contains(element))
        {
            appliedDebuffs.Remove(element);
            //mythUI.effectUIData[element].negativeBuff.isEnabled = false;
        }
        else if (!isDebuff && appliedBuffs.Contains(element))
        {
            appliedBuffs.Remove(element);
            //mythUI.effectUIData[element].positiveBuff.isEnabled = false;
        }
    }
    #endregion


    private void Update()
    {
        if (burning)
        {
            //Debug.Log("burning");
            myth.Health.Value -= burnDamage * Time.deltaTime;
        }

        if (isDisoriented)
        {
            myth.gameObject.transform.Rotate(0, Time.deltaTime * rotateSpeed, 0);
        }
    }

    public void CleanseAllDebuffs()//@Will, any buff/debuff implementation that you write, ensure that you are able to disable it in here
    {
        RemoveLifeStealDebuff();
        RemoveStaminaDebuff();
        RemoveAgilityDebuff();
        RemoveAttackDebuff();
        RemoveFreezeDebuff();
        EndDisorient();
        EndBurn();
    }

    public void CleanseAllBuffs()
    {
        RemoveAttackBuff();
        RemoveDefenceBuff();
        RemoveIceOvershield();
        RemoveAgilityBuff();
        RemoveSizeBuff();
        CleanseHealthBuff();
    }
}

namespace Elements
{
    [System.Serializable]
    struct AlternateEffects
    {
        public SO_Element element;
        public GameObject effectObject;
    }
}