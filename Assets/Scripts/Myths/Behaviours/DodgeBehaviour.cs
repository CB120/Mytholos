using Commands;
using UnityEngine;
using UnityEngine.Events;

namespace Myths.Behaviours
{
    public class DodgeBehaviour : Behaviour
    {
        public UnityEvent DodgeComplete = new();
        private void Update()
        {
            Debug.Log($"{myth.name} used a dodge");

            myth.Command = null;

            DodgeComplete.Invoke();
        }
    }
}
