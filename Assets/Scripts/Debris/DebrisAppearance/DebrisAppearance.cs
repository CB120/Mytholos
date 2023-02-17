using UnityEngine;

// TODO: Move to Debris namespace
namespace Debris
{
    public class DebrisAppearance : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Debris debris;
        [SerializeField] private Material defaultMaterial;
        
        private void OnEnable()
        {
            debris.elementChanged.AddListener(OnDebrisChanged);
            debris.isElectrifiedChanged.AddListener(OnDebrisChanged);
        }

        private void OnDisable()
        {
            debris.elementChanged.RemoveListener(OnDebrisChanged);
            debris.isElectrifiedChanged.RemoveListener(OnDebrisChanged);
        }

        private void OnDebrisChanged(Debris _)
        {
            meshRenderer.enabled = debris.CurrentElement != null;

            if (debris.CurrentElement == null) return;

            if (debris.IsElectrified)
            {
                if (debris.CurrentElement.electrifiedMaterial != null)
                {
                    meshRenderer.material = debris.CurrentElement.electrifiedMaterial;

                    return;
                }

                Debug.LogWarning($"Element {debris.CurrentElement} does not have an {nameof(debris.CurrentElement.electrifiedMaterial)}.");
            }
            
            Material material = new Material(defaultMaterial);

            if (debris.CurrentElement.customMaterial != null)
                material = debris.CurrentElement.customMaterial;
            else if (debris.CurrentElement.debrisTexture)
                material.SetTexture("_MainTex", debris.CurrentElement.debrisTexture);
            else
                material.SetColor("_Color", debris.CurrentElement.color);

            meshRenderer.material = material;
        }
    }
}
