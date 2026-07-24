using UnityEngine;

public class LootableBox : MonoBehaviour, IInteractable
{
    [field: SerializeField] public AudioClip interactSound { get; set; }

    public bool isInteractable { get; set; } = true;
    public GameObject objectToCreate;

    public void Interact()
    {
        Instantiate(objectToCreate, transform.position + Vector3.up * 2f, Quaternion.identity);
        Destroy(gameObject);
    }
}
