using UnityEngine;

public class ExtendedMagazine : MonoBehaviour, IInteractable
{
    public bool isInteractable { get; set; } = true;
    [field: SerializeField] public AudioClip interactSound { get; set; }
    [field: SerializeField] public AudioClip nonInteractableSound { get; set; }
    [field: SerializeField] public string interactText { get; set; } = "Pick Up";
    [field: SerializeField] public string nonInteractableText { get; set; }


    public float moveHeight = 0.05f;
    private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;
    }
    public void Interact()
    {
        WeaponManager weaponManager = FindAnyObjectByType<WeaponManager>();
        weaponManager.IncreaseRifleAmmo(5);
        Destroy(gameObject);
    }

    void Update()
    {
        transform.Rotate(0, 60 * Time.deltaTime, 0);
        float newY = startingPosition.y + Mathf.Sin(Time.time * 2) * moveHeight;
        transform.position = new Vector3(startingPosition.x, newY, startingPosition.z);
    }
}
