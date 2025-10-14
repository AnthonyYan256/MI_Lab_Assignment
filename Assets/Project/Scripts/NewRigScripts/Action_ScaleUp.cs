using UnityEngine;

/// <summary>
/// Increases the object's scale by a fixed amount when called.
/// </summary>
public class Action_ScaleUp : MonoBehaviour
{
    [Tooltip("The amount to increase the scale by on each click.")]
    public float scaleFactor = 0.1f;

    // Call this from the "When Select" event for a single action.
    public void PerformScaleUp()
    {
        transform.localScale += new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }
}
