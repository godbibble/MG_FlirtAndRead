using UnityEngine;

public class FallingUIObject : MonoBehaviour
{
    [HideInInspector] public float fallSpeed = 400f; // Wird vom Director eingestellt
    [HideInInspector] public bool isVlad = false;

    private RectTransform rectTransform;
    private AureliaUIController player;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        player = Object.FindFirstObjectByType<AureliaUIController>();
    }

    void Update()
    {
        // Nach unten fliegen im UI-Raum
        rectTransform.anchoredPosition += Vector2.down * fallSpeed * Time.deltaTime;

        // Kollisionsabfrage über Pixel-Abstand
        if (player != null)
        {
            RectTransform playerRect = player.GetComponent<RectTransform>();
            float distance = Vector2.Distance(rectTransform.anchoredPosition, playerRect.anchoredPosition);

            // Wenn sich die UI-Bilder nahe genug kommen (Wert anpassen je nach Sprite-Größe)
            if (distance < 90f)
            {
                GameDirectorUI director = Object.FindFirstObjectByType<GameDirectorUI>();
                if (director != null)
                {
                    if (isVlad) director.WinGame();
                    else director.GameOver();
                }
                Destroy(gameObject);
            }
        }

        // Unten aus dem Bildschirm gelaufen -> Löschen
        if (rectTransform.anchoredPosition.y < -1100f)
        {
            Destroy(gameObject);
        }
    }
}