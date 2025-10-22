using UnityEngine;

/// <summary>
/// Action script for scaling up on a single press of Button A.
/// Inherits from InteractableAction and is managed by InteractionMenu.
/// Conforms to Assignment 3 specifications.
/// </summary>
public class Action_ScaleUp : InteractableAction
{
    [Tooltip("The amount to increase the scale by on each click.")]
    public float scaleFactor = 0.1f;

    /// <summary>
    /// Update is called every frame, but this script is only enabled by the
    /// InteractionMenu when the 'Scale' mode is selected.
    /// </summary>
    void Update()
    {
        // Check if the primary button ('A' on the right controller) was pressed down this frame.
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            // Increase the local scale of the object uniformly by the scale factor.
            transform.localScale += new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
    }
}

