// Obstacle.cs
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Tag used for identification, especially if not relying on Unity's tag system for this.
    // Could be useful if you have many types of obstacles managed by different systems.
    public string obstacleTypeTag = "Obstacle";

    // Optional: Enum for more specific obstacle types if they have different behaviors
    public enum ObstacleCategory
    {
        StaticBarrier,  // Simple, non-moving barrier
        MovingObstacle, // e.g., a gate opening/closing
        Breakable,      // Can be destroyed by player (maybe with a power-up)
        Hazard          // e.g., a pit or a laser that causes instant game over
    }
    public ObstacleCategory category = ObstacleCategory.StaticBarrier;

    // This script serves as a marker for objects that are considered obstacles.
    // It could be expanded to include:
    // - Health (if breakable)
    // - Movement patterns (if dynamic)
    // - Points awarded/penalized on interaction
    // - Specific sound effects or particle effects on collision

    void Start()
    {
        // Example: if it's a moving obstacle, it might start its movement pattern here.
        // if (category == ObstacleCategory.MovingObstacle) { /* InitializeMovement(); */ }
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     // While PlayerController handles its own collision with obstacles,
    //     // the obstacle itself might react too (e.g., play a sound, break apart).
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("Obstacle: Player collided with me (" + gameObject.name + ")");
    //         // Play a sound effect
    //         // Trigger a particle effect
    //         // If breakable and player has powerup, Destroy(gameObject) or return to pool
    //     }
    // }

    // If this obstacle is pooled, this method could be part of IPooledObject interface
    // public void OnObjectSpawn()
    // {
    //     // Reset any state if it's being reused from a pool
    //     // e.g., if it was breakable and broken, restore its state.
    // }

    // public void OnObjectReturn()
    // {
    //     // Any cleanup before returning to pool
    // }
}
