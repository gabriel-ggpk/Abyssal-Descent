using Pathfinding;
using System.Collections;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Tilemaps;
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
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private float scale;
        [SerializeField] private float frequency = 0.1f;
        [SerializeField] private int seed;
        [SerializeField] private NoiseType noiseType;
        [Range(0, 255)]
        [SerializeField] private float thresHold;
        [SerializeField] private Vector2 offset;

        [Header("Voironoi/Worley Noise Settings")]
        [SerializeField] private int numCells;

        [Header("FBM Noise Settings")]
        [SerializeField] private int octaves;
        [Range(0, 1)]
        [SerializeField] private float persistence;
        [SerializeField] private float lacunarity;

        private Vector2 spawnPoint;
        
        public Vector2 getSpawnPoint() { return spawnPoint; }

        public enum NoiseType { Perlin, Voronoi, Worley, FBM }

        void Start()
        {
            StartCoroutine(BuildMap());
           
        }
        private void Update()
        {
            if (regen)
            {
                BuildMap();
                regen = false;
            }

        }
  
        private IEnumerator BuildMap()
        {
            TerrainGenerator terrainGenerator = new TerrainGenerator(width, height, scale, frequency, seed, noiseType, numCells, octaves, persistence, lacunarity, offset);
            float[,] noiseMap = terrainGenerator.GenerateTerrain();

            for (int y = 0; y < noiseMap.GetLength(1); y++)
            {
                for (int x = 0; x < noiseMap.GetLength(0); x++)
                {
                    // Define o tile com base no valor do noise
                    TileBase tile = DetermineTile(noiseMap[x, y]);

                    // Define a posição do tile no Tilemap
                    Vector3Int tilePosition = new Vector3Int(x + (int) offset.x, y + (int) offset.y, 0);

                    // Define o tile no Tilemap
                    tilemap.SetTile(tilePosition, tile);
                }
            }
            yield return new WaitForSeconds(0.1f);
            Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaa");
            AstarPath.active.Scan();
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
    }
}