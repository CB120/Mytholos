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
        [SerializeField] private GameObject ActiveMythController;
        [SerializeField] private WinState winState;
        private float timer;
        private SwapCommand swapCommand;


        private void Awake()
        {
            ActiveMythController = GameObject.FindGameObjectWithTag("PartyBuilder");
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            swapCommand = mythCommandHandler.Command as SwapCommand;
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
            } else if (timer >= swapTime)
            {
                Debug.Log("Swap complete!");
                if (ActiveMythController != null)
                {
                    ActiveMythController.GetComponent<PartyBuilder>().setSwappingTarget(swapCommand.SwappingInMyth, swapCommand.PartyIndex);
                } else
                {
                    Debug.Log("Could not find the PartyBuilder to set the active myth on " + this.gameObject.name);
                }

                swapCommand.sendingPlayer.SwapReserveAtIndex(swapCommand.TriggerIndex);
                mythCommandHandler.Command = null;
                swapComplete.Invoke();
            }
        }
    }
}
