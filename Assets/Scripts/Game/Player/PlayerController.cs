// PlayerController.cs
using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))] // Ensure input handler is present
[RequireComponent(typeof(PlayerAnimation))] // Ensure animation handler is present
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardMoveSpeed = 5f; // Speed for constant forward movement
    public float laneChangeSpeed = 10f;

    [Header("Lane Configuration")]
    public int numberOfLanes = 3;
    public float laneWidth = 2.0f;

    private int currentLane = 1; // Start in the middle lane (0, 1, 2 for 3 lanes)
    private Vector3 targetPosition;

    // References to other components
    private PlayerInputHandler _inputHandler;
    private PlayerAnimation _playerAnimation; // Will be assigned once PlayerAnimation.cs is created

    void Awake()
    {
        _inputHandler = GetComponent<PlayerInputHandler>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        if (_inputHandler == null)
        {
            Debug.LogError("PlayerInputHandler component not found on Player.");
        }
        if (_playerAnimation == null) Debug.LogError("PlayerAnimation component not found on Player.");
    }

    void Start()
    {
        currentLane = numberOfLanes / 2; // Integer division for center lane
        targetPosition = transform.position; // Initialize targetPosition with current position
        targetPosition.x = (currentLane - (numberOfLanes / 2)) * laneWidth;
        transform.position = targetPosition; // Snap to initial lane position

        // _playerAnimation should be found in Awake due to RequireComponent, but double check for safety.
        if (_playerAnimation == null) _playerAnimation = GetComponent<PlayerAnimation>();
        if (_playerAnimation == null) Debug.LogError("PlayerAnimation component is missing despite RequireComponent attribute.");

    }

    void Update()
    {
        if (GameManager.CurrentState != GameManager.GameState.Playing)
        {
            // If game is not in Playing state (e.g., Paused, GameOver, MainMenu), don't process player input or movement.
            // The PlayerAnimation script might handle showing an idle or specific state animation here.
            return;
        }

        // Handle input for lane changes, jump, roll
        PlayerInputHandler.SwipeDirection swipe = _inputHandler.DetectSwipe();

        if (swipe == PlayerInputHandler.SwipeDirection.Left)
        {
            ChangeLane(-1);
        }
        else if (swipe == PlayerInputHandler.SwipeDirection.Right)
        {
            ChangeLane(1);
        }
        else if (swipe == PlayerInputHandler.SwipeDirection.Up)
        {
            Jump();
        }
        else if (swipe == PlayerInputHandler.SwipeDirection.Down)
        {
            Roll();
        }

        // Smoothly move towards the target lane's x position
        Vector3 currentPos = transform.position;
        if (Mathf.Abs(currentPos.x - targetPosition.x) > 0.01f)
        {
            currentPos.x = Mathf.Lerp(currentPos.x, targetPosition.x, Time.deltaTime * laneChangeSpeed);
            transform.position = currentPos;
        }

        // Constant forward movement
        transform.Translate(Vector3.forward * forwardMoveSpeed * Time.deltaTime);
    }

    void ChangeLane(int direction)
    {
        int newLane = currentLane + direction;
        // Clamp newLane to be within the valid lane range (0 to numberOfLanes - 1)
        newLane = Mathf.Clamp(newLane, 0, numberOfLanes - 1);

        if (newLane != currentLane) // Only change if it's a valid new lane
        {
            currentLane = newLane;
            targetPosition.x = (currentLane - (numberOfLanes / 2)) * laneWidth;

            if (_playerAnimation != null) _playerAnimation.PlayLaneSwitchAnimation(direction > 0);
            else Debug.LogWarning("PlayerAnimation not found, cannot play lane switch animation.");
            Debug.Log("Changed lane to: " + currentLane + ". Target X: " + targetPosition.x);
        }
    }

    void Jump()
    {
        Debug.Log("Jump action triggered");
        if (_playerAnimation != null) _playerAnimation.PlayJumpAnimation();
        else Debug.LogWarning("PlayerAnimation not found, cannot play jump animation.");
        // Add actual jump logic here (e.g., using Rigidbody.AddForce or CharacterController.Move)
    }

    void Roll()
    {
        Debug.Log("Roll action triggered");
        if (_playerAnimation != null) _playerAnimation.PlayRollAnimation();
        else Debug.LogWarning("PlayerAnimation not found, cannot play roll animation.");
        // Add actual roll logic here (e.g., changing collider size, animation state)
    }

    // Collision Handling - Transferred from PlayerMovement.cs
    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has a "Vehicle" tag
        // Make sure your vehicle prefabs have this tag.
        if (collision.gameObject.CompareTag("Vehicle"))
        {
            OnCollisionWithVehicle(collision.gameObject);
        }
        // Check for Obstacle collision
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            OnCollisionWithObstacle(collision.gameObject);
        }
        // Add other collision checks if needed (e.g., collectibles)
        // else if (collision.gameObject.CompareTag("Collectible")) { /* Handle collectible */ }
    }

    public void OnCollisionWithVehicle(GameObject vehicle)
    {
        Debug.Log("Collided with vehicle: " + vehicle.name + ". Triggering Game Over.");
        if (_playerAnimation != null) _playerAnimation.PlayDeathAnimation();
        else Debug.LogWarning("PlayerAnimation not found, cannot play death animation.");

        if (GameManager.Instance != null && GameManager.CurrentState == GameManager.GameState.Playing)
        {
            GameManager.Instance.TriggerGameOver();
        }
        // Disable this script or the GameObject to prevent further movement/actions
        this.enabled = false; // Stop controller logic on death
        // Or gameObject.SetActive(false); // If player disappears on death
    }

    public void OnCollisionWithObstacle(GameObject obstacle)
    {
        Debug.Log("Hit an Obstacle: " + obstacle.name + ". Triggering Game Over.");
        if (_playerAnimation != null) _playerAnimation.PlayDeathAnimation();
        else Debug.LogWarning("PlayerAnimation not found, cannot play death animation for obstacle collision.");

        if (GameManager.Instance != null && GameManager.CurrentState == GameManager.GameState.Playing)
        {
            GameManager.Instance.TriggerGameOver();
        }
        this.enabled = false; // Stop controller logic on death
    }

    // Public method to reset player (e.g., on game restart)
    public void ResetPlayer()
    {
        currentLane = numberOfLanes / 2;
        targetPosition = Vector3.zero; // Or specific start Z
        targetPosition.x = (currentLane - (numberOfLanes / 2)) * laneWidth;
        transform.position = targetPosition;
        this.enabled = true; // Ensure controller is enabled
        if (_playerAnimation != null) _playerAnimation.PlayRunAnimation(); // Reset to run animation
        else Debug.LogWarning("PlayerAnimation not found, cannot reset to run animation.");
        Debug.Log("PlayerController: ResetPlayer called.");
    }
}
