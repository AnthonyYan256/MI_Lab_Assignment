using UnityEngine;

[RequireComponent(typeof(Outline))]
public class HighlightOnHover : MonoBehaviour, IHoverable
{
    private Outline outline;
    private int hoverCounter = 0; // Counts how many pointers are on this object.

    void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false; 
    }

    public void OnHoverEnter(ControllerHand hand)
    {
        hoverCounter++;
        Debug.Log("Hover ENTER triggered by: " + hand); // Add this line
        // The outline is on if one or more pointers are hovering.
        outline.enabled = hoverCounter > 0;
    }

    public void OnHoverExit(ControllerHand hand)
    {
        hoverCounter--;
        Debug.Log("Hover EXIT triggered by: " + hand); // Add this line
        // To be safe, ensure the counter never goes below zero.
        if (hoverCounter < 0)
        {
            hoverCounter = 0;
        }
        
        // The outline is only disabled when the last pointer leaves.
        outline.enabled = hoverCounter > 0;
    }
}