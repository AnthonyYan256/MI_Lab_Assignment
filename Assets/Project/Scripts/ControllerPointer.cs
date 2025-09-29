using UnityEngine;

/// <summary>
/// This script controls a 3D object (a Cube scaled into a beam) to act as a laser pointer.
/// It dynamically adjusts the beam's length based on what it hits.
/// This script should be attached to the RightHandAnchor.
/// </summary>
public class ControllerPointer : MonoBehaviour
{
    [Tooltip("The 3D object that will be used as the laser beam.")]
    public Transform laserBeam; 

    [Tooltip("The default length of the laser pointer when it's not hitting anything.")]
    public float defaultLength = 100.0f;

    void Update()
    {
        if (laserBeam == null) return;
        
        // Create a ray from this object's position (the controller), pointing forward.
        Ray pointerRay = new Ray(transform.position, transform.forward);

        RaycastHit hit;
        // Perform a physics raycast.
        if (Physics.Raycast(pointerRay, out hit, defaultLength))
        {
            // If the ray hits an object, scale the beam to match the distance to the hit point.
            laserBeam.localScale = new Vector3(laserBeam.localScale.x, laserBeam.localScale.y, hit.distance);
        }
        else
        {
            // If the ray doesn't hit anything, scale the beam to the default length.
            laserBeam.localScale = new Vector3(laserBeam.localScale.x, laserBeam.localScale.y, defaultLength);
        }
    }
}

