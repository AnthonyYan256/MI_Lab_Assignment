using UnityEngine;
using UnityEngine.Events; // Required for UnityEvent

/// <summary>
/// This script makes a 3D object with a collider act like a UI button
/// for the physics-based pointer.
/// 
/// It listens for the "A" button (OVRInput.Button.One) from the
/// physics pointer and fires a UnityEvent, just like a regular UI button.
/// </summary>
public class VRPhysicsButton : MonoBehaviour
{
    [Tooltip("The UnityEvent to fire when this button is 'clicked' with Button A.")]
    public UnityEvent OnClick;

    // We only want to fire the click event once per press,
    // so we need to track the button state.
    private bool isButtonDown = false;

    // This method is called by the PhysicsPointer_2 script.
    // We can't use OnHoverEnter/OnHoverExit because those are
    // for IHoverable (which is on the main object), not the buttons.
    // Instead, this script will be checked directly by the pointer.

    void Update()
    {
        // Check if either pointer is hovering over us
        bool isHovered = false;
        if (PhysicsPointer_2.LeftInstance != null && PhysicsPointer_2.LeftInstance.HoveredVRButton == this)
        {
            isHovered = true;
        }
        else if (PhysicsPointer_2.RightInstance != null && PhysicsPointer_2.RightInstance.HoveredVRButton == this)
        {
            isHovered = true;
        }

        if (isHovered)
        {
            // Pointers are hovering. Check for the "Click" button (A on Right, X on Left)
            // OVRInput.Button.One is 'A' on the Right Controller
            // OVRInput.Button.Three is 'X' on the Left Controller
            bool clickButtonPressed = OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch) ||
                                      OVRInput.Get(OVRInput.Button.Three, OVRInput.Controller.LTouch);

            if (clickButtonPressed)
            {
                if (!isButtonDown)
                {
                    // This is the first frame the button is down, fire the event!
                    isButtonDown = true;
                    OnClick.Invoke();
                }
            }
            else
            {
                // Button is not pressed, reset the flag.
                isButtonDown = false;
            }
        }
        else
        {
            // Not hovering, reset the flag.
            isButtonDown = false;
        }
    }
}

