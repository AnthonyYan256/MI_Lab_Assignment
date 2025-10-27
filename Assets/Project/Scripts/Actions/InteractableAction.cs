using UnityEngine;

/// <summary>
/// This is a base class for all "action" scripts (like Translate, Rotate, etc.).
/// The InteractionMenu will find all components that inherit from this class
/// and manage them.
///
/// --- MODIFIED ---
/// - CanExecuteAction() now checks for the 'VRPhysicsButton' component
///   via the pointer, instead of checking for a tag.
/// - Includes 'IsPointerHoveringThisObject()' helper method.
/// </summary>
public abstract class InteractableAction : MonoBehaviour
{
    /// <summary>
    /// A helper function to check if the controller's pointer is
    /// currently hovering over any VRPhysicsButton.
    /// We use this to prevent an action (like Translate)
    /// from running when the user is just trying to click a menu button.
    /// </summary>
    /// <returns>True if the pointer is NOT over a button, false if it is.</returns>
    protected virtual bool CanExecuteAction()
    {
        // Check if either pointer is currently hovering over an object
        // that has a VRPhysicsButton component.
        bool leftHover = PhysicsPointer_2.LeftInstance != null && PhysicsPointer_2.LeftInstance.HoveredVRButton != null;
        bool rightHover = PhysicsPointer_2.RightInstance != null && PhysicsPointer_2.RightInstance.HoveredVRButton != null;
        
        // Return true (can execute) only if NEITHER pointer is hovering a button.
        return !(leftHover || rightHover);
    }

    /// <summary>
    /// A helper function to check if the pointer is hovering over
    /// THIS specific interactable object (the one this script is on).
    /// </summary>
    /// <returns>True if either pointer is hovering this object.</returns>
    protected virtual bool IsPointerHoveringThisObject()
    {
        Collider myCollider = GetComponent<Collider>();
        if (myCollider == null) return false;

        // Check if either pointer's 'HoveredCollider' is our own collider.
        bool leftHover = PhysicsPointer_2.LeftInstance != null && PhysicsPointer_2.LeftInstance.HoveredCollider == myCollider;
        bool rightHover = PhysicsPointer_2.RightInstance != null && PhysicsPointer_2.RightInstance.HoveredCollider == myCollider;

        return leftHover || rightHover;
    }
}

