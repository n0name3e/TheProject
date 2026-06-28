using UnityEngine;

public class BossDoor : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = false;
    [SerializeField] private GameObject onCube;
    [SerializeField] private GameObject offCube;
    [SerializeField] private Transform tpPosition;
    [SerializeField] private BossAI bossToActivate;

    public void Activate()
    {
        isInteractable = true;
        offCube.SetActive(false);
        onCube.SetActive(true);
    }
    public void Interact()
    {
        CharacterController player = FindAnyObjectByType<PlayerMovement>().GetComponent<CharacterController>();
        player.enabled = false;
        player.transform.position = tpPosition.position;
        player.enabled = true;
        bossToActivate.ActivateBoss();
    }
}
