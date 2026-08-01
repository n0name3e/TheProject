using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHover : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip pressedSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = UI.Instance.monitorAudio;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(pressedSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(hoverSound);

    }
}
