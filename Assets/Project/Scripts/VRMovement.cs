using UnityEngine;
// Note: We no longer need the Oculus.Interaction.Locomotion namespace here

/// <summary>
/// This script is now simplified to only handle gravity and other physics forces
/// that are not directly controlled by user input for sliding.
/// It works alongside a separate SlideLocomotionBroadcaster component.
/// </summary>
public class VRMovement : MonoBehaviour
{
    [Header("Physics Settings")]
    [Tooltip("The force of gravity applied to the player.")]
    public float gravity = -9.81f;
    [Tooltip("The minimum Y-axis position the player can fall to before respawning.")]
    public float minimumHeight = -50f;

    private UnityEngine.CharacterController _characterController;
    private Vector3 _velocity; // Stores the player's vertical velocity for gravity
    private Vector3 _startPosition; // To store the initial spawn position

    void Awake()
    {
        // This script now expects to be on the same object as the CharacterController.
        // This is typically the main "Player" root object.
        _characterController = GetComponent<UnityEngine.CharacterController>();
        _startPosition = transform.position;
    }

    void FixedUpdate()
    {
        // This script is now only responsible for applying gravity.
        HandleGravity();
        CheckForFallRespawn();
    }

    private void HandleGravity()
    {
        if (_characterController.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; // Reset velocity when grounded
        }

        // Apply gravity to the vertical velocity over time.
        _velocity.y += gravity * Time.fixedDeltaTime;

        // Apply the calculated gravity to the CharacterController.
        _characterController.Move(_velocity * Time.fixedDeltaTime);
    }

    private void CheckForFallRespawn()
    {
        if (transform.position.y < minimumHeight)
        {
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        _characterController.enabled = false;
        transform.position = _startPosition;
        _velocity.y = 0; // Reset falling speed
        _characterController.enabled = true;
    }

    // You can keep your height reset logic if you are also using teleportation.
    // ... (Your existing ResetHeightToBase method)
}

