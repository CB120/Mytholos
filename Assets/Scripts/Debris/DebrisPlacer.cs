using System.Collections.Generic;
using UnityEngine;

namespace Debris
{
    public class DebrisPlacer : MonoBehaviour
    {
        // TODO: Element should be taken from the ability
        [SerializeField] private SO_Element element;
        
        // private List<Collider> colliders = new();

        private void OnTriggerEnter(Collider other)
        {
            var debris = other.GetComponent<Debris>();

            if (debris == null) return;
            
            // colliders.Add(other);
            
            debris.PlaceDebris(element);
        }
        
        private void OnTriggerExit(Collider other)
        {
            var debris = other.GetComponent<Debris>();

            if (debris == null) return;
            
            // colliders.Remove(other);
        }
    }
}