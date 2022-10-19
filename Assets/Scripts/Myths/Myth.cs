using StateMachines;
using StateMachines.Commands;
using UnityEngine;

namespace Myths
{
    public class Myth : MonoBehaviour
    {
        [SerializeField] private MythStat health;
        [SerializeField] private MythStat stamina;
        [SerializeField] private MythCommandHandler mythCommandHandler;

        public MythStat Health => health;
        public MythStat Stamina => stamina;

        public SO_Myth myth;

        public float walkSpeed;
        public float AttackStat;
        public float DefenceStat;
        
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

        private void Awake()
        {
            walkSpeed = myth.agility;
            AttackStat = myth.attack;
        }

        /*Remove everything after this after 5/09/22*/
        public WinState ws;
        public void TemporaryUpdateTeam()
        {
            ws.DecreaseScore(partyIndex);
        }

        public void Knockback(float abilityKnockback, GameObject sendingMyth, float abilityStunTime)
        {
            mythCommandHandler.PushCommand(new KnockbackCommand(sendingMyth, myth.size, abilityKnockback, abilityStunTime));
        }

        public void Stun(float abilityStunTime)
        {
            mythCommandHandler.PushCommand(new StunCommand(abilityStunTime));
        }
    }
}