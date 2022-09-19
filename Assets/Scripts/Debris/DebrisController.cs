using System;
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
                    
                    tileObject.GetComponent<Debris>().Initialise(gridPos);

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

        public List<Vector3Int> FloodGetTiles(Vector3Int startTile, Func<Vector3Int, bool> testFunc)
        {
            var stack = new Stack<Vector3Int>();
            var visited = new List<Vector3Int>();
            
            stack.Push(startTile);

            while (stack.Count > 0)
            {
                var currentTile = stack.Pop();

                if (!testFunc(currentTile)) continue;
                
                if (visited.Contains(currentTile)) continue;

                foreach (var adjacentTile in GetAdjacentTiles(currentTile))
                {
                    // TODO: Is it more efficient to do the contains check here?
                    stack.Push(adjacentTile);
                }
                
                visited.Add(currentTile);
            }

            return visited;
        }

        private List<Vector3Int> GetAdjacentTiles(Vector3Int tile)
        {
            var output = new List<Vector3Int>();
            
            output.Add(new Vector3Int(tile.x + 1, tile.y, tile.z));
            output.Add(new Vector3Int(tile.x - 1, tile.y, tile.z));
            output.Add(new Vector3Int(tile.x, tile.y + 1, tile.z));
            output.Add(new Vector3Int(tile.x, tile.y - 1, tile.z));

            return output;
        }
    }
}