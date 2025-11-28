using UnityEngine;

public class Bow : Item
{
    public GameObject arrowPrefab;
    public Transform spawnPoint;
    public float fireRate = 0.15f;

    public int bowUpgradeLevel = 0;

    private float lastFireTime = 0f;
    private Player owner;

    private void Awake()
    {
        owner = GetComponentInParent<Player>();
        if (owner == null)
        {
            Debug.LogWarning("[Bow] Owner (Player) not found in parents.");
        }
    }

    public void TryShootFromPlayer()
    {
        if (Time.time < lastFireTime + fireRate)
            return;

        if (GameManager.Instance == null)
        {
            Debug.LogError("[Bow] GameManager.Instance is null");
            return;
        }

        if (!GameManager.Instance.TryUseArrow())
        {
            Debug.Log("[Bow] No arrows left (GameManager says 0).");
            return;
        }

        if (arrowPrefab == null || spawnPoint == null)
        {
            Debug.LogError("[Bow] Missing arrowPrefab or spawnPoint");
            return;
        }

        lastFireTime = Time.time;

        Vector3 shootDir = spawnPoint.forward;
        Camera cam = Camera.main;
        if (cam != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);


            Plane plane = new Plane(Vector3.up, new Vector3(0, spawnPoint.position.y, 0));
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                shootDir = (hitPoint - spawnPoint.position).normalized;
            }
        }

        Vector3 spawnPos = spawnPoint.position;
        Quaternion rot = Quaternion.LookRotation(shootDir, Vector3.up);

        GameObject arrowObj = Instantiate(arrowPrefab, spawnPos, rot);
        Arrow arr = arrowObj.GetComponent<Arrow>();
        if (arr != null)
        {

            arr.upgradeLevel = bowUpgradeLevel;


            arr.Init(owner != null ? owner.transform : spawnPoint, shootDir);
        }

        Debug.Log($"[Bow] Shoot to mouse dir={shootDir}, level={bowUpgradeLevel}, remainArrows={GameManager.Instance.currentArrows}");
    }

}
