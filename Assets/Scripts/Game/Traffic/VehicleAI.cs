// VehicleAI.cs
using UnityEngine;

public class VehicleAI : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 _moveDirection = Vector3.forward; // Default move direction

    // Fixed Z coordinate for despawning (since playerTransform is not easily accessible here)
    private float despawnZBoundary = -20f;
    private string vehiclePoolTag = "Vehicle"; // Tag used for ObjectPooler

    void OnEnable()
    {
        // Called when SetActive(true) by the pooler.
        // Basic re-initialization can happen here if needed,
        // but specific setup like moveDirection is handled by Initialize().
        // For example, if the vehicle had health, it could be reset here.
        Debug.Log("VehicleAI OnEnable: " + gameObject.name);
        // Ensure the object has the "Vehicle" tag, vital for player collision.
        if (!gameObject.CompareTag(vehiclePoolTag))
        {
            Debug.LogWarning("VehicleAI: GameObject " + gameObject.name +
                             " is not tagged as '" + vehiclePoolTag +
                             "'. Player collision might not work as expected. Please set the tag on the prefab.", gameObject);
        }
    }

    public void Initialize(Vector3 moveDirection)
    {
        _moveDirection = moveDirection.normalized;
        // Optional: Randomize speed slightly if desired
        // speed = Random.Range(8f, 12f);
    }

    void Update()
    {
        // Move the vehicle
        transform.Translate(_moveDirection * speed * Time.deltaTime, Space.World); // Move in world space based on initialized direction

        // Check for despawn boundary
        if (transform.position.z < despawnZBoundary)
        {
            if (ObjectPooler.Instance != null)
            {
                ObjectPooler.Instance.ReturnToPool(vehiclePoolTag, gameObject);
            }
            else
            {
                // Fallback if pooler is gone, though this shouldn't happen in a structured setup
                Debug.LogWarning("VehicleAI: ObjectPooler not found. Destroying vehicle instead of returning to pool.");
                Destroy(gameObject);
            }
        }
    }

    // Optional: Collision handling if vehicles interact with each other or environment elements
    // void OnCollisionEnter(Collision collision)
    // {
    //     // Example: if a vehicle hits an obstacle that's not the player
    //     if (collision.gameObject.CompareTag("Obstacle"))
    //     {
    //         // Play sound, particle effect, or return to pool
    //         // ObjectPooler.Instance.ReturnToPool(vehiclePoolTag, gameObject);
    //     }
    // }
}
