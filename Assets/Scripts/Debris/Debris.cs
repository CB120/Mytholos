using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Debris
{
    public class Debris : MonoBehaviour
    {
        [SerializeField] private float decayTime;
        [Tooltip("Read only. For debugging purposes.")]
        [SerializeField] private Vector3Int tilePositionReadout;

        public SO_Element CurrentElement
        {
            get => currentElement;
            set
            {
                // TODO: We shouldn't need both elementToBeChanged and OldElement.
                elementToBeChanged.Invoke(this);
                OldElement = currentElement;
                currentElement = value;
                elementChanged.Invoke(this);
            }
        }
        
        // TODO: Efficient, but not well organised
        public Vector3Int TilePosition { get; private set; }
        
        public SO_Element OldElement { get; private set; }

        public bool IsElectrified
        {
            get => isElectrified;
            set
            {
                var changed = isElectrified != value;
                
                isElectrified = value;
                
                if (changed)
                    isElectrifiedChanged.Invoke(this);
            }
        }

        [NonSerialized] public UnityEvent<Debris> elementToBeChanged = new();
        [NonSerialized] public UnityEvent<Debris> elementChanged = new();
        [NonSerialized] public UnityEvent<Debris> isElectrifiedChanged = new();

        private Coroutine decayCoroutine;
        private SO_Element currentElement;
        private bool isElectrified;

        public void Initialise(Vector3Int tilePosition)
        {
            TilePosition = tilePosition;
            tilePositionReadout = tilePosition;
        }

        public bool PlaceDebris(SO_Element newElement)
        {
            if (newElement == null) return false;

            if (!newElement.hasDebris) return false;
            
            if (newElement == currentElement)
            {
                RestartDecayTimer();

                return false;
            }

            // Only override current debris if it is weak against the new debris
            if (currentElement != null && !newElement.strongAgainst.Contains(currentElement)) return false;
            
            CurrentElement = newElement;
            
            RestartDecayTimer();
            
            return true;
        }

        public void RemoveDebris()
        {
            if (CurrentElement == null) return;
            
            CurrentElement = null;
            
            if (decayCoroutine != null)
                StopCoroutine(decayCoroutine);
        }

        private void RestartDecayTimer()
        {
            if (decayCoroutine != null)
                StopCoroutine(decayCoroutine);

            decayCoroutine = StartCoroutine(Decay());
        }

        private IEnumerator Decay()
        {
            yield return new WaitForSeconds(decayTime);
            
            CurrentElement = null;

            decayCoroutine = null;
        }
    }
}