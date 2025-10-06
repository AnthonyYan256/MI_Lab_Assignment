using UnityEngine;

public class ChangeColorable : MonoBehaviour, IHoverable
{
    public Color newColor = Color.blue;

    private Renderer objectRenderer;
    private Color originalColor;
    private bool isColorChanged = false;
    
    // Tracks if the interaction-enabled controller is hovering.
    private bool isRightHandHovering = false;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }

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
        if (isRightHandHovering && OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            ToggleColor();
        }
    }

    private void ToggleColor()
    {
        if (objectRenderer == null) return;

        isColorChanged = !isColorChanged;
        objectRenderer.material.color = isColorChanged ? newColor : originalColor;
    }
}