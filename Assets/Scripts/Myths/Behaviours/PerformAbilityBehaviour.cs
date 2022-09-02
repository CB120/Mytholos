using UnityEngine;
using UnityEngine.Events;

namespace Myths.Behaviours
{
    public class PerformAbilityBehaviour : Behaviour
    {
        public UnityEvent performAbilityComplete = new();
        
        private void Update()
        {
            Debug.Log($"{myth.name} performed ability.");

            myth.Command = null;

            performAbilityComplete.Invoke();
        }
    }
}