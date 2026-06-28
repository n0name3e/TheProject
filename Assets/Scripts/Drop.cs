using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] private GameObject droppedObject;
    [SerializeField] private float chance = 1f;

    public void DropItem()
    {
        if (droppedObject == null)
            return;
        if (Random.value <= chance)
        {
            Instantiate(droppedObject, transform.position, Quaternion.identity);
        }
    }
}
