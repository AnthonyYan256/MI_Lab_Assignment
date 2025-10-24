using UnityEngine;
using System.Collections.Generic; // Required for using Lists

/// <summary>
/// A modular "master" script for interactable objects.
/// Implements IHoverable to handle pointer events (hover, click).
/// Controls object highlighting (outline).
/// Toggles a child UI menu via OnPointerClick (intended for Button B).
/// Manages a list of InteractableAction scripts based on UI button presses.
/// Conforms to Assignment 3 specifications.
/// </summary>
[RequireComponent(typeof(Outline))]
public class InteractionMenu : MonoBehaviour, IHoverable
{
    [Header("Menu Setup")]
    [Tooltip("Drag the child Canvas GameObject for this object's menu here.")]
    public GameObject menuCanvas; // The UI menu to toggle

    // This list will automatically populate with all InteractableAction scripts found on this object.
    private List<InteractableAction> allActions;
    
    private Outline outline;
    private int hoverCounter = 0; // Tracks multiple pointers for highlighting

    void Awake()
    {
        outline = GetComponent<Outline>();
        if (outline != null)
        {
             outline.enabled = false; // Start with outline off
        }

        // Find all components on this GameObject that inherit from InteractableAction
        // The order they appear in the Inspector determines their index.
        allActions = new List<InteractableAction>(GetComponents<InteractableAction>());

        // Ensure menu is hidden and actions are disabled on start.
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(false);
        }
        DisableAllInteractions(); // Ensure actions start disabled
    }

    // --- IHoverable Implementation ---

    /// <summary>
    /// Called by PhysicsPointer_2 when a pointer starts hovering over this object.
    /// Increments hover counter and enables the outline if needed.
    /// </summary>
    public void OnHoverEnter(ControllerHand hand)
    {
        hoverCounter++;
        if (outline != null)
        {
             // Turn outline on if this is the first pointer hovering.
            outline.enabled = hoverCounter > 0;
        }
    }

    /// <summary>
    /// Called by PhysicsPointer_2 when a pointer stops hovering over this object.
    /// Decrements hover counter and disables the outline if no pointers are left.
    /// </summary>
    public void OnHoverExit(ControllerHand hand)
    {
        hoverCounter--;
        // Safety check to ensure the counter doesn't go below zero.
        if (hoverCounter < 0)
        {
            hoverCounter = 0;
        }
        if (outline != null)
        {
            // Turn outline off only if this was the last pointer leaving.
            outline.enabled = hoverCounter > 0;
        }
    }

    /// <summary>
    /// Called by PhysicsPointer_2 when the designated menu button (e.g., Button B) is pressed
    /// while hovering over this object. Toggles the menu visibility.
    /// </summary>
    public void OnPointerClick()
    {
        // Toggle the menu's visibility. Matches "Exit" behavior if menu is closed.
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }

        // It's good practice to disable actions if the menu is re-opened,
        // unless you want the old action to continue.
        // For the assignment, "Exit" implies the action continues, but
        // opening the menu should probably stop the action.
        // Let's stick to the "Exit" spec:
        // If we just clicked to *open* the menu, we don't disable actions.
        // If we just clicked to *close* the menu, it's like "Exit", so we also don't disable actions.
    }

    // --- Public Methods for UI Menu Buttons ---

    /// <summary>
    /// Activates the action script at the specified index (e.g., Translate, Rotate)
    /// and disables all other actions. Then closes the menu.
    /// Called by the specific action buttons on the UI Canvas.
    /// </summary>
    /// <param name="index">The index of the action script in the Inspector order.</param>
    public void ActivateAction(int index)
    {
        DisableAllInteractions(); // Turn off any previously active action.
        
        // Validate the index before trying to access the list.
        if (allActions != null && index >= 0 && index < allActions.Count)
        {
            // Enable the selected action script.
            allActions[index].enabled = true;
        }
        else
        {
            Debug.LogWarning("InteractionMenu: Invalid action index requested: " + index, this);
        }
        
        // Close the menu after selecting an action.
        CloseMenu();
    }

    /// <summary>
    /// Deactivates all action scripts and closes the menu.
    /// Called by the "No Action" button on the UI Canvas.
    /// </summary>
    public void DeactivateAllAndClose()
    {
        DisableAllInteractions();
        CloseMenu();
    }

    /// <summary>
    /// Closes the menu without changing the currently active action.
    /// Called by the "Exit" button on the UI Canvas. Can also be called manually.
    /// </fsummary>
    public void CloseMenu()
    {
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(false);
        }
    }

    /// <summary>
    /// Helper method to disable all InteractableAction scripts attached to this object.
    /// Ensures only one action (or none) is active at a time.
    /// </summary>
    public void DisableAllInteractions()
    {
        if (allActions == null) return;

        // Loop through the list of found action scripts.
        foreach (var action in allActions)
        {
            if (action != null)
            {
                // Disable the script component.
                action.enabled = false;
            }
        }
    }
}
