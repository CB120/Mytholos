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

        [SerializeField] private float regenPauseTime;

        public bool ExternallyModifiable { get; set; } = true;

        private const float MaxValue = 100;
        private const float MinValue = 0;
        
        // TODO: Rename to avoid confusion between value and this.value
        [SerializeField] private float value;
        private bool isRegenerating;
        
        public UnityEvent<float> valueChanged = new();
        public UnityEvent valueZeroed = new();
        [SerializeField] private float regenSpeed;

        public float ValuePercent => Value / MaxValue;
        
        public float Value
        {
            get => InternalValue;
            set
            {
                if (!ExternallyModifiable) return;

                InternalValue = value;
            }
        }

        private float InternalValue
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
                
                if (this.value == 0)
                    valueZeroed.Invoke();
            }
        }

        private void Awake()
        {
            defaultRegenSpeed = RegenSpeed;
            value = MaxValue;
        }

        public void Update()
        {
            if (regenSpeed == 0) return;
            
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