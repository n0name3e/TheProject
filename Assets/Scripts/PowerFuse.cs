using UnityEngine;

public class PowerFuse : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = true;
    [field: SerializeField] public AudioClip interactSound { get; set; }
    [field: SerializeField] public AudioClip nonInteractableSound { get; set; }
    [field: SerializeField] public string interactText { get; set; } = "Turn On";
    [field: SerializeField] public string nonInteractableText { get; set; }


    [SerializeField] private GameObject closedFuse;
    [SerializeField] private GameObject openFuse;
    [SerializeField] private MonitorObject monitor;
    [SerializeField] private Light lighting;

    public void Interact()
    {
        closedFuse.SetActive(false);
        openFuse.SetActive(true);
        monitor.Activate();
        lighting.intensity = 40f;
        isInteractable = false;
    }
}
