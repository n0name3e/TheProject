using UnityEngine;
using UnityEngine.AI;

public class FallingEnemy : MonoBehaviour
{
    public bool isFalling = false;

    public void StartFalling()
    {
        isFalling = true;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isFalling)
        {
            Destroy(gameObject);
        }
    }
}
