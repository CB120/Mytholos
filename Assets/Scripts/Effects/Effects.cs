using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;
using Elements;
using Commands;
public class Effects : MonoBehaviour
{
    public Myth myth;
    private float defaultWalkSpeed = 0;
    private float defaultAttackStat = 0;
    private float defaultDefenceStat = 0;
    [SerializeField] private MythUI mythUI;
    public HashSet<Element> appliedBuffs = new();
    public HashSet<Element> appliedDebuffs = new();

    private void Start()
    {
        defaultWalkSpeed = myth.walkSpeed;
        defaultAttackStat = myth.AttackStat;
        defaultDefenceStat = myth.DefenceStat;
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
    #endregion

    #region Electric - Baxter
    public void ApplyStaminaEffect(bool isDebuff, float duration)//Electric
    {
        CancelInvoke("RemoveStaminaDebuff");
        float value = this.myth.Stamina.regenSpeed;
        if (isDebuff) value /= 2;
        else value *= 1.25f;
        this.myth.Stamina.regenSpeed = Mathf.Clamp(value, 0.25f, 15);
        Invoke("RemoveStaminaDebuff", duration);
    }

    private void RemoveStaminaDebuff()
    {
        this.myth.Stamina.regenSpeed = this.myth.Stamina.defaultRegenSpeed;
        DeactivateBuff(Element.Electric, true);
    }
    #endregion

    #region Ice - Baxter
    public void FreezeDebuff(float duration)//Ice Debuff 
    {
        CancelInvoke("RemoveFreezeDebuff");
        ActivateBuff(Element.Ice, true);
        Invoke("RemoveFreezeDebuff", duration);
    }

    private void RemoveFreezeDebuff() //Ice Debuff
    {
        //Unfreeze
        DeactivateBuff(Element.Ice, true);
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

    public void Disorient(float duration)
    {
        CancelInvoke("EndDisorient");
        isDisoriented = true;
        //Add Knockback here too
        Invoke("EndDisorient", duration);
    }

    private void EndDisorient()
    {
        isDisoriented = false;
    }
    #endregion 

    #region Fire - Will
    private float burnDamage;
    private bool burning;
    public void Burn(float damagevalue, float duration)//Fire
    {
        CancelInvoke("EndBurn");
        burnDamage = damagevalue;
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
        myth.AttackStat = Mathf.Clamp(myth.AttackStat *2, defaultAttackStat/2, defaultAttackStat * 2);
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
        myth.DefenceStat = Mathf.Clamp(myth.DefenceStat * 2, defaultDefenceStat/2, defaultDefenceStat*2);
        Invoke("RemoveDefenceBuff", duration);
    }

    private void RemoveDefenceBuff()
    {
        myth.AttackStat = defaultAttackStat;
        DeactivateBuff(Element.Earth, false);
    }

    public void AgilityDebuff(float duration)
    {
        CancelInvoke("RemoveAgilityDebuff");    
        myth.walkSpeed = Mathf.Clamp(myth.walkSpeed /2, defaultWalkSpeed/2, defaultWalkSpeed * 2);
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
    }

    public void DebuffCleanse()
    {
        CleanseAllDebuffs();
    }

    #endregion

    #region Metal - Will
    private bool DefenceDebuffActive;
    public bool MetalDefenceActive;
    public void DefenceDebuff(float value)
    {
        if (!DefenceDebuffActive)
        {
            DefenceDebuffActive = true;
            myth.DefenceStat /= 2;
            Invoke("RemoveDefenceDebuff", value);
        }
    }

    private void RemoveDefenceDebuff()
    {
        if (DefenceDebuffActive)
        {
            DefenceDebuffActive = false;
            myth.AttackStat *= 2;
        }
    }

    public void MetalDefence(float value)
    {
        if (!MetalDefenceActive)
        {
            MetalDefenceActive = true;
            myth.DefenceStat *= 3;
            Invoke("RemoveDefenceDebuff", value);
        }
    }

    private void RemoveMetalDefence()
    {
        if (MetalDefenceActive)
        {
            MetalDefenceActive = false;
            myth.AttackStat /= 3;
        }
    }

    #endregion
    #endregion

    #region Effect Interface
    public void ActivateBuff(Element element, bool isDebuff) //Baxter
    {
        if (isDebuff)
        {
            appliedDebuffs.Add(element);
            mythUI.effectUIData[element].negativeBuff.gameObject.SetActive(true);
            mythUI.effectUIData[element].negativeBuff.Play("EffectUIAnim", -1, 0f);
        }
        else { 
            appliedBuffs.Add(element);
            mythUI.effectUIData[element].positiveBuff.gameObject.SetActive(true);
            mythUI.effectUIData[element].positiveBuff.Play("EffectUIAnim", -1, 0f);
        }
    }

    public void DeactivateBuff(Element element, bool isDebuff)
    {
        if (isDebuff && appliedDebuffs.Contains(element)) {
            appliedDebuffs.Remove(element);
            mythUI.effectUIData[element].negativeBuff.SetTrigger("Close");
        }
        else if (!isDebuff && appliedBuffs.Contains(element)) { 
            appliedBuffs.Remove(element);
            mythUI.effectUIData[element].positiveBuff.SetTrigger("Close");
        }
    }
    #endregion

    private bool isDisoriented;
    private float rotateSpeed = 500;
    private void Update()
    {
        if (burning)
        {
            Debug.Log("burning");
            myth.Health.Value -= burnDamage;
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
        RemoveDefenceDebuff();
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
        //RemoveMetalDefence();
    }
}
