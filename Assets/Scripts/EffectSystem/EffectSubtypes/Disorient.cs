using Myths;
using UnityEngine;

namespace EffectSystem.EffectSubtypes
{
    public class Disorient : LastingEffect
    {
        [SerializeField] private Myth myth;
        [SerializeField] private float rotateSpeed = 300;
        
        private bool isDisoriented;

        // TODO: Never called?
        public void ApplyEffect(float knockback, Myth sendingMyth)
        {
            base.ApplyEffect();
            
            isDisoriented = true;
            myth.Knockback(knockback, sendingMyth.gameObject, duration * 0.75f);
        }

        public override void RemoveEffect()
        {
            base.RemoveEffect();
            
            isDisoriented = false;
        }
        
        private void Update()
        {
            if (isDisoriented)
            {
                myth.gameObject.transform.Rotate(0, Time.deltaTime * rotateSpeed, 0);
            }
        }
    }
}