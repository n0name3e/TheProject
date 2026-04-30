using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    public float sprintSpeed = 5f;
    public float backwardSpeedReduction = 0.25f;
    [SerializeField] private float headBobSpeed = 5f;
    [SerializeField] private float bobAmountVertical = 0.1f;
    [SerializeField] private float bobAmountHorizontal = 0.05f;
    [SerializeField] private float sprintBobMultiplier = 1.6f;

    [SerializeField] private Transform hands;

    private Camera mainCamera;
    private Vector3 startingCameraPosition;
    private Vector3 startingHandsPosition;
    private CharacterController characterController;
    private WeaponManager weaponManager;

    private float headBobTimer = 0;
    
    void Update()
    {
        HandleMovement();
    }
    private void Start()
    {
        mainCamera = Camera.main;
        startingCameraPosition = mainCamera.transform.localPosition;
        startingHandsPosition = hands.localPosition;
        characterController = GetComponent<CharacterController>();
        weaponManager = GetComponent<WeaponManager>();
    }
    private bool IsSprinting()
    {
        if (!weaponManager.CanSprint())
        {
            return false;
        }
        // only sprint when moving forward or forward and to the side
        if (Input.GetKey(KeyCode.LeftShift) 
            && Input.GetAxisRaw("Vertical") >= 1f)
        {
            return true;
        }
        return false;
    }
    private void HandleMovement()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        bool isGoingBackwards = Input.GetAxisRaw("Vertical") <= -1;

        Vector3 direction = inputX * mainCamera.transform.right;
        direction += inputY * mainCamera.transform.forward;
        direction.y = 0;
        direction.Normalize();
        float finishSpeed = IsSprinting() ? sprintSpeed : speed;
        finishSpeed *= isGoingBackwards ? (1f - backwardSpeedReduction) : 1f;
        Vector3 finishMovement = finishSpeed * direction;
    
        if (characterController.isGrounded)
        {
            if (finishMovement.y <= 0)
            {
                finishMovement.y = -2f;
            }
        }
        else
        {
            finishMovement.y = characterController.velocity.y + (-10f) * Time.deltaTime;
        }
        characterController.Move(finishMovement * Time.deltaTime);
        HandleHeadBob();
    }
    private void HandleHeadBob()
    {
        float input = Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"));
        if (input > 0.02f && characterController.isGrounded)
        {         
            headBobTimer += Time.deltaTime * (IsSprinting() ? headBobSpeed * sprintBobMultiplier : headBobSpeed);

            float bobX = Mathf.Sin(headBobTimer) * bobAmountHorizontal;
            float weaponBobX = bobX / 2;
            float bobY = Mathf.Abs(Mathf.Cos(headBobTimer)) * bobAmountVertical;

            Vector3 targetPosition = startingCameraPosition +
                new Vector3(bobX, bobY, 0);
            
            mainCamera.transform.localPosition = targetPosition;
            Vector3 targetWeaponPosition = startingHandsPosition +
                new Vector3(weaponBobX, 0, 0);
            hands.localPosition = targetWeaponPosition;
        }
        else
        {
            headBobTimer = 0f;
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition,
                startingCameraPosition, Time.deltaTime);
            hands.localPosition = Vector3.Lerp(hands.localPosition,
                startingHandsPosition, Time.deltaTime);
        }
    }
}
