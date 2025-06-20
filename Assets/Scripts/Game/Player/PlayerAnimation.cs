// PlayerAnimation.cs
using UnityEngine;

// [RequireComponent(typeof(Animator))] // Ensures an Animator component is present
public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("PlayerAnimation: Animator component not found on this GameObject.");
        }
    }

    void Start()
    {
        // Default animation to play at the start of the game
        PlayRunAnimation();
    }

    public void PlayRunAnimation()
    {
        Debug.Log("PlayerAnimation: Playing Run Animation");
        // if (_animator != null) _animator.SetTrigger("RunTrigger");
        // Or _animator.Play("RunStateName");
        // Or _animator.SetBool("IsRunning", true);
    }

    public void PlayLaneSwitchAnimation(bool movingRight)
    {
        // Animation might differ based on direction or could be a generic dodge
        if (movingRight)
        {
            Debug.Log("PlayerAnimation: Playing Lane Switch Right Animation");
            // if (_animator != null) _animator.SetTrigger("DodgeRightTrigger");
        }
        else
        {
            Debug.Log("PlayerAnimation: Playing Lane Switch Left Animation");
            // if (_animator != null) _animator.SetTrigger("DodgeLeftTrigger");
        }
    }

    public void PlayJumpAnimation()
    {
        Debug.Log("PlayerAnimation: Playing Jump Animation");
        // if (_animator != null) _animator.SetTrigger("JumpTrigger");
    }

    public void PlayRollAnimation()
    {
        Debug.Log("PlayerAnimation: Playing Roll Animation (Slide)");
        // if (_animator != null) _animator.SetTrigger("RollTrigger"); // Or "SlideTrigger"
    }

    public void PlayDeathAnimation()
    {
        Debug.Log("PlayerAnimation: Playing Death Animation");
        // if (_animator != null) _animator.SetTrigger("DeathTrigger");
    }

    // Example: Method to be called when game state changes, if needed
    // public void HandleGameStateChanged(GameManager.GameState newState)
    // {
    //     if (newState == GameManager.GameState.MainMenu || newState == GameManager.GameState.GameOver)
    //     {
    //         // PlayIdleAnimation(); or similar
    //     }
    //     else if (newState == GameManager.GameState.Playing)
    //     {
    //         PlayRunAnimation();
    //     }
    // }
}
