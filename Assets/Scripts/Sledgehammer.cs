using System;
using UnityEngine;

public class Sledgehammer : MonoBehaviour
{
    [SerializeField] private float radius = 1.25f;
    [SerializeField] private Transform hitPivot;
    public void Hit()
    {
        Collider[] hits = Physics.OverlapSphere(hitPivot.position, radius);
        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.Hit(hitPivot);
                return;
            }
        }
    }
}
