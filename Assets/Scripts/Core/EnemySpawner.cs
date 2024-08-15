using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public Grid grid;
    public Transform player; // Reference to the player's transform

    [SerializeField] GameObject EnemyPF;
    [SerializeField] float spawnTime = 3f;
    [SerializeField] float minSpawnRadius = 3f; // Minimum distance from the player
    [SerializeField] float maxSpawnRadius = 10f; // Maximum distance from the player

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
        StartCoroutine(Spawn(timer ));
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Generate a random angle and distance within the defined range
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(minSpawnRadius, maxSpawnRadius);

        // Convert polar coordinates to Cartesian coordinates
        Vector3 spawnOffset = new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0);

        // Calculate the spawn position relative to the player
        Vector3 randomPosition = player.position + spawnOffset;

        // Snap the position to the nearest grid cell
        Vector3Int cellPosition = grid.WorldToCell(randomPosition);
        return grid.GetCellCenterWorld(cellPosition); // Snap to the center of the grid cell
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
