using UnityEngine;

/// <summary>
/// Action script for continuous translation while Button A is held.
/// Inherits from InteractableAction and is managed by InteractionMenu.
/// This version is compatible with the PhysicsButton system.
/// </summary>
public class P_Action_Translate : P_InteractableAction
{
    [Tooltip("The speed at which the object will move.")]
    public float moveSpeed = 1.0f;
    [Tooltip("The direction of movement in world space.")]
    public Vector3 moveDirection = Vector3.up;

    /// <summary>
    /// We override the parent's Awake() but must also call it
    /// to ensure the 'interactionMenu' reference is set.
    /// </summary>
    protected override void Awake()
    {
        // This is crucial! It calls the parent's Awake() 
        // to find and assign the interactionMenu.
        base.Awake();
    }

    /// <summary>
    /// Update is called every frame, but this script is only enabled by the
    /// InteractionMenu when the 'Translate' mode is selected.
    /// </summary>
    void Update()
    {
        // We check if the menu is null (as a safety precaution) and
        // if the menu is NOT open before checking for the button press.
        if (interactionMenu != null && !interactionMenu.IsMenuOpen &&
            OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            // Move the object in the specified direction while the button is held.
            transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
