using UnityEngine;

public class BombPickup : Item
{
    public int ammoAmount = 1;
    public AudioClip collectSound;

    public override void OnCollect(Player player)
    {
      
        player.AddBombs(ammoAmount);

        if (SoundManager.Instance != null && collectSound != null)
        {
            SoundManager.Instance.PlaySFX(collectSound);
        }

        Destroy(gameObject);
    }
}