using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image image;
    public Color Color => image.color;
    private GameObject draggingObject;
    private CanvasGroup canvasGroup;

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggingObject = new GameObject("DraggingColor");
        var img = draggingObject.AddComponent<UnityEngine.UI.Image>();
        canvasGroup = draggingObject.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        img.color = new Color(Color.r, Color.g, Color.b, 1f);

        draggingObject.transform.SetParent(GetComponentInParent<Canvas>().transform, false);
        draggingObject.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
        draggingObject.transform.position = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingObject != null)
        {
            draggingObject.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingObject != null)
        {
            canvasGroup.blocksRaycasts = false;
            Destroy(draggingObject);
        }
    }
}
