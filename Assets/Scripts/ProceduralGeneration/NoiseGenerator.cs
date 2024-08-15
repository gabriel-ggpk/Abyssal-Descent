using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator
{
    public float[,] GeneratePerlinNoiseMatrix(int width, int height, float scale, float frequency, int seed)
    {
        float[,] noiseMatrix = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (x + seed) * scale * frequency;
                float yCoord = (y + seed) * scale * frequency;
                noiseMatrix[x, y] = Mathf.PerlinNoise(xCoord, yCoord) * 255f;
            }
        }

        return noiseMatrix;
    }

    // Voronoi
    public float[,] GenerateVoronoiNoiseMatrix(int width, int height, float scale, int numCells, int seed)
    {
        float[,] noiseMatrix = new float[width, height];
        Random.InitState(seed);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float minDist = float.MaxValue;

                for (int i = 0; i < numCells; i++)
                {
                    float cellX = Random.Range(0f, 1f);
                    float cellY = Random.Range(0f, 1f);
                    float dist = Vector2.Distance(new Vector2(x, y) * scale, new Vector2(cellX, cellY));
                    if (dist < minDist)
                    {
                        minDist = dist;
                    }
                }

                // Normaliza a distância mínima para o intervalo [0, 1] e depois escala para [0, 100]
                noiseMatrix[x, y] = Mathf.InverseLerp(0, minDist, minDist) * 255f;
            }
        }

        return noiseMatrix;
    }

    // Worley
    public float[,] GenerateWorleyNoiseMatrix(int width, int height, float scale, int numCells, int seed)
    {
        float[,] noiseMatrix = new float[width, height];
        Random.InitState(seed);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float minDist = float.MaxValue;
                float secondMinDist = float.MaxValue;

                for (int i = 0; i < numCells; i++)
                {
                    float cellX = Random.Range(0f, 1f);
                    float cellY = Random.Range(0f, 1f);
                    float dist = Vector2.Distance(new Vector2(x, y) * scale, new Vector2(cellX, cellY));

                    if (dist < minDist)
                    {
                        secondMinDist = minDist;
                        minDist = dist;
                    }
                    else if (dist < secondMinDist)
                    {
                        secondMinDist = dist;
                    }
                }

                // Normaliza a diferença das distâncias para o intervalo [0, 1] e depois escala para [0, 100]
                noiseMatrix[x, y] = Mathf.InverseLerp(0, secondMinDist - minDist, secondMinDist - minDist) * 255f;
            }
        }

        return noiseMatrix;
    }

    // Fractal Brownian Motion (fBM)
    public float[,] GenerateFractalBrownianMotionMatrix(int width, int height, float scale, int octaves, float persistence, float lacunarity, int seed, Vector2 offset)
    {
        {
            float[,] noiseMap = new float[width, height];

            System.Random prng = new System.Random(seed);
            Vector2[] octaveOffsets = new Vector2[octaves];

            for (int i = 0; i < octaves; i++)
            {
                float offsetX = prng.Next(-100000, 100000) + offset.x;
                float offsetY = prng.Next(-100000, 100000) + offset.y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            if (scale <= 0)
            {
                scale = 0.0001f;
            }

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            float halfWidth = width / 2f;
            float halfHeight = height / 2f;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for (int i = 0; i < octaves; i++)
                    {
                        float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
                        float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;

                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistence;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > maxNoiseHeight)
                    {
                        maxNoiseHeight = noiseHeight;
                    }
                    else if (noiseHeight < minNoiseHeight)
                    {
                        minNoiseHeight = noiseHeight;
                    }

                    noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseHeight) * 255;
                }
            }
            Debug.Log(noiseMap.ToString());
            return noiseMap;
        }
    }
}
