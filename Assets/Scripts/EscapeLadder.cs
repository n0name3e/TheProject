using UnityEngine;

public class EscapeLadder : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = false;
    public string interactText { get; set; } = "Escape";
    public string nonInteractableText { get; set; } = "Cannot Escape Yet";
    [field: SerializeField] public AudioClip nonInteractableSound { get; set; }
    [field: SerializeField] public AudioClip interactSound { get; set; }
    public void Interact()
    {
        // victor
        print("emow");
        UI.Instance.ShowVictoryScreen();
    }
    public void Activate()
    {
        isInteractable = true;
    }
}
