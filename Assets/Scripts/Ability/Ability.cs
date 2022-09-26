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
        public Element element { get => ability.element.element;}

        virtual public void Start()
        {
            owningMyth.Stamina.Value -= ability.stamina;
        }

        virtual public void Update()
        {

        }
        virtual public void Attack(Myth myth, float damage)
        {
            if (myth.partyIndex == this.owningMyth.partyIndex) return; //Ensures Myths cannot harm others in their party 

            var finalDamage = damage * DamageMultiplier;
            Debug.LogWarning($"{myth.gameObject.name} was Attacked by {owningMyth.gameObject.name}");
            Debug.LogWarning($"Dealt {finalDamage} damage ({DamageMultiplier * 100}% of base {damage})");
            myth.Health.Value -= finalDamage;
        }

        virtual public void Trigger(Myth myth)
        {

        }

        virtual public void Collision()
        {
            Destroy(gameObject);
        }

    }

