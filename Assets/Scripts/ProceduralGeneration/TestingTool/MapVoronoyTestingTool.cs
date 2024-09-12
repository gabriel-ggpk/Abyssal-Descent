using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.ProceduralGeneration
{
    public class MapVoronoyTestingTool : MonoBehaviour
    {

        [Header("Voronoy noise settings")]
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private int scale;
        [SerializeField] private int numCells;
        [SerializeField] private int seed;

        [Header("Render settings")]
        [SerializeField] private bool regen = false;
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileBase tileMapHighNoiseTile;
        [SerializeField] private TileBase tileMapLowNoiseTile;
        [Range(0, 255)]
        [SerializeField] private float thresHold;
        [SerializeField] private Vector2 offset;
        [SerializeField] private bool inverseRender = false;



        void Start()
        {
            BuildMap();
        }
        private void Update()
        {
            if (regen)
            {
                BuildMap();
                regen = false;
            }

        }
  
        private void BuildMap()
        {
            NoiseGenerator noiseGenerator = new NoiseGenerator();
            float[,] noiseMap = noiseGenerator.GenerateVoronoiNoiseMatrix(width, height, scale, numCells, seed, 1);

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
    }
}