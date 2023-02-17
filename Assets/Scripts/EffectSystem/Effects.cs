using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EffectSystem
{
    public class Effects : MonoBehaviour
    {
        public HashSet<SO_Element> appliedBuffs = new();
        public HashSet<SO_Element> appliedDebuffs = new();

        [Header("Effect Manipulation")]

        [NonSerialized] public UnityEvent<SO_Element, bool> ActivateBuffEvent = new();
        [NonSerialized] public UnityEvent<SO_Element, bool, bool, bool> DeactivateBuffEvent = new();

        [SerializeField] private List<Effect> effects = new ();
    
        private void OnEnable()
        {
            // CleanseAllBuffs();
            // TODO: Race condition with MythStat.Awake. Might permanently set stamina regen to zero.
            // CleanseAllDebuffs();
        }

        #region Effect Interface
        public void ActivateBuff(SO_Element element, bool isDebuff) //Baxter
        {
            ActivateBuffEvent.Invoke(element, isDebuff);

            if (isDebuff)
            {
                appliedDebuffs.Add(element);
            }
            else { 
                appliedBuffs.Add(element);
            }
        }

        public void DeactivateBuff(SO_Element element, bool isDebuff)
        {
            DeactivateBuffEvent.Invoke(element, isDebuff, appliedDebuffs.Contains(element), appliedBuffs.Contains(element));

            if (isDebuff && appliedDebuffs.Contains(element)) {
                appliedDebuffs.Remove(element);
            }
            else if (!isDebuff && appliedBuffs.Contains(element)) { 
                appliedBuffs.Remove(element);
            }
        }
        #endregion

        public void RemoveEffects(Func<Effect, bool> filter)
        {
            foreach (var effect in effects)
            {
                if (effect is LastingEffect lastingEffect)
                    if (filter(effect))
                        lastingEffect.RemoveEffect();
            }
        }

        public void ApplyEffect(SO_Element element, bool isDebuff)
        {
            var effect = FindEffect(element, isDebuff);

            if (effect == null) return;

            Debug.Log(effect.GetType().Name);
            
            effect.ApplyEffect();
        }

        public void ApplyEffectForDefaultDuration(SO_Element element, bool isDebuff)
        {
            var effect = FindEffect(element, isDebuff);

            if (effect == null) return;

            var lastingEffect = effect as LastingEffect;
            
            if (lastingEffect != null)
                lastingEffect.ApplyEffectForDefaultDuration();
        }

        private Effect FindEffect(SO_Element element, bool isDebuff)
        {
            var effect = effects.Find(effect => effect.element == element && effect.isDebuff == isDebuff);

            if (effect != null) return effect;
            
            Debug.LogWarning($"Could not find effect. element: {element}, isDebuff: {isDebuff}");
            
            return null;
        }
    }
}