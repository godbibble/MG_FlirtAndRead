using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Kleidungs-Eigenschaften")]
    public bool isWinterClothing;

    [HideInInspector] public bool isEquipped = false;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 startPosition;
    private DressUpManager manager;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        startPosition = rectTransform.anchoredPosition;
        manager = Object.FindFirstObjectByType<DressUpManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isEquipped = false;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (manager != null && manager.aureliaTransform != null)
        {
            // Berechnet den exakten Abstand zwischen dem Item und Aurelia im UI-Raum
            float distance = Vector3.Distance(rectTransform.position, manager.aureliaTransform.position);

            if (distance <= manager.dropRadius)
            {
                isEquipped = true; // Nah genug dran -> Angenommen!
            }
            else
            {
                ResetPosition(); // Zu weit weg -> Zurückfliegen
            }

            // Manager die neue Anzahl prüfen lassen
            manager.RecountAndCheck();
        }
        else
        {
            // Fallback, falls der Manager fehlt
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = startPosition;
        isEquipped = false;
    }
}