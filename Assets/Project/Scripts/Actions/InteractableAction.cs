using UnityEngine;

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
}
