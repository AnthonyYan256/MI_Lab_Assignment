using UnityEngine;
using System.Collections.Generic; // Required for using Lists

/// <summary>
/// A modular "master" script for interactable objects.
/// Implements IHoverable to handle pointer events (hover, click).
/// Controls object highlighting (outline).
/// Toggles a child UI menu via OnPointerClick (intended for Button B).
/// Manages a list of InteractableAction scripts based on UI button presses.
/// 
/// This version is updated to support the "PhysicsButton" system.
/// It adds the 'IsMenuOpen' property for actions to check.
/// </summary>
[RequireComponent(typeof(Outline))]
public class P_InteractionMenu : MonoBehaviour, IHoverable
{
    [Header("Menu Setup")]
    [Tooltip("Drag the child Canvas GameObject for this object's menu here.")]
    public GameObject menuCanvas; // The UI menu to toggle

    /// <summary>
    /// Public property that lets other scripts (like action scripts)
    /// know if the menu is currently visible.
    /// </summary>
    public bool IsMenuOpen { get; private set; }

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
            IsMenuOpen = false; // Explicitly set state
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
            // Toggle the active state
            menuCanvas.SetActive(!menuCanvas.activeSelf);
            // Update the public property
            IsMenuOpen = menuCanvas.activeSelf;
        }
    }

    // --- Public Methods for UI Menu Buttons ---

    /// <summary>
    /// Activates the action script at the specified index (e.g., Translate, Rotate)
    /// and disables all other actions. Then closes the menu.
    /// Called by the specific action buttons on the UI Canvas (via PhysicsButton).
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
    /// Called by the "No Action" button on the UI Canvas (via PhysicsButton).
    /// </summary>
    public void DeactivateAllAndClose()
    {
        DisableAllInteractions();
        CloseMenu();
    }

    /// <summary>
    /// Closes the menu without changing the currently active action.
    /// Called by the "Exit" button on the UI Canvas (via PhysicsButton).
    /// </summary>
    public void CloseMenu()
    {
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(false);
            IsMenuOpen = false; // Update state
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

