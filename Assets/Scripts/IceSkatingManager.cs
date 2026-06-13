using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IceSkatingManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject successPanel;
    public GameObject failPanel;

    [Header("Spawner Einstellungen")]
    public GameObject obstaclePrefab;
    public GameObject vladPrefab;
    public RectTransform spawnArea;
    public float gameDuration = 20f;

    [Header("Abstands-Feineinstellung (Taktung)")]
    public float minSpawnInterval = 0.4f;
    public float maxSpawnInterval = 0.8f;

    [Header("Rand-Abstand für Hindernisse")]
    public float obstacleMargin = 40f;

    private bool gameEnded = false;
    private float timer = 0f;
    private bool vladSpawned = false;

    void Start()
    {
        if (successPanel != null) successPanel.SetActive(false);
        if (failPanel != null) failPanel.SetActive(false);
        Time.timeScale = 1f;

        StartCoroutine(SpawnObstacles());
    }

    void Update()
    {
        if (gameEnded) return;

        timer += Time.deltaTime;

        if (timer >= gameDuration && !vladSpawned)
        {
            vladSpawned = true;
            SpawnVlad();
        }
    }

    IEnumerator SpawnObstacles()
    {
        while (!gameEnded && !vladSpawned)
        {
            float randomDelay = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(randomDelay);

            if (!gameEnded && !vladSpawned)
            {
                SpawnSingleObstacle();
            }
        }
    }

    void SpawnSingleObstacle()
    {
        GameObject obs = Instantiate(obstaclePrefab, spawnArea);
        obs.tag = "Obstacle"; // Weist dem Hindernis das Tag zu
        RectTransform rt = obs.GetComponent<RectTransform>();

        float minX = -spawnArea.rect.width / 2f + obstacleMargin;
        float maxX = spawnArea.rect.width / 2f - obstacleMargin;
        float randomX = Random.Range(minX, maxX);

        rt.anchoredPosition = new Vector2(randomX, spawnArea.rect.height / 2f + 100f);
    }

    void SpawnVlad()
    {
        GameObject vlad = Instantiate(vladPrefab, spawnArea);
        vlad.tag = "Vlad"; // Weist Vlad das Tag zu
        RectTransform rt = vlad.GetComponent<RectTransform>();

        // X = 0 setzt ihn genau in die Mitte
        rt.anchoredPosition = new Vector2(0f, spawnArea.rect.height / 2f + 100f);
    }

    public void LevelFailed()
    {
        if (gameEnded) return;
        gameEnded = true;
        Time.timeScale = 0f;
        failPanel.SetActive(true);
    }

    public void LevelSuccess()
    {
        if (gameEnded) return;
        gameEnded = true;
        Time.timeScale = 0f;
        successPanel.SetActive(true);
    }

    public void RetryGame() { Time.timeScale = 1f; SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    public void LoadStartScreen() { Time.timeScale = 1f; SceneManager.LoadScene("StartScreen"); }
}