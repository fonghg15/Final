using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EnemyWaveSpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public EnemyMovetoPlayer meleePrefab;
    public EnemyRange rangePrefab;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Wave Settings")]
    public int meleePerWave = 6;
    public int rangePerWave = 4;
    public float spawnDelay = 0.1f;

    [Header("Rules")]
    public int killTarget = 20;

    [Header("UI")]
    public TMP_Text killText;
    public GameObject winPanel;
    public GameObject overPanel;

    private int enemiesAlive = 0;
    private int totalKilled = 0;

    private static EnemyWaveSpawner _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        Enemy.OnAnyEnemyDeath = null;
        Player.OnPlayerDead = null;
    }

    private void OnEnable()
    {
        Enemy.OnAnyEnemyDeath += HandleEnemyDeath;
        Player.OnPlayerDead += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Enemy.OnAnyEnemyDeath -= HandleEnemyDeath;
        Player.OnPlayerDead -= HandlePlayerDeath;
    }

    private void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (overPanel != null) overPanel.SetActive(false);

        Time.timeScale = 1f;
        UpdateKillText();
        SpawnWave();
    }

    private void SpawnWave()
    {
        enemiesAlive = meleePerWave + rangePerWave;

        for (int i = 0; i < meleePerWave; i++)
            SpawnOne(meleePrefab);

        for (int i = 0; i < rangePerWave; i++)
            SpawnOne(rangePrefab);
    }

    private void SpawnOne(Enemy enemyPrefab)
    {
        Transform p = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, p.position, p.rotation);
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        totalKilled++;
        enemiesAlive--;

        UpdateKillText();

        if (totalKilled >= killTarget)
        {
            Win();
            return;
        }

        if (enemiesAlive <= 0)
        {
            SpawnWave();
        }
    }

    private void HandlePlayerDeath()
    {
        Over();
    }

    private void UpdateKillText()
    {
        if (killText != null)
            killText.text = totalKilled + " / " + killTarget;
    }

    private void Win()
    {
        if (winPanel != null) winPanel.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("🎉 WIN! 🎉");
    }

    private void Over()
    {
        if (overPanel != null) overPanel.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("💀 GAME OVER 💀");
    }

    public void RestartStage()
    {
        Time.timeScale = 1f;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetCoins();
        }

    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
