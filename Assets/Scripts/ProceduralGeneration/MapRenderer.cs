using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform mapParent;

    public void RenderMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x, heightMap[x, y], y);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                tile.transform.SetParent(mapParent);
            }
        }
    }
}

