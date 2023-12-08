using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuffButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Тип баффа")]
    public BuffProfile.BuffType bType;

    public void OnPointerEnter(PointerEventData eventData)
    {
        FindObjectOfType<BuffManager>().ShowDescr(bType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        FindObjectOfType<BuffManager>().HideDescr();
    }
}
