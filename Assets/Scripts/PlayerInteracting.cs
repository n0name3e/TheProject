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
        // should make it not every frame // done i guess // no its really bad
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward,
             out hit, 3f, layers))
        {
            if (hit.transform.TryGetComponent(out IInteractable interactable))
            {
                currentInteractable = interactable;
            }
            else
            {
                currentInteractable = null;
            }
        }
        else
        {
            currentInteractable = null;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentInteractable != null)
            {
                if (currentInteractable.isInteractable)
                {
                    currentInteractable.Interact();
                    if (currentInteractable.interactSound != null)
                    {
                        interactionSource.pitch = Random.Range(0.9f, 1.1f);
                        interactionSource.PlayOneShot(currentInteractable.interactSound);
                    }
                }
                else
                {
                    if (currentInteractable.interactSound != null)
                    {
                        interactionSource.pitch = Random.Range(0.9f, 1.1f);
                        interactionSource.PlayOneShot(currentInteractable.nonInteractableSound);
                    }
                }

                currentInteractable = null;
            }
        }
        if (Time.timeScale == 0f)
        {
            UI.Instance.ToggleInteractableStuff(null);
            return;
        }
        UI.Instance.ToggleInteractableStuff(currentInteractable);
    }

}
