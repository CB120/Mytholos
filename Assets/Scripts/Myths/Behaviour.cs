using Commands;
using UnityEngine;

namespace Myths
{
    public class Behaviour : MonoBehaviour
    {
        [Header("Behaviour")]
        // TODO: Do we still need this?
        [SerializeField] protected Myth myth;
        [SerializeField] protected MythCommandHandler mythCommandHandler;
        [SerializeField] private bool dontAllowOtherCommands;
        [SerializeField] protected Animator anim;

        public void Awake()
        {
            enabled = false;

            if (anim == null && myth != null)
                anim = myth.gameObject.GetComponentInChildren<Animator>();
        }

        protected virtual void OnEnable()
        {
            if (dontAllowOtherCommands)
            {
                if (mythCommandHandler.Command is not KnockbackService)
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