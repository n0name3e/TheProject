using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private Transform menu;
    [SerializeField] private GameObject options;
    public static bool isPaused = false;

    private void OnEnable()
    {
        isPaused = false;
    }

    private void Update()
    {
        if (!UI.Instance.isCutscene && (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape)) && ((!isPaused && Time.timeScale == 1f) || (isPaused && Time.timeScale == 0f)))
        {
            if (!isPaused)
            {
                Pausing();
            }
            else
            {
                UnPause();
            }
        }
/*#if UNITY_WEBGL
        if (!isPaused && Cursor.lockState != CursorLockMode.Locked)
        {
            Pausing(); // Force the game to pause!
        }
#endif*/
    }

    public void Pausing()
    {
        Time.timeScale = 0f;
        menu.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        UI.Instance.DisableHUD();
        isPaused = true;
    }
    public void UnPause()
    {
        if (options.activeInHierarchy)
        {
            CloseOptions();
        }
        else
        {
            Time.timeScale = 1f;
            menu.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            UI.Instance.EnableHUD();
            isPaused = false;
        }
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    public void Options()
    {
        options.SetActive(true);
        menu.gameObject.SetActive(false);
    }
    public void CloseOptions()
    {
        options.SetActive(false);
        Settings.SaveSettings();
        menu.gameObject.SetActive(true);
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetStatic()
    {
        isPaused = false;
    }
}
