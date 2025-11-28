using TMPro;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject upgradePanel;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI swordLevelText;
    public TextMeshProUGUI bowLevelText;

    [Header("Upgrade Prices")]
    public int swordLevel1Price = 10;
    public int swordLevel2Price = 20;
    public int bowLevel1Price = 10;
    public int bowLevel2Price = 20;

    [Header("Arrow Prices")]
    public int arrowPack10Price = 10;

    private PlayerCombat playerCombat;
    private bool isOpen = false;

    private void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        if (playerCombat == null)
        {
            Debug.LogWarning("UpgradeUI: ไม่พบ PlayerCombat ในฉาก");
        }

        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }

        RefreshUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            TogglePanel();
        }

        if (isOpen && GameManager.Instance != null && coinText != null)
        {
            coinText.text = GameManager.Instance.CurrentScore.ToString();
        }
    }

    public void TogglePanel()
    {
        if (upgradePanel == null) return;

        isOpen = !isOpen;
        upgradePanel.SetActive(isOpen);

        Time.timeScale = isOpen ? 0f : 1f;

        RefreshUI();
    }

    private void RefreshUI()
    {
        if (GameManager.Instance != null && coinText != null)
        {
            coinText.text = GameManager.Instance.CurrentScore.ToString();
        }

        if (playerCombat == null)
        {
            playerCombat = FindFirstObjectByType<PlayerCombat>();
        }

        if (playerCombat != null)
        {
            if (swordLevelText != null)
                swordLevelText.text = $"Sword Lv {playerCombat.swordUpgradeLevel}";

            if (bowLevelText != null)
                bowLevelText.text = $"Bow Lv {playerCombat.bowUpgradeLevel}";
        }
    }

    public void UpgradeSwordKnockback()
    {
        if (EnsurePlayerCombat() == false) return;

        if (playerCombat.swordUpgradeLevel >= 1)
        {
            Debug.Log("Sword already Lv1 or higher.");
            return;
        }

        if (GameManager.Instance != null && GameManager.Instance.TrySpendScore(swordLevel1Price))
        {
            playerCombat.swordUpgradeLevel = 1;
            RefreshUI();
        }
        else
        {
            Debug.Log("Not enough coins for Sword Lv1.");
        }
    }

    public void UpgradeSwordRange()
    {
        if (EnsurePlayerCombat() == false) return;

        if (playerCombat.swordUpgradeLevel >= 2)
        {
            Debug.Log("Sword already Lv2.");
            return;
        }

        if (GameManager.Instance != null && GameManager.Instance.TrySpendScore(swordLevel2Price))
        {
            playerCombat.swordUpgradeLevel = 2;
            RefreshUI();
        }
        else
        {
            Debug.Log("Not enough coins for Sword Lv2.");
        }
    }

    public void UpgradeBowDouble()
    {
        if (EnsurePlayerCombat() == false) return;

        if (playerCombat.bowUpgradeLevel >= 1)
        {
            Debug.Log("Bow already Lv1 or higher.");
            return;
        }

        if (GameManager.Instance != null && GameManager.Instance.TrySpendScore(bowLevel1Price))
        {
            playerCombat.bowUpgradeLevel = 1;
            RefreshUI();
        }
        else
        {
            Debug.Log("Not enough coins for Bow Lv1.");
        }
    }

    public void UpgradeBowTriple()
    {
        if (EnsurePlayerCombat() == false) return;

        if (playerCombat.bowUpgradeLevel >= 2)
        {
            Debug.Log("Bow already Lv2.");
            return;
        }

        if (GameManager.Instance != null && GameManager.Instance.TrySpendScore(bowLevel2Price))
        {
            playerCombat.bowUpgradeLevel = 2;
            RefreshUI();
        }
        else
        {
            Debug.Log("Not enough coins for Bow Lv2.");
        }
    }

    public void BuyArrowPack10()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.TrySpendScore(arrowPack10Price))
        {
            GameManager.Instance.AddArrows(10);
            RefreshUI();
            Debug.Log("[UpgradeUI] Bought 10 arrows.");
        }
        else
        {
            Debug.Log("Not enough coins to buy arrows.");
        }
    }

    private bool EnsurePlayerCombat()
    {
        if (playerCombat == null)
        {
            playerCombat = FindFirstObjectByType<PlayerCombat>();
        }

        return playerCombat != null;
    }
}
