using UnityEngine;

public class Coin : Item
{
    public int ScoreValue = 10;
    public AudioClip SoundCoin;

    public override void OnCollect(Player player)
    {
        base.OnCollect(player);

        GameManager.Instance.AddScore(ScoreValue);

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SoundCoin);
        }

        Destroy(gameObject);
    }

}