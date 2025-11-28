using UnityEngine;

public class Character : Identity, Idestoryable
{
    [Header("Stats")]
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _health = 100;
    public int Damage = 10;
    public int Deffent = 0;
    public float movementSpeed = 5f;

    protected Rigidbody rb;
    protected Animator animator;

    // --- Idestoryable interface ---
    public int health
    {
        get => _health;
        set => _health = Mathf.Clamp(value, 0, maxHealth);
    }

    public int maxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    public event System.Action<Idestoryable> OnDestory;

    public override void SetUP()
    {
        base.SetUP();

        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = GetComponentInChildren<Rigidbody>();

        animator = GetComponent<Animator>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        health = maxHealth;

        if (this is Player && GameManager.Instance != null)
        {
            GameManager.Instance.UpdateHealthBar(health, maxHealth);
        }
    }

    public virtual void TakeDamage(int damageAmount)
    {
        int finalDamage = Mathf.Max(1, damageAmount - Deffent);
        health = Mathf.Clamp(health - finalDamage, 0, maxHealth);

        if (this is Player && GameManager.Instance != null)
        {
            GameManager.Instance.UpdateHealthBar(health, maxHealth);
        }

        if (health <= 0)
        {

            OnDestory?.Invoke(this);

            if (this is Player)
            {
                Debug.Log("Player Dead");
                Player.OnPlayerDead?.Invoke();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public virtual void Heal(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);

        if (this is Player && GameManager.Instance != null)
        {
            GameManager.Instance.UpdateHealthBar(health, maxHealth);
        }
    }

    protected virtual void Turn(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
    }

    protected virtual void Move(Vector3 direction)
    {
        if (rb == null) return;

        Vector3 vel = direction * movementSpeed;
        vel.y = rb.linearVelocity.y;
        rb.linearVelocity = vel;

        if (animator != null)
        {
            animator.SetFloat("Speed", direction.magnitude);
        }
    }
}
