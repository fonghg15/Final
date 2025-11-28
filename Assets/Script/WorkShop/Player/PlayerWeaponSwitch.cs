using UnityEngine;

public class PlayerWeaponSwitch : MonoBehaviour
{
    public enum WeaponSlot { Sword = 1, Bow = 2, Bomb = 3 }

    [Header("Current Weapon")]
    public WeaponSlot currentWeapon = WeaponSlot.Sword;

    [Header("Weapon Prefabs")]
    public GameObject swordPrefab;
    public GameObject bowPrefab;
    public GameObject bombPrefab;

    private Player player;
    private Transform rightHand;

    private GameObject swordObject;
    private GameObject bowObject;
    private GameObject bombObject;

    private void Start()
    {
        player = GetComponent<Player>();
        if (player == null)
        {
            return;
        }

        rightHand = player.RightHand;
        if (rightHand == null)
        {
            return;
        }

        // สร้างอาวุธทั้งสามเป็นลูกของมือขวา
        if (swordPrefab != null)
        {
            swordObject = Instantiate(swordPrefab, rightHand);
            swordObject.transform.localPosition = Vector3.zero;
            swordObject.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        }

        if (bowPrefab != null)
        {
            bowObject = Instantiate(bowPrefab, rightHand);
            bowObject.transform.localPosition = Vector3.zero;
            bowObject.transform.localRotation = Quaternion.Euler(249f, -132f, -80f);
        }

        if (bombPrefab != null)
        {
            bombObject = Instantiate(bombPrefab, rightHand);
            bombObject.transform.localPosition = Vector3.zero;
            bombObject.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        }

        Equip(WeaponSlot.Sword);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) Equip(WeaponSlot.Sword);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Equip(WeaponSlot.Bow);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Equip(WeaponSlot.Bomb);
    }

    public void Equip(WeaponSlot slot)
    {
        currentWeapon = slot;

        if (swordObject != null)
            swordObject.SetActive(slot == WeaponSlot.Sword);

        if (bowObject != null)
            bowObject.SetActive(slot == WeaponSlot.Bow);

        if (bombObject != null)
            bombObject.SetActive(slot == WeaponSlot.Bomb);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.HighlightWeapon((int)slot);
        }

        Debug.Log("Switch weapon to: " + slot);
    }
}

