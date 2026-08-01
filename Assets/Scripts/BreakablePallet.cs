using UnityEngine;

public class BreakablePallet : MonoBehaviour
{
    [SerializeField] private FallingEnemy enemyToActivate;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private int health = 3;
    [SerializeField] private bool breakByEnemy = false;
    public void Hit(int damage)
    {
        if (!breakByEnemy && damage == 1) // player deals 3 damage
        {
            return;
        }
        health -= damage;
        if (health <= 0)
        {
            Destroy();
            return;
        }
        audioSource.PlayOneShot(breakSound);
    }
    public void Destroy()
    {
        if (enemyToActivate != null) {
            enemyToActivate.StartFalling();
        }
        audioSource.transform.SetParent(null, true);
        audioSource.PlayOneShot(breakSound);
        Destroy(gameObject);
        // play sound
    }
}
