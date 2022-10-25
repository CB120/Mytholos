using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Debris
{
    public class DebrisRegion : MonoBehaviour
    {
        [SerializeField] private DebrisController debrisController;
        [SerializeField] private BoundsInt boundsInt;
        [Header("Debug Only")]
        [SerializeField] private int numberOfTiles;
        [SerializeField] private int numberOfElectrifiedTiles;
        
        public int NumberOfTiles
        {
            get => numberOfTiles;
            private set => numberOfTiles = value;
        }

        public int NumberOfElectrifiedTiles
        {
            get => numberOfElectrifiedTiles;
            private set => numberOfElectrifiedTiles = value;
        }

        private readonly Dictionary<SO_Element, int> numberOfTilesWithElement = new();

        public UnityEvent numberOfTilesWithElementChanged = new();
        public UnityEvent numberOfElectrifiedTilesChanged = new();

        private void OnEnable()
        {
            debrisController.debrisCreated.AddListener(OnDebrisCreated);
        }

        private void OnDisable()
        {
            debrisController.debrisCreated.RemoveListener(OnDebrisCreated);
        }

        private void OnDebrisCreated(Debris debris)
        {
            if (!boundsInt.Contains(Vector3Int.FloorToInt(Grid.InverseSwizzle(debrisController.Tilemap.cellSwizzle, debris.TilePosition)))) return;
            
            // TODO: Unlisten?
            debris.elementChanged.AddListener(OnElementChanged);
            debris.isElectrifiedChanged.AddListener(OnIsElectrifiedChanged);

            NumberOfTiles++;
        }

        private void OnElementChanged(Debris debris)
        {
            if (debris.OldElement != null)
                numberOfTilesWithElement[debris.OldElement]--;

            if (debris.CurrentElement != null)
            {
                if (!numberOfTilesWithElement.ContainsKey(debris.CurrentElement))
                    numberOfTilesWithElement[debris.CurrentElement] = 0;

                numberOfTilesWithElement[debris.CurrentElement]++;
            }
            
            numberOfTilesWithElementChanged.Invoke();
        }

        public int NumberOfTilesWithElement(SO_Element element)
        {
            return !numberOfTilesWithElement.ContainsKey(element) ? 0 : numberOfTilesWithElement[element];
        }

        private void OnIsElectrifiedChanged(Debris debris)
        {
            if (debris.IsElectrified)
                NumberOfElectrifiedTiles++;
            else
                NumberOfElectrifiedTiles--;
            
            numberOfElectrifiedTilesChanged.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);

            var boxSize = Vector3.Scale(boundsInt.size, debrisController.Tilemap.cellSize);

            var scaledCenter = Vector3.Scale(boundsInt.center, debrisController.Tilemap.cellSize);
            
            Gizmos.DrawCube(transform.position + scaledCenter, boxSize);
        }
    }
}