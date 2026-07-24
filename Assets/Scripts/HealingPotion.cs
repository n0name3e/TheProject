using UnityEngine;

public class HealingPotion : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = true;
    [field: SerializeField] public AudioClip interactSound { get; set; }


    public void Interact()
    {
        FindAnyObjectByType<PlayerHealth>().Heal();
        Destroy(gameObject);
    }
}
