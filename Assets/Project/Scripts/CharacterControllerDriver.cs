using UnityEngine;

/// <summary>
/// This is the "secret sauce" for most VR CharacterControllers.
/// It runs every frame to ensure the CharacterController's collider
/// is always "driven" by the camera's XZ position.
/// 
/// This version is CORRECTED:
/// Instead of calling Move(), it sets the controller's "center"
/// property to match the camera's local XZ position. This avoids
/// the feedback loop that caused the rapid backward movement.
///
/// NEW UPDATE:
/// - Added a 'deadzoneRadius' to allow for leaning.
/// - The collider's center will only be moved if the head's
///   horizontal position moves *outside* this radius.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class CharacterControllerDriver : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("Drag the 'CenterEyeAnchor' (inside OVRCameraRig/TrackingSpace) here.")]
    public Transform centerEyeAnchor;

    [Header("Tuning")]
    [Tooltip("The radius (in meters) that the head can move before it starts 'pulling' the body collider. Allows for leaning without snap-back.")]
    public float deadzoneRadius = 0.25f; // 25cm deadzone for leaning

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (centerEyeAnchor == null)
        {
            Debug.LogError("CharacterControllerDriver: 'Center Eye Anchor' is not set!");
        }

        if(characterController != null)
        {
            // Note: In a production project, you might want to save the original
            // Y-center, but for most standard capsules, it's half the height.
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (characterController == null || centerEyeAnchor == null) return;

        // 1. Get the camera's position *relative to this player transform*.
        // This is its "local" position.
        Vector3 cameraLocalPos = transform.InverseTransformPoint(centerEyeAnchor.position);

        // 2. Get the controller's current center (which is also a local position).
        Vector3 controllerCenter = characterController.center;

        // --- NEW DEADZONE LOGIC ---

        // 3. Calculate the horizontal offset between camera and collider center.
        Vector3 horizontalOffset = new Vector3(cameraLocalPos.x - controllerCenter.x, 0, cameraLocalPos.z - controllerCenter.z);
        float horizontalDistance = horizontalOffset.magnitude;

        // 4. Only move the collider center if the head has moved outside the deadzone.
        if (horizontalDistance > deadzoneRadius)
        {
            // 5. Calculate how much we need to move the center to bring it back to the *edge* of the deadzone.
            float moveAmount = horizontalDistance - deadzoneRadius;
            Vector3 moveDirection = horizontalOffset.normalized;

            // 6. Calculate the new center position.
            Vector3 newCenter = controllerCenter + moveDirection * moveAmount;

            // 7. Set the controller's new center, keeping the original Y.
            characterController.center = new Vector3(newCenter.x,
                                                     controllerCenter.y,
                                                     newCenter.z);
        }
        // If (horizontalDistance <= deadzoneRadius), we do nothing.
        // This allows the head to move freely (lean) within the deadzone
        // without moving the collider and causing a snap-back.
    }
}

