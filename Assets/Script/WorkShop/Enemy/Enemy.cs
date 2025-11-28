using System;
using UnityEngine;

public class Enemy : Character
{
    protected enum State { idel, cheses, attack, death }

    [SerializeField] protected float TimeToAttack = 1f;
    protected State currentState = State.idel;
    protected float timer = 0f;

    [Header("Stun & Knockback Settings")]
    public float stunDuration = 0.25f;
    [Range(0f, 1f)]
    public float knockbackResistance = 0f;

    protected bool isStunned = false;
    protected float stunTimer = 0f;

    [Header("Drop Settings")]
    public GameObject coinPrefab;
    public int minCoins = 1;
    public int maxCoins = 3;
    public static Action<Enemy> OnAnyEnemyDeath;
    private bool isDead = false;

    private void Awake()
    {
        SetUP();
    }

    public override void SetUP()
    {
        base.SetUP();
        OnDestory += HandleDeath;
    }

    protected Vector3 AvoidObstacle(Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                Vector3 right = transform.right;
                Vector3 left = -transform.right;

                bool canRight = !Physics.Raycast(transform.position, right, 1f);
                bool canLeft = !Physics.Raycast(transform.position, left, 1f);

                if (canRight) return right;
                if (canLeft) return left;

                return -transform.forward;
            }
        }

        return direction;
    }


    public void Stun(float duration)
    {

        float d = duration > 0f ? duration : stunDuration;

        isStunned = true;
        stunTimer = d;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    protected override void Turn(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;
    }

    protected virtual void Attack(Player _player)
    {
        if (timer <= 0)
        {
            _player.TakeDamage(Damage);
            animator.SetBool("Attack", true);
            timer = TimeToAttack;
        }
    }

    private void HandleDeath(Idestoryable dead)
    {

        if (isDead) return;
        isDead = true;

        DropCoins();                   
        OnAnyEnemyDeath?.Invoke(this);

        Destroy(gameObject);
    }

    private void DropCoins()
    {
        if (coinPrefab == null)
        {
            Debug.LogWarning("Enemy: coinPrefab is not assigned.", this);
            return;
        }

        int coinCount = UnityEngine.Random.Range(minCoins, maxCoins + 1);

        for (int i = 0; i < coinCount; i++)
        {
            Vector3 offset = new Vector3(
                UnityEngine.Random.Range(-0.5f, 0.5f),
                0.3f,
                UnityEngine.Random.Range(-0.5f, 0.5f)
            );

            Instantiate(coinPrefab, transform.position + offset, Quaternion.identity);
        }
    }
}