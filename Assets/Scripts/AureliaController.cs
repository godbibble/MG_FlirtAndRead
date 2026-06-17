using UnityEngine;
using UnityEngine.InputSystem;

public class AureliaUIController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float slideSmoothness = 12f; // Wie "rutschig" das Eis ist
    public float minX = -450f;          // Linker Rand des Bildschirms (bei 1080er Breite)
    public float maxX = 450f;           // Rechter Rand des Bildschirms

    private RectTransform rectTransform;
    private Canvas canvas;
    private float targetX;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        targetX = rectTransform.anchoredPosition.x;
    }

    void Update()
    {
        // Drag- / Wisch-Eingabe auswerten
        if (Pointer.current != null && Pointer.current.press.isPressed)
        {
            Vector2 mousePosition = Pointer.current.position.ReadValue();

            // Rechnet die Bildschirm-Berührung in Canvas-Pixel um
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                mousePosition,
                canvas.worldCamera,
                out Vector2 localPoint
            );

            targetX = localPoint.x;
        }

        // Sanftes Hin- und Herrutschen
        float smoothedX = Mathf.Lerp(rectTransform.anchoredPosition.x, targetX, slideSmoothness * Time.deltaTime);
        smoothedX = Mathf.Clamp(smoothedX, minX, maxX);

        // Zuweisen (Y bleibt exakt auf deiner voreingestellten Höhe!)
        rectTransform.anchoredPosition = new Vector2(smoothedX, rectTransform.anchoredPosition.y);
    }
}