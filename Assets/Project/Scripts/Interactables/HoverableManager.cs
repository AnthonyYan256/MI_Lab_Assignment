using UnityEngine;

// Attach this script to your interactable objects. It will manage and
// broadcast hover events to all other IHoverable components.
public class HoverableManager : MonoBehaviour
{
    // A list of all components that care about hover events.
    private IHoverable[] hoverableComponents;

    void Awake()
    {
        // Find all components on this GameObject that implement the IHoverable interface.
        hoverableComponents = GetComponents<IHoverable>();
    }

    // This method will be called by the Event Trigger on PointerEnter.
    public void OnPointerEnter(ControllerHand hand)
    {
        Debug.Log("HoverManager received OnPointerEnter. Notifying " + hoverableComponents.Length + " components."); // Add this line

        // Notify all subscribed components that a hover has started.
        foreach (var component in hoverableComponents)
        {
            component.OnHoverEnter(hand);
        }
    }

    // This method will be called by the Event Trigger on PointerExit.
    public void OnPointerExit(ControllerHand hand)
    {
        // Notify all subscribed components that a hover has ended.
        foreach (var component in hoverableComponents)
        {
            component.OnHoverExit(hand);
        }
    }
}