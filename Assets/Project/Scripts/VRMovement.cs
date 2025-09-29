// Note: Find other movement.cs script

using UnityEngine;

/// <summary>
/// This script handles player movement in a VR environment using the Oculus controller's thumbstick.
/// It should be attached to the main player/character object that also has a CharacterController component.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class VRMovement : MonoBehaviour
{
    [Tooltip("The speed at which the player moves. Can be adjusted in the Unity Inspector.")]
    public float speed = 3.0f;

    [Tooltip("The force of gravity applied to the player.")]
    public float gravity = -9.81f;

    // A reference to the OVRCameraRig, which is needed to determine the player's forward direction.
    private OVRCameraRig cameraRig;
    
    // The CharacterController component handles collisions and movement.
    private CharacterController characterController;

    // Stores the player's vertical velocity for gravity calculations.
    private Vector3 velocity;

    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>
    void Start()
    {
        // Get the CharacterController component attached to this GameObject.
        // The [RequireComponent] attribute ensures this component exists.
        characterController = GetComponent<CharacterController>();

        // Find the OVRCameraRig in the scene. This is a crucial part of the Meta XR SDK.
        cameraRig = FindObjectOfType<OVRCameraRig>();
        if (cameraRig == null)
        {
            Debug.LogError("OVRCameraRig not found in the scene. Please ensure one is present.");
        }
    }

    /// <summary>
    /// This method is called once per frame.
    /// </summary>
    void Update()
    {
        // Only proceed if the camera rig has been found.
        if (cameraRig == null) return;

        HandleMovement();
        HandleGravity();
    }

    /// <summary>
    /// Calculates and applies movement based on controller input and head orientation.
    /// </summary>
    private void HandleMovement()
    {
        // Get the thumbstick input from the left Oculus Touch controller.
        Vector2 thumbstickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

        // Determine the forward and right directions based on the headset's orientation.
        Transform head = cameraRig.centerEyeAnchor;
        Vector3 forward = head.forward;
        Vector3 right = head.right;

        // For ground movement, we want to ignore the vertical (Y) component of the headset's tilt.
        // This prevents the player from moving up or down when they look up or down.
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Calculate the desired movement direction by combining the forward/backward and left/right inputs.
        Vector3 desiredMoveDirection = (forward * thumbstickInput.y + right * thumbstickInput.x);

        // Apply the movement to the CharacterController, scaling by speed and time.
        characterController.Move(desiredMoveDirection * speed * Time.deltaTime);
    }

    /// <summary>
    /// Applies gravity to the player.
    /// </summary>
    private void HandleGravity()
    {
        // Check if the character is on the ground.
        if (characterController.isGrounded && velocity.y < 0)
        {
            // If grounded, reset the vertical velocity to a small negative value.
            // This helps keep the character firmly on the ground.
            velocity.y = -2f;
        }

        // Apply gravity to the vertical velocity over time.
        velocity.y += gravity * Time.deltaTime;

        // Apply the calculated gravity to the CharacterController.
        characterController.Move(velocity * Time.deltaTime);
    }
}
