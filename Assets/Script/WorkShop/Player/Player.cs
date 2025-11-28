using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [Header("Hand setting")]
    public Transform RightHand;
    public Transform LeftHand;
    public List<Item> inventory = new List<Item>();

    [Header("Bomb Settings")]
    public GameObject thrownBombPrefab;
    public Transform throwPoint;
    public int bombCount = 0;

    [Header("Potion Settings")]
    public int potionCount = 0;
    public int potionHealAmount = 20;
    bool _isUsingPotion = false;

    [Header("Currency")]
    public int playerCoins = 0;

    [Header("Spawn")]
    public Transform spawnPoint;

    Vector3 _inputDirection;
    bool _isAttacking = false;
    bool _isInteract = false;
    bool _isThrowingBomb = false;

    private Camera _mainCamera;
    private Vector3 _lookDirection;

    private PlayerCombat combat;
    private PlayerInteraction interaction;
    private PlayerWeaponSwitch weaponSwitch;
    public static System.Action OnPlayerDead;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        health = maxHealth;
        _mainCamera = Camera.main;

        combat = GetComponent<PlayerCombat>();
        if (combat == null)
        {
            Debug.LogWarning("Player: PlayerCombat component not found on this GameObject.");
        }

        interaction = GetComponent<PlayerInteraction>();
        if (interaction == null)
        {
            Debug.LogWarning("Player: PlayerInteraction component not found on this GameObject.");
        }

        weaponSwitch = GetComponent<PlayerWeaponSwitch>();
        if (weaponSwitch == null)
        {
            Debug.LogWarning("Player: PlayerWeaponSwitch component not found on this GameObject.");
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateHealthBar(health, maxHealth);
        }

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
        }

        health = maxHealth;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateHealthBar(health, maxHealth);
        }

        _mainCamera = Camera.main;

        combat = GetComponent<PlayerCombat>();

        UpdateBombUI();
    }

    public void FixedUpdate()
    {
        Move(_inputDirection);


        Turn(_lookDirection);

        if (combat != null)
        {
            combat.Attack(_isAttacking);
        }
        _isAttacking = false;

        if (interaction != null)
        {
            interaction.Interact(_isInteract);
        }
        _isInteract = false;

        if (_isUsingPotion)
        {
            UsePotion();
            _isUsingPotion = false;
        }

        ThrowBomb(_isThrowingBomb);
    }

    public void Update()
    {
        HandleInput();
        HandleMouseLook();
    }

    public void AddItem(Item item)
    {
        inventory.Add(item);
    }

    // ---------------- INPUT ----------------
    private void HandleInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        _inputDirection = new Vector3(x, 0, y);

        if (Input.GetMouseButtonDown(0))
        {
            if (weaponSwitch != null)
            {
                switch (weaponSwitch.currentWeapon)
                {
                    case PlayerWeaponSwitch.WeaponSlot.Sword:
                    case PlayerWeaponSwitch.WeaponSlot.Bow:
                        _isAttacking = true;
                        break;

                    case PlayerWeaponSwitch.WeaponSlot.Bomb:
                        _isThrowingBomb = true;
                        break;
                }
            }
            else
            {
                _isAttacking = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _isInteract = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _isUsingPotion = true;
        }

    }

    private void HandleMouseLook()
    {
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float dist))
        {
            Vector3 point = ray.GetPoint(dist);
            _lookDirection = point - transform.position;
            _lookDirection.y = 0;
            _lookDirection.Normalize();
        }
    }

    public void Attack(bool isAttacking)
    {
        if (combat != null)
        {
            combat.Attack(isAttacking);
        }
    }

    public void Interact(bool interactable)
    {
        if (interaction != null)
        {
            interaction.Interact(interactable);
        }
    }

    protected override void Turn(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Quaternion lookRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 20f);
    }

    public void AddBombs(int amount)
    {
        bombCount += amount;
        UpdateBombUI();
    }

    private void UpdateBombUI()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateBombText(bombCount);
        }
    }

    private void ThrowBomb(bool isThrowing)
    {
        if (!isThrowing) return;
        _isThrowingBomb = false;

        if (bombCount <= 0)
        {
            Debug.Log("Out of bombs!");
            return;
        }

        if (thrownBombPrefab == null || throwPoint == null)
        {
            Debug.LogError("Missing bomb prefab or throwPoint");
            return;
        }

        bombCount--;
        UpdateBombUI();

        Instantiate(thrownBombPrefab, throwPoint.position, throwPoint.rotation);
        Debug.Log("Player threw a bomb!");
    }

    public void AddPotion(int amount)
    {
        potionCount += amount;
        UpdatePotionUI();
    }

    private void UsePotion()
    {
        if (potionCount <= 0)
        {
            Debug.Log("No potions left!");
            return;
        }

        if (health >= maxHealth)
        {
            Debug.Log("HP is full. No need to use potion.");
            return;
        }

        potionCount--;

        Heal(potionHealAmount);

        UpdatePotionUI();

        Debug.Log($"Used a potion. Remaining: {potionCount}");
    }

    private void UpdatePotionUI()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdatePotionText(potionCount);
        }
    }

}