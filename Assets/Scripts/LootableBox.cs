using UnityEngine;

public class LootableBox : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = true;
    public GameObject objectToCreate;

    public void Interact()
    {
        Instantiate(objectToCreate, transform.position + Vector3.up * 2f, Quaternion.identity);
        Destroy(gameObject);
    }
}
