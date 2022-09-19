using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Debris
{
    public class DebrisController : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileBase tile;
        [SerializeField] private Bounds gridBounds;

        public int NumberOfTiles { get; private set; }

        private readonly Dictionary<SO_Element, int> numberOfTilesWithElement = new();

        public UnityEvent numberOfTilesWithElementChanged = new();

        private void Awake()
        {
            var scaledSize = new Vector3(
                gridBounds.size.x / tilemap.cellSize.x,
                gridBounds.size.y / tilemap.cellSize.y,
                gridBounds.size.z / tilemap.cellSize.z
            );

            var scaledMin = new Vector3(
                gridBounds.min.x / tilemap.cellSize.x,
                gridBounds.min.y / tilemap.cellSize.y,
                gridBounds.min.z / tilemap.cellSize.z
            );

            var boundsInt = new BoundsInt(
                Vector3Int.FloorToInt(scaledMin),
                Vector3Int.FloorToInt(scaledSize)
            );

            for (int x = boundsInt.xMin; x <= boundsInt.xMax; x++)
            {
                for (int y = boundsInt.yMin; y <= boundsInt.yMax; y++)
                {
                    var gridPos = new Vector3Int(x, y, 0);
                    tilemap.SetTile(gridPos, tile);

                    var tileObject = tilemap.GetInstantiatedObject(gridPos);
                    
                    tileObject.transform.localScale = Grid.Swizzle(tilemap.cellSwizzle, tilemap.cellSize);
                    
                    // TODO: Unlisten?
                    tileObject.GetComponent<Debris>().elementChanged.AddListener(OnElementChanged);

                    NumberOfTiles++;
                }
            }
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
    }
}