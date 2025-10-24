using UnityEngine;

/// <summary>
/// This is a base class for all "action" scripts (like Translate, Rotate, etc.).
/// The InteractionMenu will find all components that inherit from this class
/// and manage them.
/// 
/// This version is updated to support the "PhysicsButton" system.
/// It gets a reference to the InteractionMenu so child actions
/// can check if the menu is open.
/// </summary>
public abstract class P_InteractableAction : MonoBehaviour
{
    // A protected reference to the main menu script.
    // Child classes (like Action_Translate) can access this.
    protected P_InteractionMenu interactionMenu;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// We make this 'virtual' so child classes can override it,
    /// but they *must* call 'base.Awake()' to ensure this code runs.
    /// </summary>
    protected virtual void Awake()
    {
        // Get the InteractionMenu component from this same GameObject.
        interactionMenu = GetComponent<P_InteractionMenu>();
        if (interactionMenu == null)
        {
            Debug.LogError("InteractableAction cannot find its InteractionMenu component.", this);
        }
    }
}

