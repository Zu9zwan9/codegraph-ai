// GameHUDScreen.cs
using UnityEngine;
using UnityEngine.UI; // For common UI elements like Slider, Image, Text
using System; // For System.Action if any local events were needed

public class GameHUDScreen : MonoBehaviour
{
    [Header("Core HUD Elements")]
    // CityPop Style: Bright, possibly pixelated or retro font. Neon pinks, blues, yellows.
    public Text scoreText;
    // CityPop Style: Similar to score, maybe a distinct coin icon (sprite).
    public Text coinCountText;
    // CityPop Style: Geometric shape, possibly with a subtle animation or glow.
    public Button pauseButton;

    [Header("Gameplay HUD Elements")]
    // CityPop Style: Sleek bar, maybe with a gradient fill (e.g., pink to yellow). Animated pips.
    public Slider comboBarSlider;
    // CityPop Style: Placeholder for an icon that changes based on powerup. Could have a glowing border.
    public Image powerupIndicatorImage;

    [Header("Visual Effects Placeholders")]
    // CityPop Style: This text would be a duplicate of scoreText but rendered with a neon glow shader or effect.
    public Text neonScoreTextEffectPlaceholder;

    void Awake()
    {
        // Initial checks for assigned elements
        if (scoreText == null) Debug.LogWarning("GameHUDScreen: scoreText is not assigned.", this);
        if (coinCountText == null) Debug.LogWarning("GameHUDScreen: coinCountText is not assigned.", this);
        if (pauseButton == null) Debug.LogWarning("GameHUDScreen: pauseButton is not assigned.", this);
        if (comboBarSlider == null) Debug.LogWarning("GameHUDScreen: comboBarSlider is not assigned.", this);
        if (powerupIndicatorImage == null) Debug.LogWarning("GameHUDScreen: powerupIndicatorImage is not assigned.", this);
        if (neonScoreTextEffectPlaceholder == null) Debug.LogWarning("GameHUDScreen: neonScoreTextEffectPlaceholder not assigned.", this);
    }

    void Start()
    {
        // Subscribe to ScoreManager events
        if (ScoreManager.Instance != null)
        {
            ScoreManager.OnScoreChanged += UpdateScoreDisplay;
            ScoreManager.OnSessionCoinsChanged += UpdateCoinDisplay;
            // Initialize display with current values
            UpdateScoreDisplay(ScoreManager.Instance.CurrentScore);
            UpdateCoinDisplay(ScoreManager.Instance.SessionCoins);
        }
        else
        {
            Debug.LogError("GameHUDScreen: ScoreManager.Instance is null in Start. Score and Coin HUD will not update.", this);
        }

        // Pause Button Logic
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(HandlePauseButtonPressed);
        }
        // else already warned in Awake

        // Initialize other HUD elements
        if (powerupIndicatorImage != null)
        {
            powerupIndicatorImage.gameObject.SetActive(false); // Initially hidden
        }
        if (comboBarSlider != null)
        {
            comboBarSlider.value = 0; // Initial combo bar value
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        if (ScoreManager.Instance != null)
        {
            ScoreManager.OnScoreChanged -= UpdateScoreDisplay;
            ScoreManager.OnSessionCoinsChanged -= UpdateCoinDisplay;
        }
        if (pauseButton != null)
        {
            pauseButton.onClick.RemoveListener(HandlePauseButtonPressed);
        }
    }

    public void UpdateScoreDisplay(int score)
    {
        if (scoreText != null)
        {
            // CityPop Style: Could use a custom number formatter here for a retro look.
            scoreText.text = "SCORE: " + score.ToString("D6"); // Example: 001234
        }
        if (neonScoreTextEffectPlaceholder != null)
        {
            neonScoreTextEffectPlaceholder.text = score.ToString("D6"); // Neon effect handled by shader/component
        }
    }

    public void UpdateCoinDisplay(int coins)
    {
        if (coinCountText != null)
        {
            // CityPop Style: Could have a small coin icon (Image) next to the text.
            coinCountText.text = "COINS: " + coins.ToString("D3"); // Example: 042
        }
    }

    public void UpdateComboBar(float value)
    {
        if (comboBarSlider != null)
        {
            comboBarSlider.value = Mathf.Clamp01(value);
        }
        else Debug.LogWarning("GameHUDScreen: comboBarSlider not assigned, cannot update.", this);
    }

    public void DisplayPowerupIndicator(Sprite icon, bool show)
    {
        if (powerupIndicatorImage != null)
        {
            if (show && icon != null)
            {
                powerupIndicatorImage.sprite = icon;
                powerupIndicatorImage.gameObject.SetActive(true);
            }
            else
            {
                powerupIndicatorImage.gameObject.SetActive(false);
            }
        }
        else Debug.LogWarning("GameHUDScreen: powerupIndicatorImage not assigned, cannot display powerup.", this);
    }

    private void HandlePauseButtonPressed()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameHUDScreen: GameManager.Instance is null. Cannot process pause button press.");
            return;
        }

        Debug.Log("Pause button pressed. Current game state: " + GameManager.CurrentState);
        if (GameManager.CurrentState == GameManager.GameState.Playing)
        {
            GameManager.Instance.Pause();
        }
        else if (GameManager.CurrentState == GameManager.GameState.Paused)
        {
            GameManager.Instance.Resume();
        }
    }

    // Call this method to show/hide the entire HUD (e.g., during loading or game over)
    // public void ShowHUD(bool show)
    // {
    //    gameObject.SetActive(show);
    // }
}
