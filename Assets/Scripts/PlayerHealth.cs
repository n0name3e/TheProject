using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float health { get; private set; } = 5f;
    public float immunityTime = 2f;

    [field: SerializeField] public float maxHealth { get; private set; } = 5f;

    private bool isImmune = false;
    private float immunityTimer = 0f;
    private float flickeringTimer = 0f;

    private void Start()
    {
        health = maxHealth;
        UI.Instance.SetHealth(health, maxHealth);
    }
    private void Update()
    {
        if (immunityTimer <= 0f)
        {
            if (isImmune)
            {
                isImmune = false;
                UI.Instance.DisableWhiteHeart();
                return;
            }
            return;
        }
        immunityTimer -= Time.deltaTime;
        if (flickeringTimer <= 0f)
        {
            UI.Instance.ToggleWhiteHeart();
            flickeringTimer += 0.1f;
            return;
        }
        flickeringTimer -= Time.deltaTime;

    }

    public void Hit(Transform hitter)
    {
        if (immunityTimer > 0f)
        {
            print("immune");
            return;
        }
        health--;
        UI.Instance.SetHealth(health, maxHealth);

        if (health <= 0)
        {
            Camera.main.transform.SetParent(null);
            Camera.main.GetComponent<CameraController>().TriggerDeath((transform.position - hitter.position).normalized);
            Destroy(gameObject);
            //SceneManager.LoadScene(0);
            return;
        }
        UI.Instance.EnableWhiteHeart();
        flickeringTimer = 0.1f;
        isImmune = true;
        immunityTimer = immunityTime;
    }
}
