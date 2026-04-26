using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 5f;
    [SerializeField] private float yLimit = 85f;
    private Transform player;
    private Vector2 rotation = Vector2.zero;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>().transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y") * (-1);
        float yRotation = rotation.y + mouseY;
        yRotation = Mathf.Clamp(yRotation, -yLimit, yLimit);
        rotation.x += mouseX;
        rotation.y = yRotation;
        
        player.Rotate(0, mouseX, 0);
        transform.Rotate(mouseY, 0, 0);
        //transform.rotation = new Vector3(mouseX);
    }
}
