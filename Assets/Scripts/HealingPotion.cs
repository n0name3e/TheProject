using UnityEngine;

public class HealingPotion : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = true;

    public void Interact()
    {
        FindAnyObjectByType<PlayerHealth>().Heal();
        Destroy(gameObject);
    }
}
