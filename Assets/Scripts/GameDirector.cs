using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirectorUI : MonoBehaviour
{
    [Header("UI Prefabs (Müssen UI Images sein!)")]
    public GameObject[] obstaclePrefabs;
    public GameObject vladPrefab;

    [Header("Einstellungen")]
    public float spawnInterval = 0.8f;
    public float obstacleSpeed = 500f;
    public float gameDuration = 45f;

    [Header("Schwierigkeit & Spuren (Lanes)")]
    [Tooltip("Die festen X-Positionen auf der Eisbahn, auf denen Hindernisse fallen können.")]
    public float[] spawnLanes = new float[] { -400f, -200f, 0f, 200f, 400f };

    [Tooltip("Nach wie vielen Sekunden gähnender Leere eine Spur absolut erzwungen wird (Anti-Camping).")]
    public float maxLaneEmptyTime = 3.0f;

    public float speedVariance = 120f;

    [Header("UI Endscreens")]
    public GameObject gameOverPanel;
    public GameObject winPanel;

    private float gameTimer = 0f;
    private float spawnTimer = 0f;
    private bool vladSpawned = false;
    private bool gameEnded = false;
    private Canvas targetCanvas;

    private float[] laneEmptyTimers;

    void Start()
    {
        Time.timeScale = 1f;
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (winPanel) winPanel.SetActive(false);

        targetCanvas = Object.FindFirstObjectByType<Canvas>();

        if (spawnLanes != null && spawnLanes.Length > 0)
        {
            laneEmptyTimers = new float[spawnLanes.Length];
        }
    }

    void Update()
    {
        if (gameEnded) return;

        gameTimer += Time.deltaTime;

        if (gameTimer < (gameDuration - 3f) && laneEmptyTimers != null)
        {
            for (int i = 0; i < laneEmptyTimers.Length; i++)
            {
                laneEmptyTimers[i] += Time.deltaTime;
            }
        }

        if (gameTimer < (gameDuration - 3f))
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                Spawn(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], false);
                spawnTimer = 0f;
            }
        }
        else if (gameTimer >= gameDuration && !vladSpawned)
        {
            Spawn(vladPrefab, true);
            vladSpawned = true;
        }
    }

    void Spawn(GameObject prefab, bool isVlad)
    {
        if (prefab == null || targetCanvas == null) return;

        GameObject spawned = Instantiate(prefab, targetCanvas.transform);
        RectTransform rt = spawned.GetComponent<RectTransform>();

        rt.localScale = Vector3.one;

        float spawnX = 0f;
        if (isVlad)
        {
            spawnX = 0f;
        }
        else
        {
            int chosenLaneIndex = 0;
            float longestTime = 0f;
            int longestEmptyIndex = 0;

            for (int i = 0; i < laneEmptyTimers.Length; i++)
            {
                if (laneEmptyTimers[i] > longestTime)
                {
                    longestTime = laneEmptyTimers[i];
                    longestEmptyIndex = i;
                }
            }

            if (longestTime >= maxLaneEmptyTime)
            {
                chosenLaneIndex = longestEmptyIndex;
            }
            else
            {
                chosenLaneIndex = Random.Range(0, spawnLanes.Length);
            }

            spawnX = spawnLanes[chosenLaneIndex];
            laneEmptyTimers[chosenLaneIndex] = 0f;
        }

        rt.anchoredPosition = new Vector2(spawnX, 1050f);

        FallingUIObject falling = spawned.GetComponent<FallingUIObject>();
        if (!falling) falling = spawned.AddComponent<FallingUIObject>();

        if (isVlad)
        {
            falling.fallSpeed = obstacleSpeed;
        }
        else
        {
            float randomSpeedOffset = Random.Range(-speedVariance, speedVariance);
            falling.fallSpeed = obstacleSpeed + randomSpeedOffset;
        }

        falling.isVlad = isVlad;
    }

    public void GameOver()
    {
        gameEnded = true;
        Time.timeScale = 0f;
        if (gameOverPanel) gameOverPanel.SetActive(true);
    }

    public void WinGame()
    {
        gameEnded = true;
        Time.timeScale = 0f;
        if (winPanel) winPanel.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadEndScreen()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("EndScreen");
    }
}