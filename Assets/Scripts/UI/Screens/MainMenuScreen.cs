// MainMenuScreen.cs
// CityPop Style Overall: Dark background (deep blues/purples) with bold gradients (pink to yellow, teal to magenta).
// Grid lines or subtle geometric patterns in the background. Palm tree silhouettes or cityscape outlines.
using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Button, Image
using UnityEngine.SceneManagement; // Though scene loading is now GameManager's job

public class MainMenuScreen : MonoBehaviour
{
    [Header("UI Buttons")]
    // CityPop Style: Rounded corners, possibly a slight 3D layered effect or an inner glow. Neon text or icon.
    public Button playButton;
    // CityPop Style: Similar to playButton, maybe a distinct icon (e.g., a pixelated character or clothing item).
    public Button characterShopButton;
    // CityPop Style: Gear icon, possibly with a retro-futuristic design.
    public Button settingsButton;
    // CityPop Style: Calendar or star icon, bright and inviting.
    public Button dailyChallengeButton;

    [Header("UI Elements")]
    // CityPop Style: Small, bright (e.g., neon yellow or pink), possibly with a subtle pulsating animation.
    public Image dailyChallengeBadge;

    // public string gameSceneName = "GameScene"; // Kept for reference, but GameManager handles scene name now.

    void Awake()
    {
        // Null checks for buttons to prevent errors if not assigned in Inspector
        if (playButton == null) Debug.LogError("MainMenuScreen: Play Button not assigned!", this);
        if (characterShopButton == null) Debug.LogError("MainMenuScreen: Character Shop Button not assigned!", this);
        if (settingsButton == null) Debug.LogError("MainMenuScreen: Settings Button not assigned!", this);
        if (dailyChallengeButton == null) Debug.LogError("MainMenuScreen: Daily Challenge Button not assigned!", this);
        if (dailyChallengeBadge == null) Debug.LogError("MainMenuScreen: Daily Challenge Badge not assigned!", this);
    }

    void Start()
    {
        // Assign listeners to buttons
        if (playButton != null) playButton.onClick.AddListener(HandlePlayButtonPressed);
        if (characterShopButton != null) characterShopButton.onClick.AddListener(HandleCharacterShopButtonPressed);
        if (settingsButton != null) settingsButton.onClick.AddListener(HandleSettingsButtonPressed);
        if (dailyChallengeButton != null) dailyChallengeButton.onClick.AddListener(HandleDailyChallengeButtonPressed);

        // Initialize daily challenge badge visibility
        // Logic to show/hide badge based on actual challenge status needed later.
        SetDailyChallengeBadgeVisibility(true); // Example: show by default for now
    }

    void OnDestroy()
    {
        // Remove listeners to prevent memory leaks and errors
        if (playButton != null) playButton.onClick.RemoveListener(HandlePlayButtonPressed);
        if (characterShopButton != null) characterShopButton.onClick.RemoveListener(HandleCharacterShopButtonPressed);
        if (settingsButton != null) settingsButton.onClick.RemoveListener(HandleSettingsButtonPressed);
        if (dailyChallengeButton != null) dailyChallengeButton.onClick.RemoveListener(HandleDailyChallengeButtonPressed);
    }

    private void HandlePlayButtonPressed()
    {
        Debug.Log("Play Button Pressed. Requesting GameManager to load game.");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadGame();
        }
        else
        {
            Debug.LogError("MainMenuScreen: GameManager not found. Cannot start game.");
        }
    }

    private void HandleCharacterShopButtonPressed()
    {
        Debug.Log("Character Shop Button Pressed. (Placeholder - UI navigation logic to CharacterShopScreen needed later)");
        // Example: UIManager.Instance.ShowScreen("CharacterShop"); or similar navigation.
        // This might involve activating a different canvas or loading an additive scene.
    }

    private void HandleSettingsButtonPressed()
    {
        Debug.Log("Settings Button Pressed. (Placeholder - UI navigation logic to SettingsScreen needed later)");
        // Example: UIManager.Instance.ShowScreen("Settings"); or similar navigation.
    }

    private void HandleDailyChallengeButtonPressed()
    {
        Debug.Log("Daily Challenge Button Pressed. (Placeholder - UI navigation logic to DailyChallengeScreen needed later)");
        // Example: UIManager.Instance.ShowScreen("DailyChallenge");
        // Could also set the badge to invisible after clicking:
        // SetDailyChallengeBadgeVisibility(false);
    }

    public void SetDailyChallengeBadgeVisibility(bool isVisible)
    {
        if (dailyChallengeBadge != null)
        {
            dailyChallengeBadge.gameObject.SetActive(isVisible);
        }
        else
        {
            Debug.LogWarning("MainMenuScreen: Daily Challenge Badge reference is missing. Cannot set visibility.", this);
        }
    }

    // QuitGame method can remain if a quit button is part of the main menu directly
    public void QuitGame()
    {
        Debug.Log("QuitGame method called from MainMenuScreen.");
        // Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBGL
        Debug.Log("MainMenuScreen: Application.Quit() called on WebGL. Action ignored by browser.");
        #else
        Application.Quit();
        #endif
    }
}
