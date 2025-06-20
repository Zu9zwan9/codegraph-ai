// UIManager.cs
using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Text

public class UIManager : MonoBehaviour
{
    // Singleton pattern instance (optional, but common for UIManagers)
    public static UIManager Instance { get; private set; }

    // Reference to the UI Text element for displaying the score - MOVED TO GameHUDScreen.cs
    // public Text scoreText;

    [Header("HUD Elements (Primarily managed by GameHUDScreen.cs now)")]
    // These fields might be kept if UIManager needs to directly manipulate them for some reason,
    // but their primary update logic is in GameHUDScreen. For now, commenting out to avoid confusion.
    // public Slider comboBarSlider;
    // public Image powerupIndicatorImage;
    // public Button pauseButton; // Pause button interactions are handled by GameHUDScreen.cs

    [Header("UI Panels")]
    public GameObject pauseMenuPanel; // Assign your Pause Menu Panel in the Inspector
    public GameObject gameOverScreenPanel; // Assign your Game Over Screen Panel in the Inspector

    [Header("Screen Managers")]
    public GameOverScreen gameOverScreenManager; // Assign GameOverScreen component (formerly GameOverScreenManager)

    // Reference to PauseManager (can be set in Inspector or found if PauseManager is a singleton)
    // public PauseManager pauseManager;

    void Awake()
    {
        // Optional: Implement Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Usually UIs are scene-specific
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Score display is now handled by GameHUDScreen.cs
        // if (ScoreManager.Instance != null)
        // {
        //     ScoreManager.OnScoreChanged += UpdateScoreDisplay;
        //     UpdateScoreDisplay(ScoreManager.Instance.CurrentScore);
        // }
        // else
        // {
        //     UpdateScoreDisplay(0);
        // }

        // Pause button logic is now in GameHUDScreen.cs
        // if (pauseButton != null)
        // {
        //     pauseButton.onClick.AddListener(HandlePauseButtonPressed);
        // }

        // Powerup indicator is now in GameHUDScreen.cs
        // ShowPowerupIndicator(false);

        // Subscribe to pause state changes to show/hide the pause menu
        // This remains UIManager's responsibility for the Pause *Menu Panel*
        PauseManager.OnPauseStateChanged += ShowPauseMenu;
        // Ensure pause menu is initially hidden
        ShowPauseMenu(false);

        // Ensure Game Over screen is initially hidden
        if (gameOverScreenPanel != null)
        {
            gameOverScreenPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("GameOverScreenPanel not assigned in UIManager. Cannot hide it initially.");
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from events when the UIManager is destroyed to prevent memory leaks
        // ScoreManager.OnScoreChanged -= UpdateScoreDisplay; // Handled by GameHUDScreen
        PauseManager.OnPauseStateChanged -= ShowPauseMenu;

        // Pause button listener is handled by GameHUDScreen
        // if (pauseButton != null)
        // {
        //     pauseButton.onClick.RemoveListener(HandlePauseButtonPressed);
        // }
    }

    public void ShowPauseMenu(bool show)
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(show);
        }
        else
        {
            Debug.LogWarning("PauseMenuPanel not assigned in UIManager. Cannot show/hide it.");
        }
    }

    // UpdateScoreDisplay and other HUD methods are now primarily in GameHUDScreen.cs
    // public void UpdateScoreDisplay(int score) { /* ... */ }
    // public void UpdateComboBar(float value) { /* ... */ }
    // public void UpdatePowerupIndicator(Sprite powerupSprite) { /* ... */ }
    // public void ShowPowerupIndicator(bool show) { /* ... */ }
    // public void HandlePauseButtonPressed() { /* ... */ } // This is now in GameHUDScreen.cs

    // Example: Method to show a game over screen - REMAINS in UIManager
    // public GameObject gameOverScreenPanel; // Already defined above
    // public void ShowGameOverScreen()
    // {
    //    if(gameOverScreen != null) gameOverScreen.SetActive(true);
    // }

    // Example: Method to update a health display
    // public Text healthText; // Assign in Inspector
    // public void UpdateHealthDisplay(int health)
    // {
    //    if(healthText != null) healthText.text = "Health: " + health;
    // }

    public void ShowGameOverScreen(int finalScore)
    {
        if (gameOverScreenPanel != null)
        {
            gameOverScreenPanel.SetActive(true);
            if (gameOverScreenManager != null)
            {
                gameOverScreenManager.SetupScreen(finalScore);
            }
            else
            {
                Debug.LogError("GameOverScreen (script) not assigned in UIManager. Cannot setup Game Over screen.");
            }
        }
        else
        {
            Debug.LogError("GameOverScreenPanel (GameObject) not assigned in UIManager. Cannot show Game Over screen.");
        }
    }
}
