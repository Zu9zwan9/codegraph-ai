// PlayerInputHandler.cs
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public enum SwipeDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    // Minimum distance for a swipe to be considered a swipe
    // private float minSwipeDistance = 50f;
    // Store the initial touch position
    // private Vector2 touchStartPos;

    // Placeholder for touch/mouse input handling logic
    // Example: Detect taps for moving forward, swipes for dashing

    // void Update() // Update might not be needed if input is polled directly by PlayerController
    // {
    //     // Example: Can be used for continuous input checks or debugging
    //     // SwipeDirection swipe = DetectSwipe();
    //     // if (swipe != SwipeDirection.None) Debug.Log("Swipe Detected: " + swipe);
    // }

    public SwipeDirection DetectSwipe()
    {
        // Placeholder for actual touch-based swipe detection
        // This example uses arrow keys for simplicity in non-mobile testing

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return SwipeDirection.Left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return SwipeDirection.Right;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            return SwipeDirection.Up;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            return SwipeDirection.Down;
        }

        // Actual touch input logic would look something like this:
        /*
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                Vector2 touchEndPos = touch.position;
                float swipeDistanceX = Mathf.Abs(touchEndPos.x - touchStartPos.x);
                float swipeDistanceY = Mathf.Abs(touchEndPos.y - touchStartPos.y);

                // Determine if horizontal or vertical swipe is more significant
                if (swipeDistanceX > minSwipeDistance || swipeDistanceY > minSwipeDistance)
                {
                    if (swipeDistanceX > swipeDistanceY) // Horizontal swipe
                    {
                        if (touchEndPos.x < touchStartPos.x) return SwipeDirection.Left;
                        else return SwipeDirection.Right;
                    }
                    else // Vertical swipe
                    {
                        if (touchEndPos.y < touchStartPos.y) return SwipeDirection.Down;
                        else return SwipeDirection.Up;
                    }
                }
            }
        }
        */
        return SwipeDirection.None;
    }

    public bool IsTapDetected()
    {
        // Kept for potential UI interactions or specific game actions (e.g., tap to start).
        // For core gameplay, swipes are now primary.
        // Example: using Input.GetMouseButtonDown(0) or specific touch phase checks
        if (Input.GetMouseButtonDown(0)) // Simple mouse tap for testing
        {
            // Consider adding logic to differentiate taps from swipes if both are used.
            // e.g., a tap is a touch that ends without significant movement.
            return true;
        }
        // if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) return true;
        return false;
    }

    // Unused methods from previous version - can be removed or repurposed
    // public void HandleInput() { }
    // public bool IsSwipeDetected() { return false; } // Replaced by DetectSwipe()
}
