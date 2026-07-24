using UnityEngine;

public class Card : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = true;
    [SerializeField] private CardSwipe cardSwipeToActivate;
    [field: SerializeField] public AudioClip interactSound { get; set; }


    public float moveHeight = 0.05f;
    private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;
    }
    public void Interact()
    {
        if (cardSwipeToActivate != null)
        {
            cardSwipeToActivate.isInteractable = true;
        }
        else
        {
            FindAnyObjectByType<CardSwipe>().isInteractable = true;
        }
        Destroy(gameObject);
    }
    private void Update()
    {
        transform.Rotate(0, 60 * Time.deltaTime, 0);
        float newY = startingPosition.y + Mathf.Sin(Time.time * 2) * moveHeight;
        transform.position = new Vector3(startingPosition.x, newY, startingPosition.z);
    }
}
