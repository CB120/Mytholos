using Myths;
using StateMachines.Commands;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachines
{
    public abstract class AnyStateTransition : MonoBehaviour
    {
        [SerializeField] protected Myth myth;
        
        [SerializeField] protected UnityEvent transitionEvent = new();
    }
}