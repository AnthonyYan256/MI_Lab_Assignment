using UnityEngine;
using UnityEngine.Events; // Required for UnityEvent
using UnityEngine.UI; // Required for Image

/// <summary>
/// A "physics-based" button that can be clicked by the PhysicsPointer.
/// This script replaces the standard Button component and Graphic Raycaster system.
/// It uses a 3D collider to be "hit" by the physics raycast.
/// </summary>
[RequireComponent(typeof(Collider))] // Must have a 3D collider
[RequireComponent(typeof(Image))]
public class PhysicsButton : MonoBehaviour
{
    [Tooltip("The event fired when this button is clicked by the PhysicsPointer.")]
    public UnityEvent OnClick;

    [Header("Visual Feedback")]
    [Tooltip("The color the button will tint to when hovered.")]
    public Color hoverColor = new Color(0.8f, 0.8f, 0.8f);

    private Image buttonImage;
    private Color originalColor;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
        originalColor = buttonImage.color;
    }

    /// <summary>
    /// Called by the PhysicsPointer when it hits this button.
    /// </summary>
    public void Click()
    {
        // Fire the UnityEvent, just like a normal button's OnClick()
        if (OnClick != null)
        {
            OnClick.Invoke();
        }
    }

    /// <summary>
    /// Called by the PhysicsPointer when its ray enters the collider.
    /// </summary>
    public void OnPointerEnter()
    {
        buttonImage.color = hoverColor;
    }

    /// <summary>
    /// Called by the PhysicsPointer when its ray exits the collider.
    /// </summary>
    public void OnPointerExit()
    {
        buttonImage.color = originalColor;
    }
}
