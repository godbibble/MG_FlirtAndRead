using UnityEngine;
using UnityEngine.InputSystem;

public class AureliaSkating : MonoBehaviour
{
    [Header("Bewegungseinstellungen")]
    public float speed = 500f;

    [Header("Grenzen (Y-Achse)")]
    [SerializeField] private float bottomY = -288f; // Ihre Startposition ganz unten (aus deinem Inspector)
    [SerializeField] private float middleY = 0f;    // Die Mitte der Bahn, weiter darf sie nicht

    private RectTransform rectTransform;
    private IceSkatingManager manager;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        manager = FindFirstObjectByType<IceSkatingManager>();
    }

    void Update()
    {
        Vector2 moveDirection = Vector2.zero;

        // 1. STEUERUNG FÜR TASTATUR
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveDirection.y += 1;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveDirection.y -= 1;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveDirection.x -= 1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveDirection.x += 1;
        }

        // 2. STEUERUNG FÜR MAUS / TOUCH
        if (Pointer.current != null && Pointer.current.press.isPressed)
        {
            Vector2 screenPos = Pointer.current.position.ReadValue();
            Vector2 localMousePos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                screenPos,
                null,
                out localMousePos
            );

            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, localMousePos, 0.15f);
        }

        // Tastatur-Richtung anwenden
        if (moveDirection != Vector2.zero)
        {
            rectTransform.anchoredPosition += moveDirection.normalized * speed * Time.deltaTime;
        }

        // 3. DYNAMISCHE BLOCKADE (Hier passiert die Magie)
        float minY = bottomY;
        float maxY = bottomY; // Standard: Sie MUSS ganz unten bleiben

        // Sucht in der Szene nach einem aktiven Objekt mit dem Tag "Vlad"
        GameObject vladInScene = GameObject.FindWithTag("Vlad");

        if (vladInScene != null)
        {
            // Sobald Vlad vom Manager gespawnt wurde, wird das Limit bis zur Mitte (middleY) freigegeben!
            maxY = middleY;
        }

        // 4. BAHN-GRENZEN ANWENDEN
        float borderX = 160f; // Links/Rechts Begrenzung der weißen Bahn

        rectTransform.anchoredPosition = new Vector2(
            Mathf.Clamp(rectTransform.anchoredPosition.x, -borderX, borderX),
            Mathf.Clamp(rectTransform.anchoredPosition.y, minY, maxY) // Nutzt das dynamische Y-Limit
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            if (manager != null) manager.LevelFailed();
        }
        else if (collision.CompareTag("Vlad"))
        {
            if (manager != null) manager.LevelSuccess();
        }
    }
}