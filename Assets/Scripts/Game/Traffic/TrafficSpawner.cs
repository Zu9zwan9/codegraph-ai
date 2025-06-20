// TrafficSpawner.cs
using UnityEngine;
using System.Collections; // Required for IEnumerator

public class TrafficSpawner : MonoBehaviour
{
    public GameObject vehiclePrefab; // Assign your Vehicle Prefab in the Inspector
    public Transform[] spawnPoints;   // Assign spawn point Transforms in the Inspector
    public float spawnInterval = 2.0f; // Time between spawns

    void Start()
    {
        if (vehiclePrefab == null)
        {
            Debug.LogError("Vehicle Prefab not assigned in TrafficSpawner.");
            return;
        }
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Spawn points not assigned in TrafficSpawner.");
            return;
        }

        StartSpawning();
    }

    public void StartSpawning()
    {
        // Start a coroutine to spawn vehicles at regular intervals
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true) // Infinite loop to keep spawning
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnVehicle();
        }
    }

    public void SpawnVehicle()
    {
        if (spawnPoints.Length == 0) return;

        // Select a random spawn point
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedSpawnPoint = spawnPoints[spawnPointIndex];

        // Placeholder for instantiation and setup
        // GameObject newVehicle = Instantiate(vehiclePrefab, selectedSpawnPoint.position, selectedSpawnPoint.rotation);

        // Potentially set properties on the newVehicle's VehicleAI component
        // VehicleAI ai = newVehicle.GetComponent<VehicleAI>();
        // if (ai != null)
        // {
        //     // e.g., ai.moveSpeed = Random.Range(2.0f, 5.0f);
        // }

        Debug.Log("SpawnVehicle called. Would spawn at: " + selectedSpawnPoint.name);
    }
}
