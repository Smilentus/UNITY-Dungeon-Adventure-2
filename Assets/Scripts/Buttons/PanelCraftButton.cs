using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelCraftButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Номер кнопки 0 - Зелья. 1 - Материалы. 2 - Броня. 3 - Оружие")]
    public int buttonNum = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        FindObjectOfType<CraftingManager>().OpenPanel(buttonNum);
    }
}
