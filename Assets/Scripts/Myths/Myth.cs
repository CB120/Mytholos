using System;
using StateMachines;
using StateMachines.Commands;
using UnityEngine;
using UnityEngine.Events;

namespace Myths
{
    public class Myth : MonoBehaviour
    {
        [SerializeField] private MythStat health;
        [SerializeField] private MythStat stamina;
        [SerializeField] private MythCommandHandler mythCommandHandler;
        [SerializeField] private MythRuntimeSet mythRuntimeSet;
        Animator anim;
        GameObject[] visuals;

        public MythStat Health => health;
        public MythStat Stamina => stamina;

        public SO_Myth myth;

        public float walkSpeed;
        public float AttackStat;
        public float DefenceStat;
        public float SizeStat;
        
        public GameObject targetEnemy;
        // TODO: Make serialised, fix naming mismatch
        public Effects effectController;

        public int partyIndex;
        public int PartyIndex
        {
            get => partyIndex;
            set
            {
                partyIndex = value;
                ring.color = value == 0 ? new Color(1.0f, 0.3f, 0.0f, 1.0f)
                    : new Color(0.0f, 0.5f, 1.0f, 1.0f);
            }
        }
        public bool isInvulnerable = false;

        //References
        public SO_Element element;
        public SO_Ability northAbility;
        public SO_Ability westAbility;
        public SO_Ability southAbility;
        public SO_Ability eastAbility;

        public SpriteRenderer ring;

        [NonSerialized] public UnityEvent<Myth> died = new();

        private void Awake()
        {
            walkSpeed = myth.agility;
            AttackStat = myth.attack;
            SizeStat = myth.size;
            anim = GetComponentInChildren<Animator>();
            if (anim)
            {
                visuals = new GameObject[anim.transform.childCount];
                for (int i = 0; i < anim.transform.childCount; i++)
                     visuals[i] = anim.transform.GetChild(i).gameObject;
            }
            
            mythRuntimeSet.Add(this);
        }

        public void Die()
        {
            died.Invoke(this);
        }

        public void Knockback(float abilityKnockback, GameObject sendingMyth, float abilityStunTime)
        {
            mythCommandHandler.PushCommand(new KnockbackCommand(sendingMyth, SizeStat, abilityKnockback, abilityStunTime));
        }

        public void Stun(float abilityStunTime)
        {
            mythCommandHandler.PushCommand(new StunCommand(abilityStunTime));
        }


        // For 'Enough Stamina' SFX
        public int NumberOfAvailableAbilities()
        {
            int output = 0;
            if (northAbility.staminaCost <= Stamina.Value) output++;
            if (westAbility.staminaCost <= Stamina.Value) output++;
            if (southAbility.staminaCost <= Stamina.Value) output++;
            return output;
        }

        public void SetAnimatorTrigger(string trigger)
        {
            if (anim == null)
            {
                anim = GetComponentInChildren<Animator>();
                if (anim == null) return;
            }

            bool enable = trigger != "Reset";

            foreach (GameObject child in visuals)
                child.SetActive(enable);

            anim.SetTrigger(trigger);
            anim.speed = 1.0f;
        }

        private void OnDestroy()
        {
            mythRuntimeSet.Remove(this);
        }
    }
}