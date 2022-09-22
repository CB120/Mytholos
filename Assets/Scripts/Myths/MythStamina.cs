using UnityEngine;
using UnityEngine.Events;

namespace Myths
{
    public class MythStamina : MonoBehaviour
    {
        // TODO: Depends on MaxStamina
        [SerializeField] private float regenSpeed;
        [SerializeField] private float regenPauseTime;

        private const float MaxStamina = 100;
        private const float MinStamina = 0;
        
        private float stamina;
        private bool staminaRegen;
        
        public UnityEvent<float> staminaChanged = new();

        public float StaminaPercent => Stamina / MaxStamina;
        
        public float Stamina
        {
            get => stamina;
            set
            {
                if (value < stamina)
                {
                    StopRegen();
                    Invoke("StartRegen", regenPauseTime);
                }

                stamina = Mathf.Clamp(value, MinStamina, MaxStamina);
                
                staminaChanged.Invoke(stamina / MaxStamina);
            }
        }

        private void Awake()
        {
            stamina = MaxStamina;
        }

        private void Update()
        {
            if (!staminaRegen) return;
            
            if (!(Stamina < MaxStamina)) return;
            
            Stamina += regenSpeed * Time.deltaTime;
        }
        
        private void StartRegen()
        {
            staminaRegen = true;
        }

        private void StopRegen()
        {
            staminaRegen = false;
        }
    }
}