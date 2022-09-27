using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myths;

public class HealingAbility : Ability
{
    public float areaOfEffect = 2;
    public float expandSpeed = 2f;
    public float timeToDestroy = 5f;
    [SerializeField] private GameObject[] children;
    [SerializeField] private TrailRenderer[] trails;

    public override void Start()
    {
        Invoke("ResetScale", (timeToDestroy * 0.75f));
        Destroy(gameObject, timeToDestroy);

        foreach(TrailRenderer trail in trails)
        {
            trail.startColor = ability.element.color;
            trail.endColor = ability.element.color * new Color(1, 1, 1, 0.25f);
        }

        base.Start();
    }

    
    public override void Update()
    {
        transform.localScale = Vector3.Lerp(
                transform.localScale,
                new Vector3(areaOfEffect, transform.localScale.y, areaOfEffect),
                expandSpeed * Time.deltaTime
        );
        base.Update();
    }

    private void ResetScale()
    {
        foreach (GameObject obj in children) obj.SetActive(false);
        areaOfEffect = 0;
    }


    //Effect Application
    private void OnTriggerStay(Collider other)//Would have preferred this to be onTrigger Enter, however if there are overlapping pools, an effect may be removed
    {
        Myth myth = other.gameObject.GetComponent<Myth>();
        if (myth)
        {
            ApplyEffect(myth);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Myth myth = other.gameObject.GetComponent<Myth>();
        if (myth)
        {
            myth.Health.regenSpeed = 0;
            myth.Stamina.regenSpeed = 5;
        }
    }

    public override void ApplyEarthEffect(Myth myth, bool isInParty)
    {
        base.ApplyEarthEffect(myth, isInParty);
    }

    public override void ApplyElectricEffect(Myth myth, bool isInParty)
    {
        if (isInParty) myth.Stamina.regenSpeed = ability.regenSpeed;
    }

    public override void ApplyFireEffect(Myth myth, bool isInParty)
    {
    }

    public override void ApplyIceEffect(Myth myth, bool isInParty)
    {
    }

    public override void ApplyMetalEffect(Myth myth, bool isInParty)
    {
    }

    public override void ApplyWaterEffect(Myth myth, bool isInParty)
    {
    }

    public override void ApplyWindEffect(Myth myth, bool isInParty)
    {
        myth.effectController.AdjustAgility(ability.statIncrease);;
    }

    public override void ApplyWoodEffect(Myth myth, bool isInParty)
    {
        myth.Health.regenSpeed = ability.regenSpeed;
    }
}
