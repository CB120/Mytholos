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

    public override void Start()
    {
        Invoke("ResetScale", (timeToDestroy * 0.75f));
        Destroy(gameObject, timeToDestroy);
        base.Start();
    }

    
    public override void Update()
    {
        transform.localScale = Vector3.Lerp(
                transform.localScale,
                new Vector3(areaOfEffect, transform.localScale.y, areaOfEffect),
                expandSpeed * Time.deltaTime
        );
    }

    private void ResetScale()
    {
        foreach (GameObject obj in children) obj.SetActive(false);
        areaOfEffect = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        Myth myth = other.gameObject.GetComponent<Myth>();
        if (myth)
        {
            
            myth.healthRegenTick = ability.healing;
            myth.healthRegen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Myth myth = other.gameObject.GetComponent<Myth>();
        if (myth)
        {
            myth.healthRegenTick = 0;
            myth.healthRegen = false;
        }
    }

}
