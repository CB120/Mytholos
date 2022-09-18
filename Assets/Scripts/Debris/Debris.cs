using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Debris
{
    public class Debris : MonoBehaviour
    {
        // TODO: Move all mesh renderer responsibility to TempDebrisColor
        [SerializeField] private MeshRenderer meshRenderer;
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

        public bool PlaceDebris(SO_Element newElement)
        {
            if (newElement == null) return false;
            
            // Only override current debris if it is weak against the new debris
            if (currentElement != null && !newElement.strongAgainst.Contains(currentElement)) return false;
            
            meshRenderer.enabled = true;
            
            CurrentElement = newElement;
            
            if (decayCoroutine != null)
                StopCoroutine(decayCoroutine);

            decayCoroutine = StartCoroutine(Decay());
            
            return true;
        }

        private IEnumerator Decay()
        {
            yield return new WaitForSeconds(decayTime);
            
            meshRenderer.enabled = false;
            
            CurrentElement = null;

            decayCoroutine = null;
        }
    }
}