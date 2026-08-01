using UnityEngine;

public class LootableBox : MonoBehaviour, IInteractable
{
    [field: SerializeField] public AudioClip interactSound { get; set; }
    [field: SerializeField] public AudioClip nonInteractableSound { get; set; }
    [field: SerializeField] public string interactText { get; set; } = "Open";
    [field: SerializeField] public string nonInteractableText { get; set; }

    public bool isInteractable { get; set; } = true;
    public GameObject objectToCreate;


    public void Interact()
    {
        Instantiate(objectToCreate, transform.position + Vector3.up * 2f, Quaternion.identity);
        Destroy(gameObject);
    }
}
