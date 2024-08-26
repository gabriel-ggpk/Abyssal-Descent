using Pathfinding;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.ProceduralGeneration
{
    public class MapController : MonoBehaviour
    {
        [Header("Tools")]
        [SerializeField] private bool regen = false;

        [Header("Tile settings")]
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileBase tileMapHighNoiseTile;
        [SerializeField] private TileBase tileMapLowNoiseTile;
        [SerializeField] private bool inverseRender = false;

        [Header("Map basic Settings")]
        [Range(0, 255)]
        [SerializeField] private float thresHold;
        [SerializeField] private Vector2 offset;
        [SerializeField] private int seed;

        [Header("TileMap removal test")]
        [SerializeField] private bool remove = false;
        [SerializeField] private Vector3Int tilePosition;

        private bool mapRendered = false;
        private bool pathMapped = false;
        private Vector2 spawnPoint;

        public Vector2 GetSpawnPoint() => spawnPoint;

        private void Start()
        {
            GenerateAndRenderMap();
            tilemap.CompressBounds();
            tilemap.RefreshAllTiles();
            mapRendered = true;
        }

        private void LateUpdate()
        {
            if (mapRendered && !pathMapped)
            {
                AstarPath.active.Scan();
                pathMapped = true;
            }
        }

        private void Update()
        {
            HandleMapRegeneration();
            HandleTileRemoval();
        }

        private void HandleMapRegeneration()
        {
            if (regen)
            {
                GenerateAndRenderMap();
                tilemap.CompressBounds();
                tilemap.RefreshAllTiles();
                regen = false;
                pathMapped = false;
            }
        }

        private void HandleTileRemoval()
        {
            Vector3Int clickedTilePosition = GetTilePositionByClick();
            if (clickedTilePosition != Vector3Int.zero)
            {
                RemoveTile(clickedTilePosition);
                pathMapped = false;
            }
        }

        private void GenerateAndRenderMap()
        {
            TerrainGenerator terrainGenerator = new TerrainGenerator();
            float[,] noiseMap = terrainGenerator.buildNoiseMap(seed);
            RenderMap(noiseMap);
        }

        private void RenderMap(float[,] noiseMap)
        {
            for (int y = 0; y < noiseMap.GetLength(1); y++)
            {
                for (int x = 0; x < noiseMap.GetLength(0); x++)
                {
                    TileBase tile = DetermineTile(noiseMap[x, y]);
                    Vector3Int tilePosition = new Vector3Int(x + (int)offset.x, y + (int)offset.y, 0);
                    tilemap.SetTile(tilePosition, tile);
                }
            }
        }

        private TileBase DetermineTile(float noiseValue)
        {
            bool isHighNoise = inverseRender ? noiseValue < thresHold : noiseValue > thresHold;
            return isHighNoise ? tileMapHighNoiseTile : tileMapLowNoiseTile;
        }

        private Vector3Int GetTilePositionByClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                return tilemap.WorldToCell(mouseWorldPos);
            }
            return Vector3Int.zero;
        }

        public void RemoveTile(Vector3Int tilePosition)
        {
            if (tilemap == null)
            {
                Debug.LogError("Tilemap não fornecido!");
                return;
            }

            if (tilemap.HasTile(tilePosition))
            {
                var tilemapCollider = tilemap.GetComponent<TilemapCollider2D>();
                if (tilemapCollider != null)
                {
                    tilemapCollider.enabled = false;
                }

                // Modifique os tiles aqui
                tilemap.SetTile(tilePosition, null);
                if (tilemapCollider != null)
                {
                    tilemapCollider.enabled = true;
                }

                Debug.Log($"Tile removido na posição {tilePosition}");
            }
            else
            {
                Debug.Log($"Nenhum tile encontrado na posição {tilePosition}");
            }
        }
    }
}
