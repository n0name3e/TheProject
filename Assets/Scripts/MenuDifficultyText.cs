using UnityEngine;
using UnityEngine.EventSystems;

public class MenuDifficultyText : MonoBehaviour, IPointerEnterHandler
{
    //[SerializeField] private int difficultyLevel; // 0 = Easy, 1 = Medium, 2 = Hard
    [SerializeField] private RectTransform arrow;

    public void OnPointerEnter(PointerEventData eventData)
    {
        arrow.position = new Vector2(arrow.position.x, transform.position.y);
    }
}
