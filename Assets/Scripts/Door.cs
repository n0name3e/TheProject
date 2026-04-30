using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [field: SerializeField] public bool isInteractable { get; set; } = true; // if false then use PC
    [SerializeField] private GameObject offCube;
    [SerializeField] private GameObject onCube;
    
    //public bool hasBeenInteracted = false; // if true then door won't open again
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        offCube.SetActive(!isInteractable);
        onCube.SetActive(isInteractable);
    }
    public void Activate()
    {
        isInteractable = true;
        offCube.SetActive(false);
        onCube.SetActive(true);
    }
    public void Interact()
    {
        /*if (!isInteractable)
        {
            return;
        }*/
        animator.SetTrigger("Open");
        isInteractable = false;
    }
}
