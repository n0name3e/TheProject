using UnityEngine;

public class PlayerInteracting : MonoBehaviour
{
    [SerializeField] private LayerMask layers;
    [SerializeField] private AudioSource interactionSource;
    private Camera mainCamera;
    private IInteractable currentInteractable;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (interactionSource == null)
        {
            interactionSource = GetComponent<AudioSource>();
        }
    }
    void Update()
    {
        RaycastHit hit;
        // should make it not every frame
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
                if (currentInteractable.interactSound != null)
                {
                    interactionSource.pitch = Random.Range(0.9f, 1.1f);
                    interactionSource.PlayOneShot(currentInteractable.interactSound);
                }
                currentInteractable = null;
            }
        }
        UI.Instance.ToggleInteractableStuff(currentInteractable != null && Time.timeScale != 0);
    }
}
