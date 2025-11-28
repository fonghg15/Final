using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerCombat : MonoBehaviour
{
    private Player player;
    private Animator animator;
    private PlayerWeaponSwitch weaponSwitch;
    private Bow bow;

    [Header("Upgrade Levels")]
      public int swordUpgradeLevel = 0;
    public int bowUpgradeLevel = 0;

    [Header("Sword Hit Settings")]
    public float baseAttackRange = 1.2f;
    public float extraRangeLevel2 = 0.5f;
    public float attackRadius = 0.7f;

    [Header("Sword Attack Window")]
    public bool swordAttackActive = false;
    public float swordAttackDuration = 0.25f;

    [Header("Sword Knockback Settings")]
    public float knockbackForce = 5f; 
    public float knockbackStunTime = 0.25f;

    private void Awake()
    {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
        weaponSwitch = GetComponent<PlayerWeaponSwitch>();
    }

    public void Attack(bool isAttacking)
    {
        if (!isAttacking) return;

        if (weaponSwitch == null)
        {
            weaponSwitch = GetComponent<PlayerWeaponSwitch>();
            if (weaponSwitch == null) return;
        }

        switch (weaponSwitch.currentWeapon)
        {
            case PlayerWeaponSwitch.WeaponSlot.Sword:
                if (animator != null)
                {
                    animator.SetTrigger("Attack");
                }
                if (!swordAttackActive)
                {
                    StartCoroutine(SwordAttackWindow());
                }
                break;

            case PlayerWeaponSwitch.WeaponSlot.Bow:

                if (animator != null)
                {
                    animator.SetTrigger("Shoot");
                }

                break;

            case PlayerWeaponSwitch.WeaponSlot.Bomb:
                // ระเบิดจัดการใน Player.cs แล้ว
                break;
        }
    }


    public void FireArrow()
    {
        Debug.Log("ANIM EVENT REACHED!");

        if (bow == null)
        {
            bow = GetComponentInChildren<Bow>(true);
            Debug.Log("Search BOW again: " + bow);
        }

        if (bow == null)
        {
            Debug.LogError("BOW STILL NULL!");
            return;
        }

        bow.bowUpgradeLevel = bowUpgradeLevel;
        Debug.Log("Firing Arrow now!");
        bow.TryShootFromPlayer();
    }




    private IEnumerator SwordAttackWindow()
    {
        swordAttackActive = true;
        yield return new WaitForSeconds(swordAttackDuration);
        swordAttackActive = false;
    }
}
