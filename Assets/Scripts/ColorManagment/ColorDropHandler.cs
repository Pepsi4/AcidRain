using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorDropHandler : MonoBehaviour, IDropHandler
{
    public CellColorChanger colorChanger;
    [SerializeField] Image image;
    [SerializeField] private ColorStateManager colorStateManager;
    public void OnDrop(PointerEventData eventData)
    {
        ColorDragHandler draggingColor = eventData.pointerDrag?.GetComponent<ColorDragHandler>();

        if (draggingColor != null)
        {
            colorChanger.NewColor = new Color(draggingColor.Color.r, draggingColor.Color.g, draggingColor.Color.b, 1f);
            colorChanger.ChangeCellColor();
            colorStateManager.MarkColorChanged(this);
        }
    }
}
