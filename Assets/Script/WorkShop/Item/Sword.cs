using UnityEngine;

public class Sword : Item
{
    public int Damage = 25;

    private Player owner;
    private PlayerCombat ownerCombat;

    private void Start()
    {
        owner = GetComponentInParent<Player>();
        if (owner != null)
        {
            owner.Damage = Damage;
            ownerCombat = owner.GetComponent<PlayerCombat>();
            Debug.Log($"Sword: Set player damage to {Damage}");
        }
        else
        {
            Debug.LogWarning("Sword: No Player found in parent hierarchy.");
        }
    }

    private new void OnTriggerEnter(Collider other)
    {
        TryHitTarget(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryHitTarget(other);
    }

    private void TryHitTarget(Collider other)
    {
        if (owner == null || ownerCombat == null) return;
        if (!ownerCombat.swordAttackActive) return;

        if (other.GetComponentInParent<Player>() != null) return;

        Idestoryable dest = other.GetComponentInParent<Idestoryable>();
        if (dest == null) return;

        dest.TakeDamage(owner.Damage);

        if (ownerCombat.swordUpgradeLevel >= 1)
        {
            Rigidbody enemyRb = other.GetComponentInParent<Rigidbody>();
            if (enemyRb != null)
            {
                Vector3 vel = enemyRb.linearVelocity;
                vel.x = 0f;
                vel.z = 0f;
                enemyRb.linearVelocity = vel;

                Vector3 dir = (other.transform.position - owner.transform.position).normalized;
                dir.y = 0f;

                enemyRb.AddForce(dir * ownerCombat.knockbackForce, ForceMode.Impulse);

                Enemy enemyAI = other.GetComponentInParent<Enemy>();
                if (enemyAI != null)
                {
                    enemyAI.Stun(ownerCombat.knockbackStunTime);
                }
            }
        }

        ownerCombat.swordAttackActive = false;
    }
}