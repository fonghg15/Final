using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Arrow : MonoBehaviour
{
    public float speed = 30f;
    public int damage = 25;
    public float lifeTime = 5f;

    [Header("Upgrade Effect")]
    public int upgradeLevel = 0;
    public int extraDamageLv2 = 10;
    public float knockbackForce = 8f;

    private Rigidbody rb;
    private Transform owner;
    private Vector3 shootDirection = Vector3.forward;
    public Vector3 modelRotationOffset = new Vector3(90f, 0f, 0f);

    public void Init(Transform ownerTransform, Vector3 direction)
    {
        owner = ownerTransform;
        shootDirection = direction.normalized;
        SetupAndFire();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = 0;
    }

    private void SetupAndFire()
    {
        transform.rotation =
            Quaternion.LookRotation(shootDirection, Vector3.up) *
            Quaternion.Euler(modelRotationOffset);

        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponentInParent<Player>() != null)
            return;


        var target = other.GetComponentInParent<Idestoryable>();
        if (target != null)
        {
            int finalDamage = damage;
            if (upgradeLevel >= 2)
                finalDamage += extraDamageLv2;

            target.TakeDamage(finalDamage);


            Enemy enemyAI = other.GetComponentInParent<Enemy>();
            Rigidbody targetRb = other.GetComponentInParent<Rigidbody>();

            if (upgradeLevel >= 1 && targetRb != null)
            {

                Vector3 vel = targetRb.linearVelocity;
                vel.x = 0f;
                vel.z = 0f;
                targetRb.linearVelocity = vel;


                Transform source = owner != null ? owner : transform;
                Vector3 dir = (other.transform.position - source.position).normalized;
                dir.y = 0f;

                targetRb.AddForce(dir * knockbackForce, ForceMode.Impulse);

                if (enemyAI != null)
                {
                    enemyAI.Stun(0.25f);
                }
            }

            Destroy(gameObject);
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Wall") ||
            other.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            Destroy(gameObject);
        }
    }
}
