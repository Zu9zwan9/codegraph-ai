// ObjectPooler.cs
using UnityEngine;
using System.Collections.Generic; // Required for List and Queue

// Serializable class to define items to be pooled.
// This would be configured in the Unity Inspector.
[System.Serializable]
public class ObjectPoolerItem
{
    public string tag;
    public GameObject prefab;
    public int size;
    public bool expandPool = false; // Optional: allow pool to grow if empty
}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    public List<ObjectPoolerItem> itemsToPool;
    private Dictionary<string, Queue<GameObject>> pooledObjects;
    private Dictionary<string, GameObject> poolParents; // To keep the hierarchy clean

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Optional: if pooler should persist across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        pooledObjects = new Dictionary<string, Queue<GameObject>>();
        poolParents = new Dictionary<string, GameObject>();
    }

    void Start()
    {
        // This is where prefabs would be dragged in via the Unity Inspector.
        // For this subtask, itemsToPool will be empty unless configured externally.
        // Add a log if no items are configured to remind the user.
        if (itemsToPool == null || itemsToPool.Count == 0)
        {
            Debug.LogWarning("ObjectPooler: No itemsToPool configured. The object pooler will be empty. Please configure items in the Inspector.");
            return;
        }

        foreach (ObjectPoolerItem item in itemsToPool)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();

            // Create a parent GameObject for this pool to keep the hierarchy organized
            GameObject parentObject = new GameObject(item.tag + " Pool");
            parentObject.transform.SetParent(this.transform); // Parent to the ObjectPooler itself
            poolParents[item.tag] = parentObject;

            for (int i = 0; i < item.size; i++)
            {
                if (item.prefab == null)
                {
                    Debug.LogError($"ObjectPooler: Prefab for tag '{item.tag}' is null. Skipping instantiation for this item.");
                    continue; // Skip this item if prefab is null
                }
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(parentObject.transform); // Parent to the specific pool's parent
                objectQueue.Enqueue(obj);
            }
            pooledObjects.Add(item.tag, objectQueue);
            Debug.Log($"ObjectPooler: Pooled {item.size} objects for tag '{item.tag}'");
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!pooledObjects.ContainsKey(tag))
        {
            Debug.LogWarning($"ObjectPooler: Pool with tag '{tag}' doesn't exist.");
            return null;
        }

        if (pooledObjects[tag].Count > 0)
        {
            GameObject objectToSpawn = pooledObjects[tag].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            // Optional: Call an interface method like IPooledObject.OnObjectSpawn()
            // IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
            // if (pooledObj != null) pooledObj.OnObjectSpawn();

            return objectToSpawn;
        }
        else
        {
            // Check if this pool can expand
            ObjectPoolerItem item = itemsToPool.Find(i => i.tag == tag);
            if (item != null && item.expandPool)
            {
                if (item.prefab == null)
                {
                    Debug.LogError($"ObjectPooler: Prefab for tag '{tag}' is null. Cannot expand pool.");
                    return null;
                }
                Debug.LogWarning($"ObjectPooler: Pool with tag '{tag}' is empty but set to expand. Creating new object.");
                GameObject obj = Instantiate(item.prefab);
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                // Ensure it's parented correctly if it's a new object not from the queue
                if (poolParents.TryGetValue(tag, out GameObject parent))
                {
                    obj.transform.SetParent(parent.transform);
                }
                // IPooledObject pooledObj = obj.GetComponent<IPooledObject>();
                // if (pooledObj != null) pooledObj.OnObjectSpawn(); // Call spawn method if it's a new object
                return obj; // Return the newly created object (it's not added to the queue here)
            }
            else
            {
                Debug.LogWarning($"ObjectPooler: Pool with tag '{tag}' is empty and not set to expand.");
                return null;
            }
        }
    }

    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (!pooledObjects.ContainsKey(tag))
        {
            Debug.LogWarning($"ObjectPooler: Pool with tag '{tag}' doesn't exist. Cannot return object. Destroying it instead.");
            Destroy(objectToReturn); // Or handle differently
            return;
        }

        // Optional: Call an interface method like IPooledObject.OnObjectReturn()
        // IPooledObject pooledObj = objectToReturn.GetComponent<IPooledObject>();
        // if (pooledObj != null) pooledObj.OnObjectReturn();

        objectToReturn.SetActive(false);
        pooledObjects[tag].Enqueue(objectToReturn);
    }
}

// Optional Interface for pooled objects to listen to spawn/return events
// public interface IPooledObject
// {
//    void OnObjectSpawn();
//    void OnObjectReturn();
// }
