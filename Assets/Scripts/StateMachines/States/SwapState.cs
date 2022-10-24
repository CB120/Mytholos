using Myths;
using StateMachines.Commands;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachines.States
{
    public class SwapState : State
    {
        // Events
        public UnityEvent swapFailed = new();
        public UnityEvent swapComplete = new();
        [SerializeField] private float swapTime;
        [SerializeField] private GameObject ActiveMythController;
        [SerializeField] private WinState winState;
        private float timer;
        private const float swapAnimationDuration = 0.79166667f; // Arcane numerics
        private SwapCommand swapCommand;

        [Header("SFX")]
        public GameObject swapStartSFXPrefabLeft;
        public GameObject swapEndSFXPrefabLeft;
        public GameObject swapStartSFXPrefabRight;
        public GameObject swapEndSFXPrefabRight;
        public float timeToDestroySwapSFX = 0.3f;
        int partyIndex = 0;

        protected override void Awake()
        {
            base.Awake();
            ActiveMythController = GameObject.FindGameObjectWithTag("PartyBuilder");
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            swapCommand = mythCommandHandler.LastCommand as SwapCommand;

            // Start the Animation
            if (anim)
            {
                anim.SetTrigger("SwapOut");
                anim.speed = swapAnimationDuration / swapTime;
            }
            //Debug.Log("Swapping!");
            timer = 0;

            // SFX
            partyIndex = gameObject.GetComponentInParent<Myth>().partyIndex; // SFX

            GameObject selectedPrefab = swapStartSFXPrefabLeft;
            if (partyIndex == 1) selectedPrefab = swapStartSFXPrefabRight;
            GameObject swapStartSFX = Instantiate(selectedPrefab, Vector3.zero, Quaternion.identity);
            Destroy(swapStartSFX, timeToDestroySwapSFX);

            
        }

        private void Update()
        {
            if(timer < swapTime)
            {
                timer += Time.deltaTime;
                // add some logic here to cancel the swap if you get hit!
            } else if (timer >= swapTime)
            {
                //Debug.Log("Swap complete!");
                if (ActiveMythController != null)
                {
                    ActiveMythController.GetComponent<PartyBuilder>().setSwappingTarget(swapCommand.SwappingInMyth, swapCommand.PartyIndex);
                } else
                {
                    Debug.Log("Could not find the PartyBuilder to set the active myth on " + this.gameObject.name);
                }

                swapCommand.sendingPlayer.SwapInDirection(swapCommand.TriggerIndex);
                mythCommandHandler.DemoteCurrentCommand();
                swapComplete.Invoke();

                // SFX
                GameObject selectedPrefab = swapEndSFXPrefabLeft;
                if (partyIndex == 1) selectedPrefab = swapEndSFXPrefabRight;
                GameObject swapStartSFX = Instantiate(selectedPrefab, Vector3.zero, Quaternion.identity);
                Destroy(swapStartSFX, timeToDestroySwapSFX);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (anim)
            {
                anim.speed = 1.0f;
            }
        }
    }
}
