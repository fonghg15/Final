using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInteraction : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("PlayerInteraction: Player component not found!");
        }
    }


    public void Interact(bool isInteract)
    {
        if (!isInteract || player == null)
            return;

        Identity targetIdentity = player.InFront;
        if (targetIdentity == null)
        {
            return;
        }

        IInteractable interactTarget = targetIdentity.GetComponent<IInteractable>();
        if (interactTarget == null)
        {
            return;
        }

        if (interactTarget.isInteractable)
        {
            interactTarget.Interact(player);
            Debug.Log($"Player interacts with {targetIdentity.Name}");
        }
    }
}
