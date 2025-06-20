// TrafficManager.cs
using UnityEngine;

public class TrafficManager : MonoBehaviour
{
    public static TrafficManager Instance;

    [Header("Pooling Settings")]
    public string vehicleObjectTag = "Vehicle"; // Tag used in ObjectPooler for vehicle prefabs

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Optional: if TrafficManager should persist
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject SpawnVehicle(Vector3 position, Quaternion rotation, Vector3 moveDirection)
    {
        if (ObjectPooler.Instance == null)
        {
            Debug.LogError("TrafficManager: ObjectPooler not found in scene. Cannot spawn vehicle.");
            return null;
        }

        GameObject vehicleGO = ObjectPooler.Instance.SpawnFromPool(vehicleObjectTag, position, rotation);

        if (vehicleGO != null)
        {
            VehicleAI ai = vehicleGO.GetComponent<VehicleAI>();
            if (ai != null)
            {
                ai.Initialize(moveDirection);
            }
            else
            {
                Debug.LogError("TrafficManager: Spawned vehicle prefab with tag '" + vehicleObjectTag +
                               "' does not have a VehicleAI component. Returning to pool.", vehicleGO);
                ObjectPooler.Instance.ReturnToPool(vehicleObjectTag, vehicleGO); // Return problematic object
                return null;
            }
        }
        else
        {
            Debug.LogWarning("TrafficManager: Failed to spawn vehicle from pool with tag '" + vehicleObjectTag +
                             "'. Check pool configuration and availability.");
            // This warning is important. It means either the pool is empty and not set to expand,
            // or the tag is incorrect, or the prefab for that tag is null in ObjectPooler.
        }
        return vehicleGO;
    }

    // Future enhancements for TrafficManager:
    // - Manage different types of vehicles (e.g., cars, trucks) with different prefabs/tags.
    // - Control spawn rates and densities based on game difficulty or specific zones.
    // - Define lanes for vehicles and manage their movement within those lanes.
    // - Handle more complex behaviors like vehicles changing lanes or reacting to player.
}
