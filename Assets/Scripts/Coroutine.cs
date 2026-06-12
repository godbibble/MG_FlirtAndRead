using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class WebtoonSequenceMasked : MonoBehaviour
{
    // Wir ändern den Typ von Mask[] zu GameObject[]
    public GameObject[] panelMasks;
    public GameObject chatButton;
    public float delayBetweenPanels = 2.5f;

    void Start()
    {
        Debug.Log("1. Start-Funktion aufgerufen!");

        // Schaltet jetzt das KOMPLETTE Masken-Objekt am Anfang unsichtbar
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
                // Schaltet das gesamte Objekt inklusive des Bildes darunter sichtbar
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

    public void LoadChatroom()
    {
        SceneManager.LoadScene("Chatroom");
    }
}