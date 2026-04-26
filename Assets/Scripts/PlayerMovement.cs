using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    public float sprintSpeed = 5f;

    private Camera mainCamera;
    private CharacterController characterController;
    
    void Update()
    {
        HandleMovement();
    }
    private void Start()
    {
        mainCamera = Camera.main;
        characterController = GetComponent<CharacterController>();
    }
    private void HandleMovement()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector3 direction = inputX * mainCamera.transform.right;
        direction += inputY * mainCamera.transform.forward;
        direction.y = 0;
        direction.Normalize();
        float finishSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;
        characterController.Move(finishSpeed * Time.deltaTime * direction);
    }
}
