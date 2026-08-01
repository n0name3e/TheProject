using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private Transform menu;
    private bool isPaused = false;

    private void Update()
    {
        if (!UI.Instance.isCutscene && Input.GetKeyDown(KeyCode.Escape) && ((!isPaused && Time.timeScale == 1f) || (isPaused && Time.timeScale == 0f)))
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
        Time.timeScale = 1f;
        menu.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        UI.Instance.EnableHUD();
        isPaused = false;
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
