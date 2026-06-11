using UnityEngine;

public class EnemyActivator : MonoBehaviour
{
    [SerializeField] private EnemyAI[] enemiesToActivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement p))
        {
            foreach (EnemyAI enemy in enemiesToActivate)
            {
                enemy.ActivateChasing();
            }
            Destroy(gameObject);
        }
    }
}
