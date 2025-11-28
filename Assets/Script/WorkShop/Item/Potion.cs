public class Potion : Item
{
    public int AmountHealth = 20;

    public override void OnCollect(Player player)
    {
        base.OnCollect(player);

        player.AddPotion(1);

        if (player.potionHealAmount != AmountHealth)
        {
            player.potionHealAmount = AmountHealth;
        }

        Destroy(gameObject);
    }
}
