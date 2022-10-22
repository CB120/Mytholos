using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debris
{
    public class DebrisAppearance : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Debris debris;
        [SerializeField] private Material defaultMaterial;
        private void OnEnable()
        {
            debris.elementChanged.AddListener(OnElementChanged);
        }

        private void OnDisable()
        {
            debris.elementChanged.RemoveListener(OnElementChanged);
        }

        private void OnElementChanged(Debris _)
        {
            // TODO: Duplicate code. See UIGameAbility
            var color = debris.CurrentElement == null ? Color.black : debris.CurrentElement.color;
            Material material = new Material(defaultMaterial);
            if(debris.CurrentElement != null && debris.CurrentElement.customMaterial != null)
                material = debris.CurrentElement.customMaterial;
            else if(debris.CurrentElement != null && debris.CurrentElement.debrisTexture)
                material.SetTexture("_MainTex", debris.CurrentElement.debrisTexture);
            else
                material.SetColor("_Color", color);

            meshRenderer.material = material;

            meshRenderer.enabled = debris.CurrentElement != null;
        }
    }
}
