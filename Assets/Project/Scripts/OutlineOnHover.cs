using UnityEngine;

[RequireComponent(typeof(Outline))]
public class HighlightOnHover : MonoBehaviour, IHoverable
{
    private Outline outline;

    void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false; // start disabled
    }

    public void OnHoverEnter()
    {
        outline.enabled = true; // highlight on
    }

    public void OnHoverExit()
    {
        outline.enabled = false; // highlight off
    }
}