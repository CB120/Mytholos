using UnityEngine;

namespace Debris
{
    // TODO: This is just a temporary way to tell different debris types apart until we get the textures in
    public class TempDebrisColor : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Debris debris;

        private Color oldColor;
        
        private void OnEnable()
        {
            debris.elementChanged.AddListener(OnElementChanged);
            debris.isElectrifiedChanged.AddListener(OnIsElectrifiedChanged);
        }
        
        private void OnDisable()
        {
            debris.elementChanged.RemoveListener(OnElementChanged);
            debris.isElectrifiedChanged.RemoveListener(OnIsElectrifiedChanged);
        }

        private void OnElementChanged(Debris _)
        {
            // TODO: Duplicate code. See UIGameAbility
            var color = debris.CurrentElement == null ? Color.black : debris.CurrentElement.color;
            
            meshRenderer.material.color = color;

            meshRenderer.enabled = debris.CurrentElement != null;
        }

        private void OnIsElectrifiedChanged(Debris _)
        {
            var color = debris.CurrentElement == null ? Color.black : debris.CurrentElement.color;

            color = debris.IsElectrified ? Color.white : color;
            
            meshRenderer.material.color = color;

            meshRenderer.enabled = debris.CurrentElement != null;
        }
    }
}