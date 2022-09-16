using System;
using System.Collections.Generic;
using UnityEngine;

namespace Debris
{
    public class DebrisPlacer : MonoBehaviour
    {
        [SerializeField] private Collider collider;

        private List<Collider> colliders = new();

        private void Awake()
        {
            // TODO: Find all of the cells that intersect the collider
            // Could iterate over all the cells and compare the centre points
            // Could cut this down by only getting the ones in the bounds of the collider
            
            // The other option would be to place a collider on each of the tiles and then do comparative collisions between them
        }

        private void OnTriggerEnter(Collider other)
        {
            var debris = other.GetComponent<Debris>();

            if (debris == null)
                throw new Exception($"Debris instance does not have a {nameof(Debris)} component.");
            
            colliders.Add(other);
            
            debris.Activate();
        }
        
        private void OnTriggerExit(Collider other)
        {
            var debris = other.GetComponent<Debris>();

            if (debris == null)
                throw new Exception($"Debris instance does not have a {nameof(Debris)} component.");
            
            colliders.Remove(other);
            
            // debris.Activate();
        }
    }
}