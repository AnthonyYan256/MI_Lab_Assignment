using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Outline))]
public class HighlightOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Outline outline;

    void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false; // start disabled
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.enabled = true; // highlight on
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false; // highlight off
    }
}