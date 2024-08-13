using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilemapShadowCaster : MonoBehaviour
{
    private Tilemap tilemap;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        GenerateShadowCasters();
    }

    void GenerateShadowCasters()
    {
        // Get the bounds of the tilemap
        BoundsInt bounds = tilemap.cellBounds;

        // Loop through all the tiles in the tilemap
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(tilePosition);

                if (tile != null && IsTileOnEdge(tilePosition))
                {
                    // Create a new GameObject for the shadow caster
                    GameObject shadowCasterObject = new GameObject("ShadowCaster");
                    shadowCasterObject.transform.parent = transform;
                    shadowCasterObject.transform.position = tilemap.CellToWorld(tilePosition) + tilemap.tileAnchor;

                    // Add the ShadowCaster2D component and configure it
                    ShadowCaster2D shadowCaster = shadowCasterObject.AddComponent<ShadowCaster2D>();
                    shadowCaster.selfShadows = false;
                    shadowCaster.useRendererSilhouette = true;
                    shadowCaster.castsShadows = false;
                }
            }
        }
    }

    bool IsTileOnEdge(Vector3Int tilePosition)
    {
        // Check the surrounding tiles to determine if the tile is on the edge
        if (tilemap.GetTile(tilePosition + Vector3Int.left) == null ||
            tilemap.GetTile(tilePosition + Vector3Int.right) == null ||
            tilemap.GetTile(tilePosition + Vector3Int.up) == null ||
            tilemap.GetTile(tilePosition + Vector3Int.down) == null)
        {
            return true;
        }

        return false;
    }
}
