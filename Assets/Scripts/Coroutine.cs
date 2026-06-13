using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class WebtoonSequenceMasked : MonoBehaviour
{
    public GameObject[] panelMasks;
    public GameObject chatButton;
    public float delayBetweenPanels = 2.5f;

    [Header("Szenen-Wechsel")]
    // "FlirtA" ist der Standardwert. Webtoon 1 funktioniert also sofort weiter wie gewohnt!
    public string nextSceneName = "FlirtA";

    void Start()
    {
        Debug.Log("1. Start-Funktion aufgerufen!");

        foreach (GameObject maskObj in panelMasks)
        {
            if (maskObj != null) maskObj.SetActive(false);
        }

        if (chatButton != null) chatButton.SetActive(false);

        StartCoroutine(PlayWebtoon());
    }

    IEnumerator PlayWebtoon()
    {
        Debug.Log("2. Coroutine gestartet!");

        for (int i = 0; i < panelMasks.Length; i++)
        {
            if (panelMasks[i] != null)
            {
                panelMasks[i].SetActive(true);
                Debug.Log("Schalte Panel frei: " + i);
            }
            yield return new WaitForSeconds(delayBetweenPanels);
        }

        if (chatButton != null)
        {
            chatButton.SetActive(true);
            Debug.Log("3. Button sollte jetzt sichtbar sein!");
        }
    }

    // Wir lassen den Funktionsnamen gleich, damit du in Szene 1 nichts neu verknüpfen musst!
    public void LoadFlirtA()
    {
        // Lädt jetzt dynamisch die Szene, die im Inspector steht
        SceneManager.LoadScene(nextSceneName);
    }
}