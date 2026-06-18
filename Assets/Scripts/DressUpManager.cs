using UnityEngine;
using UnityEngine.SceneManagement;

public class DressUpManager : MonoBehaviour
{
    [Header("Kleidungs-Liste")]
    public DragDropItem[] allClothingItems;

    [Header("Abstands-Erkennung")]
    public RectTransform aureliaTransform; // Ziehe hier Aurelia aus der Hierarchy rein
    public float dropRadius = 200f;        // Wie nah muss das Item ran? (In Pixeln)

    [Header("UI Panels")]
    public GameObject successPanel;
    public GameObject failPanel;

    void Start()
    {
        if (successPanel != null) successPanel.SetActive(false);
        if (failPanel != null) failPanel.SetActive(false);
    }

    public void RecountAndCheck()
    {
        int totalEquipped = 0;
        int equippedCorrect = 0;
        int equippedIncorrect = 0;
        int totalCorrectNeeded = 8;

        foreach (DragDropItem item in allClothingItems)
        {
            if (item.isEquipped)
            {
                totalEquipped++;
                if (item.isWinterClothing) equippedCorrect++;
                else equippedIncorrect++;
            }
        }

        if (totalEquipped == 8)
        {
            if (equippedIncorrect > 0 || equippedCorrect < totalCorrectNeeded)
            {
                failPanel.SetActive(true);
                successPanel.SetActive(false);
            }
            else
            {
                successPanel.SetActive(true);
                failPanel.SetActive(false);
            }
        }
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextWebtoon()
    {
        SceneManager.LoadScene("Webtoon3.1");
    }
}