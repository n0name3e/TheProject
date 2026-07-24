using UnityEngine;

public class CardSwipe : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = false;
    [field: SerializeField] public AudioClip interactSound { get; set; }

    [SerializeField] private BossDoor doorToActivate;

    public void Interact()
    {
        doorToActivate.isInteractable = true;
        GetComponent<Animator>().Play("Swipe");
        isInteractable = false;
    }
}
