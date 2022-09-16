using Myths;
using UnityEngine;

namespace Debris
{
    public class DamageMyths : DebrisBehaviour
    {
        [SerializeField] private float damagePerSecond;
        
        private void OnTriggerStay(Collider other)
        {
            var myth = other.GetComponent<Myth>();

            if (myth == null) return;
            
            myth.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}