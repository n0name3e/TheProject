using UnityEngine;

public class HealingPotion : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = true;
    [SerializeField] private bool activeOnEasyOnly = false; // made for starting healing potion
    [field: SerializeField] public AudioClip interactSound { get; set; }
    [field: SerializeField] public AudioClip nonInteractableSound { get; set; }
    [field: SerializeField] public string interactText { get; set; } = "Drink";
    [field: SerializeField] public string nonInteractableText { get; set; }

    public float moveHeight = 0.05f;
    private Vector3 startingPosition;


    private void Start()
    {
        if (activeOnEasyOnly && GameDifficulty.difficulty != DifficultyLevel.Easy)
        {
            gameObject.SetActive(false);
        }
        startingPosition = transform.position;
    }

    private void Update()
    {
            transform.Rotate(0, 60 * Time.deltaTime, 0);
            float newY = startingPosition.y + Mathf.Sin(Time.time * 2) * moveHeight;
            transform.position = new Vector3(startingPosition.x, newY, startingPosition.z);
        
    }

    public void Interact()
    {
        FindAnyObjectByType<PlayerHealth>().Heal();
        Destroy(gameObject);
    }
}
