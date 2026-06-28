using UnityEngine;

public class BreakablePallet : MonoBehaviour
{
    [SerializeField] private FallingEnemy enemyToActivate;
    public void Destroy()
    {
        if (enemyToActivate != null) {
            enemyToActivate.StartFalling();
        }
        Destroy(gameObject);
        // play sound
    }
}
