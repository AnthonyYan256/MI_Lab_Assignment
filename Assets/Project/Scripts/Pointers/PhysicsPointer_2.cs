using UnityEngine;

/// <summary>
/// This script casts a ray from the controller to find IHoverable objects.
/// It passes hover (Enter/Exit) and click events to the IHoverable component it finds.
/// This version is compatible with the new modular InteractionMenu system.
/// </summary>
public class PhysicsPointer_2 : MonoBehaviour
{
    [Tooltip("Set this in the Inspector to identify the controller (Left or Right).")]
    public ControllerHand hand; // The identity of this pointer.

    [Tooltip("The default length of the laser pointer when it's not hitting anything.")]
    public float defaultLength = 20.0f;

    private LineRenderer lineRenderer = null;
    
    // This now stores the IHoverable component we are currently hovering over (e.g., the InteractionMenu).
    private IHoverable _currentHoverable = null;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        // Check for the "Menu Open" button (Button B)
        CheckForMenuClick();

        // Then, update the raycast and check for hover state changes.
        UpdatePointer();
    }

    /// <summary>
    /// Checks for the correct button press to open the menu
    /// and sends a click event if we are hovering over an object.
    /// </summary>
    private void CheckForMenuClick()
    {
        if (_currentHoverable == null) return;

        bool menuButtonPressed = false;
        if (hand == ControllerHand.Right)
        {
            // 'B' Button on the right controller
            menuButtonPressed = OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch);
        }
        else if (hand == ControllerHand.Left)
        {
            // 'Y' Button on the left controller
            menuButtonPressed = OVRInput.GetDown(OVRInput.Button.Four, OVRInput.Controller.LTouch);
        }

        if (menuButtonPressed)
        {
            _currentHoverable.OnPointerClick();
        }
    }

    /// <summary>
    /// Updates the raycast, draws the line, and processes hover logic.
    /// </summary>
    private void UpdatePointer()
    {
        RaycastHit hit = CreateForwardRaycast();
        Vector3 endPosition = transform.position + (transform.forward * defaultLength);

        if (hit.collider != null)
        {
            endPosition = hit.point;
        }
        
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);

        ProcessHover(hit);
    }

    /// <summary>
    /// Compares the currently hovered object with the one from the last frame
    /// and sends OnHoverEnter or OnHoverExit events as needed.
    /// </summary>
    private void ProcessHover(RaycastHit hit)
    {
        // Get the IHoverable component directly from the object we hit.
        IHoverable hitHoverable = hit.collider?.GetComponent<IHoverable>();

        // If the object we're hovering over has changed...
        if (_currentHoverable != hitHoverable)
        {
            // Tell the old object we are exiting (and pass our hand identity).
            _currentHoverable?.OnHoverExit(hand);
            
            // Store the new object as the current one.
            _currentHoverable = hitHoverable;
            
            // Tell the new object we are entering (and pass our hand identity).
            _currentHoverable?.OnHoverEnter(hand);
        }
    }

    /// <summary>
    /// Creates the physics raycast from the controller.
    /// </summary>
    private RaycastHit CreateForwardRaycast()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, defaultLength);
        return hit;
    }
}

