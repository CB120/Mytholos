using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Debris
{
    public class DebrisController : MonoBehaviour
    {
        [SerializeField] private DebrisControllerService debrisControllerService;
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileBase tile;
        [SerializeField] private Bounds gridBounds;

        public Tilemap Tilemap => tilemap;

        [NonSerialized] public UnityEvent<Debris> debrisCreated = new();

        private void Awake()
        {
            debrisControllerService.DebrisController = this;
            
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

                    var debris = tileObject.GetComponent<Debris>();
                    
                    debris.Initialise(gridPos);
                    
                    debrisCreated.Invoke(debris);
                }
            }
        }

        public List<Debris> FloodGetTiles(Debris startDebris, Func<Debris, bool> testFunc)
        {
            var stack = new Stack<Debris>();
            var visited = new List<Debris>();
            
            stack.Push(startDebris);

            while (stack.Count > 0)
            {
                var currentDebris = stack.Pop();

                if (!testFunc(currentDebris)) continue;
                
                if (visited.Contains(currentDebris)) continue;

                foreach (var adjacentTile in GetAdjacentTiles(currentDebris.TilePosition))
                {
                    var adjacentDebris = TilePositionToDebris(adjacentTile);
                    
                    // TODO: Is it more efficient to do the contains check here?
                    if (adjacentDebris != null)
                        stack.Push(adjacentDebris);
                }
                
                visited.Add(currentDebris);
            }

            return visited;
        }

        private Debris TilePositionToDebris(Vector3Int position)
        {
            var instantiatedObject = tilemap.GetInstantiatedObject(position);
                
            if (instantiatedObject == null) return null;
                
            return instantiatedObject.GetComponent<Debris>();
        }

        private static List<Vector3Int> GetAdjacentTiles(Vector3Int tile) =>
            new List<Vector3Int>
            {
                new(tile.x + 1, tile.y, tile.z),
                new(tile.x - 1, tile.y, tile.z),
                new(tile.x, tile.y + 1, tile.z),
                new(tile.x, tile.y - 1, tile.z)
            };
    }
}