using UnityEngine;

public class PlayerInteracting : MonoBehaviour
{
    [SerializeField] private LayerMask layers;
    private Camera mainCamera;
    private IInteractable currentInteractable;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward,
             out hit, 3f, layers))
        {
            if (hit.transform.TryGetComponent(out IInteractable interactable))
            {
                if (interactable.isInteractable)
                {
                    currentInteractable = interactable;
                }
                return;
            }
        }
        currentInteractable = null;
    }
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentInteractable != null)
            {
                currentInteractable.Interact();
                currentInteractable = null;
            }
        }
        UI.Instance.ToggleInteractableStuff(currentInteractable != null && Time.timeScale != 0);
    }
}
