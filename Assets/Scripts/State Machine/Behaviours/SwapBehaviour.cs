using UnityEngine;
using UnityEngine.Events;
using Behaviour = Myths.Behaviour;

namespace Commands.Behaviours
{
    public class SwapBehaviour : Behaviour
    {
        // Events
        public UnityEvent swapFailed = new();
        public UnityEvent swapComplete = new();
        [SerializeField] private float swapTime;
        private float timer;
        protected override void OnEnable()
        {
            base.OnEnable();
            // Start the Animation
            Debug.Log("Swapping!");
            timer = 0;
        }

        private void Update()
        {
            if(timer < swapTime)
            {
                timer += Time.deltaTime;
                // add some logic here to cancel the swap if you get hit!
            } else
            {
                Debug.Log("Swap complete!");
                swapComplete.Invoke();
            }
        }
    }
}
