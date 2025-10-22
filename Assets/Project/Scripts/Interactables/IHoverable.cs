// IHoverable.cs
public interface IHoverable
{
    void OnHoverEnter(ControllerHand hand);
    void OnHoverExit(ControllerHand hand);

    // Called when the pointer is hovering and the primary input button is pressed.
    void OnPointerClick();
}