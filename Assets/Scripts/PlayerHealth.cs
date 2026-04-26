using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float health { get; private set; } = 5f;

    [field: SerializeField] public float maxHealth { get; private set; } = 5f;

    private void Start()
    {
        health = maxHealth;
        UI.Instance.SetHealth(health, maxHealth);
    }

    public void Hit()
    {
        health--;
        UI.Instance.SetHealth(health, maxHealth);

        if (health <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(0);
        }
    }
}
