using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Debris
{
    public class Debris : MonoBehaviour
    {
        [SerializeField] private float decayTime;

        public SO_Element CurrentElement
        {
            get => currentElement;
            set
            {
                OldElement = currentElement;
                currentElement = value;
                elementChanged.Invoke(this);
            }
        }
        
        public SO_Element OldElement { get; private set; }

        public UnityEvent<Debris> elementChanged = new();

        private Coroutine decayCoroutine;
        private SO_Element currentElement;

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