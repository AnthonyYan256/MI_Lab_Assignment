using UnityEngine;

public class Teleportable : MonoBehaviour, IHoverable
{
    [Tooltip("The main player rig (e.g., OVRCameraRig).")]
    public Transform playerTransform;
    [Tooltip("The camera or 'CenterEyeAnchor' of the OVRCameraRig.")]
    public Transform centerEyeAnchor;

    // Tracks if the interaction-enabled controller is hovering.
    private bool isRightHandHovering = false;

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
    }
    
    // Sets the hover state to true if the interacting hand enters.
    public void OnHoverEnter(ControllerHand hand)
    {
        if (hand == ControllerHand.Right)
        {
            isRightHandHovering = true;
        }
    }

    // Sets the hover state to false if the interacting hand exits.
    public void OnHoverExit(ControllerHand hand)
    {
        if (hand == ControllerHand.Right)
        {
            isRightHandHovering = false;
        }
    }

    void Update()
    {
        // Checks for the secondary button press on the active controller.
        if (isRightHandHovering && OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            TeleportPlayer();
        }
    }

    private void TeleportPlayer()
    {
        if (playerTransform == null || centerEyeAnchor == null)
        {
            Debug.LogError("Player Transform or Center Eye Anchor is not assigned. Cannot teleport.");
            return;
        }

        Vector3 headOffset = centerEyeAnchor.localPosition;
        headOffset.y = 0; 
        Vector3 destinationPosition = new Vector3(transform.position.x, playerTransform.position.y, transform.position.z);
        Vector3 newRigPosition = destinationPosition - headOffset;
        
        CharacterController characterController = playerTransform.GetComponent<CharacterController>();

        if (characterController != null)
        {
            characterController.enabled = false;
        }

        playerTransform.position = newRigPosition;

        if (characterController != null)
        {
            characterController.enabled = true;
        }

        gameObject.SetActive(false);
    }
}