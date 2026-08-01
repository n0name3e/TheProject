using UnityEngine;

public class Locker : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = true;
    public string interactText { get; set; } = "Open";
    public string nonInteractableText { get; set; }
    public AudioClip nonInteractableSound { get; set; }
    public AudioClip interactSound { get; set; }

    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider colliderToDisable;

    public void Interact()
    {
        animator.SetTrigger("Open");
        isInteractable = false;
        nonInteractableText = "";
        this.enabled = false;
        colliderToDisable.enabled = false;
    }
}
