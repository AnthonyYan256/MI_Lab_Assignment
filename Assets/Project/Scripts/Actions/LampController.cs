using UnityEngine;

/// <summary>
/// ...
/// </summary>
// We removed [RequireComponent(typeof(Light))] from here
public class LampController : MonoBehaviour
{
    [Header("Lamp Setup")]
    [Tooltip("Drag your child 'BulbLight' GameObject (the one with the Light component) here.")]
    public Light lampLight; // Changed from private to public

    /// <summary>
    /// ...
    /// </summary>
    void Awake()
    {
        // We removed this line: lampLight = GetComponent<Light>();
        
        // Start with the light off by default.
        if (lampLight != null)
        {
            lampLight.enabled = false;
        }
    }

    /// <summary>
    /// Toggles the lamp's Light component on or off.
    /// This is called by the 'Power' UI Button.
    /// </summary>
    public void TogglePower()
    {
        if (lampLight != null)
        {
            lampLight.enabled = !lampLight.enabled;
        }
    }

    /// <summary>
    /// Sets the intensity of the Light component.
    /// This is called by the 'Intensity' UI Slider's OnValueChanged event.
    /// </summary>
    /// <param name="newIntensity">A value between 0.0 and 1.0 from the slider.</param>
    public void SetIntensity(float newIntensity)
    {
        if (lampLight != null)
        {
            // The slider's 0-1 value maps directly to the light's intensity
            lampLight.intensity = newIntensity;
        }
    }
}