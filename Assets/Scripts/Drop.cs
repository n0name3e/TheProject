using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] private GameObject droppedObject;
    [SerializeField] private float chance = 1f;

    public void DropItem()
    {
        if (droppedObject == null)
            return;
        if (UI.Instance.emptyDropsInARow >= 3)
        {
            if (chance < 1f)
            {
                chance += (0.15f * (UI.Instance.emptyDropsInARow - 2));
                chance = Mathf.Clamp(chance, 0f, 0.95f);
            }
        }
        if (Random.value <= chance)
        {
            Instantiate(droppedObject, transform.position, Quaternion.identity);
            if (chance < 1f)
            {
                UI.Instance.emptyDropsInARow = 0;
            }
        }
        else
        {
            UI.Instance.emptyDropsInARow++;
        }
    }
}
