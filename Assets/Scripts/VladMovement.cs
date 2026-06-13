using UnityEngine;

public class VladMovement : MonoBehaviour
{
    [Header("Position & Bewegung")]
    // Auf 450 erhöht, damit er weit über dem oberen Bildschirmrand völlig unsichtbar startet
    [SerializeField] private float spawnY = 450f;
    [SerializeField] private float speed = 150f;  // Seine Gleit-Geschwindigkeit

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        // Awake läuft SOFORT beim Instanziieren. Das verhindert das visuelle Aufblitzen!
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = new Vector2(0f, spawnY);
        }
    }

    void Update()
    {
        if (rectTransform == null) return;

        // Bewegt Vlad in einer perfekten geraden Linie nach unten
        rectTransform.anchoredPosition += Vector2.down * speed * Time.deltaTime;
    }
}