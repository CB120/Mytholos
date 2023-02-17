using UnityEngine;

namespace EffectSystem
{
    public class Effect : MonoBehaviour
    {
        [SerializeField] public SO_Element element;
        [SerializeField] public bool isDebuff;
        [SerializeField] protected Effects effects;

        public virtual void ApplyEffect()
        {
            
        }
    }
}