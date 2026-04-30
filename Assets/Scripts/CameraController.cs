using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 5f;
    [SerializeField] private float yLimit = 85f;
    [SerializeField] private Transform player;
    private Vector3 rotation = Vector3.zero;

    // recoil
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;
    private Vector3 targetRecoilRotation = Vector3.zero;
    private Vector3 recoilRotation = Vector3.zero;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // player is dead
        if (Time.timeScale <= 0 || player == null)
        {          
            return;
        }
        rotation.x += Input.GetAxis("Mouse X") * sensitivity;
        rotation.y += Input.GetAxis("Mouse Y") * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yLimit, yLimit);

        targetRecoilRotation = Vector3.Lerp(targetRecoilRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        recoilRotation = Vector3.Slerp(recoilRotation, targetRecoilRotation, snappiness * Time.deltaTime);
        player.localRotation = Quaternion.Euler(0, rotation.x, 0);
        transform.localRotation = Quaternion.Euler(new Vector3(-rotation.y, 0, 0) + recoilRotation);
    }
    public void TriggerRecoil(float recoilMultiplier)
    {
        targetRecoilRotation += new Vector3(-recoilX * recoilMultiplier, Random.Range(-recoilY * recoilMultiplier, +recoilY * recoilMultiplier), Random.Range(-recoilZ, +recoilZ));
    }
    public void TriggerDeath(Vector3 direction)
    {
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(direction * 5f, ForceMode.Impulse);
    }
}
