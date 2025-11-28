using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Box : Stuff, IInteractable, Idestoryable
{
    private static int destroyedBoxCount = 0;

    public Box()
    {
        Name = "Box";
    }

    [Header("Drop Prefabs")]
    public GameObject potionPrefab;
    public GameObject bombPrefab;

    public bool isInteractable
    {
        get => isLock;
        set => isLock = value;
    }

    [SerializeField] private int _maxHealth = 1;
    private int _health;

    public int health
    {
        get => _health;
        set => _health = Mathf.Clamp(value, 0, _maxHealth);
    }

    public int maxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }
    // -----------------------------------

    private Rigidbody rb;

    public event System.Action<Idestoryable> OnDestory;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = maxHealth;
    }

    public void Interact(Player player)
    {
        rb.isKinematic = !rb.isKinematic;
        isInteractable = false;
    }

    public void TakeDamage(int damageAmount)
    {
        if (health <= 0) return;

        health -= damageAmount;

        if (health <= 0)
        {
            destroyedBoxCount++;    

            DropItem();
            OnDestory?.Invoke(this);
            Destroy(gameObject);
        }
    }

    private void DropItem()
    {
        bool dropBomb = (destroyedBoxCount % 3 == 0);

        GameObject prefab = dropBomb ? bombPrefab : potionPrefab;

        if (prefab == null)
        {
            Debug.LogWarning("Box drop prefab is not assigned!", this);
            return;
        }

        Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
        Instantiate(prefab, spawnPos, Quaternion.identity);

        Debug.Log(dropBomb ? "Dropped Bomb" : "Dropped Potion");
    }
}
