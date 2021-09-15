using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DirButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [Header("Обычный цвет")]
    public Color normalColor;
    [Header("Цвет наводки")]
    public Color hoverColor;
    [Header("Цвет нажатия")]
    public Color pressedColor;

    bool isHover;

    [Header("Локация")]
    public LocationManager.Location loc;

    [Header("Действие при нажатии")]
    public LocationManager.Action act;

    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponent<Image>().color = hoverColor;
        if (act == LocationManager.Action.None && loc != LocationManager.Location.None)
            FindObjectOfType<GameHelper>().PressDirectionButton(loc);
        else
            FindObjectOfType<GameHelper>().PressActionButton(act);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<Image>().color = pressedColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHover = true;
        GetComponent<Image>().color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;
        GetComponent<Image>().color = normalColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isHover)
            GetComponent<Image>().color = hoverColor;
        else
            GetComponent<Image>().color = normalColor;
    }
}
