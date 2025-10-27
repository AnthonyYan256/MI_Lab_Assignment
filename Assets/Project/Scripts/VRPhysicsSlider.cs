using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This script makes a 3D object with a BoxCollider act like a UI Slider
/// for the physics-based pointer.
/// 
/// It listens for the "A" button (OVRInput.Button.One) being *held*
/// while the pointer is over it. It calculates the value based on
/// the hit point and fires a UnityEvent<float>.
/// 
/// This version is updated to reference PhysicsPointer_2.
/// --- MODIFIED ---
/// - Removed minValue and maxValue fields to simplify the script.
/// - The OnValueChanged event now *directly* invokes the 0-1 normalized value.
/// - Re-wrote value calculation logic to be robust and support any BoxCollider.center.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class VRPhysicsSlider : MonoBehaviour
{
    [Header("Slider Visuals")]
    [Tooltip("The RectTransform of the 'Fill' object.")]
    public RectTransform fillRect;
    [Tooltip("The RectTransform of the 'Handle' object.")]
    public RectTransform handleRect;

    [Header("Slider Event")]
    [Tooltip("This event fires when the slider value changes, passing a 0.0-1.0 value.")]
    public UnityEvent<float> OnValueChanged;

    private BoxCollider sliderCollider;
    private float currentValue; // Will store the 0-1 value

    void Awake()
    {
        // Get the collider that defines our "track"
        sliderCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        // 1. Find which pointer (if any) is hovering over our collider
        PhysicsPointer_2 activePointer = null; // <-- References PhysicsPointer_2
        if (PhysicsPointer_2.LeftInstance != null && PhysicsPointer_2.LeftInstance.HoveredCollider == sliderCollider)
        {
            activePointer = PhysicsPointer_2.LeftInstance; // <-- References PhysicsPointer_2
        }
        else if (PhysicsPointer_2.RightInstance != null && PhysicsPointer_2.RightInstance.HoveredCollider == sliderCollider)
        {
            activePointer = PhysicsPointer_2.RightInstance; // <-- References PhysicsPointer_2
        }

        // If no pointer is hovering, do nothing.
        if (activePointer == null) return;

        // 2. Check if the "click" button is being held down
        // OVRInput.Button.One is 'A' on the Right Controller
        // OVRInput.Button.Three is 'X' on the Left Controller
        bool clickButtonHeld = OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch) ||
                               OVRInput.Get(OVRInput.Button.Three, OVRInput.Controller.LTouch);

        if (clickButtonHeld)
        {
            // 3. Get the hit information from the pointer
            RaycastHit hit = activePointer.CurrentHit;
            
            // 4. Convert the world-space hit point to this object's local space
            Vector3 localHitPoint = transform.InverseTransformPoint(hit.point);

            // 5. Calculate the normalized value (0.0 to 1.0)
            // --- NEW ROBUST MATH ---
            float sliderWidth = sliderCollider.size.x;
            float colliderCenterX = sliderCollider.center.x;

            // Find the local X position of the collider's left edge
            float leftEdge = colliderCenterX - (sliderWidth / 2f);

            // Calculate how far the hit is from the left edge
            float valueOnTrack = localHitPoint.x - leftEdge;
            
            // Normalize this value by dividing by the total width
            float normalizedValue = valueOnTrack / sliderWidth;
            normalizedValue = Mathf.Clamp01(normalizedValue); // Ensure it's between 0 and 1

            // 6. Invoke the event with the normalized 0-1 value
            currentValue = normalizedValue;
            OnValueChanged.Invoke(currentValue);

            // 7. Update the visuals
            UpdateVisuals(normalizedValue);
        }
    }

    /// <summary>
    /// Updates the slider's Fill and Handle visuals based on the
    /// normalized (0-1) value.
    /// </summary>
    private void UpdateVisuals(float normalizedValue)
    {
        // This logic assumes a standard Left-to-Right slider setup
        // where the Fill object's anchorMax.x controls its width.
        if (fillRect != null)
        {
            fillRect.anchorMax = new Vector2(normalizedValue, fillRect.anchorMax.y);
        }

        // This assumes the Handle's anchorMin.x and anchorMax.x
        // are both set to the normalized value to position it.
        if (handleRect != null)
        {
            handleRect.anchorMin = new Vector2(normalizedValue, handleRect.anchorMin.y);
            handleRect.anchorMax = new Vector2(normalizedValue, handleRect.anchorMax.y);
        }
    }
}

