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

        private GameObject debrisBehavioursObject;
        private Coroutine decayCoroutine;
        private SO_Element currentElement;

        public void PlaceDebris(SO_Element newElement)
        {
            // Only override current debris if it is weak against the new debris
            if (currentElement != null && !newElement.strongAgainst.Contains(currentElement)) return;
            
            meshRenderer.enabled = true;
            
            if (debrisBehavioursObject != null)
                Destroy(debrisBehavioursObject);

            debrisBehavioursObject = Instantiate(newElement.debrisBehavioursPrefab, transform);

            CurrentElement = newElement;
            
            if (decayCoroutine != null)
                StopCoroutine(decayCoroutine);

            decayCoroutine = StartCoroutine(Decay());
        }

        private IEnumerator Decay()
        {
            yield return new WaitForSeconds(decayTime);
            
            meshRenderer.enabled = false;
            
            if (debrisBehavioursObject != null)
                Destroy(debrisBehavioursObject);

            CurrentElement = null;

            decayCoroutine = null;
        }
    }
}