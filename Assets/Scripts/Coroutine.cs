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
    public string nextSceneName = "FlirtA";

    // NEU: Wenn das AN ist, wechselt die Szene automatisch nach dem letzten Panel!
    public bool autoAdvanceAtEnd = false;

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

        // NEU: Hier entscheiden wir anhand des Häkchens, was passiert
        if (autoAdvanceAtEnd)
        {
            Debug.Log("Automatischer Szenenwechsel zu: " + nextSceneName);
            LoadFlirtA(); // Wechselt sofort die Szene
        }
        else if (chatButton != null)
        {
            chatButton.SetActive(true);
            Debug.Log("3. Button sollte jetzt sichtbar sein!");
        }
    }

    public void LoadFlirtA()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}