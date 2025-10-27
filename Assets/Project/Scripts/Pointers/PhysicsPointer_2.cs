using UnityEngine;
using System.Linq; // Required for sorting the raycast hits

/// <summary>
/// A more robust version of the Physics Pointer.
/// 
/// --- MODIFIED ---
/// - Class renamed to PhysicsPointer_2
/// - Added 'CurrentHit' property to expose the full RaycastHit
///   information for other scripts (like the slider) to use.
/// - ... (other modifications from before)
/// </summary>
public class PhysicsPointer_2 : MonoBehaviour // <-- RENAMED
{
    [Header("Setup")]
    [Tooltip("Set this in the Inspector to identify the controller (Left or Right).")]
    public ControllerHand hand; // The identity of this pointer.

    [Tooltip("The default length of the laser pointer when it's not hitting anything.")]
    public float defaultLength = 20.0f;

    [Tooltip("Set this to the 'Player' layer to prevent the ray from hitting the player's own body.")]
    public LayerMask layersToIgnore; // <-- IMPORTANT: Set this in the Inspector

    // --- Private ---
    private LineRenderer lineRenderer = null;
    private IHoverable _currentHoverable = null;

    // --- Public Properties (for InteractableAction) ---
    [HideInInspector]
    public VRPhysicsButton HoveredVRButton { get; private set; }
    
    [HideInInspector]
    public Collider HoveredCollider { get; private set; }
    
    /// <summary>
    /// Stores the full raycast hit details from the last frame.
    /// This is used by scripts like the VRPhysicsSlider.
    /// </summary>
    [HideInInspector]
    public RaycastHit CurrentHit { get; private set; } 

    // --- Static Instances (for InteractableAction) ---
    /// <summary>
    /// Static reference to the Left Hand's pointer.
    /// </summary>
    public static PhysicsPointer_2 LeftInstance { get; private set; } // <-- RENAMED
    
    /// <summary>
    /// Static reference to the Right Hand's pointer.
    /// </summary>
    public static PhysicsPointer_2 RightInstance { get; private set; } // <-- RENAMED


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (hand == ControllerHand.Left)
        {
            LeftInstance = this;
        }
        else if (hand == ControllerHand.Right)
        {
            RightInstance = this;
        }
    }

    private void LateUpdate()
    {
        CheckForMenuClick();
        UpdatePointer();
    }

    private void CheckForMenuClick()
    {
        if (_currentHoverable == null) return;

        bool menuButtonPressed = false;
        if (hand == ControllerHand.Right)
        {
            menuButtonPressed = OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch);
        }
        else if (hand == ControllerHand.Left)
        {
            menuButtonPressed = OVRInput.GetDown(OVRInput.Button.Four, OVRInput.Controller.LTouch);
        }

        if (menuButtonPressed)
        {
            _currentHoverable.OnPointerClick();
        }
    }

    private void UpdatePointer()
    {
        RaycastHit hit = CreateForwardRaycast(); // Use our new robust raycast
        CurrentHit = hit; 
        
        Vector3 endPosition = transform.position + (transform.forward * defaultLength);

        if (hit.collider != null)
        {
            endPosition = hit.point;
        }
        
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);

        ProcessHover(hit);
    }

    private void ProcessHover(RaycastHit hit)
    {
        // Reset public properties
        HoveredVRButton = null;
        HoveredCollider = hit.collider; 

        IHoverable hitHoverable = hit.collider?.GetComponent<IHoverable>();

        if (hit.collider != null)
        {
            HoveredVRButton = hit.collider.GetComponent<VRPhysicsButton>();
        }
        
        if (_currentHoverable != hitHoverable)
        {
            _currentHoverable?.OnHoverExit(hand);
            _currentHoverable = hitHoverable;
            _currentHoverable?.OnHoverEnter(hand);
        }
    }

    private RaycastHit CreateForwardRaycast()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        
        int layerMask = ~layersToIgnore; 
        
        RaycastHit[] hits = Physics.RaycastAll(ray, defaultLength, layerMask, QueryTriggerInteraction.Ignore);
        
        RaycastHit closestHit = new RaycastHit(); 

        if (hits.Length > 0)
        {
            var sortedHits = hits.OrderBy(h => h.distance);

            foreach (RaycastHit hit in sortedHits)
            {
                // This filters out the collider we are starting inside of
                if (hit.distance == 0)
                {
                    continue; 
                }
                
                // Once we find the first valid hit, it's the closest one.
                closestHit = hit;
                break; 
            }
        }
        
        return closestHit; 
    }
}

