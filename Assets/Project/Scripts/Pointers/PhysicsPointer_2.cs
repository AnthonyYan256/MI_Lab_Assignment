// PhysicsPointer_2.cs
using UnityEngine;

public class PhysicsPointer_2 : MonoBehaviour
{
    [Tooltip("The default length of the laser pointer when it's not hitting anything.")]
    public float defaultLength = 8.0f;

    private LineRenderer lineRenderer = null;
    
    // This will store the object we are currently hovering over
    private IHoverable _currentHoverable = null;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdatePointer();
    }

    private void UpdatePointer()
    {
        RaycastHit hit = CreateForwardRaycast();
        Vector3 endPosition = transform.position + (transform.forward * defaultLength);

        // If we hit something, update the end position
        if (hit.collider != null)
        {
            endPosition = hit.point;
        }
        
        // Update the line renderer
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);

        // Check for hoverable objects
        ProcessHover(hit);
    }

    private void ProcessHover(RaycastHit hit)
    {
        // Try to get an IHoverable component from the object we hit
        IHoverable hitHoverable = hit.collider?.GetComponent<IHoverable>();

        // If we are no longer hovering over the same object
        if (_currentHoverable != hitHoverable)
        {
            // If we were hovering over something before, tell it to exit
            _currentHoverable?.OnHoverExit();
            
            // Update the current hoverable object and tell it to enter
            _currentHoverable = hitHoverable;
            _currentHoverable?.OnHoverEnter();
        }
    }

    private RaycastHit CreateForwardRaycast()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, defaultLength);
        return hit;
    }
}