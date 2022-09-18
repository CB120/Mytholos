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
                currentElement = value;
                elementChanged.Invoke();
            }
        }

        public UnityEvent elementChanged = new();

        private Coroutine decayCoroutine;
        private SO_Element currentElement;

        // TODO: Restart the decay coroutine even if it's the same element, but return false
        public bool PlaceDebris(SO_Element newElement)
        {
            if (newElement == null) return false;
            
            // Only override current debris if it is weak against the new debris
            if (currentElement != null && !newElement.strongAgainst.Contains(currentElement)) return false;
            
            CurrentElement = newElement;
            
            if (decayCoroutine != null)
                StopCoroutine(decayCoroutine);

            decayCoroutine = StartCoroutine(Decay());
            
            return true;
        }

        private IEnumerator Decay()
        {
            yield return new WaitForSeconds(decayTime);
            
            CurrentElement = null;

            decayCoroutine = null;
        }
    }
}