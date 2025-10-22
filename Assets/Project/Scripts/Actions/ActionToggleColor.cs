using UnityEngine;

/// <summary>
/// Action script for toggling color on a single press of Button A.
/// Inherits from InteractableAction and is managed by InteractionMenu.
/// Conforms to Assignment 3 specifications.
/// </summary>
public class Action_ToggleColor : InteractableAction
{
    [Tooltip("The alternate color to apply to the object.")]
    public Color newColor = Color.blue;

    private Renderer objectRenderer;
    private Color originalColor;
    private bool isColorChanged = false;

    /// <summary>
    /// Called when the script instance is being loaded. Caches the Renderer and original color.
    /// </summary>
    void Start()
    {
        // Get the Renderer component to modify its material.
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            // Store the initial color of the object's material.
            originalColor = objectRenderer.material.color;
        }
        else
        {
            Debug.LogWarning("Action_ToggleColor: No Renderer found on this object.", this);
        }
    }

    /// <summary>
    /// Update is called every frame, but this script is only enabled by the
    /// InteractionMenu when the 'Change Color' mode is selected.
    /// </summary>
    void Update()
    {
        // Check if the primary button ('A' on the right controller) was pressed down this frame.
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            PerformToggleColor();
        }
    }

    /// <summary>
    /// Toggles the object's material color between the original and the newColor.
    /// </summary>
    private void PerformToggleColor()
    {
        if (objectRenderer == null) return; // Exit if no renderer was found.

        // Flip the boolean flag.
        isColorChanged = !isColorChanged;

        // Apply the appropriate color based on the flag's state.
        objectRenderer.material.color = isColorChanged ? newColor : originalColor;
    }
}

