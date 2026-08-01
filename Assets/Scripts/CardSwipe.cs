using UnityEngine;

public class CardSwipe : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = false;
    [field: SerializeField] public AudioClip interactSound { get; set; }
    [field: SerializeField] public AudioClip nonInteractableSound { get; set; }
    [field: SerializeField] public string interactText { get; set; }
    [field: SerializeField] public string nonInteractableText { get; set; }

    [SerializeField] private BossDoor doorToActivate;

    public void Interact()
    {
        doorToActivate.isInteractable = true;
        GetComponent<Animator>().Play("Swipe");
        isInteractable = false;
    }
}
