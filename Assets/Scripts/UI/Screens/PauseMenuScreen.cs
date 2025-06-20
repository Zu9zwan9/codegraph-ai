// PauseMenuScreen.cs
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class PauseMenuScreen : MonoBehaviour
{
    [Header("Scene Configuration")]
    public string gameSceneName = "GameScene"; // Default game scene name
    public string mainMenuSceneName = "MainMenu"; // Default main menu scene name

    void Start()
    {
        // Ensure the pause menu is correctly linked with PauseManager state if needed
        // For example, if the game starts paused for some reason, this panel might need to show.
        // However, typically UIManager handles showing/hiding this panel.
    }

    public void ResumeGame()
    {
        Debug.Log("Resume button pressed.");
        // GameManager will handle unpausing, which then tells PauseManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Resume();
        }
        else
        {
            Debug.LogError("GameManager instance not found. Cannot resume game via GameManager.");
            // Fallback if GameManager is not found
            if (PauseManager.Instance != null) PauseManager.Instance.ResumeGame();
            else Time.timeScale = 1f;
        }
    }

    public void RestartGame()
    {
        Debug.Log("Restart button pressed. Requesting GameManager to reload game.");
        // GameManager will handle reloading the game (which includes unpausing and scene load)
        if (GameManager.Instance != null)
        {
            // GameManager needs a RestartGame or similar method, or LoadGame can be used if it handles reset.
            // For now, assuming LoadGame implies a fresh start.
            GameManager.Instance.LoadGame();
        }
        else
        {
            Debug.LogError("GameManager instance not found. Cannot restart game via GameManager.");
            // Fallback: Ensure time is flowing and attempt direct scene load
            Time.timeScale = 1f;
            // SceneManager.LoadScene(gameSceneName); // Direct fallback
        }
    }

    public void ExitToMainMenu()
    {
        Debug.Log("Exit to Main Menu button pressed. Requesting GameManager to return to main menu.");
        // GameManager will handle returning to main menu (which includes unpausing and scene load)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReturnToMainMenu();
        }
        else
        {
            Debug.LogError("GameManager instance not found. Cannot exit to main menu via GameManager.");
            // Fallback: Ensure time is flowing and attempt direct scene load
            Time.timeScale = 1f;
            // SceneManager.LoadScene(mainMenuSceneName); // Direct fallback
        }
    }
}
