using System.Collections.Generic;
using UnityEngine;

namespace StateMachines
{
    // TODO: Could be improved as a local service, to save two-step setup of each AnyStateTransition.
    // TODO: Or use a find on Awake.
    public class AnyStateTransitionManager : MonoBehaviour
    {
        [SerializeField] private List<AnyStateTransition> anyStateTransitions;

        public IReadOnlyList<AnyStateTransition> AnyStateTransitions => anyStateTransitions.AsReadOnly();
    }
}