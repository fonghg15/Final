using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class GameManager : MonoBehaviour
{
    // ------------------- Singleton -------------------
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameManager>();

                if (_instance == null)
                {
                    Debug.LogError("GameManager instance is null! Is it in the scene?");
                }
            }

            return _instance;
        }
    }

    // ------------------- UI References -------------------
    [Header("UI - General")]
    public GameObject pauseMenuUI;

    [Header("UI - Health")]
    public Slider HPBar;

    [Header("UI - Score / Coins (บน HUD)")]
    public TMP_Text scoreText;

    [Header("UI - Weapons")]
    public Image weaponSlot1Icon;
    public Image weaponSlot2Icon;
    public Image weaponSlot3Icon;
    public Color weaponNormalColor = Color.white;
    public Color weaponSelectedColor = Color.deepPink;

    [Header("UI - Ammo / Bomb / Potion")]
    public TMP_Text ammoText;
    public TMP_Text bombText;
    public TMP_Text potionText;

    [Header("UI - Panels")]
    public GameObject gameOverPanel;

    // ------------------- Game Data -------------------
    [Header("Arrows")]
    public int maxArrows = 30;
    public int currentArrows = 0;

    [Header("Score / Coins")]
    [Tooltip("Coins HUD and UpgradeUI")]
    public int currentScore = 0;
    public int CurrentScore => currentScore;

    [Header("Scene")]
    public string mainMenuSceneName = "MainMenu";

    // ------------------- Unity Life Cycle -------------------
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        if (ammoText != null)
            ammoText.gameObject.SetActive(false);

        if (bombText != null)
            bombText.gameObject.SetActive(false);

        if (potionText != null)
            potionText.gameObject.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void Start()
    {
        Player p = FindFirstObjectByType<Player>();
        if (p != null)
        {
            p.playerCoins = currentScore;
            UpdateHealthBar(p.health, p.maxHealth);
        }

        UpdateScoreUI();
    }

    // ------------------- Health -------------------
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (HPBar == null) return;

        HPBar.maxValue = maxHealth;
        HPBar.value = currentHealth;
    }

    // ------------------- Score / Coins -------------------
    public void AddScore(int amount)
    {
        if (amount == 0) return;

        currentScore += amount;
        if (currentScore < 0) currentScore = 0;

        Player p = FindFirstObjectByType<Player>();
        if (p != null)
        {
            p.playerCoins = currentScore;
        }

        UpdateScoreUI();
    }

    public bool TrySpendScore(int amount)
    {
        if (amount <= 0) return true;

        if (currentScore < amount)
        {
            Debug.Log("Not enough coins.");
            return false;
        }

        currentScore -= amount;
        if (currentScore < 0) currentScore = 0;

        Player p = FindFirstObjectByType<Player>();
        if (p != null)
        {
            p.playerCoins = currentScore;
        }

        UpdateScoreUI();
        return true;
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }

    public void ResetCoins()
    {
        currentScore = 0;

        Player player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            player.playerCoins = 0;
        }

        UpdateScoreUI();
    }

    // ------------------- Pause -------------------
    public void TogglePause()
    {
        bool isPaused = Time.timeScale <= 0f;
        if (isPaused)
        {
            Time.timeScale = 1f;
            if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        }
    }

    // ------------------- Ammo / Bomb / Potion -------------------
    public void UpdateAmmoText(int count)
    {
        if (ammoText == null) return;

        ammoText.gameObject.SetActive(true);
        ammoText.text = count.ToString();
    }

    public void UpdateBombText(int count)
    {
        if (bombText == null) return;

        bombText.gameObject.SetActive(true);
        bombText.text = count.ToString();
    }

    public void UpdatePotionText(int count)
    {
        if (potionText == null) return;

        potionText.gameObject.SetActive(true);
        potionText.text = count.ToString();
    }

    public void AddArrows(int amount)
    {
        currentArrows = Mathf.Clamp(currentArrows + amount, 0, maxArrows);
        UpdateAmmoText(currentArrows);
    }

    public bool TryUseArrow()
    {
        if (currentArrows <= 0) return false;

        currentArrows--;
        UpdateAmmoText(currentArrows);
        return true;
    }

    public void HighlightWeapon(int slotIndex)
    {
        if (weaponSlot1Icon != null)
            weaponSlot1Icon.color = weaponNormalColor;
        if (weaponSlot2Icon != null)
            weaponSlot2Icon.color = weaponNormalColor;
        if (weaponSlot3Icon != null)
            weaponSlot3Icon.color = weaponNormalColor;

        switch (slotIndex)
        {
            case 1:
                if (weaponSlot1Icon != null) weaponSlot1Icon.color = weaponSelectedColor;
                break;
            case 2:
                if (weaponSlot2Icon != null) weaponSlot2Icon.color = weaponSelectedColor;
                break;
            case 3:
                if (weaponSlot3Icon != null) weaponSlot3Icon.color = weaponSelectedColor;
                break;
        }
    }

    public void ShowGameOver()
    {
        Debug.Log("[GameManager] Player died – GameOver system disabled.");
    }

    public void RestartGame()
    {
        Debug.Log(">>> RestartGame CLICKED");

        Time.timeScale = 1f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        ResetCoins();

        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
