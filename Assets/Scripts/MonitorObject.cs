using UnityEngine;

public class MonitorObject : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = true;
    public void Interact()
    {
        UI.Instance.EnablePC();
    }
}
