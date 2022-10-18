using Commands;
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
        //placeholder stat of 1
        public float AttackStat = 1;
        public float DefenceStat = 1;

        public GameObject targetEnemy;
        public Effects effectController;

        // TODO: Do we still need this? Some instances can be replaced by passing this in the command // 
        [HideInInspector]public Vector2 lastInputDirection;
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
        }

        /*Remove everything after this after 5/09/22*/
        public WinState ws;
        public void TemporaryUpdateTeam()
        {
            ws.DecreaseScore(partyIndex);
        }

        public void Knockback(float abilityKnockback, GameObject sendingMyth, float abilityStunTime)
        {
            // BUG: This is the wrong mythHandler
            mythCommandHandler.PushCommand(new KnockbackCommand());
            if (mythCommandHandler.LastCommand is KnockbackCommand knockbackService)
            {
                Debug.Log(abilityStunTime);
                knockbackService.abilitySender = sendingMyth;
                knockbackService.senderStrength = myth.size;
                knockbackService.knockbackStrength = abilityKnockback;
                knockbackService.stunTime = abilityStunTime;
            }
            
        }

        public void Stun(float abilityStunTime)
        {
            // BUG: This is the wrong mythHandler
            mythCommandHandler.PushCommand(new StunCommand(abilityStunTime));
            if (mythCommandHandler.LastCommand is StunCommand stunService)
            {
                stunService.stunTime = abilityStunTime;
            }
        }
    }
}