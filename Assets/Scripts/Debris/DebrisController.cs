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
                    tilemap.SetTile(gridPos, blankTile);
                    
                    tilemap.GetInstantiatedObject(gridPos).transform.localScale = Grid.Swizzle(tilemap.cellSwizzle, tilemap.cellSize);
                }
            }
        }
    }
}