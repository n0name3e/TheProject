using UnityEngine;

public interface IInteractable
{
    public bool isInteractable { get; set; }
    public string interactText { get; set; }
    public string nonInteractableText { get; set; }
    public AudioClip nonInteractableSound { get; set; }
    public AudioClip interactSound { get; set; }
    void Interact();
}
