using UnityEngine;
using UnityEngine.EventSystems;

public class DressUpDropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // Prüfen, ob überhaupt etwas gezogen wurde
        if (eventData.pointerDrag != null)
        {
            DragDropItem item = eventData.pointerDrag.GetComponent<DragDropItem>();
            if (item != null)
            {
                item.isEquipped = true; // Item erfolgreich angezogen!
            }
        }
    }
}