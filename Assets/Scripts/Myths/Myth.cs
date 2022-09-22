using UnityEngine;
using UnityEngine.Events;

namespace Myths
{
    public class Myth : MonoBehaviour
    {
        //Events
        public UnityEvent<float> HealthChanged = new();
        public UnityEvent<float> StaminaChanged = new();
    
        //Properties
        public SO_Myth myth;
        // TODO: Make private
        public float stamina;
        public float walkSpeed;
        public float health;
        public GameObject targetEnemy;
        // TODO: Do we still need this? Some instances can be replaced by passing this in the command
        public Vector2 lastInputDirection;
        public float Health
        {
            get => health;
            set
            {
                health = value;
                HealthChanged.Invoke(health / 100.0f); // We're assuming 100.0f is the maximum health a myth can have
            }
        }
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
        public float Stamina
        {
            get => stamina;
            set
            {
                if (value < stamina)
                {
                    Invoke("startRegen", 0.75f);
                    StopRegen();
                }
                    stamina = value;
                    StaminaChanged.Invoke(stamina / 100.0f); // We're assuming 100.0f is the maximum stamina a myth can have
            }
        }

        public bool isInvulnerable = false;
        public bool staminaRegen = false;
        [HideInInspector]
        public float healthRegenTick = 0f;
        [HideInInspector]
        public bool healthRegen = false;

        //References
        public SO_Element element;
        public SO_Ability northAbility;
        public SO_Ability westAbility;
        public SO_Ability southAbility;
        public SO_Ability eastAbility;

        public SpriteRenderer ring;
        
        //#if UNITY_EDITOR
        //private void OnValidate()
        //{
        //    Health = health;
        //}
        //#endif

        private void Update()
        {
            if (staminaRegen)
            {
                if (Stamina < 100f && Stamina > 0.3f)
                {
                    Stamina += 5f * Time.deltaTime;
                } else if (Stamina < 0.6f)
                {
                    Stamina = 1;
                }
            }

            if (healthRegen)
            {
                if(Health <= 100)
                {
                    Health += healthRegenTick * Time.deltaTime;
                }
            }
        }

        private void startRegen()
        {
            staminaRegen = true;
        }

        private void StopRegen()
        {
            staminaRegen = false;
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                TemporaryUpdateTeam(); //Remove this after 5/09/22
                this.gameObject.SetActive(false);
                
            }
        }

        

        /*Remove everything after this after 5/09/22*/
        public WinState ws;
        public void TemporaryUpdateTeam()
        {
            ws.DecreaseScore(partyIndex);
        }
    }
}