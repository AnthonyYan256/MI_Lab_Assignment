using UnityEngine;

/// <summary>
/// Action script for continuous rotation while Button A is held.
/// Inherits from InteractableAction and is managed by InteractionMenu.
/// Conforms to Assignment 3 specifications.
/// </summary>
public class Action_Rotate : InteractableAction
{
    [Tooltip("The speed at which the object will rotate.")]
    public float rotationSpeed = 50.0f;
    [Tooltip("The axis around which the object will rotate.")]
    public Vector3 rotationAxis = Vector3.up;

    /// <summary>
    /// Update is called every frame, but this script is only enabled by the
    /// InteractionMenu when the 'Rotate' mode is selected.
    /// </summary>
    void Update()
    {
        // ** THE FIX IS HERE: Added "CanExecuteAction()" check **
        // This stops the action from running if the pointer is over any UI.
        if (CanExecuteAction() && IsPointerHoveringThisObject() && OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            // Rotate the object around the specified axis while the button is held.
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}
