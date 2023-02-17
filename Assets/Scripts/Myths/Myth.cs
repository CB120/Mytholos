using System;
using System.Collections;
using EffectSystem;
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
        private Animator anim;
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

        private int partyIndex;
        public int PartyIndex
        {
            get => partyIndex;
            set
            {
                // TODO: Remove hardcoding
                partyIndex = value;
                ring.color = value == 0 ? new Color(1.0f, 0.3f, 0.0f, 1.0f)
                    : new Color(0.0f, 0.5f, 1.0f, 1.0f);
            }
        }

        public bool IsInvulnerable
        {
            get => isInvulnerable;
            set
            {
                isInvulnerable = value;

                Health.ExternallyModifiable = !isInvulnerable;
            }
        }

        public MythCommandHandler MythCommandHandler => mythCommandHandler;

        private MythData mythData;

        //References
        public SO_Element element;
        public SO_Ability NorthAbility => mythData.northAbility;
        public SO_Ability WestAbility => mythData.westAbility;
        public SO_Ability SouthAbility => mythData.southAbility;
        public SO_Ability EastAbility => mythData.eastAbility;

        public SpriteRenderer ring;

        // TODO: Implement
        [NonSerialized] public UnityEvent toBeDamaged = new();
        [NonSerialized] public UnityEvent<Myth> died = new();
        private bool isInvulnerable = false;

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

        public void Invulnerability(float duration)
        {
            // TODO: Should store the routine to prevent duplicates
            StartCoroutine(InvulnerabilityRoutine(duration));
        }

        private IEnumerator InvulnerabilityRoutine(float duration)
        {
            IsInvulnerable = true;

            yield return new WaitForSeconds(duration);

            IsInvulnerable = false;
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
            if (NorthAbility.staminaCost <= Stamina.Value) output++;
            if (WestAbility.staminaCost <= Stamina.Value) output++;
            if (SouthAbility.staminaCost <= Stamina.Value) output++;
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
            //print("SetAnimatorTrigger() enable = " + enable);

            foreach (GameObject child in visuals)
                child.SetActive(enable);

            anim.SetTrigger(trigger);
            anim.speed = 1.0f;
        }

        private void OnDestroy()
        {
            mythRuntimeSet.Remove(this);
        }

        public void Initialise(MythData mythData)
        {
            this.mythData = mythData;
        }

        public void ResetAndDisable()
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine(ResetAndDisableRoutine());
        }

        private IEnumerator ResetAndDisableRoutine()
        {
            SetAnimatorTrigger("Reset");

            // Need to wait a frame for the animator to return to a neutral pose, else when reenabled, will be incorrect
            yield return null;

            gameObject.SetActive(false);
        }
    }
}