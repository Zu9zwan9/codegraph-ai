// LevelGenerator.cs
using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;

    [Header("Pooler Settings")]
    public string trackSegmentTag = "TrackSegment"; // Tag for ObjectPooler

    [Header("Generation Settings")]
    public int initialSegments = 5; // Number of segments to spawn at the start
    public float segmentSpawnLookAhead = 40f; // How far ahead to spawn the next segment relative to player
    public float segmentDespawnOffset = 20f; // How far behind the player a segment should be before despawning
    // public float defaultSegmentLength = 20f; // Fallback if TrackSegment.length is 0, remove if TrackSegment always has length

    [Header("Player Reference")]
    public Transform playerTransform;

    private List<GameObject> activeSegments = new List<GameObject>();
    private Vector3 nextSpawnPoint = Vector3.zero;
    private float lastPlayerZ = 0f; // To track player's forward movement for spawning

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Optional
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (playerTransform == null)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogError("LevelGenerator: PlayerTransform not assigned and PlayerController not found in scene! Level generation will not work.");
                this.enabled = false; // Disable script if no player
                return;
            }
        }

        if (ObjectPooler.Instance == null)
        {
             Debug.LogError("LevelGenerator: ObjectPooler not found in scene! Level generation requires an active ObjectPooler.");
             this.enabled = false;
             return;
        }

        // Initialize nextSpawnPoint based on player's starting position if desired, or keep at world zero.
        // If player starts at (0,0,0), nextSpawnPoint can be (0,0,0).
        // If player starts elsewhere, adjust accordingly or ensure first segment is placed at player's start.
        nextSpawnPoint = playerTransform.position;
        // To avoid spawning the first segment *on* the player, you might adjust nextSpawnPoint:
        // nextSpawnPoint = playerTransform.position - (playerTransform.forward * someOffsetToStartBehindPlayer);
        // For simplicity, assuming player starts at origin and first segment spawns from there.

        lastPlayerZ = playerTransform.position.z;

        for (int i = 0; i < initialSegments; i++)
        {
            SpawnSegment();
            if(activeSegments.Count == 0 && i == 0) // Check if first spawn failed
            {
                 Debug.LogError("LevelGenerator: First segment failed to spawn. Aborting initial generation.");
                 this.enabled = false;
                 return;
            }
        }
    }

    void Update()
    {
        if (playerTransform == null || ObjectPooler.Instance == null || !this.enabled) return;

        // Check if player has moved enough to warrant spawning a new segment
        // This is a simple way; a more robust way might involve checking distance to `nextSpawnPoint`.
        if (playerTransform.position.z + segmentSpawnLookAhead > nextSpawnPoint.z)
        {
            SpawnSegment();
        }

        DespawnSegments();
        lastPlayerZ = playerTransform.position.z; // Update last player Z for next frame (not strictly needed with current logic but good for other approaches)
    }

    void SpawnSegment()
    {
        GameObject segmentGO = ObjectPooler.Instance.SpawnFromPool(trackSegmentTag, nextSpawnPoint, Quaternion.identity);
        if (segmentGO == null)
        {
            Debug.LogError("LevelGenerator: Failed to spawn segment from pool. Tag: " + trackSegmentTag + ". Check ObjectPooler configuration and prefab availability.");
            return;
        }

        TrackSegment segment = segmentGO.GetComponent<TrackSegment>();
        if (segment == null)
        {
            Debug.LogError("LevelGenerator: Spawned object with tag '" + trackSegmentTag + "' does not have a TrackSegment component.", segmentGO);
            ObjectPooler.Instance.ReturnToPool(trackSegmentTag, segmentGO); // Return invalid segment
            return;
        }

        if (segment.length <= 0)
        {
             Debug.LogWarning("LevelGenerator: Spawned segment " + segmentGO.name + " has length 0 or less. Using a default length assumption for next spawn point calculation. Check segment prefab.", segmentGO);
             // nextSpawnPoint += segmentGO.transform.forward * defaultSegmentLength; // Use default if length is bad
        }
        // The TrackSegment's endPoint should be in its local space, correctly positioned.
        // When the segment is placed at nextSpawnPoint (world space), its endPoint.position gives the new world space spawn point.
        nextSpawnPoint = segment.GetWorldEndPointPosition();

        activeSegments.Add(segmentGO);
    }

    void DespawnSegments()
    {
        // Iterate backwards to allow removal from list during iteration
        for (int i = activeSegments.Count - 1; i >= 0; i--)
        {
            GameObject segmentGO = activeSegments[i];
            if (segmentGO.transform.position.z < playerTransform.position.z - segmentDespawnOffset)
            {
                ObjectPooler.Instance.ReturnToPool(trackSegmentTag, segmentGO);
                activeSegments.RemoveAt(i);
            }
        }
    }

    // Helper for TrackSegment to know if it's managed by LevelGenerator (not currently used by TrackSegment)
    // public bool IsSegmentActive(GameObject segmentGO)
    // {
    //     return activeSegments.Contains(segmentGO);
    // }
}
