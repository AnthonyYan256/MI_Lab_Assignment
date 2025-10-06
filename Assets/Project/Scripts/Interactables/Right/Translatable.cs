using UnityEngine;

public class Translatable : MonoBehaviour, IHoverable
{
    public float moveSpeed = 1.0f;

    // We only need to track the right hand's hover state now.
    private bool isRightHandHovering = false;

    // This method now only cares if the right hand enters.
    public void OnHoverEnter(ControllerHand hand)
    {
        if (hand == ControllerHand.Right)
        {
            isRightHandHovering = true;
        }
    }

    // This method now only cares if the right hand exits.
    public void OnHoverExit(ControllerHand hand)
    {
        if (hand == ControllerHand.Right)
        {
            isRightHandHovering = false;
        }
    }

    void Update()
    {
        // We only check for right-hand input ('A' button).
        // The check for the left hand has been completely removed.
        if (isRightHandHovering && OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}