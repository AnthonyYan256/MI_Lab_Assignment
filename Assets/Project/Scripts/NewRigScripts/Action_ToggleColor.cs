using UnityEngine;

/// <summary>
/// Toggles the object's color between its original and a new color.
/// </summary>
public class Action_ToggleColor : MonoBehaviour
{
    [Tooltip("The new color to apply to the object.")]
    public Color newColor = Color.blue;

    private Renderer objectRenderer;
    private Color originalColor;
    private bool isColorChanged = false;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }

    // Call this from the "When Select" event.
    public void PerformToggleColor()
    {
        if (objectRenderer == null) return;

        isColorChanged = !isColorChanged;
        objectRenderer.material.color = isColorChanged ? newColor : originalColor;
    }
}
