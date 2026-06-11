using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChatManager : MonoBehaviour
{
    [System.Serializable]
    public class VladAnswerOption
    {
        [TextArea(2, 5)]
        public string answerText;         // Vlads Text auf dem Button
        public float lovometerPoints;     // Die Plus- oder Minuspunkte
        [TextArea(2, 5)]
        public string aureliaReaction;    // Aurelias direkte Antwortblase auf diesen Text
    }

    [System.Serializable]
    public class ChatNode
    {
        public string nodeName;           // Nur zur Übersicht im Inspector (z.B. "Schritt 1")
        public VladAnswerOption[] answers; // Die 2-3 Antwortmöglichkeiten für Vlad
        public bool isLastNode;           // Ist das die finale Date-Einladung?
    }

    [Header("UI Scroll-Ansicht")]
    public Transform chatContentFolder;
    public ScrollRect scrollRect;

    [Header("Sprechblasen Vorlagen (Prefabs)")]
    public GameObject aureliaBubblePrefab; // Lila Bubble (Links)
    public GameObject vladBubblePrefab;    // Rote Bubble (Rechts)

    [Header("UI Steuerung")]
    public Slider lovometerSlider;
    public Button[] answerButtons;

    [Header("Feedback Panels")]
    public GameObject successPanel;
    public Button nextLevelButton;
    public GameObject failPanel;
    public Button retryButton;

    [Header("Dialog Einstellungen")]
    public ChatNode[] dialogueFlow;

    private int currentNodeIndex = 0;
    private float currentLoveScore = 25f;
    private float maxLoveScore = 100f;

    void Start()
    {
        lovometerSlider.maxValue = maxLoveScore;
        lovometerSlider.value = currentLoveScore;

        if (successPanel != null) successPanel.SetActive(false);
        if (failPanel != null) failPanel.SetActive(false);
        if (retryButton != null) retryButton.onClick.AddListener(ReloadScene);
        if (nextLevelButton != null) nextLevelButton.onClick.AddListener(LoadWebtoon2);

        
        ShowCurrentNode();
    }

    void ShowCurrentNode()
    {
        if (currentNodeIndex >= dialogueFlow.Length) return;

        ChatNode currentNode = dialogueFlow[currentNodeIndex];

        
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < currentNode.answers.Length)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentNode.answers[i].answerText;

                int index = i;
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnAnswerSelected(int answerIndex)
    {
        ChatNode currentNode = dialogueFlow[currentNodeIndex];
        VladAnswerOption selectedOption = currentNode.answers[answerIndex];

        
        SpawnBubble(selectedOption.answerText, false);

        
        currentLoveScore += selectedOption.lovometerPoints;
        currentLoveScore = Mathf.Clamp(currentLoveScore, 0f, maxLoveScore);
        lovometerSlider.value = currentLoveScore;

        
        foreach (Button btn in answerButtons) btn.gameObject.SetActive(false);

        
        if (currentNode.isLastNode)
        {
            StartCoroutine(FinalOutcomeDelay());
            return;
        }

        
        StartCoroutine(AureliaTypingDelay(selectedOption.aureliaReaction));
    }

    IEnumerator AureliaTypingDelay(string reactionText)
    {
        yield return new WaitForSeconds(1.5f);

        
        if (!string.IsNullOrEmpty(reactionText))
        {
            SpawnBubble(reactionText, true);
        }

        
        currentNodeIndex++;

        yield return new WaitForSeconds(0.5f);
        ShowCurrentNode();
    }

    IEnumerator FinalOutcomeDelay()
    {
        yield return new WaitForSeconds(1.5f);

        if (currentLoveScore >= 50f)
        {
            SpawnBubble("Aurelia: Gerne, in zwei Stunden kann ich da sein ^^.", true);
            yield return new WaitForSeconds(1.5f);
            if (successPanel != null) successPanel.SetActive(true);
        }
        else
        {
            SpawnBubble("Aurelia: Ne, lieber nicht. Bye.", true);
            yield return new WaitForSeconds(1.5f);
            if (failPanel != null) failPanel.SetActive(true);
        }
    }

    void SpawnBubble(string text, bool isAurelia)
    {
        GameObject prefab = isAurelia ? aureliaBubblePrefab : vladBubblePrefab;
        GameObject newBubble = Instantiate(prefab, chatContentFolder);

        newBubble.transform.Find("Bubble/Text (TMP)").GetComponent<TextMeshProUGUI>().text = text;

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    public void LoadWebtoon2() => SceneManager.LoadScene("Webtoon2");
}