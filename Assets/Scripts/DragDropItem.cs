using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Kleidungs-Eigenschaften")]
    public bool isWinterClothing;

    [Header("Snapping (Ziel-Anker)")]
    [Tooltip("Das unsichtbare Ziel-Objekt auf Aurelia, an das dieses Kleidungsstück snappen soll.")]
    public RectTransform snapTarget;

    [Header("Größen-Anpassung beim Anziehen")]
    [Tooltip("Soll das Kleidungsstück seine Größe ändern, wenn es angezogen wird?")]
    public bool changeSizeOnEquip = true;
    [Tooltip("Die exakte UI-Größe (Width und Height), die das Item hat, wenn Aurelia es trägt.")]
    public Vector2 equippedSize = new Vector2(200f, 300f);

    [HideInInspector] public bool isEquipped = false;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 startPosition;
    private Vector2 startSize; // Speichert die Größe, die es im Raum hatte
    private DressUpManager manager;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // Startwerte für Position UND Größe merken
        startPosition = rectTransform.anchoredPosition;
        startSize = rectTransform.sizeDelta;

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

        if (manager != null)
        {
            RectTransform target = (snapTarget != null) ? snapTarget : manager.aureliaTransform;

            if (target != null)
            {
                float distance = Vector3.Distance(rectTransform.position, target.position);

                if (distance <= manager.dropRadius)
                {
                    isEquipped = true;
                    rectTransform.position = target.position;

                    // NEU: Bild auf die perfekte Trage-Größe skalieren
                    if (changeSizeOnEquip)
                    {
                        rectTransform.sizeDelta = equippedSize;
                    }
                }
                else
                {
                    ResetPosition();
                }

                manager.RecountAndCheck();
            }
        }
        else
        {
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = startPosition;
        // NEU: Beim Zurücklegen wieder auf die kleine Raum-Größe schrumpfen
        rectTransform.sizeDelta = startSize;
        isEquipped = false;
    }
}