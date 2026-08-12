using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public GameObject currentActiveWindow { get; private set; }

    [SerializeField] private AudioClip windowSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = UI.Instance.monitorAudio;
    }

    public void OpenTheWindow(GameObject window)
    {
        audioSource.PlayOneShot(windowSound);
        currentActiveWindow?.SetActive(false);
        window.SetActive(true);
        currentActiveWindow = window;
    }
    public void CloseCurrentWindow() {
        if (currentActiveWindow != null)
        {
            audioSource.PlayOneShot(windowSound);
            currentActiveWindow.SetActive(false);
            currentActiveWindow = null;
        }

        else {
            audioSource.PlayOneShot(windowSound);
            UI.Instance.DisablePC();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape)) // as windowManager is disabled in runtime, it won't cause problems
        {
            if (currentActiveWindow != null)
            {
                CloseCurrentWindow();
            }
            else
            {
                audioSource.PlayOneShot(windowSound);
                UI.Instance.DisablePC();
            }
        }
    }
}
