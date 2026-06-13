using UnityEngine;
using UnityEngine.SceneManagement; // Ganz wichtig für den Szenenwechsel!
using System.Collections;

public class SplashTimer : MonoBehaviour
{
    void Start()
    {
        // Startet den zeitgesteuerten Ablauf direkt beim Laden der Szene
        StartCoroutine(WaitAndLoadScene());
    }

    IEnumerator WaitAndLoadScene()
    {
        // Wartet exakt 10 Sekunden (du kannst den Wert hier jederzeit ändern)
        yield return new WaitForSeconds(10f);

        // Lädt die nächste Szene. Achte darauf, dass der Name exakt übereinstimmt!
        SceneManager.LoadScene("Profil");
    }
}