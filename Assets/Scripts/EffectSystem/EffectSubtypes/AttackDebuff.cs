using Myths;
using UnityEngine;

namespace EffectSystem.EffectSubtypes
{
    public class AttackDebuff : LastingEffect
    {
        [SerializeField] private Myth myth;
        
        private float defaultAttackStat = 0;

        private void Awake()
        {
            defaultAttackStat = myth.AttackStat;
        }

        public override void ApplyEffect()
        {
            base.ApplyEffect();
            
            myth.AttackStat = Mathf.Clamp(myth.AttackStat / 2, defaultAttackStat / 2, defaultAttackStat * 2);
        }

        public override void RemoveEffect()
        {
            base.RemoveEffect();
            
            myth.AttackStat = defaultAttackStat;
        }
    }
}