using UnityEngine;

/// <summary>
/// Action script for teleporting the player on a single press of Button A.
/// Inherits from InteractableAction and is managed by InteractionMenu.
/// Conforms to Assignment 3 specifications.
/// </summary>
public class Action_TeleportPlayer : InteractableAction
{
    [Header("Required References")]
    [Tooltip("The main player object to teleport (the one with the CharacterController).")]
    public Transform playerRig;
    [Tooltip("The head/camera of the player rig (e.g., CenterEyeAnchor or CharacterHead).")]
    public Transform head;

    // Reference to the menu script on the same object to close it after teleporting.
    private InteractionMenu interactionMenu;

    /// <summary>
    /// Called when the script instance is being loaded. Caches the InteractionMenu.
    /// </summary>
    void Awake()
    {
        // Get a reference to the master menu script on this same GameObject.
        interactionMenu = GetComponent<InteractionMenu>();
        if (interactionMenu == null)
        {
            Debug.LogError("Action_TeleportPlayer: Could not find InteractionMenu script on the same object.", this);
        }
    }

    /// <summary>
    /// Update is called every frame, but this script is only enabled by the
    /// InteractionMenu when the 'Teleport' mode is selected.
    /// </summary>
    void Update()
    {
        // Check if the primary button ('A' on the right controller) was pressed down this frame.
        if (CanExecuteAction() && IsPointerHoveringThisObject() && OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            PerformTeleport();
        }
    }

    /// <summary>
    /// Calculates the correct teleport position and moves the player rig.
    /// Also disables the sphere and closes the menu afterwards.
    /// </summary>
    private void PerformTeleport()
    {
        // --- 1. Validate References ---
        if (playerRig == null || head == null)
        {
            Debug.LogError("Action_TeleportPlayer: Player Rig or Head is not assigned. Cannot teleport.", this);
            return;
        }

        // --- 2. Calculate Teleport Position ---
        // Get the horizontal offset of the head (camera) from the rig's center point.
        Vector3 headOffset = head.localPosition;
        headOffset.y = 0; // Ignore vertical offset for ground-level teleportation.

        // Determine the target position for the player's head, keeping their current height.
        Vector3 destinationPosition = new Vector3(transform.position.x, playerRig.position.y, transform.position.z);

        // Calculate the required position for the rig itself so the head lands at the destination.
        Vector3 newRigPosition = destinationPosition - headOffset;

        // --- 3. Move the Player Rig ---
        // Safely move the CharacterController by temporarily disabling it.
        CharacterController characterController = playerRig.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        // Apply the calculated position to the player rig.
        playerRig.position = newRigPosition;

        // Re-enable the CharacterController.
        if (characterController != null)
        {
            characterController.enabled = true;
        }

        // --- 4. Post-Teleport Cleanup ---
        // Disable the sphere GameObject as required by the assignment.
        gameObject.SetActive(false);

        // Close the interaction menu and disable this action script.
        if (interactionMenu != null)
        {
            interactionMenu.CloseMenu();
            interactionMenu.DisableAllInteractions(); // Disables this script as well
        }
    }
}

