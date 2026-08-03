using UnityEngine;
using TMPro;

public class ExtendedStatsWindow : MonoBehaviour
{
    [SerializeField] private GameObject victoryScreen;

    [SerializeField] private TMP_Text hitsText;
    [SerializeField] private TMP_Text missesText;
    [SerializeField] private TMP_Text accuracyText;
    [SerializeField] private TMP_Text hitsTakenText;
    [SerializeField] private TMP_Text iframeHitsText;
    [SerializeField] private TMP_Text interactionsText;
    [SerializeField] private TMP_Text pistolKillsText;
    [SerializeField] private TMP_Text rifleKillsText;
    [SerializeField] private TMP_Text barrelKillsText;
    [SerializeField] private TMP_Text explosionsText;
    [SerializeField] private TMP_Text bossTimeText;
    [SerializeField] private TMP_Text palletHitsText;

    private StatsManager stats;

    private void OnEnable()
    {
        stats = StatsManager.Instance;

        hitsText.text = stats.hits.ToString();
        missesText.text = stats.misses.ToString();
        float accuracy = stats.hits + stats.misses > 0 ? (float)stats.hits / (stats.hits + stats.misses) * 100f : 0f;
        accuracyText.text = accuracy.ToString("F2") + "%";
        hitsTakenText.text = stats.hitsTaken.ToString();
        iframeHitsText.text = stats.hitsTakenWhileImmune.ToString();
        interactionsText.text = stats.objectsInteracted.ToString();
        pistolKillsText.text = stats.pistolKills.ToString();
        rifleKillsText.text = stats.rifleKills.ToString();
        barrelKillsText.text = stats.barrelKills.ToString();
        explosionsText.text = stats.barrelsExploded.ToString();
        palletHitsText.text = stats.palletHits.ToString();

        int minutes = Mathf.FloorToInt(stats.bossTime / 60f);
        int seconds = Mathf.FloorToInt(stats.bossTime % 60f);
        bossTimeText.text = $"{minutes.ToString("00")}:{seconds.ToString("00")}";
    }
    public void CloseWindow()
    {
        victoryScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}
