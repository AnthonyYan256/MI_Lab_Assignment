using UnityEngine;

/// <summary>
/// Action script for continuous rotation while Button A is held.
/// Inherits from InteractableAction and is managed by InteractionMenu.
/// This version is compatible with the PhysicsButton system.
/// </summary>
public class P_Action_Rotate : P_InteractableAction
{
    [Tooltip("The speed at which the object will rotate.")]
    public float rotationSpeed = 50.0f;
    [Tooltip("The axis around which the object will rotate.")]
    public Vector3 rotationAxis = Vector3.up;

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
    /// InteractionMenu when the 'Rotate' mode is selected.
    /// </summary>
    void Update()
    {
        // We check if the menu is null (as a safety precaution) and
        // if the menu is NOT open before checking for the button press.
        if (interactionMenu != null && !interactionMenu.IsMenuOpen &&
            OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            // Rotate the object around the specified axis while the button is held.
            transform.Rotate(rotationAxis.normalized, rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}

