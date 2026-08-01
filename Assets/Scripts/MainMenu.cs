using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Transform mainCameraPosition;
    [SerializeField] private Transform optionsCameraPosition;
    [SerializeField] private Transform creditsCameraPosition;
    [SerializeField] private Transform playCameraPosition;
    [SerializeField] private GameObject credits;

    public CanvasGroup mainTitleUI;
    public CanvasGroup optionsUI;
    public CanvasGroup creditsUI;
    public CanvasGroup playUI;

    public float moveDuration = 1.5f;
    public float fadeDuration = 0.3f; // How fast the UI fades in/out
    public AnimationCurve movementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private bool isMoving = false;
    private bool isOptions = false;
    private bool isCredits = false;
    private bool isPlay = false;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void Options()
    {
        if (isMoving)
            return;
        if (isOptions)
        {
            StartCoroutine(SlideCameraToMain());
        }
        else {
            StartCoroutine(SlideCameraToOptions());
        }
    }
    public void Credits()
    {
        if (isMoving)
            return;
        if (isCredits) { 
        StartCoroutine(SlideCameraToMain());
        }
        else
        {
            StartCoroutine(SlideCameraToCredits());
        }
    }
    public void Play()
    {
        if (isMoving)
            return;
        if (isPlay)
        {
            StartCoroutine(SlideCameraToMain());
        }
        else
        {
            StartCoroutine(SlideCameraToPlay());
        }
    }
    public void SetDifficulty(int diff)
    {
        //GameDifficulty.difficultyLevel = diff;
        GameDifficulty.difficulty = (DifficultyLevel)diff;
        print(diff);

        SceneManager.LoadScene(1);
    }
    private IEnumerator SlideCameraToOptions()
    {
        StartCoroutine(FadeTitle(0f));

        isMoving = true;
        Vector3 startPos = mainCam.transform.position;
        Quaternion startRot = mainCam.transform.rotation;
        float elapsedTime = 0f;

        // 2. Slide the camera (Same as before)
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / moveDuration;
            float curveValue = movementCurve.Evaluate(percentage);

            mainCam.transform.position = Vector3.Lerp(startPos, optionsCameraPosition.position, curveValue);
            mainCam.transform.rotation = Quaternion.Lerp(startRot, optionsCameraPosition.rotation, curveValue);
            yield return null;
        }

        mainCam.transform.position = optionsCameraPosition.position;
        mainCam.transform.rotation = optionsCameraPosition.rotation;

        StartCoroutine(ActivateOptions(1f));
    }
    private IEnumerator SlideCameraToMain()
    {
        StartCoroutine(ActivateOptions(0f));
        StartCoroutine(ActivateCredits(0f));
        StartCoroutine(ActivatePlay(0f));

        Settings.SaveSettings();

        isMoving = true;
        Vector3 startPos = mainCam.transform.position;
        Quaternion startRot = mainCam.transform.rotation;
        float elapsedTime = 0f;

        // 2. Slide the camera (Same as before)
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / moveDuration;
            float curveValue = movementCurve.Evaluate(percentage);

            mainCam.transform.position = Vector3.Lerp(startPos, mainCameraPosition.position, curveValue);
            mainCam.transform.rotation = Quaternion.Lerp(startRot, mainCameraPosition.rotation, curveValue);
            yield return null;
        }

        mainCam.transform.position = mainCameraPosition.position;
        mainCam.transform.rotation = mainCameraPosition.rotation;

        isMoving = false;
        isOptions = false;
        isCredits = false;
        isPlay = false;
        StartCoroutine(FadeTitle(1f));
    }
    private IEnumerator SlideCameraToCredits()
    {
        StartCoroutine(FadeTitle(0f));

        isMoving = true;
        Vector3 startPos = mainCam.transform.position;
        Quaternion startRot = mainCam.transform.rotation;
        float elapsedTime = 0f;

        // 2. Slide the camera (Same as before)
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / moveDuration;
            float curveValue = movementCurve.Evaluate(percentage);

            mainCam.transform.position = Vector3.Lerp(startPos, creditsCameraPosition.position, curveValue);
            mainCam.transform.rotation = Quaternion.Lerp(startRot, creditsCameraPosition.rotation, curveValue);
            yield return null;
        }

        mainCam.transform.position = creditsCameraPosition.position;
        mainCam.transform.rotation = creditsCameraPosition.rotation;

        isOptions = false;
        StartCoroutine(ActivateCredits(1f));
    }
    private IEnumerator SlideCameraToPlay()
    {
        StartCoroutine(FadeTitle(0f));
        isMoving = true;
        Vector3 startPos = mainCam.transform.position;
        Quaternion startRot = mainCam.transform.rotation;
        float elapsedTime = 0f;
        // 2. Slide the camera (Same as before)
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / moveDuration;
            float curveValue = movementCurve.Evaluate(percentage);
            mainCam.transform.position = Vector3.Lerp(startPos, playCameraPosition.position, curveValue);
            mainCam.transform.rotation = Quaternion.Lerp(startRot, playCameraPosition.rotation, curveValue);
            yield return null;
        }

        mainCam.transform.position = playCameraPosition.position;
        mainCam.transform.rotation = playCameraPosition.rotation;
        isOptions = false;
        isCredits = false;
        isPlay = true;

        StartCoroutine(ActivatePlay(1f));
    }
    private IEnumerator FadeTitle(float a)
    {
        float startAlpha = mainTitleUI.alpha;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            mainTitleUI.alpha = Mathf.Lerp(startAlpha, a, elapsed / fadeDuration);
            yield return null;
        }

        mainTitleUI.alpha = a;
    }
    private IEnumerator ActivateOptions(float a)
    {
        float startAlpha = optionsUI.alpha;
        float elapsed = 0f;
        optionsUI.gameObject.SetActive(true);
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            optionsUI.alpha = Mathf.Lerp(startAlpha, a, elapsed / fadeDuration);
            yield return null;
        }

        optionsUI.alpha = a;
        optionsUI.blocksRaycasts = (a == 1f);
        optionsUI.interactable = (a == 1f);

        if (a == 1f)
        {
            isMoving = false;
            isOptions = true;
        }
    }
    private IEnumerator ActivateCredits(float a)
    {
        float startAlpha = creditsUI.alpha;
        float elapsed = 0f;
        creditsUI.gameObject.SetActive(true);
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            creditsUI.alpha = Mathf.Lerp(startAlpha, a, elapsed / fadeDuration);
            yield return null;
        }

        creditsUI.alpha = a;
        creditsUI.blocksRaycasts = (a == 1f);
        creditsUI.interactable = (a == 1f);

        if (a == 1f)
        {
            isMoving = false;
            isCredits = true;
        }
    }
    private IEnumerator ActivatePlay(float a)
    {
        float startAlpha = playUI.alpha;
        float elapsed = 0f;
        playUI.gameObject.SetActive(true);
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            playUI.alpha = Mathf.Lerp(startAlpha, a, elapsed / fadeDuration);
            yield return null;
        }
        playUI.alpha = a;
        playUI.blocksRaycasts = (a == 1f);
        playUI.interactable = (a == 1f);
        if (a == 1f)
        {
            isMoving = false;
            isPlay = true;
        }
    }
}
