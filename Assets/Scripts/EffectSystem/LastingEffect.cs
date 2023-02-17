using System.Collections;
using UnityEngine;

namespace EffectSystem
{
    public class LastingEffect : Effect
    {
        [SerializeField] protected float duration;
        
        private Coroutine applyEffectCoroutine;

        public override void ApplyEffect()
        {
            base.ApplyEffect();
            
            if (applyEffectCoroutine != null)
                StopCoroutine(applyEffectCoroutine);
            
            effects.ActivateBuff(element, isDebuff);
        }

        public void ApplyEffectForDefaultDuration()
        {
            ApplyEffect();

            if (duration > 0)
                applyEffectCoroutine = StartCoroutine(ApplyEffectRoutine());
        }

        private IEnumerator ApplyEffectRoutine()
        {
            yield return new WaitForSeconds(duration);
            
            RemoveEffect();
        }

        // TODO: We should keep track here if the effect is applied, so that we don't call RemoveEffect twice
        public virtual void RemoveEffect()
        {
            effects.DeactivateBuff(element, isDebuff);
        }
    }
}