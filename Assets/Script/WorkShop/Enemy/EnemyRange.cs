using UnityEngine;

public class EnemyRange : Enemy
{
    [Header("Range Move Settings")]
    public float stopDistance = 6f;
    public float fleeDistance = 3f;
    public float moveSpeedMultiplier = 1f;
    public float fleeSpeedMultiplier = 1.2f;

    [Header("Combat Range")]
    public float shootDistance = 12f;

    [Header("Projectile Settings")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletSpeed = 8f;
    public float attackCooldown = 1.2f;

    private void Update()
    {
        if (player == null)
        {
            animator.SetBool("Attack", false);
            return;
        }

        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
            }

            animator.SetBool("Attack", false);
            return;
        }

        timer -= Time.deltaTime;

        Vector3 toPlayer = player.transform.position - transform.position;
        toPlayer.y = 0f;

        float dist = toPlayer.magnitude;

        if (toPlayer.sqrMagnitude > 0.001f)
        {
            Turn(toPlayer);
        }

        Vector3 moveDir = Vector3.zero;

        if (dist > stopDistance)
        {

            moveDir = toPlayer.normalized * moveSpeedMultiplier;
        }
        else if (dist < fleeDistance)
        {
            moveDir = (-toPlayer).normalized * fleeSpeedMultiplier;
        }

        if (moveDir != Vector3.zero)
        {
            animator.SetBool("Attack", false);

            Vector3 avoidDir = AvoidObstacle(moveDir);
            Move(avoidDir);
        }
        else
        {
            animator.SetBool("Attack", false);

            if (dist <= shootDistance)
            {
                Attack(player);
            }
            else
            {
                Vector3 extraMove = toPlayer.normalized * moveSpeedMultiplier;
                Vector3 avoidDir = AvoidObstacle(extraMove);
                Move(avoidDir);
            }
        }
    }

    protected override void Attack(Player _player)
    {
        if (timer > 0f) return;

        timer = attackCooldown;
        animator.SetBool("Attack", true);

        if (bulletPrefab == null || shootPoint == null)
        {
            Debug.LogWarning("[EnemyRange] Missing bulletPrefab or shootPoint", this);
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 dir = (_player.transform.position - shootPoint.position);
            dir.y = 0f;
            dir.Normalize();
            rb.linearVelocity = dir * bulletSpeed;
        }
    }
}