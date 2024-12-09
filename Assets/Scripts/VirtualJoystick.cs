using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform joystickBackground;
    [SerializeField] private RectTransform joystickHandle;
    [SerializeField] private float radiusMultiplier = 0.7f;

    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    private Vector2 joystickCenter;


    private void Start()
    {
        joystickCenter = joystickBackground.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 input = eventData.position - joystickCenter;
        float radius = (joystickBackground.sizeDelta.x / 2f) * radiusMultiplier;

        Vector2 normalizedInput = Vector2.ClampMagnitude(input, radius) / radius;
        Horizontal = normalizedInput.x;
        Vertical = normalizedInput.y;

        joystickHandle.position = joystickCenter + normalizedInput * radius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Horizontal = Vertical = 0f;
        joystickHandle.position = joystickCenter;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }
}
