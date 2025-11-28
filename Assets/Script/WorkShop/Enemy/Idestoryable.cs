using UnityEngine;

public interface Idestoryable
{
    // ¤Ø³ÊÁºÑµÔÊÓËÃÑº¤èÒ¾ÅÑ§ªÕÇÔµ»Ñ¨¨ØºÑ¹
    int health { get; set; }

    // ¤Ø³ÊÁºÑµÔÊÓËÃÑº¤èÒ¾ÅÑ§ªÕÇÔµÊÙ§ÊØ´ (ÍÒ¨ãªéËÃ×ÍäÁè¡çä´é)
    int maxHealth { get; set; }

    // àÁ¸Í´ÊÓËÃÑºÃÑº¤ÇÒÁàÊÕÂËÒÂ
    void TakeDamage(int damageAmount) { 
        health -= damageAmount;
    }
    public event System.Action<Idestoryable> OnDestory;
}
