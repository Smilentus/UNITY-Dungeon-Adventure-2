using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropOnMapPart : MonoBehaviour
{
    [Header("Текущая часть ...")]
    public LocationManager.Location thisLocation;

    [Header("Текущая локация по-русски")]
    public string curLocStr;

    [Header("Вещи, выпадающие на этой локации")]
    [TextArea(3, 5)]
    public string itemsToDrop;

    public void Press()
    {
        if(FindObjectOfType<BattleHelper>().isBattle == false)
        {
            FindObjectOfType<LocationManager>().ShowLocationInfo(thisLocation, curLocStr, itemsToDrop, GetComponent<RawImage>().texture);
        }
    }
}
