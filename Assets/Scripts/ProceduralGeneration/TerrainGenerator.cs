using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.ProceduralGeneration.MapController;

public class TerrainGenerator
{

    public int width;
    public int height;
    public float scale;
    public float frequency = 0.1f;
    public int seed;
    public NoiseType noiseType;
    public int numCells;
    public int octaves;
    public float persistence;
    public float lacunarity;
    public Vector2 offset;

    public TerrainGenerator(int width, int height, float scale, float frequency, int seed, NoiseType noiseType, int numCells, int octaves, float persistence, float lacunarity, Vector2 offset)
    {
        this.width = width;
        this.height = height;
        this.scale = scale;
        this.frequency = frequency;
        this.seed = seed;
        this.noiseType = noiseType;
        this.numCells = numCells;
        this.octaves = octaves;
        this.persistence = persistence;
        this.lacunarity = lacunarity;
        this.offset = offset;
    }

    public float[,] GenerateTerrain()
    {
        NoiseGenerator noiseGenerator = new NoiseGenerator();
        switch (noiseType)
        {
            case NoiseType.Voronoi:
                return noiseGenerator.GenerateVoronoiNoiseMatrix(width, height, scale, numCells, seed);
            case NoiseType.Worley:
                return noiseGenerator.GenerateWorleyNoiseMatrix(width, height, scale, numCells, seed);
            case NoiseType.FBM:
                return noiseGenerator.GenerateFractalBrownianMotionMatrix(width, height, scale, octaves, persistence, lacunarity, seed, offset);
        }
            return noiseGenerator.GeneratePerlinNoiseMatrix(width, height, scale, frequency, seed);
        }
}

