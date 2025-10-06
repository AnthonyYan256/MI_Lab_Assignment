using UnityEngine;

public class PhysicsPointer_2 : MonoBehaviour
{
    [Tooltip("Set this in the Inspector to identify the controller.")]
    public ControllerHand hand; // The identity of this pointer.

    public float defaultLength = 8.0f;
    private LineRenderer lineRenderer = null;
    private HoverableManager _currentManager = null;

    // (Awake and Update methods are unchanged)
    #region Unchanged Methods
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdatePointer();
    }
    #endregion

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

    private void ProcessHover(RaycastHit hit)
    {
        HoverableManager hitManager = hit.collider?.GetComponent<HoverableManager>();

        if (_currentManager != hitManager)
        {
            // Pass the hand identity when exiting the old manager.
            _currentManager?.OnPointerExit(hand);
            
            _currentManager = hitManager;

            // Pass the hand identity when entering the new manager.
            _currentManager?.OnPointerEnter(hand);
        }
    }

    // (CreateForwardRaycast method is unchanged)
    #region Unchanged Methods
    private RaycastHit CreateForwardRaycast()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, defaultLength);
        return hit;
    }
    #endregion
}