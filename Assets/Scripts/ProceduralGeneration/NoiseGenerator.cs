using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator
{
    // M�todo para converter o valor em 0 ou 1 com base no threshold de 128f
    private float ApplyThreshold(float value, int sett)
    {
        return value >= 128f ? sett : 999;
    }

    private float[,] SetRandomPowerUp(float[,] noiseMap)
    {
        int widthTenth = noiseMap.GetLength(0) / 10;
        int heightTenth = noiseMap.GetLength(1) / 10;

        System.Random random = new System.Random();
        int randomX = random.Next(widthTenth, widthTenth * 9);
        int randomY = random.Next(heightTenth, heightTenth * 9);

        noiseMap[randomX, randomY] = 99;
        return noiseMap;
    }

    public float[,] GeneratePerlinNoiseMatrix(int width, int height, float scale, float frequency, int seed, int sett)
    {
        float[,] noiseMatrix = new float[width, height];
        System.Random random = new System.Random(seed);

        // Gerar offset aleat�rio baseado no seed para cada eixo
        float offsetX = random.Next(-100000, 100000);
        float offsetY = random.Next(-100000, 100000);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Incorporar o offset ao c�lculo do Perlin Noise para torn�-lo reprodut�vel
                float xCoord = (x + offsetX) / scale * frequency;
                float yCoord = (y + offsetY) / scale * frequency;
                float perlinValue = Mathf.PerlinNoise(xCoord, yCoord) * 255f;
                noiseMatrix[x, y] = ApplyThreshold(perlinValue, sett);
                
            }
        }

        return SetRandomPowerUp(noiseMatrix);
    }

    // Voronoi
    public float[,] GenerateVoronoiNoiseMatrix(int width, int height, float scale, int numCells, int seed, int sett)
    {
        float[,] map = new float[width, height];
        System.Random random = new System.Random(seed);

        // Gerar centros das c�lulas Voronoi
        Vector2[] cellCenters = new Vector2[numCells];
        for (int i = 0; i < numCells; i++)
        {
            float x = (float)random.NextDouble() * width;
            float y = (float)random.NextDouble() * height;
            cellCenters[i] = new Vector2(x, y);
        }

        // Gerar matriz com base no algoritmo de Voronoi
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 point = new Vector2(x, y);
                float minDistance = float.MaxValue;

                // Encontrar a c�lula numCells mais pr�xima
                for (int i = 0; i < numCells; i++)
                {
                    float distance = Vector2.Distance(point, cellCenters[i]);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                    }
                }

                // Usar a dist�ncia para calcular o valor na matriz (entre 0 e 255)
                float value = Mathf.InverseLerp(0, width / scale, minDistance) * 255f;
                map[x, y] = ApplyThreshold(value,  sett);
            }
        }

        return SetRandomPowerUp(map);
    }

    // Worley
    public float[,] GenerateWorleyNoiseMatrix(int width, int height, float scale, int numCells, int seed, int sett)
    {
        float[,] map = new float[width, height];
        System.Random random = new System.Random(seed);

        // Gerar centros das c�lulas Worley
        Vector2[] cellCenters = new Vector2[numCells];
        for (int i = 0; i < numCells; i++)
        {
            float x = (float)random.NextDouble() * width;
            float y = (float)random.NextDouble() * height;
            cellCenters[i] = new Vector2(x, y);
        }

        // Gerar matriz com base no algoritmo Worley
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 point = new Vector2(x, y);
                float minDistance = float.MaxValue;

                // Encontrar a dist�ncia para o centro de c�lula mais pr�ximo
                for (int i = 0; i < numCells; i++)
                {
                    float distance = Vector2.Distance(point, cellCenters[i]);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                    }
                }

                // Normalizar e escalar o valor da dist�ncia para o intervalo de 0 a 255
                float value = Mathf.InverseLerp(0, width / scale, minDistance) * 255f;
                map[x, y] = ApplyThreshold(value,  sett);
            }
        }

        return SetRandomPowerUp(map);
    }

    // Fractal Brownian Motion (fBM)
    public float[,] GenerateFractalBrownianMotionMatrix(int width, int height, float scale, int octaves, float persistence, float lacunarity, int seed, Vector2 offset, int sett)
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

                float normalizedHeight = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseHeight) * 255f;
                noiseMap[x, y] = ApplyThreshold(normalizedHeight, sett);
            }
        }

        return SetRandomPowerUp(noiseMap);
    }
}
