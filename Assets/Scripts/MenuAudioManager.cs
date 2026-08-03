using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    public static MenuAudioManager Instance { get; private set; }
    private AudioSource audioSource;

    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip buttonHoverSound;
    [SerializeField] private AudioClip monitorClickSound;
    [SerializeField] private AudioClip monitorHoverSound;
    [SerializeField] private AudioClip signHoverSound;
    [SerializeField] private AudioClip signClickSound;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickSound);
    }
    public void PlayButtonHoverSound()
    {
        audioSource.PlayOneShot(buttonHoverSound);
    }
    public void PlayMonitorClickSound()
    {
        print("click");
        audioSource.PlayOneShot(monitorClickSound);
    }
    public void PlayMonitorHoverSound()
    {
        print("hober");
        audioSource.PlayOneShot(monitorHoverSound);
    }
    public void PlaySignClickSound()
    {
        audioSource.PlayOneShot(signClickSound);
    } public void PlaySignHoverSound()
    {
        audioSource.PlayOneShot(signHoverSound);
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatic()
    {
        Instance = null;
    }
}
