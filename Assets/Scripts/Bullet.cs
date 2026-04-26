using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float Speed;
    private Vector3 Direction;

    private void Update()
    {
        transform.position += Speed * Time.deltaTime * Direction;
    }
    public void Launch(float speed, Vector3 direction)
    {
        Speed = speed;
        Direction = direction;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            return;
        }
        if (collision.transform.TryGetComponent(out PlayerHealth player))
        {
            player.Hit();
        }
        Destroy(gameObject);
    }
}
