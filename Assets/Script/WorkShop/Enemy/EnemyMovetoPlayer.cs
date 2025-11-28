using UnityEngine;

public class EnemyMovetoPlayer : Enemy
{
    [Header("Melee Settings")]
    public float attackDistance = 1.5f;
    public float moveSpeedMultiplier = 1f;

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

        Vector3 toPlayer = player.transform.position - transform.position;
        toPlayer.y = 0f;

        float dist = toPlayer.magnitude;

        if (toPlayer.sqrMagnitude > 0.001f)
        {
            Turn(toPlayer);
        }

        timer -= Time.deltaTime;

        if (dist < attackDistance)
        {
            Attack(player);
        }
        else
        {
            animator.SetBool("Attack", false);

            Vector3 moveDir = toPlayer.normalized * moveSpeedMultiplier;
            Vector3 avoidDir = AvoidObstacle(moveDir);

            Move(avoidDir);
        }
    }
}