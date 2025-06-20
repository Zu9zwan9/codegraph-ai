// PauseManager.cs
using UnityEngine;
using System; // Required for Action

public class PauseManager : MonoBehaviour
{
    // Singleton instance
    public static PauseManager Instance { get; private set; }

    // Public property to check if the game is paused
    public static bool IsPaused { get; private set; }

    // Event to notify other scripts of pause state changes
    public static event Action<bool> OnPauseStateChanged;

    void Awake()
    {
        // Implement Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: if pause state should persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TogglePause()
    {
        if (IsPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (IsPaused) return; // Already paused

        IsPaused = true;
        Time.timeScale = 0f; // Stop time in the game
        OnPauseStateChanged?.Invoke(true);
        Debug.Log("Game Paused. Time.timeScale = 0;");
        // Additional logic: Show pause menu, disable player input, etc.
        // UIManager.Instance.ShowPauseMenu(true);
    }

    public void ResumeGame()
    {
        if (!IsPaused) return; // Already resumed

        IsPaused = false;
        Time.timeScale = 1f; // Resume normal time flow
        OnPauseStateChanged?.Invoke(false);
        Debug.Log("Game Resumed. Time.timeScale = 1;");
        // Additional logic: Hide pause menu, enable player input, etc.
        // UIManager.Instance.ShowPauseMenu(false);
    }

    // Optional: Ensure game is not paused when the application quits or editor stops
    void OnApplicationQuit()
    {
        if (IsPaused)
        {
            IsPaused = false; // Reset for next play session
            Time.timeScale = 1f;
        }
    }
}
