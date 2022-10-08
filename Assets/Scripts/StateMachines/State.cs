using Myths;
using StateMachines.Commands;
using UnityEngine;

namespace StateMachines
{
    public class State : MonoBehaviour
    {
        [Header("Behaviour")]
        // TODO: Do we still need this?
        [SerializeField] protected Myth myth;
        [SerializeField] protected MythCommandHandler mythCommandHandler;
        [SerializeField] private bool dontAllowOtherCommands;


        public void Awake()
        {
            enabled = false;
        }

        protected virtual void OnEnable()
        {
            if (dontAllowOtherCommands)
            {
                if (mythCommandHandler.Command is not KnockbackCommand)
                {
                    mythCommandHandler.WillStoreNewCommands = false;
                }
            }
        }

        protected virtual void OnDisable()
        {
            if (dontAllowOtherCommands)
                mythCommandHandler.WillStoreNewCommands = true;
        }
    }
}