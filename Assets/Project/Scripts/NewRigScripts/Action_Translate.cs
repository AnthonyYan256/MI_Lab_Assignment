using UnityEngine;

/// <summary>
/// Continuously moves the object up while it is selected by an interactor.
/// </summary>
public class Action_Translate : MonoBehaviour
{
    [Tooltip("The speed at which the object will move upwards.")]
    public float moveSpeed = 1.0f;

    private bool isBeingTranslated = false;

    // This method will be called by the "When Select" event.
    public void StartTranslation()
    {
        isBeingTranslated = true;
    }

    // This method will be called by the "When Unselect" event.
    public void StopTranslation()
    {
        isBeingTranslated = false;
    }

    void Update()
    {
        // If the flag is true, move the object.
        if (isBeingTranslated)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
