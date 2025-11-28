using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponPickup : MonoBehaviour
{
    public PlayerWeaponSwitch.WeaponSlot weaponType;
    public bool destroyOnPickup = true;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player == null) return;

        PlayerWeaponSwitch weaponSwitch = player.GetComponent<PlayerWeaponSwitch>();
        if (weaponSwitch == null) return;

        weaponSwitch.Equip(weaponType);

        if (destroyOnPickup)
        {
            Destroy(gameObject);
        }
    }
}
