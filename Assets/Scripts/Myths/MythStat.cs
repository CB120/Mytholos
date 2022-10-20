using UnityEngine;
using UnityEngine.Events;

namespace Myths
{
    public class MythStat : MonoBehaviour
    {
        [Header("Myth Stat")]
        // TODO: Depends on MaxValue
        [HideInInspector] public float defaultRegenSpeed;

        public float RegenSpeed
        {
            get => regenSpeed;
            set => regenSpeed = value;
        }

        public float regenPauseTime;

        private const float MaxValue = 100;
        private const float MinValue = 0;
        
        private float value;
        private bool isRegenerating;
        
        public UnityEvent<float> valueChanged = new();
        public UnityEvent valueZeroed = new();
        [SerializeField] private float regenSpeed;

        public float ValuePercent => Value / MaxValue;
        
        public float Value
        {
            get => value;
            set
            {
                if (value < this.value)
                {
                    StopRegen();
                    Invoke("StartRegen", regenPauseTime);
                }

                this.value = Mathf.Clamp(value, MinValue, MaxValue);
                
                valueChanged.Invoke(this.value / MaxValue);
                
                if (value == 0)
                    valueZeroed.Invoke();
            }
        }

        private void Awake()
        {
            defaultRegenSpeed = RegenSpeed;
            value = MaxValue;
        }

        private void Update()
        {
            if (!isRegenerating) return;
            
            if (Value >= MaxValue) return;
            
            Value += RegenSpeed * Time.deltaTime;
        }
        
        private void StartRegen()
        {
            isRegenerating = true;
        }

        private void StopRegen()
        {
            isRegenerating = false;
        }
    }
}