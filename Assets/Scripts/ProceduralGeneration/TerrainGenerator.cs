using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Assets.Scripts.ProceduralGeneration.MapController;

public class TerrainGenerator
{

    private float[,] transitionBuilder(int widht, int height, int sett)
    {
        float[,] noiseTransition = new float[widht, height];
        for (int x = 0; x < height; x++)
        {
            for (int i = 0; i < widht; i++)
            {
                noiseTransition[i, x] = sett;
            }
        }
        return noiseTransition;
    }

    public float[,] buildNoiseMap(int seed)
    {
        NoiseGenerator noiseGenerator = new NoiseGenerator();

        float[,] cavenoise = noiseGenerator.GenerateFractalBrownianMotionMatrix(512,192, 10, 2, 1.0f, 1.0f, seed, Vector2.zero,1);
        float[,] poisoncavenoise = noiseGenerator.GenerateVoronoiNoiseMatrix(512, 192, 32, 600, seed,3);
        float[,] tundranoise = noiseGenerator.GeneratePerlinNoiseMatrix(512,192,13,1.5f, seed, 2);
        float[,] hellnoise = noiseGenerator.GeneratePerlinNoiseMatrix(512,192,15,1, seed, 4);
        float[,] voidnoise = noiseGenerator.GenerateVoronoiNoiseMatrix(512,192,38,1000, seed, 5);
        List<float[,]> noisemaps = new List<float[,]>();
        noisemaps.Add(cavenoise);
        noisemaps.Add(transitionBuilder(512,4,11));
        noisemaps.Add(tundranoise);
        noisemaps.Add(transitionBuilder(512,4,21));
        noisemaps.Add(poisoncavenoise);
        noisemaps.Add(transitionBuilder(512, 4, 31));
        noisemaps.Add(hellnoise);
        noisemaps.Add(transitionBuilder(512, 4,41));
        noisemaps.Add(voidnoise);
        noisemaps.Add(transitionBuilder(512, 4, 51));
        noisemaps.Add(transitionBuilder(512, 4, 51));
        noisemaps.Add(transitionBuilder(512, 4, 51));
        noisemaps.Add(transitionBuilder(512, 4, 51));
        return StackMatricesVertically(noisemaps);
    }

    private float[,] StackMatricesVertically(List<float[,]> matrices)
    {
        // Verifica se há matrizes na lista
        if (matrices == null || matrices.Count == 0)
        {
            return null; // ou lançar uma exceção, dependendo do seu caso de uso
        }

        // Verifica se todas as matrizes têm a mesma largura (primeira dimensão)
        int xDimension = matrices[0].GetLength(0); // Largura

        foreach (var matrix in matrices)
        {
            if (matrix.GetLength(0) != xDimension)
            {
                return null; // ou lançar uma exceção
            }
        }

        // Calcula a altura total da matriz resultante
        int resultHeight = matrices.Sum(matrix => matrix.GetLength(1));
        float[,] resultMatrix = new float[xDimension, resultHeight];

        // Empilha as matrizes verticalmente
        int currentY = 0;
        foreach (var matrix in matrices)
        {
            int matrixHeight = matrix.GetLength(1);

            // Copia cada matriz para a posição correta na matriz resultante
            for (int x = 0; x < xDimension; x++)
            {
                for (int y = 0; y < matrixHeight; y++)
                {
                    resultMatrix[x, currentY + y] = matrix[x, y];
                }
            }

            // Atualiza o índice da altura atual na matriz resultante
            currentY += matrixHeight;
        }

        return resultMatrix;
    }
}

