// TrackSegment.cs
using UnityEngine;

public class TrackSegment : MonoBehaviour
{
    // endPoint should be a child GameObject whose world position marks the start of the next segment.
    // Its local position should be at the "end" of this segment's geometry.
    public Transform endPoint;

    // Length of the segment along the Z-axis (forward direction).
    // This can be set manually in the prefab or calculated.
    public float length = 20f; // Default length, adjust as needed per prefab.

    [Header("Vehicle Spawning")]
    public bool spawnVehicles = true; // Whether this segment should spawn vehicles
    public Transform[] vehicleSpawnPoints; // Assign child GameObjects as spawn points in Inspector
    public Vector3 vehicleBaseMoveDirection = Vector3.forward; // Base direction, will be transformed by spawn point's orientation

    [Header("Coin Spawning")]
    public bool spawnCoins = true; // Whether this segment should spawn coins
    public Transform[] coinSpawnPoints; // Assign child GameObjects as spawn points for coins
    public string coinObjectTag = "Coin"; // Tag for ObjectPooler for coins

    void OnEnable()
    {
        // This method is called when the segment is activated (e.g., after being spawned from the pool)
        SpawnVehicles();
        SpawnCoins();
        Debug.Log("TrackSegment OnEnable processed for: " + gameObject.name, gameObject);
    }

    private void SpawnVehicles()
    {
        if (!spawnVehicles || vehicleSpawnPoints == null || vehicleSpawnPoints.Length == 0)
        {
            if (spawnVehicles)
                Debug.Log("TrackSegment: 'spawnVehicles' is true but no 'vehicleSpawnPoints' are assigned for " + gameObject.name, gameObject);
            return;
        }

        if (TrafficManager.Instance == null)
        {
            Debug.LogWarning("TrackSegment: TrafficManager not found. Cannot spawn vehicles for segment " + gameObject.name, gameObject);
            return;
        }

        foreach (Transform spawnPoint in vehicleSpawnPoints)
        {
            if (spawnPoint == null)
            {
                Debug.LogWarning("TrackSegment: A vehicle spawn point is null in segment " + gameObject.name, gameObject);
                continue;
            }

            Vector3 spawnPos = spawnPoint.position;
            Quaternion spawnRot = spawnPoint.rotation;
            Vector3 moveDir = spawnPoint.TransformDirection(vehicleBaseMoveDirection);

            TrafficManager.Instance.SpawnVehicle(spawnPos, spawnRot, moveDir);
        }
        // Debug.Log("TrackSegment: Attempted to spawn " + vehicleSpawnPoints.Length + " vehicles for " + gameObject.name, gameObject);
    }

    private void SpawnCoins()
    {
        if (!spawnCoins || coinSpawnPoints == null || coinSpawnPoints.Length == 0)
        {
            if (spawnCoins)
                Debug.Log("TrackSegment: 'spawnCoins' is true but no 'coinSpawnPoints' are assigned for " + gameObject.name, gameObject);
            return;
        }

        if (ObjectPooler.Instance == null)
        {
            Debug.LogWarning("TrackSegment: ObjectPooler not found. Cannot spawn coins for segment " + gameObject.name, gameObject);
            return;
        }

        foreach (Transform spawnPoint in coinSpawnPoints)
        {
            if (spawnPoint == null)
            {
                Debug.LogWarning("TrackSegment: A coin spawn point is null in segment " + gameObject.name, gameObject);
                continue;
            }
            // Coins typically don't need special initialization like movement direction from the track segment itself.
            // Their behavior is self-contained or set on their prefab.
            ObjectPooler.Instance.SpawnFromPool(coinObjectTag, spawnPoint.position, spawnPoint.rotation);
        }
        // Debug.Log("TrackSegment: Attempted to spawn " + coinSpawnPoints.Length + " coins for " + gameObject.name, gameObject);
    }

    void Awake()
    {
        if (endPoint == null)
        {
            Debug.LogError("TrackSegment: EndPoint transform is not assigned for " + gameObject.name +
                           ". Please assign a child GameObject to indicate the segment's end.", gameObject);
            // Fallback: create a dummy endpoint if one isn't assigned, assuming segment length is along Z.
            // This is not ideal as visual alignment might be off.
            GameObject generatedEndPoint = new GameObject("GeneratedEndPoint");
            generatedEndPoint.transform.SetParent(transform);
            generatedEndPoint.transform.localPosition = new Vector3(0, 0, length); // Assumes pivot is at start
            endPoint = generatedEndPoint.transform;
        }
        else
        {
            // If length is not manually set, try to derive it from endPoint's local Z position.
            // This assumes the segment's pivot is at its start (0,0,0 local) and endPoint is correctly placed.
            if (length == 0 && endPoint.transform.parent == this.transform) // Check if it's a direct child
            {
                length = endPoint.transform.localPosition.z;
                if (length <= 0)
                {
                    Debug.LogWarning("TrackSegment: Calculated length is zero or negative for " + gameObject.name +
                                     ". Ensure endPoint is correctly positioned along local Z axis or set length manually.", gameObject);
                }
            }
        }
         if (length <= 0)
        {
            Debug.LogError("TrackSegment: Length is not set or is invalid for " + gameObject.name + ". Please set a positive length.", gameObject);
        }
    }

    // Conceptual: OnBecameInvisible might be used for returning to pool.
    // However, for continuous runners, a manager (LevelGenerator) usually handles this
    // based on distance from the player for more reliable behavior.
    /*
    void OnBecameInvisible()
    {
        // This method is called by Unity when the Renderer is no longer visible by any camera.
        // It's not always reliable for pooling track segments in a fast-moving game,
        // as segments might become visible/invisible rapidly or despawn too early/late.
        // Debug.Log("TrackSegment " + gameObject.name + " became invisible. Consider returning to pool.");
        // If LevelGenerator is active, it should handle despawning.
        // if (LevelGenerator.Instance == null || !LevelGenerator.Instance.IsSegmentActive(gameObject))
        // {
        //    ObjectPooler.Instance.ReturnToPool("TrackSegment", gameObject); // Assuming "TrackSegment" is the tag
        // }
    }
    */

    // Helper to get the world position of the end point.
    public Vector3 GetWorldEndPointPosition()
    {
        if (endPoint != null)
        {
            return endPoint.position;
        }
        // Fallback if endPoint is somehow null after Awake, though Awake tries to prevent this.
        Debug.LogError("TrackSegment: GetWorldEndPointPosition called, but endPoint is null for " + gameObject.name, gameObject);
        return transform.position + (transform.forward * length); // Estimate based on length
    }
}
