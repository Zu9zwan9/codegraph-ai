// Coin.cs
using UnityEngine;

// Ensure a Collider is attached, and it's set to be a trigger.
// This can also be done in the Inspector.
[RequireComponent(typeof(Collider))]
public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    public string coinPoolTag = "Coin"; // Tag used for ObjectPooler

    // Placeholder for sound effect - actual playback would be handled by an AudioManager
    // public AudioClip collectSound;

    private bool collected = false; // To prevent double collection if OnTriggerEnter is slow or called multiple times

    void Awake()
    {
        // Ensure the collider is set to be a trigger, as this is crucial for OnTriggerEnter.
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            // col.isTrigger = true; // This line can cause issues if called on certain collider types at runtime after physics sim starts.
            // Best practice is to set this in the prefab inspector.
            Debug.LogWarning("Coin: Collider on " + gameObject.name +
                             " is not set to 'Is Trigger = true'. Please set it in the prefab Inspector for collection to work.", gameObject);
        }
        else if (col == null)
        {
            Debug.LogError("Coin: No Collider found on " + gameObject.name +
                           ". A trigger collider is required for collection.", gameObject);
        }
    }

    void OnEnable()
    {
        // Reset collected state when reused from pool
        collected = false;
        Debug.Log("Coin OnEnable: " + gameObject.name + " is now active. Value: " + coinValue);
        // Ensure the coin itself has a distinguishable tag if needed for other systems,
        // but for player collision, the player's tag is what's checked by other.CompareTag("Player").
        // For example, if obstacles could also destroy coins:
        // if (!gameObject.CompareTag(coinPoolTag))
        // {
        //     Debug.LogWarning("Coin: Object " + gameObject.name + " should ideally be tagged as '" + coinPoolTag + "'.", gameObject);
        // }
    }

    void OnTriggerEnter(Collider other)
    {
        if (collected) return; // Already collected, do nothing

        if (other.gameObject.CompareTag("Player"))
        {
            collected = true; // Mark as collected immediately

            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.CollectCoin(coinValue);
            }
            else
            {
                Debug.LogError("Coin: ScoreManager.Instance is null. Cannot report coin collection.");
            }

            // Conceptual: Play collectSound using an AudioManager
            // if (AudioManager.Instance != null && collectSound != null)
            // {
            //     AudioManager.Instance.PlaySoundEffect(collectSound);
            // }
            // else
            // {
            //     Debug.Log("Coin collected sound placeholder for " + gameObject.name);
            // }
            Debug.Log("Coin collected sound placeholder for " + gameObject.name);


            if (ObjectPooler.Instance != null)
            {
                ObjectPooler.Instance.ReturnToPool(coinPoolTag, gameObject);
            }
            else
            {
                Debug.LogError("Coin: ObjectPooler.Instance is null. Cannot return coin to pool. Destroying instead.");
                Destroy(gameObject); // Fallback if pooler is missing
            }
        }
    }
}
