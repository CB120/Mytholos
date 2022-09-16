using System.Collections;
using UnityEngine;

namespace Debris
{
    public class Debris : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;

        private SO_Element currentElement;
        private GameObject debrisBehavioursObject;

        public void PlaceDebris(SO_Element newElement)
        {
            meshRenderer.enabled = true;
            
            if (debrisBehavioursObject != null)
                Destroy(debrisBehavioursObject);

            debrisBehavioursObject = Instantiate(newElement.debrisBehavioursPrefab, transform);

            currentElement = newElement;

            StartCoroutine(Decay());
        }

        private IEnumerator Decay()
        {
            yield return new WaitForSeconds(3);
            
            meshRenderer.enabled = false;
            
            if (debrisBehavioursObject != null)
                Destroy(debrisBehavioursObject);

            currentElement = null;
        }
    }
}