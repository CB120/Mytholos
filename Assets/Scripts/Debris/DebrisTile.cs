using UnityEngine;
using UnityEngine.Tilemaps;

namespace Debris
{
    [CreateAssetMenu]
    public class DebrisTile : TileBase
    {
        [SerializeField] private GameObject debrisPrefab;
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.gameObject = debrisPrefab;
        }
    }
}