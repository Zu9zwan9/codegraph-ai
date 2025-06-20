// GameManager.cs
// [Zenject] Bind this GameManager as a singleton.
using UnityEngine;
using System; // For Action event

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Loading,
        Playing,
        Paused,
        GameOver
    }

    public static GameState CurrentState { get; private set; }
    public static event Action<GameState> OnGameStateChanged;

    // Singleton instance
    public static GameManager Instance { get; private set; }

    // [Zenject] Inject UIManager, ScoreManager, etc.
    // For now, public fields assignable in Inspector or found in Awake
    public UIManager uiManager;
    public ScoreManager scoreManager;
    public PauseManager pauseManager; // Added reference for PauseManager

    // private bool isGameOver = false; // Replaced by GameState

    void Awake()
    {
        // Implement Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: if GameManager should persist
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Attempt to find managers if not assigned
        if (uiManager == null) uiManager = FindObjectOfType<UIManager>();
        if (scoreManager == null) scoreManager = FindObjectOfType<ScoreManager>();
        if (pauseManager == null) pauseManager = FindObjectOfType<PauseManager>();

        if (uiManager == null) Debug.LogError("GameManager: UIManager not found.");
        if (scoreManager == null) Debug.LogError("GameManager: ScoreManager not found.");
        if (pauseManager == null) Debug.LogError("GameManager: PauseManager not found.");

        // Set initial state
        // CurrentState = GameState.MainMenu; // Set directly
        SetState(GameState.MainMenu); // Set via method to invoke event
    }

    private void SetState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);
        Debug.Log($"GameManager: State changed to {newState}");
    }

    // [UniTask] Scene loading methods (LoadGame, ReturnToMainMenu) will be converted to async with UniTask.
    public void LoadGame()
    {
        if (CurrentState == GameState.Loading) return;

        SetState(GameState.Loading);
        Debug.Log("GameManager: Loading game scene... (Conceptual: SceneManager.LoadSceneAsync will be used here)");
        // Actual scene loading (e.g., SceneManager.LoadSceneAsync("GameSceneName"))
        // For now, directly call StartGamePreGameplay as if scene loaded instantly
        // Reset time scale in case it was 0 from a previous game over or main menu pause.
        Time.timeScale = 1f;
        StartGamePreGameplay();
    }

    public void StartGamePreGameplay()
    {
        // This method would typically be called after the game scene has loaded.
        // For example, by a scene loaded event or a callback from an async scene load.
        Debug.Log("GameManager: Game scene loaded. Starting pre-gameplay setup.");

        // Reset score
        if (scoreManager != null)
        {
            scoreManager.ResetSessionStats(); // Updated to new method name
        }
        else Debug.LogError("ScoreManager not found, cannot reset session stats.");

        // Reset player position (conceptual)
        Debug.Log("Conceptual: Player position would be reset here.");
        // FindObjectOfType<PlayerController>()?.ResetPlayer();

        // Start enemy/traffic spawning (conceptual)
        Debug.Log("Conceptual: Traffic/Enemy spawning would start here.");
        // FindObjectOfType<TrafficSpawner>()?.StartSpawning();

        SetState(GameState.Playing);
        Debug.Log("GameManager: Game Started (State: Playing)");
    }

    public void Pause()
    {
        if (CurrentState != GameState.Playing) return;

        if (pauseManager != null)
        {
            pauseManager.PauseGame(); // This will set Time.timeScale = 0 and invoke its own events
            SetState(GameState.Paused);
        }
        else Debug.LogError("PauseManager not found, cannot pause game.");
    }

    public void Resume()
    {
        if (CurrentState != GameState.Paused) return;

        if (pauseManager != null)
        {
            pauseManager.ResumeGame(); // This will set Time.timeScale = 1 and invoke its own events
            SetState(GameState.Playing); // Set state back to Playing
        }
        else Debug.LogError("PauseManager not found, cannot resume game.");
    }

    public void TriggerGameOver() // Replaces EndGame
    {
        if (CurrentState == GameState.GameOver || CurrentState == GameState.MainMenu) return; // Prevent multiple calls or calling from menu

        Debug.Log("GameManager: Triggering Game Over...");
        SetState(GameState.GameOver);

        // Stop player input (conceptual)
        Debug.Log("Conceptual: Player input would be disabled here.");
        // PlayerInputHandler inputHandler = FindObjectOfType<PlayerInputHandler>();
        // if(inputHandler != null) inputHandler.enabled = false; // Example of disabling input

        // Stop enemy/traffic spawning (conceptual)
        Debug.Log("Conceptual: Traffic/Enemy spawning would stop here.");
        // FindObjectOfType<TrafficSpawner>()?.StopSpawning();

        // Retrieve final score
        int finalScore = 0;
        if (scoreManager != null)
        {
            finalScore = scoreManager.CurrentScore;
        }
        else Debug.LogError("ScoreManager not found, cannot retrieve final score for Game Over screen.");

        // Show Game Over screen
        if (uiManager != null)
        {
            uiManager.ShowGameOverScreen(finalScore);
        }
        else Debug.LogError("UIManager not found, cannot show Game Over screen.");

        // Set time scale to 0 for game over. This is a "hard" pause.
        // PauseManager's IsPaused state might become out of sync if it was true,
        // but GameOver state takes precedence.
        Time.timeScale = 0f;
        Debug.Log("GameManager: Time.timeScale set to 0 due to GameOver.");

        // [Addressables] Game assets like level segments or character skins will be loaded via Addressables.
    }

    public void ReturnToMainMenu()
    {
        // Ensure time scale is reset if game was paused or game over
        Time.timeScale = 1f;

        SetState(GameState.Loading); // Or directly to MainMenu if scene load is instant for menu
        Debug.Log("GameManager: Returning to Main Menu... (Conceptual: SceneManager.LoadSceneAsync will be used here)");
        // Actual scene loading (e.g., SceneManager.LoadSceneAsync("MainMenuSceneName"))
        // For now, just set state to MainMenu as if scene loaded instantly
        SetState(GameState.MainMenu);
    }
}
