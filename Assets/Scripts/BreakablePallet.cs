using UnityEngine;

public class BreakablePallet : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
        // play sound
    }
}
