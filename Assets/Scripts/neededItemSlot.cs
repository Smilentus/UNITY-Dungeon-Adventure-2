﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class neededItemSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Нужный предмет")]
    public Item neededItem;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameHelper._GH.ShowMessageText(("Как получить предмет: \""+neededItem.Name+"\"\n\n" + neededItem.WhereToFind), 1);
    }
}
