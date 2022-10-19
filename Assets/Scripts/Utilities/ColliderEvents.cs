using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    public class ColliderEvents : MonoBehaviour
    {
        [NonSerialized] public UnityEvent<Collider> triggerEntered = new();
        [NonSerialized] public UnityEvent<Collider> triggerStayed = new();
        [NonSerialized] public UnityEvent<Collider> triggerExited = new();

        private void OnTriggerEnter(Collider other)
        {
            triggerEntered.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            triggerStayed.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            triggerExited.Invoke(other);
        }
    }
}