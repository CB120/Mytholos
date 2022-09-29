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
    private void Awake()
    {
        defaultWalkSpeed = myth.walkSpeed;
    }

    #region Wood - Baxter
    public void Heal(float value)//Wood
    {
        myth.Health.Value += value;
    }
    #endregion

    #region Fire
    private bool burning;
    private float burnDamage;
    private float burnTimer;
    [SerializeField] private float burnDuration;
    public void Burn(float damagevalue, float durationvalue)//Fire
    {
        burnDamage = damagevalue;
        burnDuration = durationvalue;
        burning = true;
    }

    public void attackBuff(float value)
    {

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

    public void AdjustAgility(float value)//Wind
    {
        myth.walkSpeed = defaultWalkSpeed * 2; //Currently Not Stackable
        Invoke("SetDefaultAgility", value);
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

    #region Earth
    public void DefenceBuff()
    {

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
