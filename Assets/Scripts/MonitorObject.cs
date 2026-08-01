using UnityEngine;

public class MonitorObject : MonoBehaviour, IInteractable
{
    [field: SerializeField] public bool isInteractable { get; set; } = true;
    [field: SerializeField] public AudioClip interactSound { get; set; }
    [field: SerializeField] public AudioClip nonInteractableSound { get; set; }
    [field: SerializeField] public string interactText { get; set; }
    [field: SerializeField] public string nonInteractableText { get; set; } = "No Power";
    public int health = 5;
    [SerializeField] private GameObject normalMonitorMesh;
    [SerializeField] private GameObject brokenMonitorMesh;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip glassBreakSound;
    [SerializeField] private ParticleSystem shatterGlass;
    [SerializeField] private GameObject image;
    [SerializeField] private Door doorToOpen;
    [SerializeField] private Door warehouseDoor;
    [SerializeField] private EnemyAI[] enemiesToActivate;

    public void Activate()
    {
        isInteractable = true;
        image.SetActive(true);
    }
    public void Interact()
    {
        UI.Instance.EnablePC();
    }
    public void TakeDamage()
    {
        if (!isInteractable)
            return;

        health --;
        shatterGlass.Emit(20);
        audioSource.PlayOneShot(glassBreakSound);
        if (health <= 0)
        {
            isInteractable = false;
            image.SetActive(false);
            nonInteractableText = "Broken";
            normalMonitorMesh.SetActive(false);
            brokenMonitorMesh.SetActive(true);
            doorToOpen.Interact();
            warehouseDoor.Activate();
            foreach (EnemyAI enemy in enemiesToActivate)
            {
                enemy.gameObject.SetActive(true);
                enemy.ActivateChasing();
            }
            Destroy(audioSource.gameObject, 2f);
        }
    }
}
