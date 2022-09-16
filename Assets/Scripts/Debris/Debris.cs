using System.Collections;
using UnityEngine;

namespace Debris
{
    public class Debris : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        
        public void Activate()
        {
            meshRenderer.enabled = true;

            StartCoroutine(Decay());
        }

        private IEnumerator Decay()
        {
            yield return new WaitForSeconds(3);
            
            meshRenderer.enabled = false;
        }
    }
}