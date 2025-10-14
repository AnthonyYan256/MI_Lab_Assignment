using UnityEngine;

/// <summary>
/// Teleports the player to this object's location when called.
/// </summary>
public class Action_TeleportPlayer : MonoBehaviour
{
    [Header("Required References")]
    [Tooltip("The main player rig to teleport (the OVRInteractionComprehensive object).")]
    public Transform playerRig;
    [Tooltip("The head/camera of the player rig (the CharacterHead object).")]
    public Transform head;

    // Call this from the "When Select" event on your teleport sphere.
    public void PerformTeleport()
    {
        if (playerRig == null || head == null)
        {
            Debug.LogError("Player Rig or Head is not assigned. Cannot teleport.");
            return;
        }

        // Calculate the head's horizontal offset from the rig's center.
        Vector3 headOffset = head.localPosition;
        headOffset.y = 0; 

        // The destination we want our head to end up at, maintaining player height.
        Vector3 destinationPosition = new Vector3(transform.position.x, playerRig.position.y, transform.position.z);

        // The correct position for the rig is the destination minus the head's offset.
        Vector3 newRigPosition = destinationPosition - headOffset;
        
        // Temporarily disable the CharacterController to teleport.
        CharacterController characterController = playerRig.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        playerRig.position = newRigPosition;

        if (characterController != null)
        {
            characterController.enabled = true;
        }

        // The teleport sphere disappears after use.
        gameObject.SetActive(false);
    }
}
