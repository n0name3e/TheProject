using UnityEngine;
public class Barrel : MonoBehaviour
{
	[SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private int enemyDamage = 6;
    [SerializeField] private float radius = 7f;
    [SerializeField] private bool canFall;
    public void Explode()
    {
        explosionParticles.transform.position = transform.position;
        explosionParticles.Emit(60);

        Vector3 maxPos = GetComponent<Collider>().bounds.max;
        GetComponent<Collider>().enabled = false;
        
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider col in hits)
        {
            if (col.TryGetComponent(out Enemy enemy))
            {
                enemy.Hit(enemyDamage, true);
            }
            if (col.TryGetComponent(out PlayerHealth player))
            {
                Debug.DrawRay(maxPos, (player.transform.position - maxPos).normalized * radius, Color.red, 10f);
                RaycastHit hit;
                // need to check if it actually hits the player, because otherwise its unfair
                if (Physics.Raycast(maxPos, player.transform.position - maxPos, out hit, radius))
                {
                    print(hit.collider.gameObject.name);
                    if (hit.transform == player.transform)
                    {
                        player.Hit(transform);
                    }
                }
            }
        }
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (canFall)
		{
			if (collision.relativeVelocity.magnitude > 5f)
            {
                Explode();
            }
        }
    }
}
