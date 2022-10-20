using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Debris
{
    public class DebrisInteractorManager : MonoBehaviour
    {
        [Tooltip("Optional. If not specified, will use default collision messages.")]
        [SerializeField] private ColliderEvents colliderEvents;
        [Tooltip("Determines the order in which DebrisInteractors will be called.")]
        [SerializeField] private List<DebrisInteractor> debrisInteractors = new();

        private void OnEnable()
        {
            if (colliderEvents == null) return;
            
            colliderEvents.triggerEntered.AddListener(OnTriggerEntered);
            colliderEvents.triggerStayed.AddListener(OnTriggerStayed);
            colliderEvents.triggerExited.AddListener(OnTriggerExited);
        }
        
        private void OnDisable()
        {
            if (colliderEvents == null) return;
            
            colliderEvents.triggerEntered.RemoveListener(OnTriggerEntered);
            colliderEvents.triggerStayed.RemoveListener(OnTriggerStayed);
            colliderEvents.triggerExited.RemoveListener(OnTriggerExited);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (colliderEvents == null)
                OnTriggerEntered(other);
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (colliderEvents == null)
                OnTriggerStayed(other);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (colliderEvents == null)
                OnTriggerExited(other);
        }

        private void OnTriggerEntered(Collider other)
        {
            OnDebrisTrigger(other, (debrisInteractor, debris) => debrisInteractor.OnDebrisEnter(debris));
            
            var debris = other.GetComponent<Debris>();

            if (debris == null) return;
            
            debris.elementToBeChanged.AddListener(OnElementToBeChanged);
        }

        private void OnTriggerExited(Collider other)
        {
            OnDebrisTrigger(other, (debrisInteractor, debris) => debrisInteractor.OnDebrisExit(debris));
            
            var debris = other.GetComponent<Debris>();

            if (debris == null) return;
            
            debris.elementToBeChanged.RemoveListener(OnElementToBeChanged);
        }

        private void OnTriggerStayed(Collider other)
        {
            OnDebrisTrigger(other, (debrisInteractor, debris) => debrisInteractor.OnDebrisStay(debris));
        }

        public void OnDebrisTrigger(Collider other, Action<DebrisInteractor, Debris> action)
        {
            var debris = other.GetComponent<Debris>();

            if (debris == null) return;

            foreach (var debrisInteractor in debrisInteractors)
            {
                action(debrisInteractor, debris);
            }
        }

        private void OnElementToBeChanged(Debris debris)
        {
            foreach (var debrisInteractor in debrisInteractors)
            {
                debrisInteractor.OnDebrisExit(debris);
            }
        }
    }
}