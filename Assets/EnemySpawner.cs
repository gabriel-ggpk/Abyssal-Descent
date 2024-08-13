using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EnemySpawner : MonoBehaviour
{
    public Grid grid;

    [SerializeField] GameObject EnemyPF;

    [SerializeField] float spawnTime = 4f;

    void Start()
    {
        StartCoroutine(Spawn(spawnTime));
    }

    private IEnumerator Spawn(float timer)
    {
        yield return new WaitForSeconds(timer);
        Vector3 spawnPos = GetRandomSpawnPosition();

        // Keep finding a spawn position until a valid one is found
        while (!CanSpawn(spawnPos))
        {
            spawnPos = GetRandomSpawnPosition();
        }

        // Spawn the enemy
        GameObject newEnemy = Instantiate(EnemyPF, spawnPos, Quaternion.identity);
        StartCoroutine(Spawn(timer * 0.95f));
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Generate a random position within the grid bounds and snap it to the grid
        Vector3 randomPosition = new Vector3(Random.Range(-14, 20), Random.Range(-5, 10), 0);
        Vector3Int cellPosition = grid.WorldToCell(randomPosition);
        return grid.GetCellCenterWorld(cellPosition); // Snap the position to the center of the grid cell
    }

    public bool CanSpawn(Vector3 worldPosition)
    {
        Vector3Int cellPosition = grid.WorldToCell(worldPosition);
        Vector3Int cellBelow = grid.WorldToCell(worldPosition + Vector3.down);

        // Check if the current position is empty and the position below has a tile
        foreach (Tilemap tilemap in grid.GetComponentsInChildren<Tilemap>())
        {
            bool isCurrentTileEmpty = !tilemap.HasTile(cellPosition);
            bool isTileBelowPresent = tilemap.HasTile(cellBelow);

            if (isCurrentTileEmpty && isTileBelowPresent)
            {
                return true; // The current tile is empty and there's a tile below, safe to spawn
            }
        }

        return false; // The conditions are not met, so we cannot spawn here
    }
}