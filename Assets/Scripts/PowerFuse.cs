using UnityEngine;

public class PowerFuse : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = true;
    [field: SerializeField] public AudioClip interactSound { get; set; }


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
