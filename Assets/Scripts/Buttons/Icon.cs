using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Icon : MonoBehaviour, IPointerClickHandler
{
    [Header("Тип иконки")]
    public IconsHelper.Icon thisIcon;

    public void OnPointerClick(PointerEventData eventData)
    {
        FindObjectOfType<IconsHelper>().ShowIconDescr(thisIcon);
    }
}
