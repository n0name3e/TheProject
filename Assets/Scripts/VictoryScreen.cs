using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text hitsText;

    private void OnEnable()
    {
        int minutes = Mathf.FloorToInt(StatsManager.Instance.time / 60f);
        int seconds = Mathf.FloorToInt(StatsManager.Instance.time % 60f);
        timeText.text = $"{minutes.ToString("00")}:{seconds.ToString("00")}";

        killsText.text = StatsManager.Instance.kills.ToString();
        hitsText.text = StatsManager.Instance.hits.ToString();
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
