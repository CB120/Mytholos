using System;
using System.Collections.Generic;
using System.Linq;
using Myths;
using StateMachines.Commands;
using UnityEngine;

namespace StateMachines
{
    public class State : MonoBehaviour
    {
        public enum OverrideBehaviour { AllowAllExcept, DisallowAllExcept }
        
        [Header("Behaviour")]
        // TODO: Do we still need this?
        [SerializeField] protected Myth myth;
        [SerializeField] protected MythCommandHandler mythCommandHandler;
        [SerializeField] private bool dontAllowOtherCommands;
        [SerializeField] private AnyStateTransitionManager anyStateTransitionManager;
        [SerializeField] private OverrideBehaviour overrideBehaviour;
        [SerializeField] private List<AnyStateTransition> exceptions;


        public void Awake()
        {
            enabled = false;
        }

        protected virtual void OnEnable()
        {
            if (dontAllowOtherCommands)
            {
                if (mythCommandHandler.LastCommand is not KnockbackCommand)
                {
                    mythCommandHandler.WillStoreNewCommands = false;
                }
            }
            
            OverrideAnyStateTransitions(true);
        }

        protected virtual void OnDisable()
        {
            if (dontAllowOtherCommands)
                mythCommandHandler.WillStoreNewCommands = true;
            
            OverrideAnyStateTransitions(false);
        }

        private void OverrideAnyStateTransitions(bool enabled)
        {
            switch (overrideBehaviour)
            {
                case OverrideBehaviour.AllowAllExcept:
                    foreach (var anyStateTransition in exceptions)
                    {
                        anyStateTransition.enabled = enabled;
                    }
                    break;
                case OverrideBehaviour.DisallowAllExcept:
                    foreach (var anyStateTransition in anyStateTransitionManager.AnyStateTransitions.Except(exceptions))
                    {
                        anyStateTransition.enabled = enabled;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}