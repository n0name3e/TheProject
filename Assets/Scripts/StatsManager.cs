using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get; private set; }

    public float time;
    public int kills;
    public int hits;
    public int misses;
    public float accuracy;
    public int hitsTaken;
    public int hitsTakenWhileImmune;
    public int objectsInteracted;
    public int pistolKills;
    public int rifleKills;
    public float bossTime;
    public int barrelsExploded;
    public int barrelKills;
    public int palletHits;

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

    // Update is called once per frame
    void Update()
    {
        if (Pause.isPaused || UI.Instance.hasWon) return;
        time += Time.unscaledDeltaTime; // works in monitor
        if (UI.Instance.isBoss)
        {
            bossTime += Time.unscaledDeltaTime;
        }
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatic()
    {
        Instance = null;
    }
}
