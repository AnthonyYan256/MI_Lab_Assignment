using UnityEngine;

/// <summary>
/// Implements standard "Snap Turning" for a VR player using the OVRInput system.
/// This script should be on the parent PlayerObject, which has the OVRCameraRig
/// as a child. It rotates the PlayerObject, and the camera follows.
/// </summary>
public class VRPlayerRotation : MonoBehaviour
{
    [Tooltip("The number of degrees to snap turn with one flick of the stick.")]
    public float snapAngle = 45f;

    [Tooltip("The threshold the stick must pass to trigger a turn. Prevents drift.")]
    public float thumbstickDeadzone = 0.4f; 

    // We use the Right controller's thumbstick for rotation by default.
    private OVRInput.Controller controller = OVRInput.Controller.RTouch;
    
    // This flag prevents continuous rotation, forcing a "flick" motion.
    private bool isReadyToSnapTurn = true;

    // Note: We no longer need a reference to the cameraRig
    // private OVRCameraRig cameraRig;

    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>
    void Start()
    {
        // We don't need to find the camera rig anymore, as we are
        // only rotating this object's transform.
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
    /// Rotates this player object by the specified angle.
    /// </summary>
    /// <param name="angle">The angle to rotate (e.g., 45 or -45).</param>
    private void PerformSnapTurn(float angle)
    {
        // 1. Rotate this object (the PlayerObject with the collider)
        // The child OVRCameraRig will rotate automatically.
        transform.Rotate(Vector3.up, angle);
    }
}