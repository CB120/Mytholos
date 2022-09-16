using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Debris
{
    public class DebrisController : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileBase tile;
        [SerializeField] private TileBase blankTile;
        [SerializeField] private Bounds gridBounds;

        private void Awake()
        {
            Debug.Log(gridBounds);
            var scaledSize = new Vector3(
                gridBounds.size.x / tilemap.layoutGrid.cellSize.x,
                gridBounds.size.y / tilemap.layoutGrid.cellSize.y,
                gridBounds.size.z / tilemap.layoutGrid.cellSize.z
            );

            Debug.Log(scaledSize);
            var boundsInt = new BoundsInt(
                Vector3Int.FloorToInt(gridBounds.min),
                Vector3Int.FloorToInt(scaledSize)
            );

            Debug.Log(boundsInt);

            for (int x = boundsInt.xMin; x <= boundsInt.xMax; x++)
            {
                for (int y = boundsInt.yMin; y <= boundsInt.yMax; y++)
                {
                    var gridPos = new Vector3Int(x, y, 0);
                    tilemap.SetTile(gridPos, blankTile);
                    
                    tilemap.GetInstantiatedObject(gridPos).transform.localScale = Grid.Swizzle(tilemap.cellSwizzle, tilemap.cellSize);
                }
            }
            
            // tilemap.ResizeBounds();

            Debug.Log(tilemap.cellBounds);

            // tilemap.BoxFill(Vector3Int.zero, blankTile, -boundsInt, -boundsInt, boundsInt, boundsInt);
            
            // tilemap.SetTile(Vector3Int.zero, tile);
            // tilemap.SetTilesBlock(new BoundsInt(Vector3Int.zero, new Vector3Int(5,5,5)), new []{blankTile});
        }
    }
}