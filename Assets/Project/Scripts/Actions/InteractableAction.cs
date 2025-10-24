using UnityEngine;
// Import the EventSystems namespace to check for UI
using UnityEngine.EventSystems;

/// <summary>
/// This is a base class for all "action" scripts (like Translate, Rotate, etc.).
/// The InteractionMenu will find all components that inherit from this class
/// and manage them.
/// </summary>
public abstract class InteractableAction : MonoBehaviour
{
    // You can add shared properties here in the future,
    // like an "ActionName" string if you want the menu
    // to build itself dynamically.

    /// <summary>
    /// A helper function to check if the controller's pointer is
    /// currently hovering over any UI element.
    /// We will use this to prevent an action (like Translate)
    /// from running when the user is just trying to click a UI button.
    /// </summary>
    /// <returns>True if the pointer is NOT over UI, false if it is.</returns>
    protected virtual bool CanExecuteAction()
    {
        // EventSystem.current.IsPointerOverGameObject() returns true
        // if the pointer (which we set up in the OVRInputModule)
        // is currently on top of a UI element (like a button).
        // We add the "!" to flip it, so this function returns
        // true only when we are *not* over any UI.

        // Return false if there is no EventSystem.
        if (EventSystem.current == null) return false;

        return !EventSystem.current.IsPointerOverGameObject();
    }
}