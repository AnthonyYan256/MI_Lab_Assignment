using UnityEngine;

/// <summary>
/// Implements standard "Snap Turning" for a VR player using the OVRInput system.
/// 
/// MODIFICATION:
/// - This script now *relies on* the `CharacterControllerDriver` script
///   to be on the same object to handle collision safety.
/// - PerformSnapTurn is now a simple `transform.Rotate()`.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class VRPlayerRotation : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("Drag the 'CenterEyeAnchor' (inside OVRCameraRig/TrackingSpace) here.")]
    public Transform centerEyeAnchor; // Still needed for the driver, good to have.

    [Header("Tuning")]
    [Tooltip("The number of degrees to snap turn with one flick of the stick.")]
    public float snapAngle = 45f;

    [Tooltip("The threshold the stick must pass to trigger a turn. Prevents drift.")]
    public float thumbstickDeadzone = 0.4f;

    // --- Private ---
    private OVRInput.Controller controller = OVRInput.Controller.RTouch;
    private bool isReadyToSnapTurn = true;
    private CharacterController characterController;

    void Start()
    {
        // Get the CharacterController on this object
        characterController = GetComponent<CharacterController>();

        if (centerEyeAnchor == null)
        {
            Debug.LogError("VRPlayerRotation: 'Center Eye Anchor' is not set! This is required by the driver.", this);
        }
    }

    void Update()
    {
        // Get the horizontal (X) value of the right thumbstick
        float stickValue = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller).x;

        // Check if the thumbstick is flicked to the right
        if (stickValue > thumbstickDeadzone)
        {
            if (isReadyToSnapTurn)
            {
                isReadyToSnapTurn = false;
                PerformSnapTurn(snapAngle);
            }
        }
        // Check if the thumbstick is flicked to the left
        else if (stickValue < -thumbstickDeadzone)
        {
            if (isReadyToSnapTurn)
            {
                isReadyToSnapTurn = false;
                PerformSnapTurn(-snapAngle);
            }
        }
        // Check if the thumbstick has returned to the center
        else if (Mathf.Abs(stickValue) < thumbstickDeadzone * 0.5f)
        {
            isReadyToSnapTurn = true;
        }
    }

    /// <summary>
    /// Rotates the player object. The CharacterControllerDriver script
    /// will handle collision and offset correction automatically.
    /// </summary>
    private void PerformSnapTurn(float angle)
    {
        if (characterController == null) return;

        // 1. Just rotate the parent transform.
        // The CharacterControllerDriver.Update() will see the new offset
        // between the (rotated) collider and the camera, and will
        // call characterController.Move() to fix it safely.
        transform.Rotate(Vector3.up, angle);
    }
}

