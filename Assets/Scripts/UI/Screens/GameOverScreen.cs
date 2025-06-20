// GameOverScreen.cs
using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Text
using UnityEngine.SceneManagement; // Required for scene management

public class GameOverScreen : MonoBehaviour
{
    [Header("Scene Configuration")]
    public string gameSceneName = "GameScene"; // Default game scene name
    public string mainMenuSceneName = "MainMenu"; // Default main menu scene name

    [Header("UI Elements")]
    public Text finalScoreText; // Assign in Inspector

    public void SetupScreen(int finalScore)
    {
        if (finalScoreText != null)
        {
            finalScoreText.text = "Score: " + finalScore.ToString();
        }
        else
        {
            Debug.LogError("FinalScoreText not assigned in GameOverScreen.cs.");
        }
        // Ensure the Game Over screen itself is visible when this is called
        // This is typically handled by UIManager showing the panel that this script is on.
    }

    public void RetryGame()
    {
        Debug.Log("Retry button pressed. Requesting GameManager to reload game.");
        // GameManager will handle reloading the game (which includes scene load and state reset)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadGame(); // Assuming LoadGame handles reset for a retry
        }
        else
        {
            Debug.LogError("GameManager instance not found. Cannot retry game via GameManager.");
            // Fallback: Ensure time is flowing and attempt direct scene load
            Time.timeScale = 1f;
            // SceneManager.LoadScene(gameSceneName); // Direct fallback
        }
    }

    public void WatchAdForSecondChance()
    {
        Debug.Log("Watch Ad button pressed. (Placeholder - Ad logic would go here)");
        // Placeholder for ad integration and then potentially:
        // GameManager.Instance.GiveSecondChance();
        // UIManager.Instance.HideGameOverScreen(); // If ad was successful
    }

    public void ShareScore()
    {
        Debug.Log("Share Score button pressed. (Placeholder - Social media integration would go here)");
        // Placeholder for social media sharing logic
        // e.g., using a native plugin to share text/image
    }

    public void ExitToMainMenu()
    {
        Debug.Log("Exit to Main Menu button pressed. Requesting GameManager to return to main menu.");
        // GameManager will handle returning to main menu (which includes scene load and state reset)
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
