using Pathfinding;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static TerrainGenerator;

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
        
        public Vector2 getSpawnPoint() { return spawnPoint; }

        void Start()
        {
           TerrainGenerator terrainGenerator = new TerrainGenerator();
           renderMap(terrainGenerator.buildNoiseMap(seed));
           tilemap.CompressBounds();
           tilemap.RefreshAllTiles();
           mapRendered = true;
        }

        private void LateUpdate()
        {
            if (mapRendered & !pathMapped)
            {
                AstarPath.active.Scan();
                pathMapped = true;
            }
        }

        private void Update()
        {
            if (regen)
            {
                TerrainGenerator terrainGenerator = new TerrainGenerator();
                renderMap(terrainGenerator.buildNoiseMap(seed));
                tilemap.CompressBounds();
                tilemap.RefreshAllTiles();
                regen = false;
                pathMapped = false;
            }
            Vector3Int dynVector = GetTilePostitionByClick();
            if (dynVector!=Vector3Int.zero)
            {
                RemoveTile(dynVector);
                pathMapped = false;
            }
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
                tilemap.SetTile(tilePosition, null);
                Debug.Log($"Tile removido na posição {tilePosition}");
            }
            else
            {
                Debug.Log($"Nenhum tile encontrado na posição {tilePosition}");
            }
        }

        private void renderMap(float[,] noiseMap)
        {
            for (int y = 0; y < noiseMap.GetLength(1); y++)
            {
                for (int x = 0; x < noiseMap.GetLength(0); x++)
                {
                    // Define o tile com base no valor do noise
                    TileBase tile = DetermineTile(noiseMap[x, y]);

                    // Define a posição do tile no Tilemap
                    Vector3Int tilePosition = new Vector3Int(x + (int)offset.x, y + (int)offset.y, 0);

                    // Define o tile no Tilemap
                    tilemap.SetTile(tilePosition, tile);
                }
            }
        }

        private TileBase DetermineTile(float noiseValue)
        {
            if (inverseRender)
            {
                if (noiseValue < thresHold)
                {
                    // Retorna o tile específico para valores de noise altos
                    return tileMapHighNoiseTile;
                }
                else
                {
                    // Retorna o tile padrão para valores de noise baixos
                    return tileMapLowNoiseTile;
                }
            }
            else
            {
                if (noiseValue > thresHold)
                {
                    // Retorna o tile específico para valores de noise altos
                    return tileMapHighNoiseTile;
                }
                else
                {
                    // Retorna o tile padrão para valores de noise baixos
                    return tileMapLowNoiseTile;
                }
            }
        }
        Vector3Int GetTilePostitionByClick()
        {
            // Verifica se o botão esquerdo do mouse foi clicado
            if (Input.GetMouseButtonDown(0))
            {
                // Converte a posição do mouse para as coordenadas do mundo
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Converte a posição do mundo para uma posição no grid do Tilemap
                Vector3Int tilePosition = tilemap.WorldToCell(mouseWorldPos);


                 return tilePosition;
            }
            return Vector3Int.zero;
        }
    }

}