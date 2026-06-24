using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FlirtAIntroManager : MonoBehaviour
{
    [Header("UI Elemente")]
    public RectTransform logoRect;          // Hier kommt dein Herz-Logo rein

    [Header("Timing & Animation")]
    public float logoPopDuration = 0.8f;    // Wie lange das "Größer-Werden" dauert
    public float totalSceneTime = 6.0f;     // Gesamtzeit in dieser Szene bis zum Wechsel
    public string nextSceneName = "Profil"; // Name der Profil-Szene

    // Mit dieser Kurve kannst du im Inspector den Plopp-Effekt perfekt einstellen!
    public AnimationCurve popCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    void Start()
    {
        // Zu Beginn ist das Logo unsichtbar (Skalierung auf 0)
        if (logoRect != null)
        {
            logoRect.localScale = Vector3.zero;
        }

        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        // 1. Kurz im Schwarzen warten (z. B. 0.5 Sekunden) vor dem Plopp
        yield return new WaitForSeconds(0.5f);

        // 2. Die Logo-Animation abspielen
        float elapsed = 0f;
        while (elapsed < logoPopDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / logoPopDuration;

            // Holt sich den Wert aus der Animationskurve
            float currentScale = popCurve.Evaluate(t);
            logoRect.localScale = new Vector3(currentScale, currentScale, 1f);

            yield return null; // Wartet auf den nächsten Frame
        }

        // Sicherstellen, dass das Logo am Ende exakt die Zielgröße (1) hat
        logoRect.localScale = Vector3.one;

        // 3. Den Rest der Zeit warten
        // Wir ziehen die bereits vergangene Zeit ab, damit die Gesamtdauer exakt stimmt
        float remainingTime = totalSceneTime - logoPopDuration - 0.5f;
        if (remainingTime > 0)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        // 4. Automatisch zur Profil-Szene wechseln
        Debug.Log("Lade Profil-Szene...");
        SceneManager.LoadScene(nextSceneName);
    }
}