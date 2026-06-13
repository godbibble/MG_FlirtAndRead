using UnityEngine;

public class IceGameMover : MonoBehaviour
{
    public float speed = 400f; // Fahrgeschwindigkeit nach unten

    [Header("Vlad Feineinstellung")]
    public bool isVlad = false;
    public float stopAtY = 300f; // Bei welchem Y-Wert soll Vlad stehen bleiben?

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (isVlad)
        {
            // Wenn es Vlad ist, fährt er nur so lange nach unten, bis er stopAtY erreicht hat
            if (rectTransform.anchoredPosition.y > stopAtY)
            {
                rectTransform.anchoredPosition += Vector2.down * speed * Time.deltaTime;
            }
        }
        else
        {
            // Normale Hindernisse fliegen komplett nach unten durch
            rectTransform.anchoredPosition += Vector2.down * speed * Time.deltaTime;

            // Wenn das Hindernis weit unten aus dem Canvas fliegt, löschen
            if (rectTransform.anchoredPosition.y < -1100f)
            {
                Destroy(gameObject);
            }
        }
    }
}