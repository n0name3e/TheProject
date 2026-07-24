using UnityEngine;

public class ServerRoomBreaker : MonoBehaviour
{
    [SerializeField] private GameObject monitor;
    [SerializeField] private Light lighting;
    [SerializeField] private ParticleSystem sparksParticles;
    public void Break()
    {
        monitor.SetActive(false);
        lighting.intensity = 120f;
        lighting.range = 30f;
        sparksParticles.Play();
    }
}
