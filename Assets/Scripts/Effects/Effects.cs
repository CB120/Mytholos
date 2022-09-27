using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;
using Elements;
public class Effects : MonoBehaviour
{
    public Myth myth;
    private float defaultWalkSpeed = 0;
    private void Awake()
    {
        defaultWalkSpeed = myth.walkSpeed;
    }

    #region Wood
    public void Heal(float value)//Wood
    {
        myth.Health.Value += value;
    }
    #endregion

    #region Fire
    public void Burn(float value)//Fire
    {

    }
    #endregion

    #region Ice - Baxter
    public void Freeze(float value)//Ice
    {
        
    }
    #endregion

    #region Wind
    public void Displace(float value) //Wind
    {
        //Rigidbody rb = GetComponent<Rigidbody>();
        //rb.AddForce(new Vector3(Random.Range(-1, 1), Random.Range(0, 1), Random.Range(-1, 1)) * value, ForceMode.Impulse);
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
    #endregion - Baxter

    #region Electric
    public void AdjustStamina(float value)//Electric
    {
        myth.Stamina.Value += value;
    }
    #endregion

    public void CleanseAllDebuffs()//@Will, any buff/debuff implementation that you write, ensure that you are able to disable it in here
    {

    }
}
