using UnityEngine;
using UnityEngine.Events;
using Commands;

namespace Myths.Behaviours
{
    public class PerformAbilityBehaviour : Behaviour
    {
        public UnityEvent performAbilityComplete = new();
        
        private void Update()
        {
            Debug.Log($"{myth.name} performed ability.");
            GameObject ability = ((AbilityCommand)myth.Command).ability.ability;
            GameObject abilityPrefab = Instantiate(ability, this.gameObject.transform.position, new Quaternion(0f, 0f, 0f, 0f), this.gameObject.transform);
            abilityPrefab.GetComponent<Ability>().owningMyth = myth;
            myth.Command = null;

            performAbilityComplete.Invoke();
        }
    }
}