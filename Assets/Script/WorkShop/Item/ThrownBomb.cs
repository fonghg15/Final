using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThrownBomb : MonoBehaviour
{
    public float speed = 5f;
    public float explosionRadius = 5f;
    public int explosionDamage = 50;
    public GameObject explosionEffectPrefab;

    private Rigidbody rb;
    private bool hasExploded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        rb.linearVelocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasExploded) return;

        if (other.GetComponent<Player>() != null) return;

        Explode();
    }

    void Explode()
    {
        hasExploded = true;

        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            if (hit.GetComponentInParent<Player>() != null)
                continue;

            Idestoryable dest = hit.GetComponentInParent<Idestoryable>();
            if (dest != null)
            {
                dest.TakeDamage(explosionDamage);
            }
        }

        Destroy(gameObject);

    }
}
