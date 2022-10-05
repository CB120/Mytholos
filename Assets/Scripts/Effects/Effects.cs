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
    HashSet<Element> appliedBuffs = new();
    HashSet<Element> appliedDebuffs = new();


    private void Awake()
    {
        defaultWalkSpeed = myth.walkSpeed;
        defaultAttackStat = myth.AttackStat;
        defaultDefenceStat = myth.DefenceStat;
    }

    #region Effect Application

    #region Wood - Baxter
    public void Heal(float value)//Wood
    {
        myth.Health.Value += value;
    }
    #endregion

    #region Fire - Will
    private bool burning;
    private float burnDamage;
    private float burnTimer;
    private float burnDuration = 5;

    private bool AttackBuffActive;

    public void Burn(float damagevalue, float durationvalue)//Fire
    {
        burnDamage = damagevalue;
        burnDuration = durationvalue;
        burning = true;
    }

    public void AttackBuff(float value)
    {
        if(!AttackBuffActive)
        {
            AttackBuffActive = true;
            myth.AttackStat *= 2;
            Invoke("RemoveAttackBuff", value);
        }
    }

    private void RemoveAttackBuff()
    {
        if (AttackBuffActive)
        {
            AttackBuffActive = false;
            myth.AttackStat /= 2;
        }
    }
    #endregion

    #region Ice - Baxter
    public void Freeze(float value)//Ice
    {

    }
    #endregion

    #region Wind - Baxter 
    public void Displace(float value) //Wind
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float force = Random.Range(-value, value);
        rb.AddForce(new Vector3(Random.Range(-10, 10), Random.Range(0, 10), Random.Range(-10, 10)) * force);
    }

    public void AdjustAgility(float time, float value)//Wind
    {
        myth.walkSpeed += value; //Currently Not Stackable
        Invoke("SetDefaultAgility", time);
    }

    private void SetDefaultAgility()
    {
        myth.walkSpeed = defaultWalkSpeed;
    }
    #endregion 

    #region Electric - Baxter
    public void AdjustStamina(float value)//Electric
    {
        myth.Stamina.Value += value;
    }

    public void ApplyStaminaBuff(float time, float regenSpeed)
    {
        Invoke("DeactivateStaminaBuff", time);
        myth.Stamina.regenSpeed = regenSpeed;
    }

    public void DeactivateStaminaBuff()
    {
        myth.Stamina.regenSpeed = 5;
    }
    #endregion

    #region Earth - Will
    private bool DefenceBuffActive;
    private bool AgilityDebuffActive;
    public void DefenceBuff(float value)
    {
        if (!DefenceBuffActive)
        {
            DefenceBuffActive = true;
            myth.DefenceStat *= 2;
            Invoke("RemoveDefenceBuff", value);
        }
    }

    private void RemoveDefenceBuff()
    {
        if (DefenceBuffActive)
        {
            DefenceBuffActive = false;
            myth.AttackStat /= 2;
        }
    }

    public void AgilityDebuff(float value)
    {
        if (!AgilityDebuffActive)
        {
            AgilityDebuffActive = true;
            myth.walkSpeed /= 2;
            Invoke("RemoveAgilityDebuff", value);
        }
    }

    public void RemoveAgilityDebuff()
    {
        if (AgilityDebuffActive)
        {
            AgilityDebuffActive = false;
            myth.walkSpeed *= 2;
        }
    }

    #endregion

    #region Water - Will

    public void BuffCleanse()
    {
        RemoveAttackBuff();
        RemoveDefenceBuff();
        RemoveMetalDefence();
    }

    public void DebuffCleanse()
    {
        RemoveAgilityDebuff();
        RemoveDefenceDebuff();
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
    public void ActivateBuff(Element element, bool isDebuff, bool isInstantAttack) //Baxter
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

        if (isInstantAttack)//For Swipe Lob Etc
        {
            DeactivateBuff(element, isDebuff);
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
    private void Update()
    {
        if (burning)
        {
            Debug.Log("burning");
            burnTimer += Time.deltaTime;
            myth.Health.Value -= burnDamage;
            if (burnTimer > burnDuration)
                burning = false;
        }

    }

    public void CleanseAllDebuffs()//@Will, any buff/debuff implementation that you write, ensure that you are able to disable it in here
    {

    }
}
