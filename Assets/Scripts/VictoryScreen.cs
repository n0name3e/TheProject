using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] private GameObject extendedStatsWindow;

    [SerializeField] private TMP_Text difficultyText;
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

        if (GameDifficulty.difficulty == DifficultyLevel.Easy)
        {
            difficultyText.color = new Color32(8, 255, 0, 255);
            difficultyText.text = "Easy";
        }
        else if (GameDifficulty.difficulty == DifficultyLevel.Medium)
        {
            difficultyText.color = new Color32(191, 220, 0, 255);
            difficultyText.text = "Medium";
        }
        else if (GameDifficulty.difficulty == DifficultyLevel.Hard)
        {
            difficultyText.color = new Color32(197, 0, 0, 255);
            difficultyText.text = "Hard";
        }
    }
    public void ShowExtendedStats()
    {
        extendedStatsWindow.SetActive(true);
        gameObject.SetActive(false);
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
