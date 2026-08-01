using UnityEngine;
using UnityEngine.Playables;

public class BossDoor : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = false;
    [SerializeField] private GameObject onCube;
    [SerializeField] private GameObject offCube;
    //[SerializeField] private Transform tpPosition;
    [SerializeField] private PlayableDirector cutscene;
    [field: SerializeField] public AudioClip interactSound { get; set; }
    [field: SerializeField] public AudioClip nonInteractableSound { get; set; }
    [field: SerializeField] public string interactText { get; set; }
    [field: SerializeField] public string nonInteractableText { get; set; }


    //private PlayerMovement playerMovement;
    //[SerializeField] private BossAI bossToActivate;

    public void Activate()
    {
        isInteractable = true;
        offCube.SetActive(false);
        onCube.SetActive(true);
    }
    public void Interact()
    {
        //cutscene.Play();
        cutscene.GetComponent<BossCutscene>().StartCutscene();
        UI.Instance.isCutscene = true;
        UI.Instance.isBoss = true;

        for (int i = EnemyAI.AllActiveEnemies.Count - 1; i >= 0; i--)
        {
            if (EnemyAI.AllActiveEnemies[i] != null && EnemyAI.AllActiveEnemies[i].gameObject.activeInHierarchy)
            {
                Destroy(EnemyAI.AllActiveEnemies[i].gameObject);
            }
        }
        isInteractable = false;
    }
}
