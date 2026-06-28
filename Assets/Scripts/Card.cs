using UnityEngine;

public class Card : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = true;
    [SerializeField] private CardSwipe cardSwipeToActivate;

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
}
