using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour,
    IPointerDownHandler,
    IDragHandler,
    IPointerUpHandler
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;
    [SerializeField] private PlayerMovement playerMovement;

    private Vector2 input;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 position))
        {
            Vector2 radius = background.rect.size / 2f;

            input = new Vector2(
                position.x / radius.x,
                position.y / radius.y
            );

            input = Vector2.ClampMagnitude(input, 1f);

            handle.anchoredPosition = new Vector2(
                input.x * radius.x,
                input.y * radius.y
            );

            playerMovement.SetMovementInput(input);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;

        playerMovement.SetMovementInput(Vector2.zero);
    }
}