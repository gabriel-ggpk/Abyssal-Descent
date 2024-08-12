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
                noiseMatrix[x, y] = Mathf.PerlinNoise(xCoord, yCoord);
            }
        }

        return noiseMatrix;
    }


    //Voronoi
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

                noiseMatrix[x, y] = minDist;
            }
        }

        return noiseMatrix;
    }


    public float[,] GenerateWorleyNoiseMatrix(int width, int height, float scale, int seed)
    {
        float[,] noiseMatrix = new float[width, height];
        Random.InitState(seed);
        int numCells = 5;

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

                noiseMatrix[x, y] = secondMinDist - minDist;
            }
        }

        return noiseMatrix;
    }


    public float[,] GenerateFractalBrownianMotionMatrix(int width, int height, float scale, int octaves, float persistence, float lacunarity, int seed)
    {
        float[,] noiseMatrix = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float total = 0;
                float frequency = 1;
                float amplitude = 1;
                float maxValue = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = (x + seed) * scale * frequency;
                    float yCoord = (y + seed) * scale * frequency;
                    total += Mathf.PerlinNoise(xCoord, yCoord) * amplitude;

                    maxValue += amplitude;
                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                noiseMatrix[x, y] = total / maxValue;
            }
        }

        return noiseMatrix;
    }



}

