using UnityEngine;

/// <summary>
/// Continuously rotates the object while it is selected by an interactor.
/// </summary>
public class Action_Rotate : MonoBehaviour
{
    [Tooltip("The speed at which the object will rotate.")]
    public float rotationSpeed = 50.0f;
    [Tooltip("The axis around which the object will rotate.")]
    public Vector3 rotationAxis = Vector3.up;

    private bool isBeingRotated = false;

    // This method will be called by the "When Select" event.
    public void StartRotation()
    {
        isBeingRotated = true;
    }

    // This method will be called by the "When Unselect" event.
    public void StopRotation()
    {
        isBeingRotated = false;
    }

    void Update()
    {
        // If the flag is true, rotate the object.
        if (isBeingRotated)
        {
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}

