using UnityEngine;

public class Rotatable : MonoBehaviour, IHoverable
{
    public float rotationSpeed = 50.0f;
    public Vector3 rotationAxis = Vector3.up;

    // Tracks if the interaction-enabled controller is hovering.
    private bool isRightHandHovering = false;

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
        // Checks for the primary button press on the active controller.
        if (isRightHandHovering && OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}