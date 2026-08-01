using UnityEngine;

public class HealingPotion : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = true;
    [field: SerializeField] public AudioClip interactSound { get; set; }
    [field: SerializeField] public AudioClip nonInteractableSound { get; set; }
    [field: SerializeField] public string interactText { get; set; } = "Drink";
    [field: SerializeField] public string nonInteractableText { get; set; }


    public void Interact()
    {
        FindAnyObjectByType<PlayerHealth>().Heal();
        Destroy(gameObject);
    }
}
