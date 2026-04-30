using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public GameObject currentActiveWindow { get; private set; }

    public void OpenTheWindow(GameObject window)
    {
        currentActiveWindow?.SetActive(false);
        window.SetActive(true);
        currentActiveWindow = window;
    }
    public void CloseCurrentWindow() {
        currentActiveWindow.SetActive(false);
        currentActiveWindow = null;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // as windowManager is disabled in runtime, it won't cause problems
        {
            if (currentActiveWindow != null)
            {
                CloseCurrentWindow();
            }
            else
            {
                UI.Instance.DisablePC();
            }
        }
    }
}
