using UnityEngine;

public class KeyHatch : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = true;
    public string interactText { get; set; } = "Pick Up Key";
    public string nonInteractableText { get; set; }
    [field: SerializeField] public AudioClip nonInteractableSound { get; set; }
    [field: SerializeField] public AudioClip interactSound { get; set; }
    [SerializeField] private EscapeLadder escapeLadder;
    public float moveHeight = 0.05f;
    private Vector3 startingPosition;


    public void Interact()
    {
        escapeLadder.Activate();
        Destroy(gameObject);
    }

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
            transform.Rotate(0, 60 * Time.deltaTime, 0);
            float newY = startingPosition.y + Mathf.Sin(Time.time * 2) * moveHeight;
            transform.position = new Vector3(startingPosition.x, newY, startingPosition.z);
        
    }
}
