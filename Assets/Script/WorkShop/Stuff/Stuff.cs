using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class Stuff : Identity
{
    public TMP_Text interactionTextUI;
    protected Collider _collider;
    public bool isLock = true;

    public override void SetUP()
    {
        interactionTextUI = GetComponentInChildren<TMP_Text>();
        _collider = GetComponent<Collider>();

        if (interactionTextUI == null)
        {
            Debug.LogWarning("[Stuff] interactionTextUI is NULL on " + name +
                             ". Please put TMP_Text as child.", this);
        }
    }

    public void Update()
    {
        if (interactionTextUI == null) return;

        if (GetDistanPlayer() >= 2f || !isLock)
        {
            interactionTextUI.gameObject.SetActive(false);
        }
        else
        {
            interactionTextUI.gameObject.SetActive(true);
        }
    }
}
