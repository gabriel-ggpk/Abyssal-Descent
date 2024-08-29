using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Assets.Scripts.ProceduralGeneration.MapController;

public class TerrainGenerator
{

    public float[,] buildNoiseMap(int seed)
    {
        NoiseGenerator noiseGenerator = new NoiseGenerator();

        float[,] cavenoise = noiseGenerator.GenerateFractalBrownianMotionMatrix(500,300, 20, 2, 1.0f, 1.0f, seed, Vector2.zero);
        float[,] poisoncavenoise = noiseGenerator.GenerateVoronoiNoiseMatrix(500, 300, 32, 600, seed);
        //float[,] tundranoise = noiseGenerator.GeneratePerlinNoiseMatrix(500,300,13,1.5f, seed);
        //float[,] hellnoise = noiseGenerator.GeneratePerlinNoiseMatrix(500,300,15,1, seed);
        //float[,] voidnoise = noiseGenerator.GenerateVoronoiNoiseMatrix(500,300,38,1000, seed);
        List<float[,]> noisemaps = new List<float[,]>();
        float[,] noiseTransition = new float[500, 6];
        for (int x = 0; x < 6; x++)
        {
            for (int i = 0; i < 500; i++)
            {
                noiseTransition[i, x] = 255f;
            }
        }
        //noisemaps.Add(noiseTransition);
        //noisemaps.Add(voidnoise);
        //noisemaps.Add(noiseTransition);
        //noisemaps.Add(hellnoise);
        //noisemaps.Add(noiseTransition);
        //noisemaps.Add(tundranoise);
        //noisemaps.Add(noiseTransition);
        noisemaps.Add(poisoncavenoise);
        noisemaps.Add(noiseTransition);
        noisemaps.Add(cavenoise);
        //noisemaps.Add(noiseTransition);
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

