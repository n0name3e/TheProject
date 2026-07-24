using UnityEngine;

public class MonitorObject : MonoBehaviour, IInteractable
{
    [field: SerializeField] public bool isInteractable { get; set; } = true;
    [field: SerializeField] public AudioClip interactSound { get; set; }

    [SerializeField] private GameObject image;

    public void Activate()
    {
        isInteractable = true;
        image.SetActive(true);
    }
    public void Interact()
    {
        UI.Instance.EnablePC();
    }
}
