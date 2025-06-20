// ScoreManager.cs
using UnityEngine;
using System; // Required for Action

public class ScoreManager : MonoBehaviour
{
    // Singleton pattern instance
    public static ScoreManager Instance { get; private set; }

    private int currentScore;

    // Public property to access the score
    public int CurrentScore
    {
        get { return currentScore; }
        private set
        {
            currentScore = value;
            // Notify listeners that the score has changed
            OnScoreChanged?.Invoke(currentScore);
        }
    }

    // Event to notify UI or other systems of score changes
    public static event Action<int> OnScoreChanged;

    private int sessionCoins = 0;
    public int SessionCoins { get { return sessionCoins; } }
    public static event Action<int> OnSessionCoinsChanged; // For UI updates specifically for coins

    // Conceptual: Persistent total coins, saved/loaded by a SaveSystem
    // private int totalPersistentCoins = 0;

    void Awake()
    {
        // Implement Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: if score should persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // ResetSessionStats(); // Initialize stats at the start
        // GameManager.StartGamePreGameplay() will call ResetSessionStats() to ensure it's reset at the right time.
        // Load persistent coins here if implementing that feature
        // totalPersistentCoins = LoadTotalCoins(); // Example
    }

    public void AddScore(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("AddScore called with a negative amount. Use ReduceScore or ensure amount is positive.");
            return;
        }
        CurrentScore += amount;
        // Debug.Log("Score: " + CurrentScore); // OnScoreChanged event will be used by UI
    }

    public void CollectCoin(int value)
    {
        if (value <= 0)
        {
            Debug.LogWarning("CollectCoin called with zero or negative value.");
            return;
        }
        sessionCoins += value;
        OnSessionCoinsChanged?.Invoke(sessionCoins);
        Debug.Log($"Collected coin. Current session coins: {sessionCoins}");

        // Conceptual: Add to persistent total and save
        // totalPersistentCoins += value;
        // SaveTotalCoins(totalPersistentCoins); // Example
    }

    public void ResetSessionStats() // Renamed from ResetScore
    {
        CurrentScore = 0; // Resets OnScoreChanged via property setter
        sessionCoins = 0;
        OnSessionCoinsChanged?.Invoke(sessionCoins); // Explicitly invoke for coins
        Debug.Log("Session Stats Reset: Score and Coins are now 0");
    }

    // Example: Method to be called when player performs an action that grants points for score (not coins)
    // public void PlayerScoredPoints(int points)
    // {
    //     AddScore(points);
    // }

    // Conceptual: Save/Load for persistent coins
    // private void SaveTotalCoins(int coins) { PlayerPrefs.SetInt("TotalCoins", coins); PlayerPrefs.Save(); }
    // private int LoadTotalCoins() { return PlayerPrefs.GetInt("TotalCoins", 0); }
}
