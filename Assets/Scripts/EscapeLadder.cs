using UnityEngine;

public class EscapeLadder : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = false;
    [field: SerializeField] public string interactText { get; set; } = "Escape";
    [field: SerializeField] public string nonInteractableText { get; set; } = "Cannot Escape";
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
