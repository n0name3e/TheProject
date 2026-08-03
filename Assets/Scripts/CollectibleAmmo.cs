using UnityEngine;

public class CollectibleAmmo : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = true;
    [field: SerializeField] public AudioClip interactSound { get; set; }
    [field: SerializeField] public AudioClip nonInteractableSound { get; set; }
    [field: SerializeField] public string interactText { get; set; } = "Collect";
    [field: SerializeField] public string nonInteractableText { get; set; }
    [SerializeField] private bool reduceAmmoInHard = true;
    [SerializeField] private bool isRifle = false; // if so player will get rifle 

    public bool isRotating = true;
    public bool isDestroying = true; // if not then some object will be removed (like remove magazine from rifle)
    public GameObject objectToDestroy;

    public float moveHeight = 0.05f;

    public int minAmmoAmount = 10;
    public int maxAmmoAmount = 20;

    private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        if (isRotating)
        {
            transform.Rotate(0, 60 * Time.deltaTime, 0);
            float newY = startingPosition.y + Mathf.Sin(Time.time * 2) * moveHeight;
            transform.position = new Vector3(startingPosition.x, newY, startingPosition.z);
        }
    }

    public void Interact()
    {
        int ammoAmount = Random.Range(minAmmoAmount, maxAmmoAmount + 1);
        if (reduceAmmoInHard && GameDifficulty.difficulty == DifficultyLevel.Hard)
        {
            ammoAmount = Mathf.CeilToInt((float)ammoAmount * 0.85f);
        }
        if (isRifle)
        {
            FindAnyObjectByType<WeaponManager>().CollectRifle();
        }
        else
        {
            FindAnyObjectByType<WeaponManager>().CollectAmmo(ammoAmount);
        }
        if (isDestroying)
        {
            Destroy(gameObject);
            return;
        }
        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
        }
        isInteractable = false;
    }
}
