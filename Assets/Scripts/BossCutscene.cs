using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BossCutscene : MonoBehaviour
{
    [SerializeField] private Transform tpPosition;
    [SerializeField] private BossAI bossToActivate;
    [SerializeField] private PlayableDirector cutscene;
    [SerializeField] private Image blackBackground;
    [SerializeField] private AudioSource music;
    private CharacterController player;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>().GetComponent<CharacterController>();
        blackBackground.gameObject.SetActive(false);
    }
    
    private void Update()
    {
        if (UI.Instance.isCutscene && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab)))
        {
            if (cutscene.state == PlayState.Playing)
            {
                StopAllCoroutines();
                cutscene.time = cutscene.duration;
            }
        }
    }
    public void StartCutscene()
    {
        StartCoroutine(CutsceneStart());
    }
    private IEnumerator CutsceneStart()
    {
        yield return StartCoroutine(BlackImageAppear());
        //yield return new WaitForSeconds(0.5f);
        cutscene.Play();
        StartCoroutine(BlackImageFade());
    }
    private IEnumerator BlackImageAppear()
    {
        blackBackground.gameObject.SetActive(true);
        while (blackBackground.color.a < 1f)
        {
            Color c = blackBackground.color;
            c.a = Mathf.MoveTowards(blackBackground.color.a, 1f, Time.deltaTime * 2f);
            blackBackground.color = c;
            yield return null;
        }
    }
    private IEnumerator BlackImageFade()
    {
        while (blackBackground.color.a > 0f)
        {
            Color c = blackBackground.color;
            c.a = Mathf.MoveTowards(blackBackground.color.a, 0f, Time.deltaTime * 2f);
            blackBackground.color = c;
            yield return null;
        }
        blackBackground.gameObject.SetActive(false);
    }
    // called at the last frame of the cutscene timeline
    public void FinishCutscene()
    {
        cutscene.Pause();
        StartCoroutine(CutsceneFinish());
    }
    private IEnumerator CutsceneFinish()
    {
        yield return StartCoroutine(BlackImageAppear());
        yield return new WaitForSeconds(0.5f);
        cutscene.Stop();
        StartCoroutine(BlackImageFade());
        player.enabled = false;
        player.transform.position = tpPosition.position;
        player.enabled = true;
        UI.Instance.ActivateBossHealth(true);
        UI.Instance.isCutscene = false;
        bossToActivate.ActivateBoss();
        music.Play();
    }
}
