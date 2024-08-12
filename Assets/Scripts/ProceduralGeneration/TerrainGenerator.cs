using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    public int mapWidth = 100;
    public int mapHeight = 50;
    public float heightMultiplier = 10f;
    public float frequency = 0.1f;

    [Header("Noise Settings")]
    public NoiseType noiseType = NoiseType.Perlin;
    public float noiseScale = 20f;

    private float[,] heightMap;

    public enum NoiseType { Perlin, Simplex }

    void Start()
    {
        GenerateTerrain();
        GetComponent<MapRenderer>().RenderMap(heightMap);
    }

    void GenerateTerrain()
    {
        heightMap = new float[mapWidth, mapHeight];
        NoiseGenerator noiseGenerator = new NoiseGenerator();

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float noiseValue = 0f;
                switch (noiseType)
                {
                    case NoiseType.Perlin:
                        noiseValue = noiseGenerator.GeneratePerlinNoise(x, y, noiseScale, frequency);
                        break;
                    case NoiseType.Simplex:
                        noiseValue = noiseGenerator.GenerateSimplexNoise(x, y, noiseScale, frequency);
                        break;
                }
                heightMap[x, y] = Mathf.Clamp(noiseValue * heightMultiplier, 0, mapHeight);
            }
        }
    }
}

