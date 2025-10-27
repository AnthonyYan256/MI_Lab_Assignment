using UnityEngine;

/// <summary>
/// Action script for continuous translation while Button A is held.
/// Inherits from InteractableAction and is managed by InteractionMenu.
/// Conforms to Assignment 3 specifications.
/// </summary>
public class Action_Translate : InteractableAction
{
    [Tooltip("The speed at which the object will move upwards.")]
    public float moveSpeed = 1.0f;

    /// <summary>
    /// Update is called every frame, but this script is only enabled by the
    /// InteractionMenu when the 'Translate' mode is selected.
    /// </summary>
    void Update()
    {
        // ** THE FIX IS HERE: Added "IsPointerHoveringThisObject()" check **
        // This stops the action from running if the pointer is over any UI,
        // AND only runs if the pointer is hovering this object.
        if (CanExecuteAction() && IsPointerHoveringThisObject() && OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            // Move the object up along the world's Y-axis while the button is held.
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
